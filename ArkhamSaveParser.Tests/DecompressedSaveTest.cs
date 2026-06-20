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
	}
}
