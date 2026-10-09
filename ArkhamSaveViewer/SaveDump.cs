using System.Globalization;
using System.Reflection;
using System.Text;
using System.IO;
using ArkhamSaveParser;

namespace ArkhamSaveViewer {
	/// <summary>
	/// Helpers that turn a parsed save into a flat list of (field, value) pairs, and a "dump a whole folder" export
	/// (fields.csv, footer_bytes.csv and two summaries that show which values every field takes across all the saves).
	/// </summary>
	internal static class SaveDump {
		// Fields from this one on belong to segment 3, which is the part that can appear more than once in a save
		private const string FirstSegment3Field = "unknown78";

		public static List<(string Name, string Type, string Value)> CollectFields(ArkhamCitySave save) {
			var result = new List<(string, string, string)>();
			AddFields(result, save, "", false);

			// Each additional copy of segment 3 is stored inside the previous one
			var current = save;
			var number = 1;
			while (current.additionalSegment3s.Count > 0) {
				current = current.additionalSegment3s[0];
				++number;
				AddFields(result, current, "seg" + number + ".", true);
			}

			return result;
		}

		private static void AddFields(List<(string, string, string)> result, ArkhamCitySave save, string prefix, bool segment3Only) {
			var started = !segment3Only;
			foreach (var field in typeof(ArkhamCitySave).GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				if (field.Name == nameof(ArkhamCitySave.additionalSegment3s)) {
					continue;
				}

				if (!started && field.Name == FirstSegment3Field) {
					started = true;
				}

				if (!started) {
					continue;
				}

				result.Add((prefix + field.Name, Formatting.TypeName(field.FieldType), Formatting.Format(field.GetValue(save))));
			}
		}

		/// <summary>Footer bytes of the save and every additional copy of segment 3.</summary>
		public static List<byte[]> CollectFooters(ArkhamCitySave save) {
			var result = new List<byte[]>();
			var current = save;
			while (current != null) {
				result.Add(current.GetFooterBytes());
				current = current.additionalSegment3s.Count > 0 ? current.additionalSegment3s[0] : null;
			}

			return result;
		}

		public static ArkhamCitySave LoadTolerant(string path) {
			var temp = Path.Combine(Path.GetTempPath(), "ArkhamSaveViewer_" + Guid.NewGuid().ToString("N") + ".sgd");
			try {
				using (var src = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
				using (var dst = File.Create(temp)) {
					src.CopyTo(dst);
				}

				return new ArkhamCitySave(new CompressedSave(temp), true);
			} finally {
				try { File.Delete(temp); } catch { }
			}
		}

		public static string DumpFolder(string folder, string outputDir, Action<string>? progress) {
			var files = Directory.GetFiles(folder, "*.sgd", SearchOption.AllDirectories);
			Array.Sort(files, StringComparer.OrdinalIgnoreCase);

			var keys = new List<string>();
			var keySet = new HashSet<string>();
			var rows = new List<(string File, string Error, Dictionary<string, string> Values, List<byte[]> Footers)>();
			var typeOf = new Dictionary<string, string>();

			foreach (var file in files) {
				var relative = Path.GetRelativePath(folder, file);
				progress?.Invoke(relative);
				try {
					var save = LoadTolerant(file);
					var values = new Dictionary<string, string>();
					foreach (var (name, type, value) in CollectFields(save)) {
						values[name] = value;
						typeOf[name] = type;
						if (keySet.Add(name)) {
							keys.Add(name);
						}
					}

					var error = save.parseError ?? save.tailError ?? "";
					rows.Add((relative, error, values, CollectFooters(save)));
				} catch (Exception ex) {
					rows.Add((relative, "FAILED: " + ex.Message, new Dictionary<string, string>(), new List<byte[]>()));
				}
			}

			Directory.CreateDirectory(outputDir);
			var utf8 = new UTF8Encoding(true);

			// fields.csv: one row per save
			var sb = new StringBuilder();
			sb.Append("file,error");
			foreach (var key in keys) {
				sb.Append(',').Append(Csv(key));
			}

			sb.AppendLine();
			foreach (var row in rows) {
				sb.Append(Csv(row.File)).Append(',').Append(Csv(row.Error));
				foreach (var key in keys) {
					sb.Append(',').Append(Csv(row.Values.TryGetValue(key, out var v) ? v : ""));
				}

				sb.AppendLine();
			}

			File.WriteAllText(Path.Combine(outputDir, "fields.csv"), sb.ToString(), utf8);

			// field_summary.txt: for every field, the values it takes across all saves
			sb.Clear();
			sb.AppendLine("Values of every field across " + rows.Count + " saves (" + folder + ")");
			sb.AppendLine();
			foreach (var key in keys) {
				var counts = new Dictionary<string, int>();
				foreach (var row in rows) {
					if (row.Values.TryGetValue(key, out var v)) {
						counts[v] = counts.TryGetValue(v, out var c) ? c + 1 : 1;
					}
				}

				sb.Append(key).Append("  (").Append(typeOf[key]).Append(")  ");
				if (counts.Count == 1) {
					sb.AppendLine("CONSTANT: " + Short(counts.Keys.First()));
				} else {
					sb.AppendLine(counts.Count + " different values");
					foreach (var pair in counts.OrderByDescending(p => p.Value).ThenBy(p => p.Key, StringComparer.Ordinal).Take(12)) {
						sb.AppendLine("    " + pair.Value.ToString(CultureInfo.InvariantCulture).PadLeft(4) + " x  " + Short(pair.Key));
					}

					if (counts.Count > 12) {
						sb.AppendLine("    ... and " + (counts.Count - 12) + " more");
					}
				}
			}

			File.WriteAllText(Path.Combine(outputDir, "field_summary.txt"), sb.ToString(), utf8);

			// footer_bytes.csv: one row per copy of segment 3, one column per byte
			sb.Clear();
			sb.Append("file,copy");
			for (var i = 0; i < 830; ++i) {
				sb.Append(',').Append(i.ToString(CultureInfo.InvariantCulture));
			}

			sb.AppendLine();
			var perOffset = new List<Dictionary<byte, int>>();
			for (var i = 0; i < 830; ++i) {
				perOffset.Add(new Dictionary<byte, int>());
			}

			foreach (var row in rows) {
				for (var copy = 0; copy < row.Footers.Count; ++copy) {
					var bytes = row.Footers[copy];
					sb.Append(Csv(row.File)).Append(',').Append(copy + 1);
					for (var i = 0; i < 830; ++i) {
						var b = i < bytes.Length ? bytes[i] : (byte)0;
						sb.Append(',').Append(b);
						perOffset[i][b] = perOffset[i].TryGetValue(b, out var c) ? c + 1 : 1;
					}

					sb.AppendLine();
				}
			}

			File.WriteAllText(Path.Combine(outputDir, "footer_bytes.csv"), sb.ToString(), utf8);

			// footer_summary.txt: only the bytes that are not always zero
			sb.Clear();
			sb.AppendLine("Footer bytes (the 830 byte block at the end of every copy of segment 3). Offsets that are always 0 are left out.");
			sb.AppendLine();
			for (var i = 0; i < 830; ++i) {
				var counts = perOffset[i];
				if (counts.Count == 1 && counts.ContainsKey(0)) {
					continue;
				}

				sb.Append("[" + i + "]  ");
				if (counts.Count == 1) {
					sb.AppendLine("CONSTANT " + counts.Keys.First());
				} else {
					sb.AppendLine(string.Join(", ", counts.OrderByDescending(p => p.Value).Take(12).Select(p => p.Key + " (" + p.Value + "x)")));
				}
			}

			File.WriteAllText(Path.Combine(outputDir, "footer_summary.txt"), sb.ToString(), utf8);

			var failed = rows.Count(r => r.Error.Length > 0);
			return "Dumped " + rows.Count + " saves (" + failed + " with an error) to " + outputDir;
		}

		private static string Short(string value) {
			return value.Length > 160 ? value.Substring(0, 160) + " ..." : value;
		}

		private static string Csv(string value) {
			if (value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0) {
				return "\"" + value.Replace("\"", "\"\"") + "\"";
			}

			return value;
		}
	}
}
