namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Avalonia.Controls;
using Avalonia.Threading;

using Hourglass.Database;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util.Services;

using ReactiveUI;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public abstract partial class GraphPanelViewModelBase : ViewModelBase {

	public abstract int TASK_GRAPH_COLUMN_COUNT { get; }

	public abstract int MAX_TASKS { get; }

	public abstract int GRAPH_CLICK_ADDITIONAL_WIDTH { get; }
	public abstract int GRAPH_CLICK_ADDITIONAL_HEIGHT { get; }

	public abstract int GRAPH_MINIMAL_WIDTH { get; }
	public abstract int GRAPH_CORNER_RADIUS { get; }

	public abstract long TIME_INTERVALL_START_SECONDS { get; }
	public long TIME_INTERVALL_FINISH_SECONDS => TIME_INTERVALL_START_SECONDS + TIME_INTERVALL_DURATION;
	public long TIME_INTERVALL_DURATION => X_AXIS_SEGMENT_DURATION * X_AXIS_SEGMENT_COUNT;
	public abstract long X_AXIS_SEGMENT_DURATION { get; }

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
	public bool[] BlockedColumns;

	public bool TransitionRunning;

	public event Action<int> NotifyTransitionStep = (step) => { };
	public int TransitionStep { get; private set; } = 0;
	

	public ObservableCollection<TaskGraphViewModel> CurrentTasks {
		get;
		set;
	}
	public ObservableCollection<TaskGraphViewModel> PhasingOutTasks {
		get;
		set;
	}


	public IHourglassDbService dbService { set; get; }
	public DateTimeService dateTimeService { set; get; }
	public Services.CacheService cacheService;

	public GraphPageViewModel panelController;
	protected MainViewModel pageController;

	public string Title => GetTitle();

	private string _rowDefinitions;
	public string RowDefinitions {
		get => _rowDefinitions;
		set => this.RaiseAndSetIfChanged(ref _rowDefinitions, value);
	}

	public GraphPanelViewModelBase() : this(null, null, null, null, null) {

	}
	
	public GraphPanelViewModelBase(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, Services.CacheService cacheService) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.panelController = panelController;
		this.pageController = pageController;
		this.cacheService = cacheService;

		CurrentTasks = new ObservableCollection<TaskGraphViewModel>();
		CurrentTasks.Add(
			new TaskGraphViewModel(
				dbService.QueryCurrentTaskAsync().Result,
				TIME_INTERVALL_DURATION,
				TIME_INTERVALL_START_SECONDS,
				0
			)
		);
			//new TaskGraphViewModel(, TIME_INTERVALL_START_SECONDS, TIME_INTERVALL_DURATION)
		
		//Dispatcher.UIThread.InvokeAsync(
		//	async () => {
		//		await Task.Delay(1000);
		//		await RemoveItem(CurrentTasks[0]);
		//	}
		//);

		MarkedColumns = new bool[32];
		BlockedColumns = new bool[32];
		for (int i=0; i<X_AXIS_SEGMENT_COUNT; i++) {
			MarkedColumns[i] = false;
		}
		RowDefinitions = string.Join("*", Enumerable.Repeat(",2*,*", Y_AXIS_SEGMENT_COUNT));
	}

	public async Task RemoveItem(TaskGraphViewModel item) {
		item.IsRemoving = true;
		await Task.Delay(2000);
		CurrentTasks.Remove(item);
	}

	public void UpdateColumnMarkers() {
		long start = TIME_INTERVALL_START_SECONDS;
		long finish = start + X_AXIS_SEGMENT_DURATION;
		List<Database.Models.Task> tasks = dbService.QueryBlockingTasksInIntervallAsync(TIME_INTERVALL_START_SECONDS, TIME_INTERVALL_FINISH_SECONDS).Result;
		for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++) {
			BlockedColumns[i] = tasks
				.Where(x => x.start >= start && x.start <= finish)
					.Where(x => x.finish >= start && x.finish <= finish)
						.FirstOrDefault() != null;
			start += X_AXIS_SEGMENT_DURATION;
			finish += X_AXIS_SEGMENT_DURATION;
		}
	}

	public abstract Task<List<Database.Models.Task>> GetTasksAsync();

	protected abstract string GetTitle();
	public virtual void OnTaskClicked(Database.Models.Task task) {
		cacheService.SelectedTask = task;
		pageController.GoToTaskdetails(task);
	}

	public void OnMouseDragging(Avalonia.Rect dragRect, double width, double paddingX) {
		double leftRectBound = dragRect.X - paddingX;
		double rightRectBound = leftRectBound + dragRect.Width;
		for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++) {
			double leftSegmentBound = width * i / X_AXIS_SEGMENT_COUNT;
			double rightSegmentBound = width * (i + 1) / X_AXIS_SEGMENT_COUNT;
			MarkedColumns[i] = false;
			if (rightRectBound < leftSegmentBound)
				continue;
			if (leftRectBound > rightSegmentBound)
				continue;
			MarkedColumns[i] = true;
		}
	}

	public async Task SetTimeIntervallBlocked(BlockedTimeIntervallType reason) {
		if (reason == BlockedTimeIntervallType.None) {
			await SetTimeIntervallUnblocked();
			return;
		}
		long start = TIME_INTERVALL_START_SECONDS;
		long finish = start + X_AXIS_SEGMENT_DURATION;
		List<Database.Models.Task> tasks = dbService.QueryBlockingTasksInIntervallAsync(TIME_INTERVALL_START_SECONDS, TIME_INTERVALL_FINISH_SECONDS).Result;
		for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++) {
			if (MarkedColumns[i]) {
				IEnumerable<Database.Models.Task> tasks_ = tasks
					.Where(x => x.start >= start && x.start <= finish)
						.Where(x => x.finish >= start && x.finish <= finish);
				if (!tasks_.Any()) {
					await dbService.CreateIntervallBlockingTaskAsync(reason, new DateTime(start*TimeSpan.TicksPerSecond), X_AXIS_SEGMENT_DURATION);
				}
			}
			start += X_AXIS_SEGMENT_DURATION;
			finish += X_AXIS_SEGMENT_DURATION;
		}
	}

	public async Task SetTimeIntervallUnblocked() {
		long start = TIME_INTERVALL_START_SECONDS;
		long finish = start + X_AXIS_SEGMENT_DURATION;
		List<Database.Models.Task> tasks = dbService.QueryBlockingTasksInIntervallAsync(TIME_INTERVALL_START_SECONDS, TIME_INTERVALL_FINISH_SECONDS).Result;
		for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++) {
			if (MarkedColumns[i]) {
				IEnumerable<Database.Models.Task> tasks_ = tasks
					.Where(x => x.start >= start && x.start <= finish)
						.Where(x => x.finish >= start && x.finish <= finish);
				foreach (var task in tasks_)
					await dbService.DeleteTaskAsync(task);
			}
			start += X_AXIS_SEGMENT_DURATION;
			finish += X_AXIS_SEGMENT_DURATION;
		}
	}

	public void OnMouseMoved() {

	}

	public void OnMousePressed(bool isLeftDown, bool isRightdown) {
		if (!isRightdown)
			for (int i = 0; i < X_AXIS_SEGMENT_COUNT; i++)
				MarkedColumns[i] = false;
	}

	public async Task OnMissingContextMenuClicked(BlockedTimeIntervallType reason) {
		await SetTimeIntervallBlocked(reason);
		UpdateColumnMarkers();
	}

	public abstract void OnDoubleClick(DateTime clickedTime);

	public void OnLoad() {
		UpdateColumnMarkers();
	}
	
	public void PreviusIntervallClickBase() {
		PreviusIntervallClick();
		UpdateColumnMarkers();
		//PhasingOutTasks = GetTasksAsync().Result;
	}

	public void FollowingIntervallClickBase() {
		FollowingIntervallClick();
		UpdateColumnMarkers();
		NotifyTransitionStep(1);
		//PhasingOutTasks = GetTasksAsync().Result;
	}

	protected abstract void PreviusIntervallClick();
	protected abstract void FollowingIntervallClick();
}
