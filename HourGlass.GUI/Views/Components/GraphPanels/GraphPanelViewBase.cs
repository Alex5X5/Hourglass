namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Media;

using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.Util;


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

    protected double X_AXIS_SEGMENT_SIZE => (Bounds.Width - 2 * PADDING_X) / X_AXIS_SEGMENT_COUNT;
    protected double Y_AXIS_SEGMENT_SIZE => (Bounds.Height - 2.0 * PADDING_Y) / (Y_AXIS_SEGMENT_COUNT * 1.5) * TASK_GRAPH_COLUMN_COUNT;

    protected double PADDING_X => Bounds.Width / 30;
	protected double PADDING_Y => Bounds.Height / 30;
	protected static double TIMELINE_MARK_HEIGHT => 7;

	protected abstract double TASK_DESCRIPTION_GRAPH_SPAGE { get; }
    protected abstract double TASK_DESCRIPTION_FONT_SIZE { get; }

	private static int MAX_TASK_DESCRIPTION_CHARS => 30;
	
    private bool RightMouseDown = false;
    private bool LeftMouseDown = false;

    private Rect MarkerDragRectangle;
	private Point DragOrigin;

    #endregion fields

    public GraphPanelViewBase() : base() {
    }

	protected abstract void DrawTimeline(DrawingContext context);
	
	public abstract void OnDoubleClick(object? sender, TappedEventArgs e);

	protected static double ArialHeightToPt(double height, double x=1) =>
        Math.Round(Math.Log(3 * height + 1) * 3 * x+ height * 0.3 * x, 2);

    public Rect GetTaskRectanlge(Database.Models.Task task, double additionalWidth, double additionalHeight, int i) {
		double xAxisSegmentSize = (Bounds.Width - 2.0 * PADDING_X) / X_AXIS_SEGMENT_COUNT;
		double proportion = xAxisSegmentSize / X_AXIS_SEGMENT_DURATION;
		double graphPosX = (task.start - TIME_INTERVALL_START_SECONDS) * proportion + PADDING_X;
		long duration = task.running ? DateTimeService.ToSeconds(DateTime.Now) - task.start : task.finish - task.start;
		double graphLength = duration * proportion;
		double width = (graphLength > GRAPH_MINIMAL_WIDTH ? graphLength : GRAPH_MINIMAL_WIDTH) + additionalWidth * 2;
		Rect res = new(
			graphPosX - additionalWidth,
			Y_AXIS_SEGMENT_SIZE * (i % (MAX_TASKS / TASK_GRAPH_COLUMN_COUNT)) * 1.5 - additionalHeight + PADDING_Y,
			width,
			Y_AXIS_SEGMENT_SIZE + additionalHeight * 2
		);
		return res;
	}

	private void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i) {
		Rect rect = GetTaskRectanlge(task, 0, 0, i);
		Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Color gradientFinishColor = Color.FromArgb(20, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Brush brush = task.running
			? new LinearGradientBrush() {
				StartPoint = new RelativePoint(0.0, 0.5, RelativeUnit.Relative),
				EndPoint= new RelativePoint(1.0, 0.5, RelativeUnit.Relative),
				GradientStops = {
					new GradientStop(gradientStartColor, 0.0),
                    new GradientStop(gradientFinishColor, 1.0)
				}
			}
			: new SolidColorBrush(task.DisplayColor);
		double r = Math.Min(GRAPH_CORNER_RADIUS, rect.Width / 2);
        RectangleGeometry rrect = new(rect) { RadiusX = r, RadiusY = r };
        context.DrawGeometry(brush, null, rrect);
		DrawTaskDescriptionStub(context, task, rect);
	}

	private void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, Rect taskRect) {
		var formattedText = new FormattedText(
            task.description.Length <= MAX_TASK_DESCRIPTION_CHARS ? task.description : task.description[..MAX_TASK_DESCRIPTION_CHARS] + "...",
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            Math.Max(2.0, ArialHeightToPt(Y_AXIS_SEGMENT_SIZE)),
            new SolidColorBrush(Colors.Black)
        );
        Point p = new(taskRect.X - formattedText.Width - TASK_DESCRIPTION_GRAPH_SPAGE, taskRect.Y + taskRect.Height / 2 - formattedText.Height / 2);
        context.DrawText(formattedText, p);
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
		if(RightMouseDown)
			context.FillRectangle(Brushes.AliceBlue, MarkerDragRectangle);
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

    protected void MouseMoved(object sender, PointerEventArgs args) {
        Point point = args.GetCurrentPoint(this).Position;
        Console.WriteLine($"mouse moved to {point.X} {point.Y}!");
		if (RightMouseDown) {
			MarkerDragRectangle = new Rect(
				Math.Min(point.X, DragOrigin.X),
				Math.Min(point.Y, DragOrigin.Y),
				Math.Abs(point.X - DragOrigin.X),
				Math.Abs(point.Y - DragOrigin.Y)
			);
			InvalidateVisual();
		}
        //if(MarkerDragRectangle = null) {
        //}
    }

    protected void MousePressed(object sender, PointerPressedEventArgs args) {
        PointerPoint mousePoint = args.GetCurrentPoint(sender as Control);
		Point point = mousePoint.Position;
		Console.WriteLine($"mouse pressed!");
        if (mousePoint.Properties.IsRightButtonPressed) {
			if(!RightMouseDown)
				DragOrigin = point;
            RightMouseDown = true;
            MarkerDragRectangle = new Rect(
                Math.Min(point.X, DragOrigin.X),
                Math.Min(point.Y, DragOrigin.Y),
                Math.Abs(point.X - DragOrigin.X),
                Math.Abs(point.Y - DragOrigin.Y)
            );
            InvalidateVisual();
        }
        if (mousePoint.Properties.IsLeftButtonPressed)
            LeftMouseDown = true;
		DragOrigin = mousePoint.Position;
        InvalidateVisual();

    }

    protected void MouseReleased(object sender, PointerReleasedEventArgs args) {
        Console.WriteLine($"mouse released!");
        if (!args.GetCurrentPoint(sender as Control).Properties.IsRightButtonPressed)
            RightMouseDown = false;
        if (!args.GetCurrentPoint(sender as Control).Properties.IsLeftButtonPressed)
            LeftMouseDown = false;
        InvalidateVisual();
    }
}
