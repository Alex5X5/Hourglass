using Hourglass.GUI.ViewModels.Components.GraphPanels;

using ReactiveUI;

namespace Hourglass.GUI.ViewModels.Pages;

public class GraphPageViewModel : PageViewModelBase {

	private GraphPanelViewModelBase _CurrentGraphPanel;
	public GraphPanelViewModelBase CurrentGraphPanel {
		get {
			return _CurrentGraphPanel;
		}
		private set {
			Console.WriteLine($"settin current page to {value.GetType().Name}");
			this.RaiseAndSetIfChanged(ref _CurrentGraphPanel, value);
		}
	}

	private readonly GraphPanelViewModelBase[] GraphPanels;

	public GraphPageViewModel() : base() {
		GraphPanels = [
			new DayGraphPanelViewModel() { dbService = dbService },
			new WeekGraphPanelViewModel() { dbService = dbService },
			new MonthGraphPanelViewModel() { dbService = dbService }
		];
		_CurrentGraphPanel = GraphPanels[0];
	}

	public void ChangeGraphPanel<PanelT>() where PanelT : GraphPanelViewModelBase {
		CurrentGraphPanel = GraphPanels.First(x => x.GetType() == typeof(PanelT));
		Console.WriteLine($"chaged type of panel to:{_CurrentGraphPanel.GetType().Name}");
	}
}