namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.Util;

public class DayGraphPanelViewModel : GraphPanelViewModelBase {

	public DayGraphPanelViewModel() : this(null, null, null) {
		
	}

	public DayGraphPanelViewModel(GraphPageViewModel? controller, ViewBase? model, IServiceProvider? services) : base(controller, model, services) {

	}
	
	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfDayAtDateAsync(dateTimeService.SelectedDay) : [];

	public override void OnClick(Database.Models.Task task) {
		Console.WriteLine("day graph panel model click");
	}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("day graph panel model double click");
	}
}