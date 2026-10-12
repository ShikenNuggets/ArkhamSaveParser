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

			var bytesRemaining = reader.BaseStream.Length - reader.BaseStream.Position;
			if (length > bytesRemaining) {
				throw new Exception("Tried to parse a string of length " + length.ToString() + " when there were only " + bytesRemaining.ToString() + " bytes remaining in the file");
			}

			if (length > 0) {
				var asciiBytes = reader.ReadBytes(length - 1);
				reader.ReadByte(); // Discard the null terminator byte
				return Encoding.ASCII.GetString(asciiBytes);
			}

			int utf16Bytes = (Math.Abs(length) * 2) - 2;
			var strBytes = reader.ReadBytes(utf16Bytes);
			reader.ReadBytes(2); // Discard the null terminator bytes
			return Encoding.Unicode.GetString(strBytes);
		}

		private static void CheckArrayFits(BinaryReader reader, uint length, int elementSize, string typeName) {
			var bytesRemaining = reader.BaseStream.Length - reader.BaseStream.Position;
			if ((long)length * elementSize > bytesRemaining) {
				throw new Exception("Tried to parse a " + typeName + " array of length " + length.ToString() + " when there were only " + bytesRemaining.ToString() + " bytes remaining in the file");
			}
		}

		/// <summary>Reads a uint32 count followed by that many big endian uint32 values.</summary>
		public static uint[] ReadUint32Array_BE(BinaryReader reader) {
			uint length = ReadUint32_BE(reader);
			CheckArrayFits(reader, length, sizeof(uint), "uint32");

			var result = new uint[length];
			for (var i = 0; i < length; ++i) {
				result[i] = ReadUint32_BE(reader);
			}

			return result;
		}

		/// <summary>Reads a uint32 count followed by that many big endian float values.</summary>
		public static float[] ReadFloatArray_BE(BinaryReader reader) {
			uint length = ReadUint32_BE(reader);
			CheckArrayFits(reader, length, sizeof(float), "float");

			var result = new float[length];
			for (var i = 0; i < length; ++i) {
				result[i] = ReadFloat_BE(reader);
			}

			return result;
		}

		/// <summary>Reads a uint32 count followed by that many raw bytes.</summary>
		public static byte[] ReadByteArray(BinaryReader reader) {
			uint length = ReadUint32_BE(reader);
			CheckArrayFits(reader, length, sizeof(byte), "byte");

			return reader.ReadBytes((int)length);
		}

		public static List<string> ReadStringArray(BinaryReader reader) {
			uint length = ReadUint32_BE(reader);
			var bytesRemaining = reader.BaseStream.Length - reader.BaseStream.Position;
			if (length > bytesRemaining) {
				throw new Exception("Tried to parse a string array of length " + length.ToString() + " when there were only " + bytesRemaining.ToString() + " bytes remaining in the file");
			}

			List<string> result = new List<string>();
			for (var i = 0; i < length; ++i) {
				result.Add(ReadUE3String(reader));
			}

			return result;
		}
	}
}
