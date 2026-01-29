namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

public partial class VisualsSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    public override string Title => TranslatorService.Singleton["Views.Pages.Settings.Visuals.Title"] ?? "Graphics Settings";

    public VisualsSubSettingsPageViewModel() : this(null, null, null) {

    }

    public VisualsSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
    }

    public void OnLoad() {
        Console.WriteLine("loading Visuals Sub Settings Page!");
    }

    public override void SaveSettings() {
    }
}