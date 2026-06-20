namespace ArkhamSaveParser.Tests;

using System.IO;
using System.Text;

[TestClass]
public class ParserUtilsTest
{
	private BinaryReader CreateBinaryReader(byte[] bytes) {
		var ms = new MemoryStream();
		ms.Write(bytes, 0, bytes.Length);
		ms.Position = 0;
		return new BinaryReader(ms);
	}

    [TestMethod]
    public void ReadInt32_BE_Test() {
		void Run(int expected) {
			var bytes = BitConverter.GetBytes(expected);
			if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
			var reader = CreateBinaryReader(bytes);
			int actual = ParserUtils.ReadInt32_BE(reader);
			Assert.AreEqual(expected, actual);
		}

		Run(123456789);
		Run(-123456789);
		Run(0);
		Run(int.MaxValue);
		Run(int.MinValue);
    }

	[TestMethod]
	public void ReadUint32_BE_Test() {
		void Run(uint expected) {
			var bytes = BitConverter.GetBytes(expected);
			if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
			var reader = CreateBinaryReader(bytes);
			uint actual = ParserUtils.ReadUint32_BE(reader);
			Assert.AreEqual(expected, actual);
		}

		Run(1);
		Run(10);
		Run(123456789);
		Run(0);
		Run(uint.MaxValue);
		Run(uint.MinValue);
	}

	[TestMethod]
	public void ReadFloat_BE_Test() {
		void Run(float expected) {
			var bytes = BitConverter.GetBytes(expected);
			if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
			var reader = CreateBinaryReader(bytes);
			float actual = ParserUtils.ReadFloat_BE(reader);
			Assert.AreEqual(expected, actual);
		}

		Run(0.0f);
		Run(-0.0f);
		Run(1.0f);
		Run(10.0f);
		Run(float.MinValue);
		Run(float.MaxValue);
		Run(float.PositiveInfinity);
		Run(float.NegativeInfinity);
	}

	[TestMethod]
	public void ReadUE3String_Test() {
		byte[] ConstructStringASCII(string inStr) {
			if (string.IsNullOrEmpty(inStr)) {
				var zero = BitConverter.GetBytes(0);
				if (BitConverter.IsLittleEndian) Array.Reverse(zero);
				return zero;
			}

			int size = inStr.Length; // number of bytes
			var lenBytes = BitConverter.GetBytes(size);
			if (BitConverter.IsLittleEndian) Array.Reverse(lenBytes);
			var strBytes = Encoding.ASCII.GetBytes(inStr);
			var outBytes = new byte[4 + strBytes.Length];
			Buffer.BlockCopy(lenBytes, 0, outBytes, 0, 4);
			Buffer.BlockCopy(strBytes, 0, outBytes, 4, strBytes.Length);
			return outBytes;
		}

		byte[] ConstructStringUTF16(string inStr) {
			if (string.IsNullOrEmpty(inStr)) {
				var zero = BitConverter.GetBytes(0);
				if (BitConverter.IsLittleEndian) Array.Reverse(zero);
				return zero;
			}

			int utf16Len = -inStr.Length;
			var utf16LenBytes = BitConverter.GetBytes(utf16Len);
			if (BitConverter.IsLittleEndian) Array.Reverse(utf16LenBytes);
			var utf16Bytes = Encoding.Unicode.GetBytes(inStr);
			var outUtf16 = new byte[4 + utf16Bytes.Length];
			Buffer.BlockCopy(utf16LenBytes, 0, outUtf16, 0, 4);
			Buffer.BlockCopy(utf16Bytes, 0, outUtf16, 4, utf16Bytes.Length);
			// two-byte null terminator already zeroed by array init
			return outUtf16;
		}

		void Run(string expected) {
			var bytes = ConstructStringASCII(expected);
			var reader = CreateBinaryReader(bytes);
			string actual = ParserUtils.ReadUE3String(reader);
			Assert.AreEqual(expected, actual);

			bytes = ConstructStringUTF16(expected);
			reader = CreateBinaryReader(bytes);
			actual = ParserUtils.ReadUE3String(reader);
			Assert.AreEqual(expected, actual);
		}

		Run("Hi");
		Run("Hello");
		Run("");
		Run("a really LonG STRING");
	}
}
