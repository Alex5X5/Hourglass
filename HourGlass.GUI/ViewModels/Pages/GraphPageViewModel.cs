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
			new DayGraphPanelViewModel(Services?.GetService<DayGraphPanelView>(), services),
			new WeekGraphPanelViewModel(Services?.GetService<WeekGraphPanelView>(), services),
			new MonthGraphPanelViewModel(Services?.GetService<MonthGraphPanelView>(), services)
		];
		_CurrentGraphPanel = GraphPanels[0];
	}

	public override void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider? services) {
		base.OnFinishedRegisteringViews(views, services);
		foreach (var panel in GraphPanels)
			panel.OnFinishedRegisteringViews(views, services);
	}

	public void ChangeGraphPanel<PanelT>() where PanelT : GraphPanelViewModelBase {
		CurrentGraphPanel = GraphPanels.First(x => x.GetType() == typeof(PanelT));
		Console.WriteLine($"chaged type of panel to:{_CurrentGraphPanel.GetType().Name}");
	}
}