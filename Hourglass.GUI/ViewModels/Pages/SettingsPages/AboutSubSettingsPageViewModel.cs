namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using System.ComponentModel;

public partial class AboutSubSettingsPageViewModel : SubSettingsPageViewModelBase {

	public override string Title => TranslatorService.Singleton["Views.Pages.Settings.About.Title"] ?? "About Us";
	
	public AboutSubSettingsPageViewModel() : this(null, null, null) {

	}

	public AboutSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
	}

	public void OnLoad() {
		Console.WriteLine("loading About Sub Settings Page!");
	}

	public override void SaveSettings() {
	}
}