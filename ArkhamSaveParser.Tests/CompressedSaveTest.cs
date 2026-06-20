using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhamSaveParser.Tests {
	[TestClass]
	public sealed class CompressedSaveTest {
		[TestMethod]
		public void LoadCompressedSave_City_TestSave0()
		{
			var save = new CompressedSave("TestData/City/TestSave0.sgd");
			Assert.AreEqual(3, save.Segments.Count);

			Assert.AreEqual(152, save.Segments[0].compressedSize);
			Assert.AreEqual(357, save.Segments[0].decompressedSize);
			Assert.AreEqual(64, save.Segments[0].offset);

			Assert.AreEqual(1447, save.Segments[1].compressedSize);
			Assert.AreEqual(6352, save.Segments[1].decompressedSize);
			Assert.AreEqual(240, save.Segments[1].offset);

			Assert.AreEqual(11558, save.Segments[2].compressedSize);
			Assert.AreEqual(30791, save.Segments[2].decompressedSize);
			Assert.AreEqual(1711, save.Segments[2].offset);

			Assert.AreEqual(37500, save.TotalDecompressedSize);
		}
	}
}
