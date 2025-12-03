namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MonthGraphPanelViewModel : GraphPanelViewModelBase {

    public override int TASK_GRAPH_COLUMN_COUNT => 2;

    public override int MAX_TASKS => 70;

    public override int GRAPH_CLICK_ADDITIONAL_WIDTH => 6;
    public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 4;

    public override int GRAPH_MINIMAL_WIDTH => 2;
    public override int GRAPH_CORNER_RADIUS => 4;

    public override long TIME_INTERVALL_START_SECONDS => DateTimeService.ToSeconds(DateTimeService.FloorMonth(cacheService?.SelectedDay ?? DateTime.Now));
    public override long TIME_INTERVALL_FINISH_SECONDS => TIME_INTERVALL_START_SECONDS + TimeSpan.SecondsPerDay * DateTimeService.DaysInCurrentMonth() - 1;

    public override int X_AXIS_SEGMENT_COUNT =>
        DateTime.DaysInMonth(
            cacheService.SelectedDay.Year,
            cacheService.SelectedDay.Month
        );
    public override int Y_AXIS_SEGMENT_COUNT => MAX_TASKS;

    public override double TASK_DESCRIPTION_GRAPH_SPACE => 5;
    public override double TASK_DESCRIPTION_FONT_SIZE => 10;

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

    public override void PreviusIntervallClick() {
		DateTime previousMonth = DateTimeService.FloorMonth(cacheService.SelectedDay).AddDays(-1);
        previousMonth = DateTimeService.FloorMonth(previousMonth);
		cacheService.SelectedDay = previousMonth;
    }

    public override void FollowingIntervallClick() {
        DateTime nextMonth = DateTimeService.FloorMonth(cacheService.SelectedDay);
        nextMonth = DateTimeService.FloorMonth(nextMonth.AddDays(DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month) + 1));
        cacheService.SelectedDay = nextMonth;
    }

    public override void SetTimeIntervallBlocked(string reason) {

    }

    public override void SetTimeIntervallUnblocked() {

    }
}