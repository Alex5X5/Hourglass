namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.Util;

public class WeekGraphPanelViewModel : GraphPanelViewModelBase {

	public WeekGraphPanelViewModel() : this(null, null, null) {
		
	}

	public WeekGraphPanelViewModel(MainViewModel? controller, GraphPageViewModel? panelController, IServiceProvider? services) : base(controller, panelController, services) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfWeekAtDateAsync(dateTimeService?.SelectedDay ?? DateTime.Now) : [];

	public override void OnClick(Database.Models.Task task) {
		controller?.ChangePage<TaskDetailsPageViewModel>();
		Console.WriteLine("week graph panel model click");
	}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("week graph panel model double click");
		if(dateTimeService!=null)
			dateTimeService.SelectedDay = DateTimeService.FloorDay(clickedTime);
		panelController?.ChangeGraphPanel<DayGraphPanelViewModel>();
	}
}