namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.Util;

public class WeekGraphPanelViewModel : GraphPanelViewModelBase {

	public WeekGraphPanelViewModel() : this(null, null, null) {
		
	}

	public WeekGraphPanelViewModel(GraphPageViewModel? controller, ViewBase? model, IServiceProvider? services) : base(controller, model, services) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfWeekAtDateAsync(dateTimeService.SelectedDay) : [];

	public override void OnClick(Database.Models.Task task) {
		Console.WriteLine("week graph panel model click");
	}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("week graph panel model double click");
		dateTimeService.SelectedDay = DateTimeService.FloorDay(clickedTime);
		controller.ChangeGraphPanel<DayGraphPanelViewModel>();
	}
}