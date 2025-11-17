namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class DayGraphPanelViewModel : GraphPanelViewModelBase {

	public DayGraphPanelViewModel() : base() {

	}

	public DayGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, MainViewModel pageController, CacheService cacheService)
		: base(dbService, dateTimeService, null, pageController, cacheService) { }

	public DayGraphPanelViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, CacheService cacheService)
		: base(dbService, dateTimeService, panelController, pageController, cacheService) { }

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfDayAtDateAsync(cacheService.SelectedDay) : [];

	//public override void OnClick(Database.Models.Task task) {
	//	pageController.ChangePage<TaskDetailsPageViewModel>();
	//	Console.WriteLine("day graph panel model click");
	//}

	public override void OnDoubleClick(DateTime clickedTime) {
		Console.WriteLine("day graph panel model double click");
	}

	protected override string GetTitle() {
		string day = cacheService.SelectedDay.DayOfWeek switch {
			DayOfWeek.Monday => "Monday",
			DayOfWeek.Tuesday => "Tuesday",
			DayOfWeek.Wednesday => "Wednesday",
			DayOfWeek.Thursday => "Thursday",
			DayOfWeek.Friday => "Friday",
			_ => "Weekend"
		};
		string date = DateTimeService.ToDayAndMonthString(cacheService.SelectedDay);
		return $"{day}  {date}";
	}

    protected override void PreviusIntervallClick() {
        throw new NotImplementedException();
    }

    protected override void FollowingIntervallClick() {
        throw new NotImplementedException();
    }
}