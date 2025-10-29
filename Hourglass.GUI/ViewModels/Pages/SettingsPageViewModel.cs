namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;

using ReactiveUI;

using System.ComponentModel;

public partial class SettingsPageViewModel : PageViewModelBase, INotifyPropertyChanged {

    private IHourglassDbService dbService;
    private SettingsCacheService cacheService;
    private ViewModelFactory<SubSettingsPageViewModelBase> pageFactory;
    MainViewModel controller;

    private SubSettingsPageViewModelBase? _CurrentSubSettingsPage;
    public SubSettingsPageViewModelBase? CurrentPage {
        get { return _CurrentSubSettingsPage; }
        private set {
            Console.WriteLine($"settin current sub settings page to {value?.GetType()?.Name}");
            this.RaiseAndSetIfChanged(ref _CurrentSubSettingsPage, value);
            this.RaisePropertyChanged(nameof(Title));
        }
    }

    public override string Title => _CurrentSubSettingsPage?.Title ?? "";


    public new event PropertyChangedEventHandler? PropertyChanged;

    public event Action OnLoading = () => { };
    public event Action OnSave = () => { };

    public SettingsPageViewModel() : this(null, null, null, null) {
		
	}

	public SettingsPageViewModel(IHourglassDbService dbService, SettingsCacheService cacheService, ViewModelFactory<SubSettingsPageViewModelBase> pageFactory, MainViewModel controller) : base() {
        this.dbService = dbService;
        this.cacheService = cacheService;
        this.controller = controller;
        this.pageFactory = pageFactory;
        //if (cacheService != null)
        //    cacheService.OnRunningTaksChanged +=
        //        task => AllBindingPropertiesChanged();
    }

    private void AllBindingPropertiesChanged() {
        OnPropertyChanged(nameof(CurrentPage));
        controller.RaisePropertyChanged(nameof(controller.Title));
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [RelayCommand]
    public void Cancel() {
        controller.ShowNavigationBar = true;
        controller.ShowSettingsIcon = true;
        controller.GoBack();
    }

    [RelayCommand]
    public void Save() {
        foreach (Delegate act in OnSave.GetInvocationList())
            try {
                act.DynamicInvoke();
            } catch (Exception ex) {
                Console.WriteLine("an error occurred while invoking save settings subscribers: " + ex.Message);
            }
        
        Cancel();
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
        controller.ShowNavigationBar = false;
        controller.ShowSettingsIcon = false;
        foreach (Delegate act in OnLoading.GetInvocationList())
            try {
                act.DynamicInvoke();
            } catch (Exception ex) {
                Console.WriteLine("an error occurred while invoking OnLoading settings subscribers: " + ex.Message);
            }
    }

    public void ChangePage<PageT>(Action<PageT?>? afterCreation = null) where PageT : SubSettingsPageViewModelBase {
        if (pageFactory == null)
            return;
        CurrentPage = pageFactory.GetPageViewModel<PageT>(afterCreation);
        AllBindingPropertiesChanged();
        Console.WriteLine($"chaged type of sub settings page to:{_CurrentSubSettingsPage?.GetType()?.Name ?? ""}");
    }
}

