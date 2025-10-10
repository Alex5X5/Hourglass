namespace Hourglass.GUI.ViewModels.Pages;

using Hourglass.GUI.ViewModels.Components.GraphPanels;
using ReactiveUI;

public class GraphPageViewModel : PageViewModelBase {
	
	private GraphPanelViewModelBase _CurrentGraphPanel;
	public GraphPanelViewModelBase CurrentGraphPanel {
		get => _CurrentGraphPanel;
		private set {
			Console.WriteLine($"settin current page to {value.GetType().Name}");
			this.RaiseAndSetIfChanged(ref _CurrentGraphPanel, value);
		}
	}

	private ViewModelFactory<GraphPanelViewModelBase> panelFactory;

	public GraphPageViewModel() : this(null) {

	}

	public GraphPageViewModel(ViewModelFactory<GraphPanelViewModelBase> panelFactory) : base() {
		this.panelFactory = panelFactory;
		Console.WriteLine("constructing graph page view model");
	}

	public void ChangeGraphPanel<PanelT>() where PanelT : GraphPanelViewModelBase {
		if (panelFactory == null)
			return;
		CurrentGraphPanel = panelFactory.GetPageViewModel<PanelT>();
		Console.WriteLine($"chaged type of panel to:{_CurrentGraphPanel.GetType().Name}");
	}
}