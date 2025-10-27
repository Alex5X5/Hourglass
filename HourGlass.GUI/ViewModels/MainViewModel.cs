namespace Hourglass.GUI.ViewModels;

using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;
using Hourglass.GUI.Views.Pages.SettingsPages;
using Hourglass.Util;

using ReactiveUI;

using System.ComponentModel;

public partial class MainViewModel : ViewModelBase,  INotifyPropertyChanged {

	private readonly ViewModelFactory<PageViewModelBase>? pageFactory;
	private IHourglassDbService dbService;
	private DateTimeService dateTimeService;

	private PageViewModelBase? lastPage;

	private PageViewModelBase _CurrentPage;
	public PageViewModelBase CurrentPage {
		get { return _CurrentPage; }
		private set {
			Console.WriteLine($"settin current page to {value?.GetType()?.Name}");
			this.RaiseAndSetIfChanged(ref _CurrentPage, value);
			this.RaisePropertyChanged(nameof(Title));
		}
	}

	public string Title { get => _CurrentPage.Title; }
	public new event PropertyChangedEventHandler? TitleChanged;

	public bool ShowNavigationBar {
		set {
			this.RaiseAndSetIfChanged(ref navigationBarHeight, value ? new GridLength(2, GridUnitType.Star) : new GridLength(0, GridUnitType.Star));
			this.RaisePropertyChanged(nameof(NavigationBarHeight));
		}
	}

	private GridLength navigationBarHeight = new GridLength(1, GridUnitType.Star);
	public GridLength NavigationBarHeight {
		get => navigationBarHeight;
	}


	private bool IsFirstGraphPageChange = true;
	
	public MainViewModel() : this(null, null, null) {
		
	}

	public MainViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, ViewModelFactory<PageViewModelBase> pageFactory) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.pageFactory = pageFactory;

		ShowNavigationBar = true;
		CurrentPage = pageFactory?.GetPageViewModel<TimerPageViewModel>();
	}

	public void ChangePage<PageT>(Action<PageT?>? afterCreation = null) where PageT : PageViewModelBase {
		if (pageFactory == null)
			return;
		lastPage = CurrentPage;
		CurrentPage = pageFactory.GetPageViewModel<PageT>(afterCreation);
		Console.WriteLine($"chaged type of page to:{_CurrentPage.GetType().Name}");
		Console.WriteLine($"new page is {_CurrentPage.GetType().IsVisible} visible");
	}

	public void GoBack() {
		if (lastPage != null) {
			CurrentPage = lastPage;
			lastPage = null;
		}
	}

	[RelayCommand]
	private void GoToSettings() {
		Console.WriteLine("timer mode button click!");
		ChangePage<SettingsPageViewModel>(
			page => {
				page.ChangePage<AboutSubSettingsPageViewModel>();
			}
		);
	}

	[RelayCommand]
	private void GoToTimer() {
		Console.WriteLine("timer mode button click!");
		ChangePage<TimerPageViewModel>();
	}

	[RelayCommand]
	private void GoToGraphs() {
		Console.WriteLine("graph mode button click!");
		ChangePage<GraphPageViewModel>(
			IsFirstGraphPageChange ? page=> {
				page.ChangeGraphPanel<DayGraphPanelViewModel>();
				IsFirstGraphPageChange = false;
			} : null
		);
	}

	[RelayCommand]
	private void GoToExport() {
		Console.WriteLine("export mode button click!");
		ChangePage<ExportPageViewModel>();
	}

	[RelayCommand]
	private void GoToProject() {
		Console.WriteLine("project mode button click!");
		ChangePage<ProjectPageViewModel>();
	}

	public void GoToTaskdetails(Database.Models.Task task) {
		if(pageFactory==null)
			return;
		ChangePage<TaskDetailsPageViewModel>();
	}

	protected void OnPropertyChanged(string propertyName) {
		TitleChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

