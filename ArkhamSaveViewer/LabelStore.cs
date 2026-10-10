using System.Text;
using System.Text.Json;
using System.IO;

namespace ArkhamSaveViewer {
	/// <summary>Your own labels for fields, stored in labels.json next to the exe.</summary>
	internal sealed class LabelStore {
		private readonly string path = Path.Combine(AppContext.BaseDirectory, "labels.json");
		private Dictionary<string, string> labels = new();

		/// <summary>Raised with a message when labels.json could not be written.</summary>
		public event Action<string>? SaveFailed;

		public LabelStore() {
			try {
				if (File.Exists(path)) {
					labels = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path)) ?? new();
				}
			} catch {
				labels = new();
			}
		}

		public string Get(string key) => labels.TryGetValue(key, out var label) ? label : "";

		public void Set(string key, string? text) {
			if (string.IsNullOrWhiteSpace(text)) {
				labels.Remove(key);
			} else {
				labels[key] = text.Trim();
			}

			try {
				File.WriteAllText(path, JsonSerializer.Serialize(labels, new JsonSerializerOptions { WriteIndented = true }), new UTF8Encoding(false));
			} catch (Exception ex) {
				SaveFailed?.Invoke("Could not save labels.json: " + ex.Message);
			}
		}
	}
}
