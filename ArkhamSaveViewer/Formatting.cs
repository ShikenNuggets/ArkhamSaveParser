using System.Collections;
using System.Globalization;

namespace ArkhamSaveViewer {
	internal static class Formatting {
		public static string GetTypeName(Type type) {
			if (type.GenericTypeArguments.Length > 0) {
				var args = string.Join(", ", type.GenericTypeArguments.Select(GetTypeName));
				return type.Name.Split('`')[0] + "<" + args + ">";
			}

			return type.Name;
		}

		public static string Format(object? value) {
			switch (value) {
				case null:
					return "(null)";
				case string s:
					return "\"" + s + "\"";
				case float f:
					return f.ToString("R", CultureInfo.InvariantCulture);
				case byte[] bytes:
					return "byte[" + bytes.Length + "] " + BitConverter.ToString(bytes).Replace('-', ' ');
				case float[] floats:
					return "float[" + floats.Length + "] " + string.Join(", ", floats.Select(x => x.ToString("R", CultureInfo.InvariantCulture)));
				case int[] ints:
					return "int[" + ints.Length + "] " + string.Join(", ", ints);
				case uint[] uints:
					return "uint[" + uints.Length + "] " + string.Join(", ", uints);
				case IEnumerable items:
					var parts = new List<string>();
					foreach (var item in items) {
						parts.Add(Format(item));
					}

					return "[" + parts.Count + "] " + string.Join(" | ", parts);
				default:
					return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
			}
		}
	}
}
