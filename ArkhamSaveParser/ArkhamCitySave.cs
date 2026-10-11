using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace ArkhamSaveParser {
	public class ArkhamCitySave : DecompressedSave {
		public ArkhamCitySave(CompressedSave save) : base(save) {
			ParseSave();
		}

		/// <summary>
		/// When tolerant is true a parsing error is not thrown. Instead it is stored in parseError (with the
		/// byte offset in parseErrorPosition) and every field that was read before the error keeps its value.
		/// </summary>
		public ArkhamCitySave(CompressedSave save, bool tolerant) : base(save) {
			tolerantTail = tolerant;
			if (!tolerant) {
				ParseSave();
				return;
			}

			try {
				ParseSave();
			} catch (Exception ex) {
				parseError = ex.Message;
				parseErrorPosition = parseReader != null ? parseReader.BaseStream.Position : -1;
			}
		}

		private ArkhamCitySave(byte[] raw, long position, bool tolerant) : base(raw) {
			tolerantTail = tolerant;
			var reader = new BinaryReader(new MemoryStream(rawData));
			reader.BaseStream.Position = position;
			parseReader = reader;
			segment3Start = position;
			if (!tolerant) {
				ParseSegment3(reader);
				return;
			}

			try {
				ParseSegment3(reader);
			} catch (Exception ex) {
				parseError = ex.Message;
				parseErrorPosition = reader.BaseStream.Position;
			}
		}

		public long segment3Start = -1; // Offset of this copy of segment 3 in the decompressed data (-1 for the first one: see worldStrings)
		public string? parseError; // Only set by the tolerant constructor
		public string? tailError; // Only set by the tolerant constructor: the last block of segment 3 was not understood
		public long tailErrorPosition = -1;
		private bool tolerantTail = false;
		public long parseErrorPosition = -1; // Offset in the decompressed data where the error happened
		private BinaryReader? parseReader;

		public string GetDateString() {
			return year.ToString("D4") + "-" + month.ToString("D2") + "-" + day.ToString("D2");
		}

		public string GetProgressionString() {
			return progression;
		}

		public string GetRiddlesString() {
			return riddles;
		}

		public List<string> GetCollectibles() {
			return collectibles;
		}

		public List<string> GetUpgrades() {
			return upgrades;
		}

		public string GetWorldName() {
			return worldName;
		}

		public string GetRoomName() {
			return roomName;
		}

		public List<string> GetFlagStrings() {
			return flagStrings;
		}

		public List<string> GetRadioFrequencies() {
			return radioFrequencies;
		}

		public string GetLastUsedCharacter() {
			return lastUsedCharacter;
		}

		public float[] GetBatChallengeCurrent() {
			return batChallengeCurrent;
		}

		public float[] GetBatChallengeBest() {
			return batChallengeBest;
		}

		public uint[] GetBatChallengeBeaten() {
			return batChallengeBeaten;
		}

        /// <summary>slot 0 always holds the value 0, 1-40 are the base game challenges, 41-45 are the Catwoman DLC challenges.</summary>
        public bool IsBatChallengeBeaten(int slot) {
			if (slot < 0 || slot >= batChallengeBeaten.Length) {
				return false;
			}

			return batChallengeBeaten[slot] != 0;
		}

		public int GetBeatenBatChallengeCount() {
			var count = 0;
			for (var i = 0; i < batChallengeBeaten.Length; ++i) {
				if (batChallengeBeaten[i] != 0) {
					count++;
				}
			}

			return count;
		}

		private uint day = 0;
		private uint month = 0;
		private uint year = 0;

		private string progression = string.Empty;
		private string riddles = string.Empty;

		private List<string> collectibles = [];
		private List<string> upgrades = [];

		private string worldName = string.Empty;
		private string roomName = string.Empty;
		private List<string> flagStrings = [];

		private List<string> radioFrequencies = [];

		private string lastUsedCharacter = string.Empty;
		public uint constant1;
		public uint constant2;

		public uint difficulty; // 0 = Easy, 1 = Normal, 2 = Hard

		public uint subtitles; // 0 = off, 1 = on
		public uint brightness; // 0 to 100
		public uint sfxVolume; // 0 to 100
		public uint musicVolume; // 0 to 100
		public uint dialogueVolume; // 0 to 100

		public uint hints; // 0 = off, 1 = on

		public float totalCompletionWithDLC;

		public List<string> progressStrings = [];

		public string lastSelectedRiddlersRevengeCharacter = string.Empty;

		public uint segment2HeaderA;
		public uint segment2HeaderB;

		public List<string> worldStrings = [];

		public string lastRoomNameUserFacing = string.Empty;
		public string currentObjectiveTag = string.Empty;
		public string lastEquippedGadget = string.Empty;

		// Unknown Values
		public uint unknown1;
		public uint unknown5;
		public uint unknown6;
		public uint unknown7;
		public uint unknown9;

		public uint[] unknownSettings0 = [];

		public uint unknown10;
		public uint unknown11;
		public uint unknown12;

		public List<byte[]> unknownStructs = [];
		public byte[] unknownBytes1 = [];
		public byte[] unknownBytes2 = [];
		public uint unknown13;
		public uint unknown14;
		public uint unknown15;
		public uint unknown16;
		public uint unknown17;
		public uint unknown18;

		public uint[] unknownUints1 = [];

		public uint[] unknownUints2 = [];

		public uint unknown20;
		public uint unknown21;
		
		public uint unknown22;
		public byte[] unknownBytes22 = [];


		public uint unknown23;
		public byte[] unknownBytes23 = [];

		public uint unknown24;
		public byte[] unknownBytes24 = [];

		public uint unknown25;
		public byte[] unknownBytes25 = [];

		public uint unknown26;
		public byte[] unknownBytes26 = [];

		public uint unknown27;
		public byte[] unknownBytes27 = [];

		public uint unknown28;
		public byte[] unknownBytes28 = [];

		public uint unknown29;
		public byte[] unknownBytes29 = [];

		public uint unknown30;
		public byte[] unknownBytes30 = [];

		public uint unknown31;
		public byte[] unknownBytes31 = [];

		public uint unknown32;
		public byte[] unknownBytes32 = [];

		public byte[] unknownBytes33 = [];

		public float[] unknownFloats1 = [];

		public float[] unknownFloats2 = [];

		public uint unknown33;
		public uint unknown34;
		public uint unknown56;
		public uint unknown57;
		public uint unknown58;
		public uint unknown78;
		public uint unknown79;
		public uint unknown80;
		public uint unknown81;
		public uint unknown82;
		public uint unknown83;
		public uint unknown84;
		public List<string> unknownStrings85 = []; // Second list of level names, empty early in the game (OW_B1, Steel_B1, ...)

		// ---- Segment 2: per-area collection data (area order: unknown) ----
		public byte[] unknownBytes35 = []; // 9 bytes, 0-31, looks like a per-area bit mask
		public uint[] unknownUints36 = []; // 9 entries
		public uint[] unknownUints37 = []; // 9 entries
		public uint[] unknownUints38 = []; // 60 entries (5 blocks of 12), 0/1/2 in later saves
		public uint[] unknownUints39 = [];
		public uint[] unknownUints40 = [];
		public uint[] unknownUints41 = [];
		public uint[] unknownUints42 = [];
		public uint[] unknownUints43 = [];
		public uint[] unknownUints44 = [];
		public uint[] unknownUints45 = [];
		public uint[] unknownUints46 = [];
		public uint[] pickupsCollectedPerArea = [];         // sums to pickupCount (minus Catwoman pickups)
		public uint[] catwomanPickupsCollectedPerArea = []; // sums to 35 in a fully collected Catwoman save
		public uint[] teethCollectedPerArea = [];           // unsure which slots they are but presumed to be the ones associated with the subway and sewers
		public uint[] unknownUints50 = [];                  // 11 entries, grows with progress, meaning unknown
		public uint[] unknownUints51 = [];                  // 9 entries, all 0 in every save seen so far
		public uint[] balloonsCollectedPerArea = [];        // only industrial and amusment
		public uint[] penguinsCollectedPerArea = [];        // only the Museum slot is used
		public uint[] camerasBrokenPerArea = [];            // sums to numCamerasBroken
		public uint[] riddlesSolvedPerArea = [];
		// ---- Segment 2: possibly experience / statistics (matches some of the format of the Rshared/persistentdata class in game)----
		public uint unknown59; // two members before BrokenGrundyHeadTally, 12 in later saves
		public uint unknown60;
		public uint brokenGrundyHeadTally;
		public uint pickupCount; // Riddler pickups collected (247 base game, 282 with the 35 Catwoman pickups)
		public uint pickupRevealsLeft;
		public uint interrogatesLeft;
		public uint numCamerasBroken;
		public uint interrogatesPerformed;
		public uint interrogatesWithTutorialPerformed;
		// Physical challenges. 46 slots: slot 0 always holds the value 0, 1-40 are the base game challenges, 41-45 are the Catwoman DLC challenges.
		public float[] batChallengeCurrent = []; // latest attempt / running value
		public float[] batChallengeBest = [];    // best value (distance, count, ...); stays 0 for challenges that are simply booleans.
		public uint[] batChallengeBeaten = [];   // 1 once the challenge is beaten, 0 otherwise

		public float xp;
		public float xpForPrevUpgrade;
		public float xpForNextUpgrade;
		public float xpLevel;
		public uint xpOverUpgradeThreshold;
		public uint upgradesBought;
		public uint cwUpgradesBought;
		public uint playerLevel;
		public uint unknown76; // 1 in every save seen so far
		public uint unknown77; // 1 in every save seen so far

		public List<string> weaponStrings = [];

		public string unknownString87 = ""; // Empty early in the game, a level name later (e.g. Steel_B1)
		public List<string> unknownStrings88 = [];
		public List<string> unknownStrings89 = [];
		public byte[] unknownBytes90 = [];
		public byte[] unknownBytes91 = [];
		public List<string> unknownStrings92 = []; // Active objectives, empty until the first objective is active (e.g. Frontend.Objectives.CH2_Sub_2_Trail1)
		public uint unknown93;
		public List<string> unknownStrings94 = [];
		public string unknownString95 = string.Empty;
		public uint unknown96;
		public float unknown97;
		public uint unknown98;
		public uint unknown99;
		public uint unknown100;
		public uint unknown101;
		public string unknownString102 = string.Empty;
		public byte[] unknownBytes103 = [];
		public uint unknown104;
		public uint unknown105;
		public uint unknown106;
		public List<string> unknownStrings107 = [];
		public byte[] unknownBytes108 = [];
		public string unknownString109 = string.Empty;
		public byte[] unknownBytes110 = [];
		public uint unknown111;
		public byte[] unknownBytes112 = [];
		public uint unknown113;
		public byte[] unknownBytes114 = [];
		public uint unknown115;
		public byte[] unknownFlagBytes116 = [];
		public uint unknown117;
		public byte[] unknownFlagBytes118 = [];
		public uint[] unknownInts119 = [];
		public float[] unknownFloats120 = [];
		public float[] unknownFloats121 = [];
		public List<string> unknownStrings122 = [];
		public List<string> unknownStrings123 = [];
		public List<string> unknownStrings124 = [];
		public uint unknown125;
		public float unknown126;
		public float unknown127;
		public float unknown128;
		public List<string> unknownStrings129 = [];
		public List<string> unknownStrings130 = [];
		public List<string> unknownStrings131 = [];
		public List<string> unknownStrings132 = [];
		public List<string> unknownStrings133 = [];
		public List<string> unknownStrings134 = [];
		public uint[] unknownInts135 = [];
		public uint[] unknownInts136 = [];
		public uint[] unknownInts137 = [];
		public uint[] unknownInts138 = [];
		public uint[] unknownInts139 = [];
		public uint[] unknownInts140 = [];
		public uint unknown141;
		public uint unknown142;
		public byte[] unknownBytes143 = []; // Everything between unknownInts140 and unknown150. Its length varies (85 to 132 bytes), see ParseTail
		public List<string> unknownStrings143 = []; // Strings found inside unknownBytes143 (SS_* side story flags, OW_* main mission names, ...)
		public float unknown150;
		public int unknown151; // -1 in most saves, but 14, 15 or 0 in some early game saves
		public uint unknown152;
		public uint unknown153;
		public byte[] unknownBitFlags154 = [];
		public uint unknown155;
		public byte[] unknownBitFlags156 = [];
		public uint unknown157;
		public byte[] unknownBitFlags158 = [];
		public List<string> unknownStrings159A = []; // 159A-D are four lists with the same length (0, 1 or 2 so far)
		public List<string> unknownStrings159B = [];
		public int[] unknownInts159C = [];
		public int[] unknownInts159D = [];
		public byte[] unknownBytes159 = []; // 17 bytes, always 0 so far
		public uint[] unknownUints160 = []; // 9 values, see the comment where they are read
		public float[] unknownFloats162 = [];
		public float[] unknownFloats163 = [];
		public int unknown164;
		public int unknown165;
		public uint unknown166;
		public byte unknownByte167;
		public float[] unknownFloats168 = [];
		public float[] unknownFloats169 = [];

		// The fixed size (830 byte) block that ends every copy of segment 3. unknownFloats168/169 used to be followed by a
		// byte array here, but the block does not depend on their length. Layout: 89 bytes, six lists of 16 ints, 333 bytes.
		public byte[] footerHeader = [];
		public int[][] footerIntLists = [];
		public byte[] footerTail = [];

		// New Game+ and New Game Minus saves (and some others) contain more than one copy of segment 3, one after the other.
		// Each copy is parsed with the same code, so this holds the second copy (which in turn holds the third).
		public List<ArkhamCitySave> additionalSegment3s = [];

		private void ParseSave() {
			var reader = new BinaryReader(new MemoryStream(rawData));
			parseReader = reader;

			constant1 = ParserUtils.ReadUint32_BE(reader);
			if (constant1 != 16) {
				throw new ArgumentException("The loaded file was not an Arkham City save, expected " + 16 + " but found " + constant1);
			}

			constant2 = ParserUtils.ReadUint32_BE(reader);
			if (constant2 != 12) {
				throw new ArgumentException("The loaded file was not an Arkham City save, expected " + 12 + " but found " + constant2);
			}

			difficulty = ParserUtils.ReadUint32_BE(reader);
			unknown1 = ParserUtils.ReadUint32_BE(reader);
			subtitles = ParserUtils.ReadUint32_BE(reader);
			brightness = ParserUtils.ReadUint32_BE(reader);
			sfxVolume = ParserUtils.ReadUint32_BE(reader);
			dialogueVolume = ParserUtils.ReadUint32_BE(reader);
			musicVolume = ParserUtils.ReadUint32_BE(reader);

			unknown7 = ParserUtils.ReadUint32_BE(reader); // Options?
			hints = ParserUtils.ReadUint32_BE(reader); // Subtitles? Inverted look?
			unknown9 = ParserUtils.ReadUint32_BE(reader); // Another slider?

			unknownSettings0 = new uint[22];
			for (var i = 0; i < 22; ++i) {
				unknownSettings0[i] = ParserUtils.ReadUint32_BE(reader); // More settings or flags or something
			}

			day = ParserUtils.ReadUint32_BE(reader);
			month = ParserUtils.ReadUint32_BE(reader);
			year = ParserUtils.ReadUint32_BE(reader);

			unknown10 = ParserUtils.ReadUint32_BE(reader); // 0 on fresh file, 5 after starting

			// Most likely just padding
			unknown11 = ParserUtils.ReadUint32_BE(reader);
			unknown12 = ParserUtils.ReadUint32_BE(reader);

			var structCount = ParserUtils.ReadUint32_BE(reader);
			unknownStructs = new List<byte[]>();
			for (var i = 0; i < structCount; ++i) {
				unknownStructs.Add(ParseUnknownStruct(reader)); // I think this is related to challenge maps
			}

			unknownBytes1 = reader.ReadBytes(32); // Exit block

			var byteArraySize = ParserUtils.ReadUint32_BE(reader);
			unknownBytes2 = reader.ReadBytes((int)byteArraySize);

			unknown13 = ParserUtils.ReadUint32_BE(reader); // Probably just padding
			unknown14 = ParserUtils.ReadUint32_BE(reader);

			progressStrings = ParserUtils.ReadStringArray(reader);
			progression = progressStrings[0];
			riddles = progressStrings[1];

			unknown16 = reader.ReadByte(); // Map grid?
			unknown17 = reader.ReadByte();

			unknown18 = ParserUtils.ReadUint32_BE(reader); // Padding?

			totalCompletionWithDLC = ParserUtils.ReadFloat_BE(reader); // Total progress?

			// Possibly challenge map related, 2 and 4 added after doing a DLC combat matp
			unknownUints1 = new uint[10];
			for (var i = 0; i < 10; ++i) {
				unknownUints1[i] = ParserUtils.ReadUint32_BE(reader);
			}
			
			lastSelectedRiddlersRevengeCharacter = ParserUtils.ReadUE3String(reader); // Character, or something

			unknownUints2 = new uint[6];
			for (var i = 0; i < 6; ++i) {
				unknownUints2[i] = ParserUtils.ReadUint32_BE(reader); // Some flags, maybe some more padding. 257 added after starting story
			}

			// ------------------------------- //
			// ---------- Segment 2 ---------- //
			// ------------------------------- //

			segment2HeaderA = ParserUtils.ReadUint32_BE(reader);
			segment2HeaderB = ParserUtils.ReadUint32_BE(reader);

			collectibles = ParserUtils.ReadStringArray(reader);
			upgrades = ParserUtils.ReadStringArray(reader);

			// Probably just padding/end of string arrays marker
			unknown20 = ParserUtils.ReadUint32_BE(reader); // 1
			unknown21 = ParserUtils.ReadUint32_BE(reader); // 0

			unknown22 = ParserUtils.ReadUint32_BE(reader); // 5
			unknownBytes22 = reader.ReadBytes((int)unknown22); // Byte flags? 1 added after starting story

			unknown23 = ParserUtils.ReadUint32_BE(reader); // 5
			unknownBytes23 = reader.ReadBytes((int)unknown23); // Byte flags? 1 added after starting story

			unknown24 = ParserUtils.ReadUint32_BE(reader); // 0x20 (32)
			unknownBytes24 = reader.ReadBytes((int)unknown24); // Byte flags?

			unknown25 = ParserUtils.ReadUint32_BE(reader); // 0x20 (32)
			unknownBytes25 = reader.ReadBytes((int)unknown25); // Byte flags?

			unknown26 = ParserUtils.ReadUint32_BE(reader); // 0x20 (32)
			unknownBytes26 = reader.ReadBytes((int)unknown26); // Byte flags?

			unknown27 = ParserUtils.ReadUint32_BE(reader); // 0x5B (91)
			unknownBytes27 = reader.ReadBytes((int)unknown27); // Byte flags?

			unknown28 = ParserUtils.ReadUint32_BE(reader); // 0x5B (91)
			unknownBytes28 = reader.ReadBytes((int)unknown28); // Byte flags?

			unknown29 = ParserUtils.ReadUint32_BE(reader); // 0x51 (81)
			unknownBytes29 = reader.ReadBytes((int)unknown29); // Byte flags?

			unknown30 = ParserUtils.ReadUint32_BE(reader); // 0x51 (81)
			unknownBytes30 = reader.ReadBytes((int)unknown30); // Byte flags?

			unknown31 = ParserUtils.ReadUint32_BE(reader); // 0x52 (82)
			unknownBytes31 = reader.ReadBytes((int)unknown31); // Byte flags?

			unknown32 = ParserUtils.ReadUint32_BE(reader); // 0x3F (63)
			unknownBytes32 = reader.ReadBytes((int)unknown32); // Byte flags?

			unknownBytes33 = reader.ReadBytes(201); // Unclear, it's not padding or bit flags because there's 0s, 1s, and 2s

			var numFloats1 = ParserUtils.ReadUint32_BE(reader); // 0x33 (51)
			unknownFloats1 = new float[numFloats1];
			for (var i = 0; i < numFloats1; ++i) {
				unknownFloats1[i] = ParserUtils.ReadFloat_BE(reader); // Some of these are definitely riddle count/progression related
			}

			var numFloats2 = ParserUtils.ReadUint32_BE(reader); // 0x33 (51)
			unknownFloats2 = new float[numFloats2];
			for (var i = 0; i < numFloats2; ++i) {
				unknownFloats2[i] = ParserUtils.ReadFloat_BE(reader);
			}

			// These are both 3212836864 on a brand new file, maybe an EOF marker or something?
			unknown33 = ParserUtils.ReadUint32_BE(reader); // Probably padding
			unknown34 = ParserUtils.ReadUint32_BE(reader); // Probably padding

			if (unknown33 == 3212836864 && unknown34 == 3212836864) {
				// EOF? Not safe to keep parsing
				return;
			}

			unknownBytes35 = ParserUtils.ReadByteArray(reader);
			unknownUints36 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints37 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints38 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints39 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints40 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints41 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints42 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints43 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints44 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints45 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints46 = ParserUtils.ReadUint32Array_BE(reader);
			pickupsCollectedPerArea = ParserUtils.ReadUint32Array_BE(reader);
			catwomanPickupsCollectedPerArea = ParserUtils.ReadUint32Array_BE(reader);
			teethCollectedPerArea = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints50 = ParserUtils.ReadUint32Array_BE(reader);
			unknownUints51 = ParserUtils.ReadUint32Array_BE(reader);
			balloonsCollectedPerArea = ParserUtils.ReadUint32Array_BE(reader);
			penguinsCollectedPerArea = ParserUtils.ReadUint32Array_BE(reader);
			camerasBrokenPerArea = ParserUtils.ReadUint32Array_BE(reader);
			riddlesSolvedPerArea = ParserUtils.ReadUint32Array_BE(reader);

			unknown59 = ParserUtils.ReadUint32_BE(reader);
			unknown60 = ParserUtils.ReadUint32_BE(reader);
			brokenGrundyHeadTally = ParserUtils.ReadUint32_BE(reader);
			pickupCount = ParserUtils.ReadUint32_BE(reader);
			pickupRevealsLeft = ParserUtils.ReadUint32_BE(reader);
			interrogatesLeft = ParserUtils.ReadUint32_BE(reader);
			numCamerasBroken = ParserUtils.ReadUint32_BE(reader);
			interrogatesPerformed = ParserUtils.ReadUint32_BE(reader);
			interrogatesWithTutorialPerformed = ParserUtils.ReadUint32_BE(reader);

			batChallengeCurrent = ParserUtils.ReadFloatArray_BE(reader); // 46 entries
			batChallengeBest = ParserUtils.ReadFloatArray_BE(reader); // 46 entries
			batChallengeBeaten = ParserUtils.ReadUint32Array_BE(reader); // 46 entries, 4-byte integers (0 or 1), NOT floats

			xp = ParserUtils.ReadFloat_BE(reader);
			xpForPrevUpgrade = ParserUtils.ReadFloat_BE(reader);
			xpForNextUpgrade = ParserUtils.ReadFloat_BE(reader);
			xpLevel = ParserUtils.ReadFloat_BE(reader);

			xpOverUpgradeThreshold = ParserUtils.ReadUint32_BE(reader);
			upgradesBought = ParserUtils.ReadUint32_BE(reader);
			cwUpgradesBought = ParserUtils.ReadUint32_BE(reader);
			playerLevel = ParserUtils.ReadUint32_BE(reader);
			unknown76 = ParserUtils.ReadUint32_BE(reader);
			unknown77 = ParserUtils.ReadUint32_BE(reader);

			var bytesRemaining = reader.BaseStream.Length - reader.BaseStream.Position;
			if (bytesRemaining == 0) {
				return; // This file has no section 3
			}

			ParseSegment3(reader);
		}

		private void ParseSegment3(BinaryReader reader) {
			// ------------------------------- //
			// ---------- Segment 3 ---------- //
			// ------------------------------- //

			var segment3Header = ParserUtils.ReadUint32_BE(reader);
			if (segment3Header != 0x81) {
				throw new ArgumentException("The file was parsed incorrectly, expected " + 0x81 + " at section 3 start, got " + segment3Header);
			}

			unknown78 = ParserUtils.ReadUint32_BE(reader);
			unknown79 = ParserUtils.ReadUint32_BE(reader);
			unknown80 = ParserUtils.ReadUint32_BE(reader);
			unknown81 = ParserUtils.ReadUint32_BE(reader);
			unknown82 = ParserUtils.ReadUint32_BE(reader);
			unknown83 = ParserUtils.ReadUint32_BE(reader);
			unknown84 = ParserUtils.ReadUint32_BE(reader); // This one's probably just padding

			worldStrings = ParserUtils.ReadStringArray(reader); // Includes world name/room name, which chapters/LODs should be loaded, etc

			// This used to be read as a uint32 "padding" value. It is the count of a string array, and the
			// count is 0 in early saves, which is why it looked like padding.
			unknownStrings85 = ParserUtils.ReadStringArray(reader);
			weaponStrings = ParserUtils.ReadStringArray(reader); // e.g. StreamedWeapon_CombatRifle, StreamedWeapon_CombatBat

			worldName = ParserUtils.ReadUE3String(reader);
			roomName = ParserUtils.ReadUE3String(reader);

			// Same as above: the 0 was the length of an empty string
			unknownString87 = ParserUtils.ReadUE3String(reader);

			unknownStrings88 = ParserUtils.ReadStringArray(reader); // OW flags?
			unknownStrings89 = ParserUtils.ReadStringArray(reader);

			flagStrings = ParserUtils.ReadStringArray(reader); // VERY large array, there are thousands of these

			unknownBytes90 = reader.ReadBytes(10); // Not entirely sure what to make of this

			lastRoomNameUserFacing = ParserUtils.ReadUE3String(reader);

			unknownBytes91 = reader.ReadBytes(2); // Padding?

			currentObjectiveTag = ParserUtils.ReadUE3String(reader);

			// Same as above: the 0 was the count of an empty string array
			unknownStrings92 = ParserUtils.ReadStringArray(reader);
			unknownStrings94 = ParserUtils.ReadStringArray(reader); // Something objective related?
			unknownString95 = ParserUtils.ReadUE3String(reader); // Something side mission related?

			unknown96 = ParserUtils.ReadUint32_BE(reader); // 64
			unknown97 = ParserUtils.ReadFloat_BE(reader); // 2032.43981933594
			unknown98 = ParserUtils.ReadUint32_BE(reader); // 2
			unknown99 = ParserUtils.ReadUint32_BE(reader); // Padding? 0
			unknown100 = ParserUtils.ReadUint32_BE(reader); // 4
			unknown101 = ParserUtils.ReadUint32_BE(reader); // uint32 max

			lastEquippedGadget = ParserUtils.ReadUE3String(reader);
			unknownString102 = ParserUtils.ReadUE3String(reader); // Last world, maybe?

			unknownBytes103 = reader.ReadBytes(85); // Not sure how to read these, there's a stray byte in here somewhere

			unknownStrings107 = ParserUtils.ReadStringArray(reader); // Objective keys?
			unknownBytes108 = reader.ReadBytes(21); // Not sure how to read these, there's a stray byte somewhere in here
			unknownString109 = ParserUtils.ReadUE3String(reader); // Last room name?
			unknownBytes110 = reader.ReadBytes(12); // Padding?
			unknown111 = ParserUtils.ReadUint32_BE(reader); // 14
			unknownBytes112 = reader.ReadBytes(20); // Padding?
			unknown113 = ParserUtils.ReadUint32_BE(reader); // 0 on TestSave0, 5 on TestSave1 
			unknownBytes114 = reader.ReadBytes(56); // Padding?
			radioFrequencies = ParserUtils.ReadStringArray(reader);

			var numFlags115 = ParserUtils.ReadUint32_BE(reader); // 5 on TestSave0, 12 on TestSave1
			unknownFlagBytes116 = reader.ReadBytes((int)numFlags115);

			var numFlags117 = ParserUtils.ReadUint32_BE(reader); // 5 on TestSave0, 12 on TestSave1
			unknownFlagBytes118 = reader.ReadBytes((int)numFlags117);

			var numInts119 = ParserUtils.ReadUint32_BE(reader); // 5 on TestSave0, 12 on TestSave1
			unknownInts119 = new uint[numInts119];
			for (int i = 0; i < numInts119; ++i) {
				unknownInts119[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numFloats120 = ParserUtils.ReadUint32_BE(reader); // 5 on TestSave0, 12 on TestSave1
			unknownFloats120 = new float[numFloats120];
			for (int i = 0; i < numFloats120; ++i) {
				unknownFloats120[i] = ParserUtils.ReadFloat_BE(reader);
			}

			var numFloats121 = ParserUtils.ReadUint32_BE(reader); // 5 on TestSave0, 12 on TestSave1
			unknownFloats121 = new float[numFloats121];
			for (int i = 0; i < numFloats121; ++i) {
				unknownFloats121[i] = ParserUtils.ReadFloat_BE(reader);
			}

			// These all have something to do with the radio frequencies but no idea what they mean
			unknownStrings122 = ParserUtils.ReadStringArray(reader); // FM, Terrestrial, Shortwave, Microwave
			unknownStrings123 = ParserUtils.ReadStringArray(reader); // None, PGP, RSA
			unknownStrings124 = ParserUtils.ReadStringArray(reader); // Medium, High?

			unknown125 = reader.ReadByte(); // Seems to be a single padding byte

			unknown126 = ParserUtils.ReadFloat_BE(reader);
			unknown127 = ParserUtils.ReadFloat_BE(reader);
			unknown128 = ParserUtils.ReadFloat_BE(reader);

			lastUsedCharacter = ParserUtils.ReadUE3String(reader);

			unknownStrings129 = ParserUtils.ReadStringArray(reader); // Something else radio station related
			unknownStrings130 = ParserUtils.ReadStringArray(reader); // Something else radio station related
			unknownStrings131 = ParserUtils.ReadStringArray(reader); // Same entries as the previous one but with _Sequences appended
			unknownStrings132 = ParserUtils.ReadStringArray(reader); // Specific frequencies? GothamFM_01, Tyger_Chatter, etc
			unknownStrings133 = ParserUtils.ReadStringArray(reader); // Specific radio dialogue? Play_GothamFM_VickiVale03
			unknownStrings134 = ParserUtils.ReadStringArray(reader); // Lots of "Unknown"

			var numInts135 = ParserUtils.ReadUint32_BE(reader); // 12
			unknownInts135 = new uint[numInts135];
			for (int i = 0; i < numInts135; ++i) {
				unknownInts135[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numInts136 = ParserUtils.ReadUint32_BE(reader); // 12
			unknownInts136 = new uint[numInts136];
			for (int i = 0; i < numInts136; ++i) {
				unknownInts136[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numInts137 = ParserUtils.ReadUint32_BE(reader); // 12
			unknownInts137 = new uint[numInts137];
			for (int i = 0; i < numInts137; ++i) {
				unknownInts137[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numInts138 = ParserUtils.ReadUint32_BE(reader); // 12
			unknownInts138 = new uint[numInts138];
			for (int i = 0; i < numInts138; ++i) {
				unknownInts138[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numInts139 = ParserUtils.ReadUint32_BE(reader); // 12
			unknownInts139 = new uint[numInts139];
			for (int i = 0; i < numInts139; ++i) {
				unknownInts139[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numInts140 = ParserUtils.ReadUint32_BE(reader); // 12
			unknownInts140 = new uint[numInts140];
			for (int i = 0; i < numInts140; ++i) {
				unknownInts140[i] = ParserUtils.ReadUint32_BE(reader);
			}

			try {
				ParseTail(reader);
			} catch (Exception ex) when (tolerantTail) {
				tailError = ex.Message;
				tailErrorPosition = reader.BaseStream.Position;
			}
		}

		// Everything after the six 12 entry int arrays. The part between unknownInts140 and unknown150 has no
		// fixed size: it holds optional strings (side story flags, objectives and area names) and a few small
		// counters. Its end is found by looking ahead for the two 15 entry flag arrays (unknown155 and
		// unknown157), which every save so far has at a fixed distance after unknown150.
		private void ParseTail(BinaryReader reader) {
			var start = reader.BaseStream.Position;
			var rest = reader.ReadBytes((int)(reader.BaseStream.Length - start));

			var end = FindTailRegionEnd(rest);
			if (end < 0) {
				reader.BaseStream.Position = start;
				throw new Exception("Could not find the end of the variable block after unknownInts140");
			}

			unknownBytes143 = new byte[end];
			Array.Copy(rest, unknownBytes143, end);
			unknownStrings143 = FindStrings(unknownBytes143, 25);
			reader.BaseStream.Position = start + end;

			unknown150 = ParserUtils.ReadFloat_BE(reader); // 0.6 on TestSave0, 0.5 on TestSave1
			unknown151 = ParserUtils.ReadInt32_BE(reader);
			unknown152 = ParserUtils.ReadUint32_BE(reader); // 4 on TestSave0, 8 on TestSave1
			unknown153 = ParserUtils.ReadUint32_BE(reader); // 8

			unknownBitFlags154 = reader.ReadBytes(10);

			unknown155 = ParserUtils.ReadUint32_BE(reader); // 15
			unknownBitFlags156 = reader.ReadBytes((int)unknown155);

			unknown157 = ParserUtils.ReadUint32_BE(reader); // 15
			unknownBitFlags158 = reader.ReadBytes((int)unknown157);

			// Four lists of the same length: two string lists (always assassinBlood / Museum so far) and two int lists
			unknownStrings159A = ParserUtils.ReadStringArray(reader);
			unknownStrings159B = ParserUtils.ReadStringArray(reader);
			unknownInts159C = ReadIntList(reader);
			unknownInts159D = ReadIntList(reader);

			unknownBytes159 = reader.ReadBytes(17); // 0

			// Always nine 4 byte values. These used to be read as a float array (count 3) followed by an
			// int array (count 4), but the first value is not a count: it is 1 in an early game save
			// (Courthouse) and 3 in later ones, and the fifth is 0 early and 4 later. Reading them as
			// arrays made the parser run off the end of the file on early game saves.
			//   TestSave0:  3, 1106649460 (float 30.77), 11, 11, 4, 3, 0, 0, 8
			//   Courthouse: 1, 1091856018 (float 9.28),  11,  9, 0, 0, 0, 0, 4294967295 (-1)
			//   TestSave1:  3, 0, 0, 0, 4, 4, 3, 0, 650
			unknownUints160 = new uint[9];
			for (var i = 0; i < unknownUints160.Length; ++i) {
				unknownUints160[i] = ParserUtils.ReadUint32_BE(reader);
			}

			var numFloats162 = ParserUtils.ReadUint32_BE(reader); // 3
			unknownFloats162 = new float[numFloats162];
			for (int i = 0; i < numFloats162; i++) {
				unknownFloats162[i] = ParserUtils.ReadFloat_BE(reader);
			}

			var numFloats163 = ParserUtils.ReadUint32_BE(reader); // 3
			unknownFloats163 = new float[numFloats163];
			for (int i = 0; i < numFloats163; i++) {
				unknownFloats163[i] = ParserUtils.ReadFloat_BE(reader);
			}

			unknown164 = ParserUtils.ReadInt32_BE(reader); // -1 on TestSave0, 100 on TestSave1
			unknown165 = ParserUtils.ReadInt32_BE(reader); // -1 on TestSave0, 100 on TestSave1

			unknownByte167 = reader.ReadByte(); // End of section? 1
			if (unknownByte167 == 255) {
				return; // TODO - Can't parse beyond this in some files
			}

			var numFloats168 = ParserUtils.ReadUint32_BE(reader); // 4 on TestSave0, 8 on TestSave1
			unknownFloats168 = new float[numFloats168];
			for (int i = 0; i < numFloats168; i++) {
				unknownFloats168[i] = ParserUtils.ReadFloat_BE(reader);
			}

			var unknownCount169 = ParserUtils.ReadUint32_BE(reader); // 4 on TestSave0, 8 on TestSave1
			unknownFloats169 = new float[unknownCount169];
			for (int i = 0; i < unknownCount169; i++) {
				unknownFloats169[i] = ParserUtils.ReadFloat_BE(reader);
			}

			footerHeader = reader.ReadBytes(89);
			footerIntLists = new int[6][];
			for (var i = 0; i < footerIntLists.Length; ++i) {
				footerIntLists[i] = ReadIntList(reader);
			}
			footerTail = reader.ReadBytes(333);

			if (reader.BaseStream.Position < reader.BaseStream.Length) {
				// Another copy of segment 3 follows
				var extra = new ArkhamCitySave(rawData, reader.BaseStream.Position, tolerantTail);
				additionalSegment3s.Add(extra);
				if (extra.parseError != null && parseError == null) {
					parseError = "In additional segment 3 (byte " + extra.segment3Start + "): " + extra.parseError;
					parseErrorPosition = extra.parseErrorPosition;
				}
				if (extra.tailError != null && tailError == null) {
					tailError = "In additional segment 3 (byte " + extra.segment3Start + "): " + extra.tailError;
					tailErrorPosition = extra.tailErrorPosition;
				}
			}
		}

		/// <summary>The 830 byte block that ends this copy of segment 3, as it is stored in the save (header, six counted int lists, tail).</summary>
		public byte[] GetFooterBytes() {
			var bytes = new List<byte>(830);
			bytes.AddRange(footerHeader);
			foreach (var list in footerIntLists) {
				bytes.AddRange(ToBigEndian((uint)list.Length));
				foreach (var value in list) {
					bytes.AddRange(ToBigEndian((uint)value));
				}
			}

			bytes.AddRange(footerTail);
			return bytes.ToArray();
		}

		private static byte[] ToBigEndian(uint value) {
			return new byte[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value };
		}

		private static int[] ReadIntList(BinaryReader reader) {
			var count = ParserUtils.ReadUint32_BE(reader);
			if ((long)count * 4 > reader.BaseStream.Length - reader.BaseStream.Position) {
				throw new Exception("Tried to parse an int array of length " + count.ToString() + " when there were not enough bytes remaining in the file");
			}
			var result = new int[count];
			for (var i = 0; i < count; ++i) {
				result[i] = ParserUtils.ReadInt32_BE(reader);
			}
			return result;
		}

		// Looks for "15, 15 bytes, 15, 15 bytes" (unknown155 and unknown157) and returns the offset where
		// unknown150 starts, which is 26 bytes before it. Returns -1 if it is not there.
		private static int FindTailRegionEnd(byte[] data) {
			for (var i = 60; i + 38 <= data.Length; ++i) {
				if (IsUint32(data, i, 15) && IsUint32(data, i + 19, 15)) {
					return i - 26;
				}
			}
			return -1;
		}

		private static bool IsUint32(byte[] data, int offset, uint value) {
			return data[offset] == (byte)(value >> 24) && data[offset + 1] == (byte)(value >> 16) && data[offset + 2] == (byte)(value >> 8) && data[offset + 3] == (byte)value;
		}

		// Finds UE3 style strings (length including the null terminator, then plain ASCII) by trying every offset.
		private static List<string> FindStrings(byte[] data, int startOffset) {
			var result = new List<string>();
			var i = startOffset;
			while (i + 4 <= data.Length) {
				var length = (data[i] << 24) | (data[i + 1] << 16) | (data[i + 2] << 8) | data[i + 3];
				if (length >= 4 && length <= 200 && i + 4 + length <= data.Length && data[i + 4 + length - 1] == 0) {
					var printable = true;
					for (var j = i + 4; j < i + 4 + length - 1; ++j) {
						if (data[j] < 32 || data[j] >= 127) {
							printable = false;
							break;
						}
					}
					if (printable) {
						result.Add(Encoding.ASCII.GetString(data, i + 4, length - 1));
						i += 4 + length;
						continue;
					}
				}
				++i;
			}
			return result;
		}

		private byte[] ParseUnknownStruct(BinaryReader reader) {
			var identifier = ParserUtils.ReadUint32_BE(reader);
			var subCount = ParserUtils.ReadUint32_BE(reader);

			if (subCount > 0) {
				return reader.ReadBytes((int)subCount * 19);
			}

			return null;
		}
	}
}
