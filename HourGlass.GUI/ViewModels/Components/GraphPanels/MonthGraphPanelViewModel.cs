namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

public class MonthGraphPanelViewModel : GraphPanelViewModelBase {

	public MonthGraphPanelViewModel() : this(null, null, null, null, null) {

	}

	public MonthGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, CacheService cacheService)
		: base(dbService, dateTimeService, panelController, pageController, cacheService) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfMonthAtDateAsync(dateTimeService?.SelectedDay ?? DateTime.Now) : [];

	//public override void OnClick(Database.Models.Task task) {
	//	pageController.ChangePage<TaskDetailsPageViewModel>();
	//	Console.WriteLine("month graph panel model click");
	//}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("month graph panel model double click");
		if (dateTimeService != null)
			dateTimeService.SelectedDay = DateTimeService.FloorWeek(clickedTime);
		panelController?.ChangeGraphPanel<WeekGraphPanelViewModel>();
	}
}