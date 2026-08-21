using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace ArkhamSaveParser {
	public class ArkhamCitySave : DecompressedSave {
		public ArkhamCitySave(CompressedSave save) : base(save) {
			ParseSave();
		}

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

		public List<string> progressStrings;

		public uint segment2HeaderA;
		public uint segment2HeaderB;

		public List<string> worldStrings;

		public string lastRoomNameUserFacing;
		public string currentObjectiveTag;
		public string lastEquippedGadget;

		// Unknown Values
		public uint unknown1;
		public uint unknown5;
		public uint unknown6;
		public uint unknown7;
		public uint unknown9;
		public uint unknown10;
		public uint unknown11;
		public uint unknown12;
		public byte[] unknownBytes1;
		public byte[] unknownBytes2;
		public uint unknown13;
		public uint unknown14;
		public uint unknown15;
		public uint unknown16;
		public uint unknown17;
		public uint unknown18;

		public uint[] unknownUints1;

		public string unknownString1;

		public uint[] unknownUints2;

		public uint unknown20;
		public uint unknown21;
		
		public uint unknown22;
		public byte[] unknownBytes22;


		public uint unknown23;
		public byte[] unknownBytes23;

		public uint unknown24;
		public byte[] unknownBytes24;

		public uint unknown25;
		public byte[] unknownBytes25;

		public uint unknown26;
		public byte[] unknownBytes26;

		public uint unknown27;
		public byte[] unknownBytes27;

		public uint unknown28;
		public byte[] unknownBytes28;

		public uint unknown29;
		public byte[] unknownBytes29;

		public uint unknown30;
		public byte[] unknownBytes30;

		public uint unknown31;
		public byte[] unknownBytes31;

		public uint unknown32;
		public byte[] unknownBytes32;

		public byte[] unknownBytes33;

		public float[] unknownFloats1;

		public float[] unknownFloats2;

		public uint unknown33;
		public uint unknown34;
		public uint unknown35;
		public uint unknown36;
		public uint unknown37;
		public uint unknown38;
		public uint unknown39;
		public uint unknown40;
		public uint unknown41;
		public uint unknown42;
		public uint unknown43;
		public uint unknown44;
		public uint unknown45;
		public uint unknown46;
		public uint unknown47;
		public uint unknown48;
		public uint unknown49;
		public uint unknown50;
		public uint unknown51;
		public uint unknown52;
		public uint unknown53;
		public uint unknown54;
		public uint unknown55;
		public uint unknown56;
		public uint unknown57;
		public uint unknown58;
		public uint unknown59;
		public uint unknown60;
		public uint unknown61;
		public uint unknown62;
		public uint unknown63;
		public uint unknown64;
		public uint unknown65;
		public uint unknown66;
		public uint unknown67;
		public float unknown68;
		public float unknown69;
		public float unknown70;
		public float unknown71;
		public uint unknown72;
		public uint unknown73;
		public uint unknown74;
		public uint unknown75;
		public uint unknown76;
		public uint unknown77;
		public uint unknown78;
		public uint unknown79;
		public uint unknown80;
		public uint unknown81;
		public uint unknown82;
		public uint unknown83;
		public uint unknown84;
		public uint unknown85;
		public uint unknown86;
		public uint unknown87;
		public List<string> unknownStrings88;
		public List<string> unknownStrings89;
		public byte[] unknownBytes90;
		public byte[] unknownBytes91;
		public uint unknown92;
		public uint unknown93;
		public List<string> unknownStrings94;
		public string unknownString95;
		public uint unknown96;
		public float unknown97;
		public uint unknown98;
		public uint unknown99;
		public uint unknown100;
		public uint unknown101;
		public string unknownString102;
		public byte[] unknownBytes103;
		public uint unknown104;
		public uint unknown105;
		public uint unknown106;
		public List<string> unknownStrings107;
		public byte[] unknownBytes108;
		public string unknownString109;
		public byte[] unknownBytes110;
		public uint unknown111;
		public byte[] unknownBytes112;
		public uint unknown113;
		public byte[] unknownBytes114;
		public uint unknown115;
		public byte[] unknownFlagBytes116;
		public uint unknown117;
		public byte[] unknownFlagBytes118;
		public uint[] unknownInts119;
		public float[] unknownFloats120;
		public float[] unknownFloats121;
		public List<string> unknownStrings122;
		public List<string> unknownStrings123;
		public List<string> unknownStrings124;
		public uint unknown125;
		public float unknown126;
		public float unknown127;
		public float unknown128;
		public List<string> unknownStrings129;
		public List<string> unknownStrings130;
		public List<string> unknownStrings131;
		public List<string> unknownStrings132;
		public List<string> unknownStrings133;
		public List<string> unknownStrings134;
		public uint[] unknownInts135;
		public uint[] unknownInts136;
		public uint[] unknownInts137;
		public uint[] unknownInts138;
		public uint[] unknownInts139;
		public uint[] unknownInts140;
		public uint unknown141;
		public uint unknown142;
		public byte[] unknownBytes143;
		public uint unknown144;
		public string unknownString145;
		public string unknownString146;
		public uint numFlags147;
		public byte[] unknownFlagBytes148;
		public byte[] unknownBytes149;
		public float unknown150;
		public float unknown151;
		public float unknown152;
		public float unknown153;
		public byte[] unknownBitFlags154;
		public uint unknown155;
		public byte[] unknownBitFlags156;
		public uint unknown157;
		public byte[] unknownBitFlags158;
		public byte[] unknownBytes159;
		public float[] unknownFloats160;
		public uint[] unknownInts161;
		public float[] unknownFloats162;
		public float[] unknownFloats163;
		public int unknown164;
		public int unknown165;
		public uint unknown166;
		public byte unknownByte167;
		public float[] unknownFloats168;
		public float[] unknownFloats169;
		public byte[] unknownBytes170;

		private void ParseSave() {
			var reader = new BinaryReader(new MemoryStream(rawData));

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

			for (var i = 0; i < 22; ++i) {
				ParserUtils.ReadUint32_BE(reader); // More settings or flags or something
			}

			day = ParserUtils.ReadUint32_BE(reader);
			month = ParserUtils.ReadUint32_BE(reader);
			year = ParserUtils.ReadUint32_BE(reader);

			unknown10 = ParserUtils.ReadUint32_BE(reader); // 0 on fresh file, 5 after starting

			// Most likely just padding
			unknown11 = ParserUtils.ReadUint32_BE(reader);
			unknown12 = ParserUtils.ReadUint32_BE(reader);

			var structCount = ParserUtils.ReadUint32_BE(reader);
			for (var i = 0; i < structCount; ++i) {
				ParseUnknownStruct(reader); // I think this is related to challenge maps
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

			unknownUints1 = new uint[10];
			for (var i = 0; i < 10; ++i) {
				unknownUints1[i] = ParserUtils.ReadUint32_BE(reader); // Seems to just be padding
			}
			
			unknownString1 = ParserUtils.ReadUE3String(reader); // Character, or something

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

			unknown35 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown35; ++i) {
				reader.ReadByte(); // These range from 0 to 31, not sure what to make of them
			}

			unknown36 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown36; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown37 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown37; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown38 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown38; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown39 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown39; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown40 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown40; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown41 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown41; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown42 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown42; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown43 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown43; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown44 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown44; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown45 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown45; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown46 = ParserUtils.ReadUint32_BE(reader); // 0x3C (60)
			for (var i = 0; i < unknown46; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown47 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown47; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown48 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown48; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown49 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown49; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown50 = ParserUtils.ReadUint32_BE(reader); // 0x0B (11)
			for (var i = 0; i < unknown50; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown51 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown51; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown52 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown52; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown53 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown53; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown54 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown54; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			unknown55 = ParserUtils.ReadUint32_BE(reader); // 9
			for (var i = 0; i < unknown55; ++i) {
				ParserUtils.ReadUint32_BE(reader);
			}

			// No idea what these are
		    unknown59 = ParserUtils.ReadUint32_BE(reader);
			unknown60 = ParserUtils.ReadUint32_BE(reader);
			unknown61 = ParserUtils.ReadUint32_BE(reader);
			unknown62 = ParserUtils.ReadUint32_BE(reader);
			unknown63 = ParserUtils.ReadUint32_BE(reader);
			unknown64 = ParserUtils.ReadUint32_BE(reader);
			unknown65 = ParserUtils.ReadUint32_BE(reader);
			unknown66 = ParserUtils.ReadUint32_BE(reader);
			unknown67 = ParserUtils.ReadUint32_BE(reader);

			var numFloats3 = ParserUtils.ReadUint32_BE(reader); // 0x2E (46)
			for (var i = 0; i < numFloats3; ++i) {
				ParserUtils.ReadFloat_BE(reader);
			}

			var numFloats4 = ParserUtils.ReadUint32_BE(reader); // 0x2E (46)
			for (var i = 0; i < numFloats4; ++i) {
				ParserUtils.ReadFloat_BE(reader);
			}

			var numFloats5 = ParserUtils.ReadUint32_BE(reader); // 0x2E (46)
			for (var i = 0; i < numFloats5; ++i) {
				ParserUtils.ReadFloat_BE(reader);
			}

			unknown68 = ParserUtils.ReadFloat_BE(reader);
			unknown69 = ParserUtils.ReadFloat_BE(reader);
			unknown70 = ParserUtils.ReadFloat_BE(reader);
			unknown71 = ParserUtils.ReadFloat_BE(reader);

			unknown72 = ParserUtils.ReadUint32_BE(reader);
			unknown73 = ParserUtils.ReadUint32_BE(reader);
			unknown74 = ParserUtils.ReadUint32_BE(reader);
			unknown75 = ParserUtils.ReadUint32_BE(reader);
			unknown76 = ParserUtils.ReadUint32_BE(reader);
			unknown77 = ParserUtils.ReadUint32_BE(reader);

			if (unknown77 != 1) {
				//throw new ArgumentException("The file was parsed incorrectly, expected " + 1 + " at section 2 end, got " + unknown77);
				return; // Not sure if this is right but seems to be EOF for the default resave
			}

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

			unknown85 = ParserUtils.ReadUint32_BE(reader); // Padding?
			unknown86 = ParserUtils.ReadUint32_BE(reader); // Padding?

			worldName = ParserUtils.ReadUE3String(reader);
			roomName = ParserUtils.ReadUE3String(reader);

			unknown87 = ParserUtils.ReadUint32_BE(reader); // Padding?

			unknownStrings88 = ParserUtils.ReadStringArray(reader); // OW flags?
			unknownStrings89 = ParserUtils.ReadStringArray(reader);

			flagStrings = ParserUtils.ReadStringArray(reader); // VERY large array, there are thousands of these

			unknownBytes90 = reader.ReadBytes(10); // Not entirely sure what to make of this

			lastRoomNameUserFacing = ParserUtils.ReadUE3String(reader);

			unknownBytes91 = reader.ReadBytes(2); // Padding?

			currentObjectiveTag = ParserUtils.ReadUE3String(reader);

			unknown92 = ParserUtils.ReadUint32_BE(reader); // Padding?
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

			unknownBytes143 = reader.ReadBytes(39); // Not sure how to read these, there's 3 stray bytes somewhere in here

			unknownString145 = ParserUtils.ReadUE3String(reader); // Something side mission related
			unknownString146 = ParserUtils.ReadUE3String(reader); // Something side mission related

			numFlags147 = ParserUtils.ReadUint32_BE(reader); // 0 on TestSave0, 1 on TestSave1
			unknownFlagBytes148 = reader.ReadBytes((int)numFlags147);

			unknownBytes149 = reader.ReadBytes(34); // Not sure how to read these, there's 3 stray bytes somewhere in here

			unknown150 = ParserUtils.ReadFloat_BE(reader); // 0.6 on TestSave0, 0.5 on TestSave1
			unknown151 = ParserUtils.ReadInt32_BE(reader); // -1
			unknown152 = ParserUtils.ReadUint32_BE(reader); // 4 on TestSave0, 8 on TestSave1
			unknown153 = ParserUtils.ReadUint32_BE(reader); // 8

			unknownBitFlags154 = reader.ReadBytes(10);

			var numBitFlags155 = ParserUtils.ReadUint32_BE(reader); // 15
			unknownBitFlags156 = reader.ReadBytes((int)numBitFlags155);

			var numBitFlags157 = ParserUtils.ReadUint32_BE(reader); // 15
			unknownBitFlags158 = reader.ReadBytes((int)numBitFlags155);

			unknownBytes159 = reader.ReadBytes(33); // Padding? 0

			var numFloats160 = ParserUtils.ReadUint32_BE(reader); // 3
			unknownFloats160 = new float[numFloats160];
			for (int i = 0; i < numFloats160; i++) {
				unknownFloats160[i] = ParserUtils.ReadFloat_BE(reader);
			}

			var numInts161 = ParserUtils.ReadUint32_BE(reader); // 4
			unknownInts161 = new uint[numInts161];
			for (int i = 0; i < numInts161; i++) {
				unknownInts161[i] = ParserUtils.ReadUint32_BE(reader);
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

			unknownBytes170 = reader.ReadBytes((int)unknownCount169);
		}

		private void ParseUnknownStruct(BinaryReader reader) {
			var identifier = ParserUtils.ReadUint32_BE(reader);
			var subCount = ParserUtils.ReadUint32_BE(reader);

			if (subCount > 0) {
				reader.ReadBytes((int)subCount * 19);
			}
		}
	}
}
