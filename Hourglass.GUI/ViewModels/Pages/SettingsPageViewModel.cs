namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;
using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;
using Hourglass.Util;

using ReactiveUI;

using System.ComponentModel;

public partial class SettingsPageViewModel : PageViewModelBase, INotifyPropertyChanged {

    private IHourglassDbService dbService;
    private CacheService cacheService;
    private ViewModelFactory<SubSettingsPageViewModelBase> pageFactory;
    MainViewModel controller;

    private SubSettingsPageViewModelBase _CurrentSubSettingsPage;
    public SubSettingsPageViewModelBase CurrentPage {
        get { return _CurrentSubSettingsPage; }
        private set {
            Console.WriteLine($"settin current sub settings page to {value?.GetType()?.Name}");
            this.RaiseAndSetIfChanged(ref _CurrentSubSettingsPage, value);
            this.RaisePropertyChanged(nameof(Title));
        }
    }

    public override string Title => _CurrentSubSettingsPage.Title;

    public bool IsStartButtonEnabled { get => cacheService?.RunningTask == null; }
    public bool IsStopButtonEnabled { get => cacheService?.RunningTask != null; }
    public bool IsRestartButtonEnabled { get => cacheService?.RunningTask != null; }


    public new event PropertyChangedEventHandler? PropertyChanged;

    public SettingsPageViewModel() : this(null, null, null, null) {
		
	}

	public SettingsPageViewModel(IHourglassDbService dbService, CacheService cacheService, ViewModelFactory<SubSettingsPageViewModelBase> pageFactory, MainViewModel controller) : base() {
        this.dbService = dbService;
        this.cacheService = cacheService;
        this.controller = controller;
        this.pageFactory = pageFactory;
        if (cacheService != null)
            cacheService.OnRunningTaksChanged +=
                task => AllBindingPropertiesChanged();
        controller.ShowNavigationBar = false;
    }

    private void AllBindingPropertiesChanged() {
        OnPropertyChanged(nameof(IsStartButtonEnabled));
        OnPropertyChanged(nameof(IsStopButtonEnabled));
        OnPropertyChanged(nameof(IsRestartButtonEnabled));
        OnPropertyChanged(nameof(CurrentPage));
        controller.RaisePropertyChanged(nameof(controller.Title));
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [RelayCommand]
    private void Cancel() {
        controller.ShowNavigationBar = true;
        controller.GoBack();
    }

    [RelayCommand]
    private void GoToAbout() {
        ChangePage<AboutSubSettingsPageViewModel>();
    }

    [RelayCommand]
    private void GoToVisuals() {
        ChangePage<VisualsSubSettingsPageViewModel>();
    }

    [RelayCommand]
    private void GoToUserData() {
        ChangePage<UserDataSubSettingsPageViewModel>();
    }

    [RelayCommand]
    private void GoToExport() {
        ChangePage<ExportSubSettingsPageViewModel>();
    }

    public void OnLoad() {
        Console.WriteLine("loading Settings Page");
        AllBindingPropertiesChanged();
    }

    public void ChangePage<PageT>(Action<PageT?>? afterCreation = null) where PageT : SubSettingsPageViewModelBase {
        if (pageFactory == null)
            return;
        CurrentPage = pageFactory.GetPageViewModel<PageT>(afterCreation);
        AllBindingPropertiesChanged();
        Console.WriteLine($"chaged type of sub settings page to:{_CurrentSubSettingsPage.GetType().Name}");
    }
}

