namespace Hourglass.GUI.ViewModels;

using Hourglass.GUI.ViewModels.Pages;
using ReactiveUI;

public class MainViewViewModel : ViewModelBase {

	//public ViewState selectedWindow = ViewState.Timer;

	private PageViewModelBase _CurrentPage;
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

	public MainViewViewModel() : base() {
		_CurrentPage = Pages[0];
	}

	public void ChangePage<PageT>() where PageT : PageViewModelBase {
		CurrentPage = Pages.First(x => x.GetType() == typeof(PageT));
		Console.WriteLine($"chaged type of page to:{_CurrentPage.GetType().Name}");
		Console.WriteLine($"new page is {_CurrentPage.GetType().IsVisible} visible");
	}
}

