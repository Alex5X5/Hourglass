using ReactiveUI;
using System.Reflection;

namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

public partial class VisualsSubSettingsPageViewModel : SubSettingsPageViewModelBase {

	public override string Title => TranslatorService.Singleton["Views.Pages.Settings.Visuals.Title"] ?? "Graphics Settings";

	private string selectedTheme = "";
	public string SelectedTheme {
		get => selectedTheme;
		set {
			if (value == null)
				return;
			if (value != selectedTheme)
				HasUnsavedChanges = true;
			this.RaiseAndSetIfChanged(ref selectedTheme, value);
		}
	}
	public List<string> AvailableThemes { get; set; }

	public VisualsSubSettingsPageViewModel() : this(null, null, null) {

	}

	public VisualsSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
		var assembly = Assembly.GetAssembly(typeof(VisualsSubSettingsPageViewModel));
		var themeResources = assembly.GetManifestResourceNames();
		//.Where(name => name.Contains("Themes") && name.EndsWith(".axaml"));

			foreach (var resource in themeResources) {
				Console.WriteLine(resource);
			}

	}

	public void OnLoad() {
		Console.WriteLine("loading Visuals Sub Settings Page!");
	}

	public override void SaveSettings() {
	}
}