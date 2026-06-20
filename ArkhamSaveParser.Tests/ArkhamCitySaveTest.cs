using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhamSaveParser.Tests {
	[TestClass]
	public sealed class ArkhamCitySaveTest {
		[TestMethod]
		public void ParseArkhamCitySave0() {
			var compressedSave = new CompressedSave("TestData/City/TestSave0.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2026-06-14", citySave.GetDateString());
			Assert.AreEqual("0/852", citySave.GetProgressionString());
			Assert.AreEqual("50/400", citySave.GetRiddlesString());

			Assert.Contains("PickedUp_OWA_Pickup_31", citySave.GetCollectibles());
			Assert.Contains("PickedUp_OWA_Camera_5", citySave.GetCollectibles());
		}

		[TestMethod]
		public void ParseArkhamCitySave1() {
			var compressedSave = new CompressedSave("TestData/City/TestSave1.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2025-10-13", citySave.GetDateString());
			Assert.AreEqual("580/852", citySave.GetProgressionString());
			Assert.AreEqual("440/440", citySave.GetRiddlesString());

			Assert.Contains("PDLC_Harley_Balloons_Popped_1", citySave.GetCollectibles());
		}
	}
}
