using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace ArkhamSaveParser {
	public class ParserUtils {
		public static int ReadInt32_BE(BinaryReader reader) {
			return BinaryPrimitives.ReadInt32BigEndian(reader.ReadBytes(sizeof(int)));
		}

		public static uint ReadUint32_BE(BinaryReader reader) {
			return BinaryPrimitives.ReadUInt32BigEndian(reader.ReadBytes(sizeof(uint)));
		}

		public static float ReadFloat_BE(BinaryReader reader) {
			return BinaryPrimitives.ReadSingleBigEndian(reader.ReadBytes(sizeof(float)));
		}

		public static string ReadUE3String(BinaryReader reader) {
			int length = ReadInt32_BE(reader);
			if (length == 0) {
				return "";
			}

			if (length > 0) {
				return Encoding.ASCII.GetString(reader.ReadBytes(length));
			}

			int utf16Bytes = Math.Abs(length) * 2;
			return Encoding.Unicode.GetString(reader.ReadBytes(utf16Bytes));
		}

		public static List<string> ReadStringArray(BinaryReader reader) {
			uint length = ReadUint32_BE(reader);
			List<string> result = new List<string>();
			for (var i = 0; i < length; ++i) {
				result.Add(ReadUE3String(reader));
			}

			return result;
		}
	}
}
