namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MonthGraphPanelViewModel : GraphPanelViewModelBase {
	
	public MonthGraphPanelViewModel() : this(null, null, null, null, null) {

	}

	public MonthGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, CacheService cacheService)
		: base(dbService, dateTimeService, panelController, pageController, cacheService) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfMonthAtDateAsync(cacheService.SelectedDay) : [];

	//public override void OnClick(Database.Models.Task task) {
	//	pageController.ChangePage<TaskDetailsPageViewModel>();
	//	Console.WriteLine("month graph panel model click");
	//}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("month graph panel model double click");
		if (dateTimeService != null)
			cacheService.SelectedDay = DateTimeService.FloorWeek(clickedTime);
		panelController?.ChangeGraphPanel<WeekGraphPanelViewModel>();
	}

	protected override string GetTitle() {
		string month = cacheService.SelectedDay.Month switch {
			1 => "January",
			2 => "February",
			3 => "March",
			4 => "April",
			5 => "May",
			6 => "June",
			7 => "July",
			8 => "August",
			9 => "September",
			10 => "October",
			11 => "November",
			12 => "December",
			_ => "Weekend"
		};
		return $"{month}  {cacheService.SelectedDay.Year}";
    }

    protected override void PreviusIntervallClick() {
    }

    protected override void FollowingIntervallClick() {
    }
}