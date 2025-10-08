namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Avalonia.Input;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;
using Hourglass.Util;

public abstract class GraphPanelViewModelBase : ViewModelBase {

	protected GraphPageViewModel? controller;
	public DateTimeService? dateTimeService;
	public IHourglassDbService? dbService;

	public GraphPanelViewModelBase() : this(null, null, null) {

	}

	public GraphPanelViewModelBase(GraphPageViewModel? controller, ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		this.controller = controller;
		dbService = (IHourglassDbService?)Services?.GetService(typeof(HourglassDbService));
		dateTimeService = (DateTimeService?)services?.GetService(typeof(DateTimeService));
	}

	public override void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider? services) {
		base.OnFinishedRegisteringViews(views, services);
	}

	public async virtual Task<List<Database.Models.Task>> GetTasksAsync() =>
		await dbService.QueryTasksAsync() ?? [];

	public abstract void OnClick(Database.Models.Task task);

	public abstract void OnDoubleClick(DateTime clickedTime);

	public async void OnClickBase(Avalonia.Point mousePos, int xAxisSegmentCount, int xAxisSegmentDuration) {
		Console.WriteLine("base graph panel model click");
		List<Database.Models.Task>? tasks = await GetTasksAsync();
		if (owner is GraphPanelViewBase view) {
			bool taskClicked = false;
			int i = 0;
			foreach (Database.Models.Task task in tasks) {
				view.GetTaskRectanlge(task, view.GRAPH_CLICK_ADDITIONAL_WIDTH, view.GRAPH_CLICK_ADDITIONAL_HEIGHT, i).Contains(mousePos);
				if (taskClicked) {
					//TaskDetails.TaskDetailsPopup taskDetailsWindow = new(task, _dbService, _parent);
					//taskDetailsWindow.ShowDialog();
					Console.WriteLine("detected task!");
					break;
				}
				i++;
			}
		}
	}
}
	