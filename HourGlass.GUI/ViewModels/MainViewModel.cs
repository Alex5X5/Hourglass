namespace Hourglass.GUI.ViewModels;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Pages;
using Hourglass.Util;

using Microsoft.Extensions.DependencyInjection;

using ReactiveUI;

public partial class MainViewModel : ViewModelBase {

	private IHourglassDbService dbService;

	public DateTime SelectedDay {
		set => SelectedDayStartSeconds = value.Ticks / TimeSpan.TicksPerSecond;
		get => new(SelectedDayStartSeconds * TimeSpan.TicksPerSecond);
	}

	private long SelectedDayStartSeconds = DateTimeService.FloorDay(DateTime.Today).Ticks / TimeSpan.TicksPerSecond;

	public PageViewModelBase CurrentPage {
        get { return _CurrentPage; }
        private set {
			Console.WriteLine($"settin current page to {value.GetType().Name}");
			this.RaiseAndSetIfChanged(ref _CurrentPage, value);
		}
	}

	private readonly PageViewModelBase[] Pages;

	private PageViewModelBase _CurrentPage;
	
	public MainViewModel() : this(null, null) {
		
	}

	public MainViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		dbService = (IHourglassDbService)services?.GetService(typeof(HourglassDbService));
		Database.Models.Task? task = dbService?.QueryCurrentTaskAsync().Result;
		Pages = 
		//[
		//	Services?.GetRequiredService<TimerPageViewModel>() ?? new TimerPageViewModel(),
		//	Services?.GetRequiredService<GraphPageViewModel>() ?? new GraphPageViewModel(),
		//	Services?.GetRequiredService<ProjectPageViewModel>() ?? new ProjectPageViewModel(),
		//	Services?.GetRequiredService<ExportPageViewModel>() ?? new ExportPageViewModel()
		//];
		[
			new TimerPageViewModel(Services?.GetService<TimerPageView>(), services) { RunningTask = task },
			new GraphPageViewModel(Services?.GetService<GraphPageView>(), services) { RunningTask = task },
			new ProjectPageViewModel(Services?.GetService<ProjectPageView>(), services) { RunningTask = task },
			new ExportPageViewModel(Services?.GetService<ExportPageView>(), services) { RunningTask = task }
		];
		_CurrentPage = Pages[0];

	}

	public override void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider? services) {
		base.OnFinishedRegisteringViews(views, services);
		foreach (var page in Pages)
			page.OnFinishedRegisteringViews(views, services);
	}

	public void ChangePage<PageT>() where PageT : PageViewModelBase {
		CurrentPage = Pages.First(x => x.GetType() == typeof(PageT));
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
}

