using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhamSaveParser.Tests {
	[TestClass]
	public sealed class DecompressedSaveTest {
		[TestMethod]
		public void DecompressSave_City_TestSave0() {
			var compressedSave = new CompressedSave("TestData/City/TestSave0.sgd");
			var decompressedSave = new DecompressedSave(compressedSave);

			Assert.AreEqual("33625eba87c4d4251ff9d1d765beb45af477e00ff1253b76b7d22029d6b1c6c0", decompressedSave.GetHashString());
		}

		[TestMethod]
		public void DecompressSave_City_TestSave1() {
			var compressedSave = new CompressedSave("TestData/City/TestSave1.sgd");
			var decompressedSave = new DecompressedSave(compressedSave);

			Assert.AreEqual("e4e2a37d34373e75d19fd8ce367eec7b3ab19ec634b0baaf0263aed702517646", decompressedSave.GetHashString());
		}
	}
}
