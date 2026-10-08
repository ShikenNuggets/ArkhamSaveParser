using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhamSaveParser.Tests {
	[TestClass]
	public sealed class ArkhamCitySaveTest {
		[TestMethod]
		public void ParseDefaultArkhamCitySave() {
			var compressedSave = new CompressedSave("TestData/City/DefaultSave.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2026-08-20", citySave.GetDateString());
		}

		[TestMethod]
		public void ParseDefaultResaveNoChangesArkhamCitySave() {
			var compressedSave = new CompressedSave("TestData/City/DefaultSave_ResaveNoChanges.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2026-08-20", citySave.GetDateString());
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

			//Assert.AreEqual("Church", citySave.GetWorldName());
			//Assert.AreEqual("Church_B1", citySave.GetRoomName());

			//Assert.AreEqual(861, citySave.GetFlagStrings().Count);
			//Assert.Contains("Unlocked_HangAttackTd", citySave.GetFlagStrings());
			//Assert.Contains("PickedUp_OWB_Balloons_4", citySave.GetFlagStrings());

			//Assert.Contains("GothamFM", citySave.GetRadioFrequencies());
			//Assert.Contains("Puffin-Zero", citySave.GetRadioFrequencies());

			//Assert.AreEqual("Playable_Batman", citySave.GetLastUsedCharacter());
		}

		[TestMethod]
		public void ParseArkhamCitySave1() {
			var compressedSave = new CompressedSave("TestData/City/TestSave1.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual("2025-10-13", citySave.GetDateString());
			Assert.AreEqual("580/852", citySave.GetProgressionString());
			Assert.AreEqual("440/440", citySave.GetRiddlesString());

			Assert.Contains("PDLC_Harley_Balloons_Popped_1", citySave.GetCollectibles());

			//Assert.AreEqual("OW", citySave.GetWorldName());
			//Assert.AreEqual("OW_C5", citySave.GetRoomName());

			//Assert.AreEqual(8105, citySave.GetFlagStrings().Count);
			//Assert.Contains("Unlocked_HangAttackTd", citySave.GetFlagStrings());
			//Assert.Contains("OW_C2.RFractureWall_7.R_25", citySave.GetFlagStrings());
			//Assert.Contains("Steel.RLevelTransitionDoubleDoors_0.InvSpoke0", citySave.GetFlagStrings());

			//Assert.Contains("GothamFM", citySave.GetRadioFrequencies());
			//Assert.Contains("Riddler_Station_04", citySave.GetRadioFrequencies());

			//Assert.AreEqual("Playable_Batman", citySave.GetLastUsedCharacter());
		}

		private static uint Sum(uint[] values) {
			uint total = 0;
			for (var i = 0; i < values.Length; ++i) {
				total += values[i];
			}

			return total;
		}

		[TestMethod]
		public void ParseDefaultSaveHasNoChallengeData() {
			var compressedSave = new CompressedSave("TestData/City/DefaultSave.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual(0, citySave.GetBatChallengeBeaten().Length);
			Assert.AreEqual(0, citySave.GetBeatenBatChallengeCount());
			Assert.IsFalse(citySave.IsBatChallengeBeaten(1));
		}

		[TestMethod]
		public void ParseExperienceCounters0() {
			var compressedSave = new CompressedSave("TestData/City/TestSave0.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

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
		}

		[TestMethod]
		public void ParseExperienceCounters1() {
			var compressedSave = new CompressedSave("TestData/City/TestSave1.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

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
		}

		[TestMethod]
		public void ParseBatChallenges0() {
			var compressedSave = new CompressedSave("TestData/City/TestSave0.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

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
		public void ParseBatChallenges1() {
			var compressedSave = new CompressedSave("TestData/City/TestSave1.sgd");
			var citySave = new ArkhamCitySave(compressedSave);

			Assert.AreEqual(46, citySave.GetBatChallengeBeaten().Length);
			Assert.IsFalse(citySave.IsBatChallengeBeaten(0)); // Slot 0 is never used

			// All 40 base game challenges and all 5 Catwoman challenges
			Assert.AreEqual(45, citySave.GetBeatenBatChallengeCount());
			for (var slot = 1; slot <= 45; ++slot) {
				Assert.IsTrue(citySave.IsBatChallengeBeaten(slot));
			}

			Assert.AreEqual(52.59f, citySave.GetBatChallengeBest()[3], 0.01f);
		}
	}
}
