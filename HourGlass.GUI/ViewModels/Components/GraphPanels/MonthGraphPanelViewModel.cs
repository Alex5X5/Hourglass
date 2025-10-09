namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.Util;

public class MonthGraphPanelViewModel : GraphPanelViewModelBase {

	public MonthGraphPanelViewModel() : this(null, null, null) {

	}

	public MonthGraphPanelViewModel(GraphPageViewModel? controller, ViewBase? model, IServiceProvider? services) : base(controller, model, services) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfMonthAtDateAsync(dateTimeService.SelectedDay) : [];

	public override void OnClick(Database.Models.Task task) {
		Console.WriteLine("month graph panel model click");
	}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("month graph panel model double click");
		dateTimeService.SelectedDay = DateTimeService.FloorWeek(clickedTime);
		graphPageViewModel.ChangeGraphPanel<WeekGraphPanelViewModel>();
	}
}