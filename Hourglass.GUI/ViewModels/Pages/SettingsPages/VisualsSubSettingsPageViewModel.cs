namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Util;

using System.ComponentModel;

public partial class VisualsSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    public override string Title => TranslatorService.Singleton["Views.Pages.Settings.Visuals.Title"] ?? "Graphics Settings";

    public new event PropertyChangedEventHandler? PropertyChanged;

    public VisualsSubSettingsPageViewModel() : this(null, null, null) {

    }

    public VisualsSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
    }

    private void AllBindingPropertiesChanged() {
        
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnLoad() {
        Console.WriteLine("loading Visuals Sub Settings Page!");
        AllBindingPropertiesChanged();
    }

    public override void SaveSettings() {
    }
}