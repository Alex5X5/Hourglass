namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Util;

using System.ComponentModel;

public partial class ExportSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    public override string Title => TranslatorService.Singleton["Views.Pages.Settings.Export.Title"] ?? "Export Settings";

    public new event PropertyChangedEventHandler? PropertyChanged;

	public ExportSubSettingsPageViewModel() : this(null, null, null) {

	}

    public ExportSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
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
        Console.WriteLine("save button click!");
    }
}