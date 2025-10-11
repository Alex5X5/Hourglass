namespace Hourglass.GUI.ViewModels;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

using ReactiveUI;

public partial class MainViewModel : ViewModelBase {

	private PageViewModelBase _CurrentPage;
	public PageViewModelBase CurrentPage {
        get { return _CurrentPage; }
        private set {
			Console.WriteLine($"settin current page to {value.GetType().Name}");
			this.RaiseAndSetIfChanged(ref _CurrentPage, value);
		}
	}

	public Database.Models.Task? RunningTask { set; get; }

	private readonly ViewModelFactory<PageViewModelBase>? pageFactory;
	private IHourglassDbService dbService;
	private DateTimeService dateTimeService;
	
	public MainViewModel() {
		
	}

	public MainViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, ViewModelFactory<PageViewModelBase> pageFactory) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.pageFactory = pageFactory;
		RunningTask = dbService.QueryCurrentTaskAsync().Result;
		
		CurrentPage = pageFactory.GetPageViewModel<TimerPageViewModel>();
	}

	public void ChangePage<PageT>() where PageT : PageViewModelBase {
		if (pageFactory == null)
			return;
		CurrentPage = pageFactory.GetPageViewModel<PageT>();
		Console.WriteLine($"chaged type of page to:{_CurrentPage.GetType().Name}");
		Console.WriteLine($"new page is {_CurrentPage.GetType().IsVisible} visible");
	}

	[RelayCommand]
	private void GoToTimer() {
		Console.WriteLine("timer mode button click!");
		ChangePage<TimerPageViewModel>();
	}

	[RelayCommand]
	private void GoToGraphs() {
		Console.WriteLine("graph mode button click!");
		ChangePage<GraphPageViewModel>();
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
		if (CurrentPage is TaskDetailsPageViewModel model) {
			model.SelectedTask = task;
			model.UpdateTextFields();
		}
	}
}

