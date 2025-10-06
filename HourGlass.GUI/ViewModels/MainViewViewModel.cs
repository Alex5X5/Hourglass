namespace Hourglass.GUI.ViewModels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.Util;

using Microsoft.Extensions.DependencyInjection;

using ReactiveUI;

public class MainViewViewModel : ViewModelBase {

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
	
	public MainViewViewModel() : this(null, null) {
	}

	public MainViewViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		Pages = [
			Services?.GetRequiredService<TimerPageViewModel>() ?? new TimerPageViewModel(),
			Services?.GetRequiredService<GraphPageViewModel>() ?? new GraphPageViewModel(),
			Services?.GetRequiredService<ProjectPageViewModel>() ?? new ProjectPageViewModel(),
			Services?.GetRequiredService<ExportPageViewModel>() ?? new ExportPageViewModel()
		];
		_CurrentPage = Pages[0];
		
	}

	public void ChangePage<PageT>() where PageT : PageViewModelBase {
		CurrentPage = Pages.First(x => x.GetType() == typeof(PageT));
		Console.WriteLine($"chaged type of page to:{_CurrentPage.GetType().Name}");
		Console.WriteLine($"new page is {_CurrentPage.GetType().IsVisible} visible");
	}
}

