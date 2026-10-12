using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhamSaveParser.Tests {
	[TestClass]
	public sealed class ArkhamCitySaveTest {
		private static uint Sum(uint[] values) {
			uint total = 0;
			for (var i = 0; i < values.Length; ++i) {
				total += values[i];
			}

			return total;
		}

		[TestMethod]
		public void ParseDefaultArkhamCitySave() {
			var compressedSave = new CompressedSave("TestData/City/DefaultSave.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2026-08-20", citySave.GetDateString());

			Assert.AreEqual(0, citySave.GetBatChallengeBeaten().Length);
			Assert.AreEqual(0, citySave.GetBeatenBatChallengeCount());
			Assert.IsFalse(citySave.IsBatChallengeBeaten(1));
		}

		[TestMethod]
		public void ParseDefaultResaveNoChangesArkhamCitySave() {
			var compressedSave = new CompressedSave("TestData/City/DefaultSave_ResaveNoChanges.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2026-08-20", citySave.GetDateString());

			Assert.AreEqual(46, citySave.GetBatChallengeBeaten().Length);
			Assert.AreEqual(0, citySave.GetBeatenBatChallengeCount());
			Assert.IsFalse(citySave.IsBatChallengeBeaten(1));
		}

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

			Assert.Contains("GothamFM", citySave.GetRadioFrequencies());
			Assert.Contains("Puffin-Zero", citySave.GetRadioFrequencies());

			Assert.AreEqual("Playable_Batman", citySave.GetLastUsedCharacter());

			Assert.AreEqual(25u, citySave.pickupCount);
			Assert.AreEqual(16u, citySave.numCamerasBroken);
			Assert.AreEqual(2u, citySave.interrogatesPerformed);

			Assert.AreEqual(17205.0f, citySave.xp);
			Assert.AreEqual(17000.0f, citySave.xpForPrevUpgrade);
			Assert.AreEqual(22000.0f, citySave.xpForNextUpgrade);
			Assert.AreEqual(6.0f, citySave.xpLevel);
			Assert.AreEqual(4u, citySave.upgradesBought);
			Assert.AreEqual(6u, citySave.playerLevel);

			// The per-area counters add up to the global ones
			Assert.AreEqual(citySave.pickupCount, Sum(citySave.pickupsCollectedPerArea));
			Assert.AreEqual(citySave.numCamerasBroken, Sum(citySave.camerasBrokenPerArea));

			Assert.AreEqual(46, citySave.GetBatChallengeCurrent().Length);
			Assert.AreEqual(46, citySave.GetBatChallengeBest().Length);
			Assert.AreEqual(46, citySave.GetBatChallengeBeaten().Length);

			Assert.IsFalse(citySave.IsBatChallengeBeaten(0)); // Slot 0 is never used
			Assert.AreEqual(16, citySave.GetBeatenBatChallengeCount());

			// The 50 meter dive challenge has not been beaten yet (best is only about 22.8 meters)
			Assert.IsFalse(citySave.IsBatChallengeBeaten(3));
			Assert.AreEqual(22.84f, citySave.GetBatChallengeBest()[3], 0.01f);
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

			Assert.Contains("GothamFM", citySave.GetRadioFrequencies());
			Assert.Contains("Riddler_Station_04", citySave.GetRadioFrequencies());

			Assert.AreEqual("Playable_Batman", citySave.GetLastUsedCharacter());

			Assert.AreEqual(2, citySave.unknownStrings143.Count);
			Assert.AreEqual("Riddler_OWC_CWPickup_4", citySave.unknownStrings143[0]);
			Assert.AreEqual("SS_Riddler_Hostage6_Saved", citySave.unknownStrings143[1]);
			Assert.AreEqual(0.5f, citySave.unknown150, 0.0001f);
			Assert.AreEqual(8u, citySave.unknown152);

			Assert.AreEqual(282u, citySave.pickupCount);
			Assert.AreEqual(48u, citySave.numCamerasBroken);
			Assert.AreEqual(77u, citySave.interrogatesPerformed);

			Assert.AreEqual(370555.0f, citySave.xp);
			Assert.AreEqual(369500.0f, citySave.xpForPrevUpgrade);
			Assert.AreEqual(377000.0f, citySave.xpForNextUpgrade);

			// 247 base game pickups plus 35 Catwoman pickups
			Assert.AreEqual(247u, Sum(citySave.pickupsCollectedPerArea));
			Assert.AreEqual(35u, Sum(citySave.catwomanPickupsCollectedPerArea));
			Assert.AreEqual(citySave.pickupCount, Sum(citySave.pickupsCollectedPerArea) + Sum(citySave.catwomanPickupsCollectedPerArea));
			Assert.AreEqual(48u, Sum(citySave.camerasBrokenPerArea));
			Assert.AreEqual(24u, Sum(citySave.balloonsCollectedPerArea));

			Assert.AreEqual(46, citySave.GetBatChallengeBeaten().Length);
			Assert.IsFalse(citySave.IsBatChallengeBeaten(0)); // Slot 0 is never used

			// All 40 base game challenges and all 5 Catwoman challenges
			Assert.AreEqual(45, citySave.GetBeatenBatChallengeCount());
			for (var slot = 1; slot <= 45; ++slot) {
				Assert.IsTrue(citySave.IsBatChallengeBeaten(slot));
			}

			Assert.AreEqual(52.59f, citySave.GetBatChallengeBest()[3], 0.01f);
		}

		[TestMethod]
		public void ParseArkhamCityEarlyGameCourthouseSave() {
			// Saved right after the Courthouse, very early in the game. The nine values before the
			// "-1, -1" marker in segment 3 are laid out differently than in the later test saves.
			var compressedSave = new CompressedSave("TestData/City/CourthouseSave.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2025-12-07", citySave.GetDateString());
			Assert.AreEqual("0/852", citySave.GetProgressionString());
			Assert.AreEqual("5/400", citySave.GetRiddlesString());
			Assert.AreEqual("Court", citySave.GetWorldName());
			Assert.AreEqual("Court_B1", citySave.GetRoomName());
			Assert.AreEqual("Playable_Batman", citySave.GetLastUsedCharacter());

			Assert.AreEqual(9, citySave.unknownUints160.Length);
			Assert.AreEqual(1u, citySave.unknownUints160[0]);
			Assert.AreEqual(11u, citySave.unknownUints160[2]);
			Assert.AreEqual(9u, citySave.unknownUints160[3]);

			Assert.AreEqual(1u, citySave.pickupCount);
			Assert.AreEqual(1840.0f, citySave.xp);
			Assert.AreEqual(2000.0f, citySave.xpForNextUpgrade);

			// Four physical challenges are beaten so far
			Assert.AreEqual(4, citySave.GetBeatenBatChallengeCount());
			Assert.IsTrue(citySave.GetPhysicalChallengeInfo(PhysicalChallenges.QuickfireGadgets3).beaten);
			Assert.IsTrue(citySave.GetPhysicalChallengeInfo(PhysicalChallenges.Variation5).beaten);
			Assert.IsTrue(citySave.GetPhysicalChallengeInfo(PhysicalChallenges.AerialAttacks).beaten);
			Assert.IsTrue(citySave.GetPhysicalChallengeInfo(PhysicalChallenges.QuickfireGels).beaten);

			var diveChallengeInfo = citySave.GetPhysicalChallengeInfo(PhysicalChallenges.Dive50);
			Assert.IsFalse(diveChallengeInfo.beaten);
			Assert.AreEqual(2.97f, diveChallengeInfo.bestValue, 0.01f);
		}

		[TestMethod]
		public void ParseArkhamCityEnterChurchSave() {
			// Field 92 used to be read as a padding uint32. It is the count of a string array, and it is not empty here.
			var compressedSave = new CompressedSave("TestData/City/EnterChurchSave.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("OW", citySave.GetWorldName());
			Assert.AreEqual("OW_A2", citySave.GetRoomName());
			Assert.AreEqual("6/400", citySave.GetRiddlesString());
			Assert.AreEqual(1, citySave.unknownStrings92.Count);
			Assert.AreEqual("Frontend.Objectives.CH2_Sub_2_Trail1", citySave.unknownStrings92[0]);
			Assert.AreEqual(46, citySave.GetBatChallengeBeaten().Length); // The challenge arrays come before these fields
			Assert.AreEqual(99, citySave.unknownBytes143.Length);
			Assert.AreEqual(1, citySave.unknownStrings143.Count);
			Assert.AreEqual("OW_CH2_Bullet", citySave.unknownStrings143[0]);
			Assert.AreEqual(14, citySave.unknown151); // Not always -1
			Assert.AreEqual(0u, citySave.unknown152);
			Assert.AreEqual(8u, citySave.unknown153);

			Assert.AreEqual(0, citySave.additionalSegment3s.Count);
			Assert.AreEqual(89, citySave.footerHeader.Length);
			Assert.AreEqual(6, citySave.footerIntLists.Length);
			foreach (var list in citySave.footerIntLists) {
				Assert.AreEqual(16, list.Length);
			}

			Assert.AreEqual(333, citySave.footerTail.Length);
			Assert.AreEqual(830, citySave.GetFooterBytes().Length);
		}

		[TestMethod]
		public void ParseArkhamCitySteelMillSave() {
			// Fields 85 and 87 used to be read as padding. They are a string array and a string, both empty early on.
			var compressedSave = new CompressedSave("TestData/City/SteelMillSave.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("Steel", citySave.GetWorldName());
			Assert.AreEqual("Steel_C1", citySave.GetRoomName());
			Assert.AreEqual("24/400", citySave.GetRiddlesString());
			Assert.AreEqual(4, citySave.unknownStrings85.Count);
			Assert.AreEqual("Steel_B1", citySave.unknownStrings85[0]);
			Assert.AreEqual("Steel_B1", citySave.unknownString87);
			Assert.AreEqual(1, citySave.unknownStrings92.Count);
			Assert.AreEqual("Frontend.Objectives.CH2_Sub_5_Steel", citySave.unknownStrings92[0]);
			Assert.AreEqual(85, citySave.unknownBytes143.Length);
			Assert.AreEqual(0, citySave.unknownStrings143.Count);
			Assert.AreEqual(-1, citySave.unknown151);
			Assert.AreEqual(2u, citySave.unknown152);
		}

		[TestMethod]
		public void ParseArkhamCityNewGamePlusSave() {
			var citySave = new ArkhamCitySave(new CompressedSave("TestData/City/NewGamePlusStartSave.sgd"));

			Assert.AreEqual("OW_A5", citySave.GetRoomName());
			Assert.AreEqual(1, citySave.additionalSegment3s.Count);
			Assert.AreEqual("PDLCIN", citySave.additionalSegment3s[0].GetWorldName());
			Assert.AreEqual("PDLCIN_B3", citySave.additionalSegment3s[0].GetRoomName());
			Assert.AreEqual(0, citySave.additionalSegment3s[0].additionalSegment3s.Count);
			Assert.AreEqual(830, citySave.additionalSegment3s[0].GetFooterBytes().Length);
		}

		[TestMethod]
		public void ParseArkhamCityNewGamePlusWithCatwomanSave() {
			var citySave = new ArkhamCitySave(new CompressedSave("TestData/City/NewGamePlusCatwomanSave.sgd"));

			Assert.AreEqual("OW_A5", citySave.GetRoomName());
			Assert.AreEqual(1, citySave.additionalSegment3s.Count);

			var second = citySave.additionalSegment3s[0];
			Assert.AreEqual("OW_E4", second.GetRoomName());
			Assert.AreEqual(1, second.additionalSegment3s.Count);
			Assert.AreEqual("PDLCIN_B3", second.additionalSegment3s[0].GetRoomName());
		}
	}
}
