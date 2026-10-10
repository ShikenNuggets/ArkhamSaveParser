using System.Collections;
using System.Globalization;

namespace ArkhamSaveViewer {
	/// <summary>Turns parsed field types and values into the text shown in the viewer and written to the dumps.</summary>
	internal static class Formatting {
		public static string TypeName(Type type) {
			if (type == typeof(uint)) return "uint";
			if (type == typeof(float)) return "float";
			if (type == typeof(string)) return "string";
			if (type == typeof(uint[])) return "uint[]";
			if (type == typeof(int[])) return "int[]";
			if (type == typeof(int[][])) return "int[][]";
			if (type == typeof(float[])) return "float[]";
			if (type == typeof(byte[])) return "byte[]";
			if (type == typeof(List<string>)) return "string list";
			if (type == typeof(List<byte[]>)) return "byte[] list";
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
