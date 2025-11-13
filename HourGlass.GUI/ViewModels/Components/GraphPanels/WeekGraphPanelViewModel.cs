namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WeekGraphPanelViewModel : GraphPanelViewModelBase {

	public WeekGraphPanelViewModel() : this(null, null, null, null, null) {

	}

	public WeekGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, TimerCacheService cacheService)
		: base(dbService, dateTimeService, panelController, pageController, cacheService) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfWeekAtDateAsync(dateTimeService?.SelectedDay ?? DateTime.Now) : [];

	//public override void OnClick(Database.Models.Task task) {
	//	pageController.ChangePage<TaskDetailsPageViewModel>();
	//	Console.WriteLine("week graph panel model click");
	//}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("week graph panel model double click");
		if(dateTimeService!=null)
			dateTimeService.SelectedDay = DateTimeService.FloorDay(clickedTime);
		panelController?.ChangeGraphPanel<DayGraphPanelViewModel>();
	}

	protected override string GetTitle() {
		if (dateTimeService?.SelectedDay == null)
			return "no day selected";
		int week = dateTimeService.GetWeekCountAtDate(dateTimeService!.SelectedDay);
		string startDate = DateTimeService.ToDayAndMonthString(dateTimeService!.SelectedDay);
		string endDate = DateTimeService.ToDayAndMonthString(dateTimeService!.SelectedDay.AddDays(5));
		return $"KW {week}  {startDate}-{endDate}";
	}
}