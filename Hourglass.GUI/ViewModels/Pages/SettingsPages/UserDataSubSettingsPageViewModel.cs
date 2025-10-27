namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using CommunityToolkit.Mvvm.Input;
using Hourglass.Database.Models;
using Hourglass.GUI.Services;
using Hourglass.Util;
using Hourglass.Util.Services;
using System.ComponentModel;

public partial class UserDataSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    private string username = "";
    public string UsernameTextboxText {
        set {
            username = value;
            OnPropertyChanged(nameof(UsernameTextboxText));
        }
        get => username;
    }

    private string startDateString = "";
    public string StartDateTextboxText {
        set {
            startDateString = value;
            OnPropertyChanged(nameof(StartDateTextboxText));
            DateTime start = DateTimeService.InterpretDayAndTimeString(value) ?? DateTime.MinValue;
        }
        get => startDateString;
    }
    private string jobName;
    public string JobNameTextboxText {
        set {
            if (cacheService?.SelectedTask != null) {
                DateTime finish = DateTimeService.InterpretDayAndTimeString(value) ?? cacheService.SelectedTask.FinishDateTime;
                cacheService.SelectedTask.start = DateTimeService.ToSeconds(finish);
            }
            OnPropertyChanged(nameof(JobNameTextboxText));
        }
        get => cacheService?.RunningTask != null ? DateTimeService.ToDayAndTimeString(cacheService.RunningTask.FinishDateTime) : "";
    }

    public override string Title => "User Data";

    public new event PropertyChangedEventHandler? PropertyChanged;

    public UserDataSubSettingsPageViewModel() : this(null, null, null, null, null) {

    }

    public UserDataSubSettingsPageViewModel(DateTimeService dateTimeService, SettingsPageViewModel settingsController, MainViewModel pageController, CacheService cacheService, SettingsService settingsService) : base(dateTimeService, settingsController, pageController, cacheService, settingsService) {
        if (settingsService != null) {
            JobNameTextboxText = settingsService.GetSetting(SettingsService.JOB_NAME_KEY);
            UsernameTextboxText = settingsService.GetSetting(SettingsService.USER_NAME_KEY);
            StartDateTextboxText = settingsService.GetSetting(SettingsService.START_DATE_KEY);
        }
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
        settingsService.SetSetting(SettingsService.JOB_NAME_KEY, jobName);
        settingsService.SetSetting(SettingsService.USER_NAME_KEY, username);
        settingsService.SetSetting(SettingsService.START_DATE_KEY, startDateString);
    }

    [RelayCommand]
    private async System.Threading.Tasks.Task SaveSettings() {
        Console.WriteLine("start task button click!");
        if (dbService != null)
            cacheService.RunningTask = await dbService.StartNewTaskAsnc(
                UsernameTextboxText,
                null,
                new Worker { name = "new user" },
                null
            );
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