namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Util;
using Hourglass.Util.Services.SettingsService;

using System.ComponentModel;

public partial class UserDataSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    public string UsernameTextboxText {
        set => settingsService.Username = value;
        get => settingsService.Username;
    }

    public string StartDateTextboxText {
        set => settingsService.StartDateString = value;
        get => settingsService.StartDateString;
    }

    public string JobNameTextboxText {
        set => settingsService.JobName = value;
        get => settingsService.JobName;
    }

    public override string Title => "User Data";

    public new event PropertyChangedEventHandler? PropertyChanged;

    public UserDataSubSettingsPageViewModel() : this(null, null, null, null) {

    }

    public UserDataSubSettingsPageViewModel(DateTimeService dateTimeService, SettingsPageViewModel settingsController, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
        settingsService.OnUsernameChanged += val=> OnPropertyChanged(nameof(UsernameTextboxText));
        settingsService.OnStartDateStringChanged += val=> OnPropertyChanged(nameof(StartDateTextboxText));
        if (settingsService != null) {
            JobNameTextboxText = settingsService.GetSetting(SettingsService.JOB_NAME_KEY);
            UsernameTextboxText = settingsService.GetSetting(SettingsService.USER_NAME_KEY);
            StartDateTextboxText = settingsService.GetSetting(SettingsService.START_DATE_KEY);
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

    private void ParseEnteredValues() {
        settingsService.SetSetting(SettingsService.JOB_NAME_KEY, settingsService.JobName);
        settingsService.SetSetting(SettingsService.USER_NAME_KEY, settingsService.Username);
        settingsService.SetSetting(SettingsService.START_DATE_KEY, settingsService.StartDateString);
    }

    [RelayCommand]
    private void SaveSettings() {
        Console.WriteLine("start task button click!");
        ParseEnteredValues();
        settingsService.ReloadSettings();
        AllBindingPropertiesChanged();
    }

    public void OnLoad() {
        Console.WriteLine("loading User Data Sub Settings Page!");
        AllBindingPropertiesChanged();
    }

    public void AnyInput_LostFocus() {
        Console.WriteLine("any input of user data settings lost focus!");

    }
}