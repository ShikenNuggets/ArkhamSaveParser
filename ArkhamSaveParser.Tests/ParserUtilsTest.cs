namespace ArkhamSaveParser.Tests;

using System.IO;
using System.Text;

[TestClass]
public class ParserUtilsTest {
	private BinaryReader CreateBinaryReader(byte[] bytes) {
		var ms = new MemoryStream();
		ms.Write(bytes, 0, bytes.Length);
		ms.Position = 0;
		return new BinaryReader(ms);
	}

	private byte[] ConstructUint(uint value) {
		var bytes = BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}

		return bytes;
	}

	private byte[] ConstructInt(int value) {
		var bytes = BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}

		return bytes;
	}

	private byte[] ConstructFloat(float value) {
		var bytes = BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}

		return bytes;
	}

	private byte[] ConstructStringASCII(string inStr) {
		if (string.IsNullOrEmpty(inStr)) {
			return ConstructUint(0);
		}

		var lenBytes = ConstructUint((uint)inStr.Length);
		var strBytes = Encoding.ASCII.GetBytes(inStr);
		var outBytes = new byte[4 + strBytes.Length];
		Buffer.BlockCopy(lenBytes, 0, outBytes, 0, 4);
		Buffer.BlockCopy(strBytes, 0, outBytes, 4, strBytes.Length);
		return outBytes;
	}

	private byte[] ConstructStringUTF16(string inStr) {
		if (string.IsNullOrEmpty(inStr)) {
			return ConstructUint(0);
		}

		var utf16LenBytes = ConstructInt(-inStr.Length);
		var utf16Bytes = Encoding.Unicode.GetBytes(inStr);
		var outUtf16 = new byte[4 + utf16Bytes.Length];
		Buffer.BlockCopy(utf16LenBytes, 0, outUtf16, 0, 4);
		Buffer.BlockCopy(utf16Bytes, 0, outUtf16, 4, utf16Bytes.Length);
		return outUtf16;
	}

	[TestMethod]
	public void ReadInt32_BE_Test() {
		void Run(int expected) {
			var bytes = ConstructInt(expected);
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
			var bytes = ConstructUint(expected);
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
			var bytes = ConstructFloat(expected);
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

	[TestMethod]
	public void ReadStringArray_Test() {
		List<byte> bytes = [
			.. ConstructUint(2),
			.. ConstructStringASCII("Hello"),
			.. ConstructStringASCII("World"),
		];

		var reader = CreateBinaryReader(bytes.ToArray());
		var actual = ParserUtils.ReadStringArray(reader);
		Assert.AreEqual(2, actual.Count);
		Assert.AreEqual("Hello", actual[0]);
		Assert.AreEqual("World", actual[1]);
	}
}
