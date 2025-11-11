namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.Util;

using System.ComponentModel;

public partial class VisualsSubSettingsPageViewModel : SubSettingsPageViewModelBase {

    private IHourglassDbService dbService;
    private TimerCacheService cacheService;
    private ViewModelFactory<MainViewModel> pageFactory;
    private MainViewModel controller;

    private string FallbackTaskDescription = "";

    public string DescriptionTextboxText {
        set {
            if (cacheService?.RunningTask != null)
                cacheService.RunningTask.description = value;
            else
                FallbackTaskDescription = value;
            OnPropertyChanged(nameof(DescriptionTextboxText));
        }
        get => cacheService?.RunningTask?.description ?? FallbackTaskDescription;
    }
    public string ProjectTextboxText {
        set {
            if (cacheService?.RunningTask != null)
                cacheService.RunningTask.description = value;
            OnPropertyChanged(nameof(ProjectTextboxText));
        }
        get => cacheService?.RunningTask?.project?.Name ?? "";
    }
    public string TicketTextboxText {
        get => cacheService?.RunningTask?.ticket?.name ?? "";
    }
    public string StartTextboxText {
        set {
            if (cacheService?.RunningTask != null) {
                DateTime start = DateTimeService.InterpretDayAndTimeString(value) ?? cacheService.RunningTask.StartDateTime;
                cacheService.RunningTask.start = DateTimeService.ToSeconds(start);
            }
            OnPropertyChanged(nameof(StartTextboxText));
        }
        get => cacheService?.RunningTask != null ? DateTimeService.ToDayAndTimeString(cacheService.RunningTask.StartDateTime) : "";
    }
    public string FinishTextboxText {
        set {
            if (cacheService?.SelectedTask != null) {
                DateTime finish = DateTimeService.InterpretDayAndTimeString(value) ?? cacheService.SelectedTask.FinishDateTime;
                cacheService.SelectedTask.start = DateTimeService.ToSeconds(finish);
            }
            OnPropertyChanged(nameof(FinishTextboxText));
        }
        get => cacheService?.RunningTask != null ? DateTimeService.ToDayAndTimeString(cacheService.RunningTask.FinishDateTime) : "";
    }

    public override string Title => "Visual Settings";

    public bool IsStartButtonEnabled { get => cacheService?.RunningTask == null; }
    public bool IsStopButtonEnabled { get => cacheService?.RunningTask != null; }
    public bool IsRestartButtonEnabled { get => cacheService?.RunningTask != null; }

    public Project SelectedProject { get; set; }
    public List<Project> AvailableProjects { get; set; }

    public new event PropertyChangedEventHandler? PropertyChanged;

    public VisualsSubSettingsPageViewModel() : this(null, null, null) {

    }

    public VisualsSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
        //if (cacheService != null)
        //    cacheService.OnRunningTaksChanged +=
        //        task => AllBindingPropertiesChanged();
        AvailableProjects = [
            new Project() { Name="test project" },
            new Project() { Name = "failing project" },
            new Project() { Name = "sucessfull project" }
        ];
        SelectedProject = AvailableProjects[0];
    }

    private void AllBindingPropertiesChanged() {
        OnPropertyChanged(nameof(DescriptionTextboxText));
        OnPropertyChanged(nameof(StartTextboxText));
        OnPropertyChanged(nameof(FinishTextboxText));
        OnPropertyChanged(nameof(TicketTextboxText));
        OnPropertyChanged(nameof(ProjectTextboxText));
        OnPropertyChanged(nameof(IsStartButtonEnabled));
        OnPropertyChanged(nameof(IsStopButtonEnabled));
        OnPropertyChanged(nameof(IsRestartButtonEnabled));
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [RelayCommand]
    private async System.Threading.Tasks.Task StartTask() {
        Console.WriteLine("start task button click!");
        if (dbService != null)
            cacheService.RunningTask = await dbService.StartNewTaskAsnc(
                DescriptionTextboxText,
                null,
                new Worker { name = "new user" },
                null
            );
        AllBindingPropertiesChanged();
    }

    [RelayCommand]
    private async System.Threading.Tasks.Task StopTask() {
        Console.WriteLine("stop task button click!");
        if (dbService != null)
            cacheService.RunningTask = await dbService.FinishCurrentTaskAsync(
                cacheService.RunningTask?.start ?? DateTimeService.ToSeconds(DateTime.Now),
                DateTimeService.ToSeconds(DateTime.Now),
                DescriptionTextboxText,
                SelectedProject,
                null
            );
    }

    [RelayCommand]
    private void RestartTask() {
        Console.WriteLine("restart task button click! (not yet implemented)");
        AllBindingPropertiesChanged();
    }

    public void OnLoad() {
        Console.WriteLine("loading Visuals Sub Settings Page!");
        AllBindingPropertiesChanged();
    }
}