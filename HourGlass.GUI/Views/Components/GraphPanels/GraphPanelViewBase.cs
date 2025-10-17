namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

using Hourglass.GUI.ViewModels.Components.GraphPanels;

public abstract class GraphPanelViewBase : ViewBase {

	#region fields

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

	protected double PADDING_X => Bounds.Width / 30;
	protected double PADDING_Y => Bounds.Height / 30;
	protected static double TIMELINE_MARK_HEIGHT => 7;

	#endregion fields

	public GraphPanelViewBase() : base() {
		
	}

	protected abstract void DrawTimeline(DrawingContext context);

	protected abstract void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, double graphPosX, double graphPosY, double graphLength);

	public abstract void OnDoubleClick(object? sender, TappedEventArgs e);

	public Rect GetTaskRectanlge(Database.Models.Task task, double additionalWidth, double additionalHeight, int i) {
		double xAxisSegmentSize = (Bounds.Width - 2.0 * PADDING_X) / X_AXIS_SEGMENT_COUNT;
		double yAxisSegmentSize = (Bounds.Height - 2.0 * PADDING_Y) / (Y_AXIS_SEGMENT_COUNT * 1.5);
		double proportion = xAxisSegmentSize / X_AXIS_SEGMENT_DURATION;
		double graphPosX = (task.start - TIME_INTERVALL_START_SECONDS) * proportion + PADDING_X;
		long duration = task.finish - task.start;
		double graphLength = duration * proportion;
		double width = (graphLength > GRAPH_MINIMAL_WIDTH ? graphLength : GRAPH_MINIMAL_WIDTH) + additionalWidth * 2;
		Rect res = new(
			graphPosX - additionalWidth,
			yAxisSegmentSize * i * 1.5 - additionalHeight + PADDING_Y,
			width,
			yAxisSegmentSize + additionalHeight * 2
		);
		return res;
	}

	private void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i) {
		Rect rect = GetTaskRectanlge(task, 0, 0, i);
		//Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		//Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		//Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidColorBrush(task.DisplayColor);
		Brush brush = new SolidColorBrush(Color.FromArgb(255, task.displayColorRed, 0, task.displayColorBlue));
		context.FillRectangle(brush, rect);
		DrawTaskDescriptionStub(context, task, rect.X, rect.Y, rect.Width);
	}

	public async override void Render(DrawingContext context) {
		base.Render(context);
		if (!IsVisible)
			return;
		var brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

		context.FillRectangle(brush, Bounds);
		DrawTimeline(context);
		List<Database.Models.Task> tasks = [];
		if(DataContext is GraphPanelViewModelBase model)
			tasks = await model.GetTasksAsync();
		if (tasks != null && tasks.Count > 0) {
			double graphPosY = PADDING_Y;
			for (int i = 0; i < MAX_TASKS && i < tasks.Count; i++) {
				DrawTaskGraph(context, tasks[i], i);
			}
		}
	}

	public async void OnClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("base graph panel click");
		Point mousePos = e.GetPosition(this);
		if (DataContext is GraphPanelViewModelBase model) {
			int i = 0;
			Database.Models.Task? clickedTask = null;
			foreach (var task in await model.GetTasksAsync()) {
				Rect rect = GetTaskRectanlge(task, GRAPH_CLICK_ADDITIONAL_WIDTH, GRAPH_CLICK_ADDITIONAL_HEIGHT, i);
				i++;
				if (rect.Contains(mousePos)) {
					clickedTask = task;
					break;
				} else {
					continue;
				}
			}
			if(clickedTask!= null)
				model.OnClick(clickedTask);
		}
	}

	protected void OnDoubleClickBase(TappedEventArgs e, long timeIntervallStartSecond, long timeIntervallFinishSecond) {
		Console.WriteLine("base graph panel double click");
		Point mousePos = e.GetPosition(this);
		double offset = mousePos.X - PADDING_X;
		long timeIntervallSeconds = timeIntervallFinishSecond - timeIntervallStartSecond;
		double clickSeconds = timeIntervallStartSecond + timeIntervallSeconds * offset / (Bounds.Width - 2 * PADDING_X);
		DateTime clickDate = new DateTime((long)clickSeconds * TimeSpan.TicksPerSecond);
		(DataContext as GraphPanelViewModelBase)?.OnDoubleClick(clickDate);
	}
}
