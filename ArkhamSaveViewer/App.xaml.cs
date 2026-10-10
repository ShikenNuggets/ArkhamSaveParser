using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace ArkhamSaveViewer {
	public partial class App : Application {
		// TODO: Proper settings file instead of random text files
		private static string ThemePath => Path.Combine(AppContext.BaseDirectory, "viewer_theme.txt");

		protected override void OnStartup(StartupEventArgs e) {
			ApplyTheme(LoadThemeName());
			base.OnStartup(e);
		}

		internal static string LoadThemeName() {
			try {
				// TODO: Proper error handling
				if (File.Exists(ThemePath)) {
					var name = File.ReadAllText(ThemePath).Trim();
					if (name == "Light" || name == "System" || name == "Dark") {
						return name;
					}
				}
			} catch { }

			return "System";
		}

		internal static void SaveThemeName(string name) {
			try { File.WriteAllText(ThemePath, name); } catch { }
		}

		internal static void ApplyTheme(string name) {
			var app = Current;
			bool dark = name == "Dark" || (name == "System" && SystemUsesDarkMode());

			app.ThemeMode = name switch {
				"Light" => ThemeMode.Light,
				"System" => ThemeMode.System,
				_ => ThemeMode.Dark,
			};

			// TODO: Use proper resource dictionaries instead of hardcoding colors here
			app.Resources["ChangedBrush"] = Brush(dark ? "#5A4B12" : "#FFF1B8");
			app.Resources["BeatenBrush"] = Brush(dark ? "#1F4D2B" : "#E2F4E2");
			app.Resources["UnknownBrush"] = Brush(dark ? "#FF8A80" : "#B22222");
			app.Resources["ErrorBrush"] = Brush(dark ? "#FF6B6B" : "#B22222");
			app.Resources["WarnBrush"] = Brush(dark ? "#FFB454" : "#C86400");
			app.Resources["GridLineBrush"] = Brush(dark ? "#5C5C66" : "#B8B8B8");
		}

		private static bool SystemUsesDarkMode() {
			try {
				using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
				return key?.GetValue("AppsUseLightTheme") is int light && light == 0;
			} catch {
				return true; // Fallback to dark mode if we can't read the registry for some reason
			}
		}

		private static SolidColorBrush Brush(string hex) {
			var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
			brush.Freeze();
			return brush;
		}
	}
}
