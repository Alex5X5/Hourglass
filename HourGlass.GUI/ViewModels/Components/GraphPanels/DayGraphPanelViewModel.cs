namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.Util;

public class DayGraphPanelViewModel : GraphPanelViewModelBase {

	public DayGraphPanelViewModel() : this(null, null, null) {
		
	}

	public DayGraphPanelViewModel(MainViewModel? controller, GraphPageViewModel? panelController, IServiceProvider? services) : base() {

	}
	
	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfDayAtDateAsync(dateTimeService?.SelectedDay ?? DateTime.Now) : [];

	public override void OnClick(Database.Models.Task task) {
		controller?.ChangePage<TaskDetailsPageViewModel>();
		Console.WriteLine("day graph panel model click");
	}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("day graph panel model double click");
	}
}