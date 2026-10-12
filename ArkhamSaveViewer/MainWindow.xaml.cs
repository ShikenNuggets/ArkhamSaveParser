using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using ArkhamSaveParser;
using Microsoft.Win32;

namespace ArkhamSaveViewer {
	/// <summary>
	/// Loads an Arkham City .sgd, lists every public parsed field with its value, and lets you
	/// type your own label for each field (saved to labels.json next to the exe).
	/// The file is re-read automatically when it changes, and changed values are highlighted.
	/// </summary>
	public partial class MainWindow : Window {
		private enum StatusKind { Normal, Warning, Error }

		private readonly LabelStore labels = new();
		private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(1) };
		private readonly string settingsPath = Path.Combine(AppContext.BaseDirectory, "viewer_settings.txt");

		private string? currentPath;
		private DateTime lastWriteUtc;
		private Dictionary<string, string> previous = new();
		private bool loading;
		private bool ready;

		public MainWindow() {
			InitializeComponent();

			labels.SaveFailed += message => SetStatus(message, StatusKind.Error);

			var theme = App.LoadThemeName();
			foreach (ComboBoxItem item in ThemeBox.Items) {
				if ((string)item.Content == theme) {
					ThemeBox.SelectedItem = item;
				}
			}

			timer.Tick += (_, _) => CheckForChange();
			timer.Start();
			ready = true;

			Loaded += (_, _) => {
				try {
					// TODO: Proper error handling
					if (File.Exists(settingsPath)) {
						var last = File.ReadAllText(settingsPath).Trim();
						if (File.Exists(last)) {
							currentPath = last;
							Reload();
						}
					}
				} catch { }
			};
		}

		private void SetStatus(string text, StatusKind kind) {
			StatusText.Text = text;
			switch (kind) {
				case StatusKind.Error:
					StatusText.SetResourceReference(TextBlock.ForegroundProperty, "ErrorBrush"); // TODO: Use a brush from the theme instead of hardcoding the name here
					break;
				case StatusKind.Warning:
					StatusText.SetResourceReference(TextBlock.ForegroundProperty, "WarnBrush"); // TODO: Use a brush from the theme instead of hardcoding the name here
					break;
				default:
					StatusText.ClearValue(TextBlock.ForegroundProperty);
					break;
			}
		}

		// ---- toolbar -------------------------------------------------------------------------------

		private void OpenButton_Click(object sender, RoutedEventArgs e) {
			var dialog = new OpenFileDialog { Filter = "Arkham save (*.sgd)|*.sgd|All files (*.*)|*.*" };
			if (currentPath != null) {
				dialog.InitialDirectory = Path.GetDirectoryName(currentPath);
			}

			if (dialog.ShowDialog(this) == true) {
				currentPath = dialog.FileName;
				previous.Clear();
				try { File.WriteAllText(settingsPath, currentPath); } catch { }
				Reload();
			}
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e) => Reload();

		private void DumpButton_Click(object sender, RoutedEventArgs e) {
			var dialog = new OpenFolderDialog { Title = "Pick the folder with your .sgd saves (sub folders are included). fields.csv and the summaries are written to a 'dump' folder inside it." };
			if (currentPath != null) {
				dialog.InitialDirectory = Path.GetDirectoryName(currentPath);
			}

			if (dialog.ShowDialog(this) != true) {
				return;
			}

			try {
				System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
				var output = Path.Combine(dialog.FolderName, "dump");
				var message = SaveDump.DumpFolder(dialog.FolderName, output, name => {
					SetStatus("Reading " + name, StatusKind.Normal);
					Dispatcher.Invoke(() => { }, DispatcherPriority.Render); // let the status text repaint
				});
				SetStatus(message, StatusKind.Normal);
				MessageBox.Show(this, message + Environment.NewLine + Environment.NewLine + "Files: fields.csv, field_summary.txt, footer_bytes.csv, footer_summary.txt", "Dump finished");
			} catch (Exception ex) {
				SetStatus("Dump failed: " + ex.Message, StatusKind.Error);
			} finally {
				System.Windows.Input.Mouse.OverrideCursor = null;
			}
		}

		private void FilterChanged(object sender, RoutedEventArgs e) {
			if (ready) {
				ApplyFilter();
			}
		}

		private void FilterTextChanged(object sender, TextChangedEventArgs e) {
			if (ready) {
				ApplyFilter();
			}
		}

		private void HideZeroChanged(object sender, RoutedEventArgs e) {
			if (ready) {
				Reload();
			}
		}

		private void ThemeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ThemeBox.SelectedItem is ComboBoxItem item && item.Content is string name) {
				App.ApplyTheme(name);
				App.SaveThemeName(name);
			}
		}

		// ---- loading -------------------------------------------------------------------------------

		private void CheckForChange() {
			if (AutoReload.IsChecked != true || currentPath == null || loading) {
				return;
			}

			try {
				var stamp = File.GetLastWriteTimeUtc(currentPath);
				if (stamp != lastWriteUtc) {
					Reload();
				}
			} catch { }
		}

		private void Reload() {
			if (currentPath == null) {
				return;
			}

			loading = true;
			try {
				PathBox.Text = currentPath;

				var save = SaveDump.LoadTolerant(currentPath); // copies the file first so we don't fight the game for it

				lastWriteUtc = File.GetLastWriteTimeUtc(currentPath);
				Populate(save);
				SetStatus("Loaded " + Path.GetFileName(currentPath) + " at " + DateTime.Now.ToString("HH:mm:ss") + ". Highlighted = changed since the previous load.", StatusKind.Normal);
			} catch (Exception ex) {
				SetStatus("Failed to load: " + ex.Message, StatusKind.Error);
			} finally {
				loading = false;
			}
		}

		private void Populate(ArkhamCitySave save) {
			var now = new Dictionary<string, string>();
			var firstLoad = previous.Count == 0;

			var selectedName = (FieldGrid.SelectedItem as FieldRow)?.Name;

			var fields = new List<FieldRow>();
			foreach (var (name, type, full) in SaveDump.CollectFields(save)) {
				now[name] = full;
				var changed = !firstLoad && previous.TryGetValue(name, out var old) && old != full;
				fields.Add(new FieldRow(labels, name, type, full) { IsChanged = changed });
			}

			var footerRows = new List<FooterRow>();
			var footers = SaveDump.CollectFooters(save);
			for (var copy = 0; copy < footers.Count; ++copy) {
				var bytes = footers[copy];
				for (var offset = 0; offset < bytes.Length; ++offset) {
					var key = "footer:" + copy + ":" + offset;
					var value = bytes[offset].ToString(CultureInfo.InvariantCulture);
					now[key] = value;
					var changed = !firstLoad && previous.TryGetValue(key, out var old2) && old2 != value;
					if (HideZeroFooter.IsChecked == true && bytes[offset] == 0 && !changed) {
						continue;
					}

					footerRows.Add(new FooterRow(labels, copy + 1, offset, bytes[offset].ToString("X2"), value) { IsChanged = changed });
				}
			}

			var challengeRows = new List<ChallengeRow>();
			var current = save.GetBatChallengeCurrent();
			var best = save.GetBatChallengeBest();
			var beaten = save.GetBatChallengeBeaten();
			for (var slot = 0; slot < beaten.Length; ++slot) {
				var cur = slot < current.Length ? current[slot].ToString("R", CultureInfo.InvariantCulture) : "";
				var bst = slot < best.Length ? best[slot].ToString("R", CultureInfo.InvariantCulture) : "";
				var key = "slot:" + slot;
				var snapshot = cur + "|" + bst + "|" + beaten[slot];
				now[key] = snapshot;
				var changed = !firstLoad && previous.TryGetValue(key, out var old3) && old3 != snapshot;
				challengeRows.Add(new ChallengeRow(labels, slot, cur, bst, beaten[slot]) { IsChanged = changed, IsBeaten = !changed && beaten[slot] == 1 });
			}

			var collectibleRows = save.GetCollectibles().Select(c => new CollectibleRow(labels, c)).ToList();
			var upgradeRows = save.GetUpgrades().Select(u => new UpgradeRow(labels, u)).ToList();
			var flagRows = save.GetFlagStrings().Select(f => new FlagRow(labels, f)).ToList();

			previous = now;

			FieldGrid.ItemsSource = fields;
			FooterGrid.ItemsSource = footerRows;
			ChallengeGrid.ItemsSource = challengeRows;
			CollectibleGrid.ItemsSource = collectibleRows;
			UpgradeGrid.ItemsSource = upgradeRows;
			FlagGrid.ItemsSource = flagRows;

			ApplyFilter();

			if (selectedName != null) {
				var again = fields.FirstOrDefault(f => f.Name == selectedName);
				if (again != null) {
					FieldGrid.SelectedItem = again;
					FieldGrid.ScrollIntoView(again);
				}
			}

			ShowDetail();
		}

		private void ApplyFilterToGrid(DataGrid grid) {
			if (grid == null || grid.ItemsSource == null) {
				return;
			}

			var text = FilterBox.Text.Trim();
			var unknownOnly = UnknownOnly.IsChecked == true;
			var view = CollectionViewSource.GetDefaultView(grid.ItemsSource);
			view.Filter = item => {
				if (item == null) {
					return false;
				}

				dynamic dyn = item;

				string name = dyn.Name;
				if (string.IsNullOrWhiteSpace(name)) {
					return false;
				}

				return (text.Length == 0 || name.Contains(text, StringComparison.OrdinalIgnoreCase))
					&& (!unknownOnly || name.StartsWith("unknown", StringComparison.Ordinal));
			};

			view.Refresh();
		}

		private void ApplyFilter() {
			ApplyFilterToGrid(FieldGrid);
			ApplyFilterToGrid(CollectibleGrid);
			ApplyFilterToGrid(UpgradeGrid);
			ApplyFilterToGrid(FlagGrid);
		}

		private void FieldGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => ShowDetail();

		private void ShowDetail() {
			if (FieldGrid.SelectedItem is FieldRow row) {
				DetailBox.Text = row.Name + Environment.NewLine + Environment.NewLine + row.Full;
			} else {
				DetailBox.Text = "";
			}
		}

		private void Grid_LoadingRow(object? sender, DataGridRowEventArgs e) {
			if (e.Row.Item is LabelRow row) {
				if (row.IsChanged) {
					e.Row.SetResourceReference(Control.BackgroundProperty, "ChangedBrush"); // TODO: Use a brush from the theme instead of hardcoding the name here
				} else if (row.IsBeaten) {
					e.Row.SetResourceReference(Control.BackgroundProperty, "BeatenBrush"); // TODO: Use a brush from the theme instead of hardcoding the name here
				} else {
					e.Row.ClearValue(Control.BackgroundProperty); // rows are recycled, so reset
				}
			}
		}
	}
}
