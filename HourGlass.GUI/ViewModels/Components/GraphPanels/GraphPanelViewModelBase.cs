namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract partial class GraphPanelViewModelBase : ViewModelBase {

	public abstract int TASK_GRAPH_COLUMN_COUNT { get; }

	public abstract int MAX_TASKS { get; }

	public abstract int GRAPH_CLICK_ADDITIONAL_WIDTH { get; }
	public abstract int GRAPH_CLICK_ADDITIONAL_HEIGHT { get; }

	public abstract int GRAPH_MINIMAL_WIDTH { get; }
	public abstract int GRAPH_CORNER_RADIUS { get; }

	public abstract long TIME_INTERVALL_START_SECONDS { get; }
	public abstract long TIME_INTERVALL_FINISH_SECONDS { get; }
	public long TIME_INTERVALL_DURATION => TIME_INTERVALL_FINISH_SECONDS - TIME_INTERVALL_START_SECONDS;
	public long X_AXIS_SEGMENT_DURATION => TIME_INTERVALL_DURATION / X_AXIS_SEGMENT_COUNT;

	public abstract int X_AXIS_SEGMENT_COUNT { get; }
	public abstract int Y_AXIS_SEGMENT_COUNT { get; }

	public abstract double TASK_DESCRIPTION_GRAPH_SPACE { get; }
	public abstract double TASK_DESCRIPTION_FONT_SIZE { get; }

	public const double GRAPH_AREA_X_WEIGHT = 28;
	public const double GRAPH_AREA_Y_WEIGHT = 28;
	public const double PADDING_X_WEIGHT = 1;
	public const double PADDING_Y_WEIGHT = 1;

	public GridLength GraphAreaXWeight { get; } = new GridLength(GRAPH_AREA_X_WEIGHT, GridUnitType.Star);
	public GridLength GraphAreaYWeight { get; } = new GridLength(GRAPH_AREA_Y_WEIGHT, GridUnitType.Star);
	public GridLength PaddingXWeight { get; } = new GridLength(PADDING_X_WEIGHT, GridUnitType.Star);
	public GridLength PaddingYWeight { get; } = new GridLength(PADDING_Y_WEIGHT, GridUnitType.Star);

	public bool[] MarkedColumns;

	public IHourglassDbService dbService { set; get; }
	public DateTimeService dateTimeService { set; get; }
	public CacheService cacheService;

	public GraphPageViewModel panelController;
	protected MainViewModel pageController;

	public string Title => GetTitle();

	public GraphPanelViewModelBase() : this(null, null, null, null, null) {

	}
	
	public GraphPanelViewModelBase(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, CacheService cacheService) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.panelController = panelController;
		this.pageController = pageController;
		this.cacheService = cacheService;

		MarkedColumns = new bool[32];
		for(int i=0; i<X_AXIS_SEGMENT_COUNT; i++)
			MarkedColumns[i] = false;
	}

	public void OnMouseDragging(Avalonia.Rect dragRect, double width, double paddingX) {
		double leftRectBound = dragRect.X - paddingX;
		double rightRectBound = leftRectBound + dragRect.Width;
		for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++) {
			double leftSegmentBound = width * i / X_AXIS_SEGMENT_COUNT;
			double rightSegmentBound = width * (i + 1) / X_AXIS_SEGMENT_COUNT;
			MarkedColumns[i] = false;
			if(rightRectBound < leftSegmentBound)
				continue;
			if(leftRectBound > rightSegmentBound)
				continue;
            MarkedColumns[i] = true;
        }
	}
	
	public void OnMouseMoved() {
		for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++)
			MarkedColumns[i] = false;
    }

    public abstract Task<List<Database.Models.Task>> GetTasksAsync();

	public virtual void OnTaskClicked(Database.Models.Task task) {
		cacheService.SelectedTask = task;
		pageController.GoToTaskdetails(task);
	}

    public virtual void OnMissingContextMenuSickClicked() {
		SetTimeIntervallBlocked("Krank");
    }

    public virtual void OnMissingContextMenuSchoolClicked() {
		SetTimeIntervallBlocked("Berufsschule");
    }

    public virtual void MissingContextMenuVacantClicked() {
        SetTimeIntervallBlocked("Urlaub");
    }

    public virtual void MissingContextMenuNoExcuseClicked() {
        SetTimeIntervallBlocked("Unentschuldigt");
    }

    public virtual void MissingContextMenuPresentClicked() {
        SetTimeIntervallUnblocked();
    }

	public abstract void SetTimeIntervallBlocked(string reason);
	public abstract void SetTimeIntervallUnblocked();

    public abstract void OnDoubleClick(DateTime clickedTime);

	protected abstract string GetTitle();

	public abstract void PreviusIntervallClick();

	public abstract void FollowingIntervallClick();
}
