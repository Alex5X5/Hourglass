namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Util.Services;
using System.ComponentModel;

public partial class UserDataSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    public string UsernameTextboxText { set; get; }

    public string StartDateTextboxText { set; get; }

    public string JobNameTextboxText { set; get; }

    public override string Title => TranslatorService.Singleton["Views.Pages.Settings.UserData.Title"] ?? "User Data";

    public new event PropertyChangedEventHandler? PropertyChanged;

    public UserDataSubSettingsPageViewModel() : this(null, null, null, null) {

    }

    public UserDataSubSettingsPageViewModel(DateTimeService dateTimeService, SettingsPageViewModel settingsController, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
        settingsService.OnUsernameChanged += val=> OnPropertyChanged(nameof(UsernameTextboxText));
        settingsService.OnStartDateChanged += val=> OnPropertyChanged(nameof(StartDateTextboxText));
        settingsService.OnPreSettingsSave += () => {
            settingsService.StartDateString = StartDateTextboxText ?? "";
            settingsService.Username = UsernameTextboxText ?? "";
            settingsService.JobName = JobNameTextboxText ?? "";
        };
        if (settingsService != null) {
            JobNameTextboxText = settingsService.JobName;
            UsernameTextboxText = settingsService.Username;
            StartDateTextboxText = settingsService.StartDateString;
        }
        AllBindingPropertiesChanged();
    }

    private void AllBindingPropertiesChanged() {
        OnPropertyChanged(nameof(UsernameTextboxText));
        OnPropertyChanged(nameof(JobNameTextboxText));
        OnPropertyChanged(nameof(StartDateTextboxText));
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override void SaveSettings() {
        settingsService.StartDateString = StartDateTextboxText;
        settingsService.Username = UsernameTextboxText;
        settingsService.JobName = JobNameTextboxText;
        AllBindingPropertiesChanged();
    }

    public void OnLoad() {
        Console.WriteLine("loading User Data Sub Settings Page!");
        AllBindingPropertiesChanged();
    }
}