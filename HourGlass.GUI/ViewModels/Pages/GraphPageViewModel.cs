using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;
using Hourglass.GUI.Views.Pages;
using Hourglass.PDF;

using Microsoft.Extensions.DependencyInjection;

using ReactiveUI;

namespace Hourglass.GUI.ViewModels.Pages;

public class GraphPageViewModel : PageViewModelBase {
	
	private GraphPanelViewModelBase _CurrentGraphPanel;
	public GraphPanelViewModelBase CurrentGraphPanel {
		get => _CurrentGraphPanel;
		private set {
			Console.WriteLine($"settin current page to {value.GetType().Name}");
			this.RaiseAndSetIfChanged(ref _CurrentGraphPanel, value);
		}
	}

	private readonly GraphPanelViewModelBase[] GraphPanels;

	public GraphPageViewModel() : this(null, null) {

	}

	public GraphPageViewModel(MainViewModel? controller, IServiceProvider? services) : base(controller, services) {
		GraphPanels = [
			new DayGraphPanelViewModel(controller, this, services),
			new WeekGraphPanelViewModel(controller, this, services),
			new MonthGraphPanelViewModel(controller, this, services)
		];
		_CurrentGraphPanel = GraphPanels[0];
	}

	public void ChangeGraphPanel<PanelT>() where PanelT : GraphPanelViewModelBase {
		CurrentGraphPanel = GraphPanels.First(x => x.GetType() == typeof(PanelT));
		Console.WriteLine($"chaged type of panel to:{_CurrentGraphPanel.GetType().Name}");
	}
}