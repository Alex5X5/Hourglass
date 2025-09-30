namespace Hourglass.GUI.ViewModels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using ReactiveUI;

public class MainViewViewModel : ViewModelBase {
	
	IHourglassDbService? _DbService;
	public IHourglassDbService? DbService {
		get => _DbService;
		set {
			_DbService = value;
			foreach(PageViewModelBase model in Pages)
				model.dbService = value;
		}
	}

	public PageViewModelBase CurrentPage {
        get { return _CurrentPage; }
        private set {
			Console.WriteLine($"settin current page to {value.GetType().Name}");
			this.RaiseAndSetIfChanged(ref _CurrentPage, value);
		}
	}

	private readonly PageViewModelBase[] Pages = [
		new TimerPageViewModel(),
		new GraphPageViewModel(),
		new ProjectPageViewModel(),
		new ExportPageViewModel()
	];

	private PageViewModelBase _CurrentPage;

	public MainViewViewModel() : base() {
		_CurrentPage = Pages[0];
	}

	public void ChangePage<PageT>() where PageT : PageViewModelBase {
		CurrentPage = Pages.First(x => x.GetType() == typeof(PageT));
		Console.WriteLine($"chaged type of page to:{_CurrentPage.GetType().Name}");
		Console.WriteLine($"new page is {_CurrentPage.GetType().IsVisible} visible");
	}
}

