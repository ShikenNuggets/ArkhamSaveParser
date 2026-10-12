namespace ArkhamSaveViewer {
	/// <summary>One row in any of the grids. Label is the editable column and is saved straight to labels.json.</summary>
	internal class LabelRow {
		private readonly LabelStore store;

		public LabelRow(LabelStore store, string labelKey) {
			this.store = store;
			LabelKey = labelKey;
		}

		public string LabelKey { get; }
		public bool IsChanged { get; init; }
		public bool IsBeaten { get; init; }

		public string Label {
			get => store.Get(LabelKey);
			set => store.Set(LabelKey, value);
		}
	}

	internal sealed class FieldRow : LabelRow {
		public FieldRow(LabelStore store, string name, string type, string full) : base(store, name) {
			Name = name;
			Type = type;
			Full = full;
			Shown = full.Length > 300 ? full.Substring(0, 300) + " ..." : full;
			IsUnknown = name.StartsWith("unknown", StringComparison.Ordinal) || name.Contains(".unknown", StringComparison.Ordinal) || name.StartsWith("footer", StringComparison.Ordinal);
		}

		public string Name { get; }
		public string Type { get; }
		public string Full { get; }
		public string Shown { get; }
		public bool IsUnknown { get; }
	}

	internal sealed class ChallengeRow : LabelRow {
		public ChallengeRow(LabelStore store, int slot, string current, string best, uint beaten) : base(store, "slot:" + slot) {
			Slot = slot;
			Current = current;
			Best = best;
			Beaten = beaten;
		}

		public int Slot { get; }
		public string Current { get; }
		public string Best { get; }
		public uint Beaten { get; }
	}

	internal sealed class CollectibleRow : LabelRow {
		public CollectibleRow(LabelStore store, string name) : base(store, "collectible:" + name) {
			Name = name;
		}

		public string Name { get; }
	}

	internal sealed class FooterRow : LabelRow {
		public FooterRow(LabelStore store, int copy, int offset, string hex, string dec) : base(store, "footer:" + offset) {
			Copy = copy;
			Offset = offset;
			Hex = hex;
			Dec = dec;
		}

		public int Copy { get; }
		public int Offset { get; }
		public string Hex { get; }
		public string Dec { get; }
	}
}
