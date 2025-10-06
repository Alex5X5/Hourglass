using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Pages;
using Hourglass.PDF;

using Microsoft.Extensions.DependencyInjection;

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

	public GraphPageViewModel() : this(null, null) {

	}

	public GraphPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		GraphPanels = [
			Services?.GetRequiredService<DayGraphPanelViewModel>() ?? new DayGraphPanelViewModel(),
			Services?.GetRequiredService<WeekGraphPanelViewModel>() ?? new WeekGraphPanelViewModel(),
			Services?.GetRequiredService<MonthGraphPanelViewModel>() ?? new MonthGraphPanelViewModel()
		];
		_CurrentGraphPanel = GraphPanels[0];
	}

	public void ChangeGraphPanel<PanelT>() where PanelT : GraphPanelViewModelBase {
		CurrentGraphPanel = GraphPanels.First(x => x.GetType() == typeof(PanelT));
		Console.WriteLine($"chaged type of panel to:{_CurrentGraphPanel.GetType().Name}");
	}
}