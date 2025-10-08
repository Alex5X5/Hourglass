namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Avalonia.Input;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;
using Hourglass.Util;

public abstract class GraphPanelViewModelBase : ViewModelBase {

	protected GraphPageViewModel? controller;
	public DateTimeService? dateTimeService;
	public IHourglassDbService? dbService;

	public GraphPanelViewModelBase() : this(null, null, null) {

	}

	public GraphPanelViewModelBase(GraphPageViewModel? controller, ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		this.controller = controller;
		dbService = (IHourglassDbService?)Services?.GetService(typeof(HourglassDbService));
		dateTimeService = (DateTimeService?)services?.GetService(typeof(DateTimeService));
	}

	public override void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider? services) {
		base.OnFinishedRegisteringViews(views, services);
		//dbService = (HourglassDbService?)services?.GetService(typeof(HourglassDbService));
	}

	public async virtual Task<List<Database.Models.Task>> GetTasksAsync() =>
		await dbService.QueryTasksAsync() ?? [];

	public abstract void OnClick(Database.Models.Task task);

	public abstract void OnDoubleClick(DateTime clickedTime);

	public async void OnClickBase(Avalonia.Point mousePos, int xAxisSegmentCount, int xAxisSegmentDuration) {
		Console.WriteLine("base graph panel model click");
		List<Database.Models.Task>? tasks = await GetTasksAsync();
		if (owner is GraphPanelViewBase view) {
			bool taskClicked = false;
			int i = 0;
			foreach (Database.Models.Task task in tasks) {
				view.GetTaskRectanlge(task, view.GRAPH_CLICK_ADDITIONAL_WIDTH, view.GRAPH_CLICK_ADDITIONAL_HEIGHT, i).Contains(mousePos);
				if (taskClicked) {
					//TaskDetails.TaskDetailsPopup taskDetailsWindow = new(task, _dbService, _parent);
					//taskDetailsWindow.ShowDialog();
					Console.WriteLine("detected task!");
					break;
				}
				i++;
			}
			//Point mousePos = PointToClient(MousePosition);
			////Console.WriteLine($"click at {mousePos}");
			//int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
			//using (Graphics g = Graphics.FromImage(image))
			//	g.Clear(Color.Gainsboro);
			//int graphPosY = PADDING_Y;
			//int i = 0;
			//foreach (Database.Models.Task task in tasks) {
			//	taskClicked = WindowMode switch {
			//		TimerWindowMode.Day =>
			//			GetTaskRectanlge(
			//				task,
			//				TimeSpan.SecondsPerHour,
			//				_parent.SelectedDay.Ticks / TimeSpan.TicksPerSecond,
			//				24,
			//				MAX_TASKS,
			//				GRAPH_CLICK_ADDITIONAL_WIDTH,
			//				GRAPH_CLICK_ADDITIONAL_WIDTH,
			//				GRAPH_MINIMAL_WIDTH,
			//				i,
			//				1
			//			).Contains(mousePos),
			//		TimerWindowMode.Week =>
			//			GetTaskRectanlge(
			//				task,
			//				TimeSpan.SecondsPerDay,
			//				DateTimeService.GetMondayOfWeekAtDate(_parent.SelectedDay).Ticks / TimeSpan.TicksPerSecond,
			//				7,
			//				MAX_TASKS,
			//				GRAPH_CLICK_ADDITIONAL_WIDTH,
			//				GRAPH_CLICK_ADDITIONAL_WIDTH,
			//				GRAPH_MINIMAL_WIDTH,
			//				i,
			//				1
			//			).Contains(mousePos),
			//		TimerWindowMode.Month =>
			//			GetTaskRectanlge(
			//				task,
			//				TimeSpan.SecondsPerDay,
			//				DateTimeService.FloorMonth(DateTime.Now).Ticks / TimeSpan.TicksPerSecond,
			//				daysInCurrentMonth,
			//				MAX_TASKS,
			//				GRAPH_CLICK_ADDITIONAL_WIDTH,
			//				GRAPH_CLICK_ADDITIONAL_HEIGHT,
			//				GRAPH_MINIMAL_WIDTH,
			//				i,
			//				1
			//			).Contains(mousePos),
			//		_ => false
			//	};
			//	if (taskClicked) {
			//		TaskDetails.TaskDetailsPopup taskDetailsWindow = new(task, _dbService, _parent);
			//		taskDetailsWindow.ShowDialog();
			//		break;
			//	}
			//	i++;
			//}
		}
	}

	public void OnDoubleClick_() {
		Console.WriteLine("base graph panel model double click");
		//Point mousePos = PointToClient(MousePosition);
		//int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		//if (WindowMode == TimerWindowMode.Week) {
		//	int offset = (int)Math.Floor((mousePos.X - PADDING_X) * 7.0 / (Width - 2 * PADDING_X));
		//	DateTime modayOfWeek = DateTimeService.GetMondayOfWeekAtDate(_parent.SelectedDay);
		//	_parent.ChangeGraphRendererMode(TimerWindowMode.Day, modayOfWeek.AddDays(offset));
		//} else if (WindowMode == TimerWindowMode.Month) {
		//	int offset = (int)Math.Floor((double)(mousePos.X - PADDING_X) * daysInCurrentMonth / (Width - 2 * PADDING_X));
		//	DateTime firstDayOfMonth = DateTimeService.GetFirstDayOfMonthAtDate(DateTimeService.GetFirstDayOfCurrentMonth());
		//	_parent.ChangeGraphRendererMode(TimerWindowMode.Week, firstDayOfMonth.AddDays(offset));
		//}
	}

	//public static Rect GetTaskRectangle(Database.Models.Task task, double viewWidth, double viewHeight, long xAxisSegmentDuration, long originSecond, int xAxisSegmentCount, int yAxisSegmentCount, int additionalWidth, int additionalHeight, int minimalWidth, int i, int columns) {
	//	double xAxisSegmentSize = (viewWidth- 2.0 * PADDING_X) / xAxisSegmentCount;
	//	double yAxisSegmentSize = (viewWidth - 2.0 * PADDING_Y) / (yAxisSegmentCount + 1.0);
	//	double proportion = xAxisSegmentSize / xAxisSegmentDuration;
	//	int graphPosX = (int)Math.Floor((task.start - originSecond) * proportion) + PADDING_X;
	//	long duration = task.finish - task.start;
	//	int graphLength = (int)Math.Floor(duration * proportion);
	//	int width = (graphLength > minimalWidth ? graphLength : minimalWidth) + additionalWidth * 2;
	//	Rect res = new(
	//		graphPosX - additionalWidth,
	//		(int)(yAxisSegmentSize * i * 1.5) - additionalHeight + PADDING_Y,
	//		width,
	//		(int)(yAxisSegmentSize) + additionalHeight * 2
	//	);
	//	//using (Graphics g = Graphics.FromImage(image))
	//	//using (Brush b = new SolidBrush(Color.AliceBlue))
	//	//	g.FillRectangle(b, res.X, res.Y, res.Width, res.Height);
	//	return res;

	//	return new Rect(0.0, 0.0, 0.0, 0.0);
	//}
}
	