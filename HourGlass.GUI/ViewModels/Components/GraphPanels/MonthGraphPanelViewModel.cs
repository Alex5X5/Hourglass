namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

public class MonthGraphPanelViewModel : GraphPanelViewModelBase {

	public MonthGraphPanelViewModel() : this(null, null, null) {

	}

	public MonthGraphPanelViewModel(MainViewModel? controller, GraphPageViewModel? panelController, IServiceProvider? services) : base(controller, panelController, services) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfMonthAtDateAsync(dateTimeService?.SelectedDay ?? DateTime.Now) : [];

	public override void OnClick(Database.Models.Task task) {
		controller?.ChangePage<TaskDetailsPageViewModel>();
		Console.WriteLine("month graph panel model click");
	}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("month graph panel model double click");
		if (dateTimeService != null)
			dateTimeService.SelectedDay = DateTimeService.FloorWeek(clickedTime);
		graphPageViewModel?.ChangeGraphPanel<WeekGraphPanelViewModel>();
	}
}