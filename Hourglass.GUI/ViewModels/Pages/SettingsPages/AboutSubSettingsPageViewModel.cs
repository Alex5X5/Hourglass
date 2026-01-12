namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Util;

using System.ComponentModel;

public partial class AboutSubSettingsPageViewModel : SubSettingsPageViewModelBase {

	public override string Title => TranslatorService.Singleton["Views.Pages.Settings.About.Title"] ?? "About Us";
	
	public new event PropertyChangedEventHandler? PropertyChanged;

	public AboutSubSettingsPageViewModel() : this(null, null, null) {

	}

	public AboutSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
	}

	private void AllBindingPropertiesChanged() {
	}

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void OnLoad() {
		Console.WriteLine("loading About Sub Settings Page!");
		AllBindingPropertiesChanged();
	}

	public override void SaveSettings() {
		AllBindingPropertiesChanged();
	}
}