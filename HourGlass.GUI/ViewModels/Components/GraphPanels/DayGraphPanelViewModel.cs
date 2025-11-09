namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

public class DayGraphPanelViewModel : GraphPanelViewModelBase {

	public DayGraphPanelViewModel() : base() {

	}

	public DayGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, MainViewModel pageController, TimerCacheService cacheService)
		: base(dbService, dateTimeService, null, pageController, cacheService) { }

	public DayGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, TimerCacheService cacheService)
		: base(dbService, dateTimeService, panelController, pageController, cacheService) { }

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfDayAtDateAsync(dateTimeService?.SelectedDay ?? DateTime.Now) : [];

	//public override void OnClick(Database.Models.Task task) {
	//	pageController.ChangePage<TaskDetailsPageViewModel>();
	//	Console.WriteLine("day graph panel model click");
	//}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("day graph panel model double click");
	}

	protected override string GetTitle() {
		if (dateTimeService?.SelectedDay == null)
			return "no day selected";
		string day = dateTimeService!.SelectedDay.DayOfWeek switch {
			DayOfWeek.Monday => "Monday",
			DayOfWeek.Tuesday => "Tuesday",
			DayOfWeek.Wednesday => "Wednesday",
			DayOfWeek.Thursday => "Thursday",
			DayOfWeek.Friday => "Friday",
			_ => "Weekend"
		};
		string date = DateTimeService.ToDayAndMonthString(dateTimeService!.SelectedDay);
		return $"{day}  {date}";
	}
}