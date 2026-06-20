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

		private uint day = 0;
		private uint month = 0;
		private uint year = 0;

		private string progression = string.Empty;
		private string riddles = string.Empty;

		private List<string> collectibles = [];
		private List<string> upgrades = [];

		private void ParseSave() {
			var reader = new BinaryReader(new MemoryStream(rawData));

			var constant1 = ParserUtils.ReadUint32_BE(reader);
			if (constant1 != 16) {
				throw new ArgumentException("The loaded file was not an Arkham City save, expected " + 16 + " but found " + constant1);
			}

			var constant2 = ParserUtils.ReadUint32_BE(reader);
			if (constant2 != 12) {
				throw new ArgumentException("The loaded file was not an Arkham City save, expected " + 12 + " but found " + constant2);
			}

			var unknown0 = ParserUtils.ReadUint32_BE(reader); // Slot? Character?
			var unknown1 = ParserUtils.ReadUint32_BE(reader); // Difficulty? Mode?
			var unknown2 = ParserUtils.ReadUint32_BE(reader);
			var unknown3 = ParserUtils.ReadUint32_BE(reader); // Brightness?
			var unknown4 = ParserUtils.ReadUint32_BE(reader); // SFX Volume?
			var unknown5 = ParserUtils.ReadUint32_BE(reader); // Music Volume?
			var unknown6 = ParserUtils.ReadUint32_BE(reader); // Dialogue Volume?

			var unknown7 = ParserUtils.ReadUint32_BE(reader); // Options?
			var unknown8 = ParserUtils.ReadUint32_BE(reader); // Subtitles? Inverted look?
			var unknown9 = ParserUtils.ReadUint32_BE(reader); // Another slider?

			for (var i = 0; i < 22; ++i) {
				ParserUtils.ReadUint32_BE(reader); // More settings or flags or something
			}

			day = ParserUtils.ReadUint32_BE(reader);
			month = ParserUtils.ReadUint32_BE(reader);
			year = ParserUtils.ReadUint32_BE(reader);

			var unknown10 = ParserUtils.ReadUint32_BE(reader);

			// Most likely just padding
			var unknown11 = ParserUtils.ReadUint32_BE(reader);
			var unknown12 = ParserUtils.ReadUint32_BE(reader);

			var structCount = ParserUtils.ReadUint32_BE(reader);
			for (var i = 0; i < structCount; ++i) {
				ParseUnknownStruct(reader); // I think this is related to challenge maps
			}

			reader.ReadBytes(32); // Exit block

			var byteArraySize = ParserUtils.ReadUint32_BE(reader);
			reader.ReadBytes((int)byteArraySize);

			var unknown13 = ParserUtils.ReadUint32_BE(reader); // Probably just padding
			var unknown14 = ParserUtils.ReadUint32_BE(reader);

			var strings = ParserUtils.ReadStringArray(reader);
			progression = strings[0];
			riddles = strings[1];

			var unknown16 = reader.ReadByte(); // Map grid?
			var unknown17 = reader.ReadByte();

			var unknown18 = ParserUtils.ReadUint32_BE(reader); // Padding?

			var unknown19 = ParserUtils.ReadFloat_BE(reader);

			for (var i = 0; i < 10; ++i) {
				ParserUtils.ReadUint32_BE(reader); // Seems to just be padding
			}
			
			ParserUtils.ReadUE3String(reader); // Character, or something

			for (var i = 0; i < 6; ++i) {
				ParserUtils.ReadUint32_BE(reader); // Some flags, maybe some more padding
			}

			// ------------------------------- //
			// ---------- Segment 2 ---------- //
			// ------------------------------- //

			var segment2Header1 = ParserUtils.ReadUint32_BE(reader);
			var segment2Header2 = ParserUtils.ReadUint32_BE(reader);

			collectibles = ParserUtils.ReadStringArray(reader);
			upgrades = ParserUtils.ReadStringArray(reader);

			// Probably just padding/end of string arrays marker
			var unknown20 = ParserUtils.ReadUint32_BE(reader);
			var unknown21 = ParserUtils.ReadUint32_BE(reader);
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
