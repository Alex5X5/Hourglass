namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

using Hourglass.Database;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util.Services;

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
	protected ComponentViewModelFactory<TaskGraphViewModel> graphFactory;

    public string Title => GetTitle();
	
    public GraphPanelViewModelBase() : this(null, null, null, null, null, null) {
    }
	
	public GraphPanelViewModelBase(ComponentViewModelFactory<TaskGraphViewModel> graphFactory, IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, Services.CacheService cacheService) : base() {
		this.graphFactory = graphFactory;
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.panelController = panelController;
		this.pageController = pageController;
		this.cacheService = cacheService;

		CurrentTasks = new ObservableCollection<TaskGraphViewModel>();
		List<Database.Models.Task> tasks = GetTasksAsync().Result;

		int skippedCounter = 0;
		for(int i=0; i<tasks.Count; i++) {
			if (tasks[i].blocksTime != BlockedTimeIntervallType.None) {
				skippedCounter++;
				continue;
			}
			int i_ = i - skippedCounter;
			Console.WriteLine($"add loop i_:{i_} count:{tasks.Count} len:{CurrentTasks.Count}");
			Dictionary<string, object?> data = new Dictionary<string, object?> {
				{ nameof(TaskGraphViewModel.Task), tasks[i] }
			};
            CurrentTasks.Add(
				graphFactory.GetComponentViewModel(
					null,
					new Dictionary<string, object?> {
						{ nameof(TaskGraphViewModel.Task), tasks[i_] },
						{ nameof(TaskGraphViewModel.Index), i_ }
					}
				)
			);
		}

		//int count = CurrentTasks.Count;
		//for (int i=0; i< count; i++) {
		//	Console.WriteLine($"remove loop i:{i} count:{count} len:{CurrentTasks.Count}");
		//	TaskGraphViewModel task = CurrentTasks[i];
		//	Console.WriteLine($"removing task {task}");
		//	Task.Run(
		//		async ()=> {
		//			await Task.Delay(1000);
		//			await RemoveItem(task);
		//		}
		//	);
		//}
		MarkedColumns = new bool[32];
		BlockedColumns = new bool[32];
		for (int i=0; i<X_AXIS_SEGMENT_COUNT; i++) {
			MarkedColumns[i] = false;
        }
    }

	public async Task RemoveItem(TaskGraphViewModel item) {
		Dispatcher.UIThread.Invoke(() => item.IsRemoving = true);
		await Task.Delay(1000);
		Dispatcher.UIThread.Invoke(() => CurrentTasks.Remove(item));
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
	}

	public void FollowingIntervallClickBase() {
		FollowingIntervallClick();
		UpdateColumnMarkers();
	}

	protected abstract void PreviusIntervallClick();
	protected abstract void FollowingIntervallClick();
}
