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

			Assert.AreEqual("Church", citySave.GetWorldName());
			Assert.AreEqual("Church_B1", citySave.GetRoomName());

			Assert.AreEqual(861, citySave.GetFlagStrings().Count);
			Assert.Contains("Unlocked_HangAttackTd", citySave.GetFlagStrings());
			Assert.Contains("PickedUp_OWB_Balloons_4", citySave.GetFlagStrings());
		}

		[TestMethod]
		public void ParseArkhamCitySave1() {
			var compressedSave = new CompressedSave("TestData/City/TestSave1.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2025-10-13", citySave.GetDateString());
			Assert.AreEqual("580/852", citySave.GetProgressionString());
			Assert.AreEqual("440/440", citySave.GetRiddlesString());

			Assert.Contains("PDLC_Harley_Balloons_Popped_1", citySave.GetCollectibles());

			Assert.AreEqual("OW", citySave.GetWorldName());
			Assert.AreEqual("OW_C5", citySave.GetRoomName());

			Assert.AreEqual(8105, citySave.GetFlagStrings().Count);
			Assert.Contains("Unlocked_HangAttackTd", citySave.GetFlagStrings());
			Assert.Contains("OW_C2.RFractureWall_7.R_25", citySave.GetFlagStrings());
			Assert.Contains("Steel.RLevelTransitionDoubleDoors_0.InvSpoke0", citySave.GetFlagStrings());
		}
	}
}
