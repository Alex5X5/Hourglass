namespace Hourglass.GUI.Views.Components.GraphPanels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

public abstract partial class GraphPanelViewBase : ViewBase {

	#region fields

	private GraphPanelViewModelBase Model => (DataContext as GraphPanelViewModelBase)!;

    protected int MAX_TASKS => Model.MAX_TASKS;

    protected int GRAPH_CLICK_ADDITIONAL_WIDTH => Model.GRAPH_CLICK_ADDITIONAL_WIDTH;
    protected int GRAPH_CLICK_ADDITIONAL_HEIGHT => Model.GRAPH_CLICK_ADDITIONAL_HEIGHT;

	protected int GRAPH_MINIMAL_WIDTH => Model.GRAPH_MINIMAL_WIDTH;
    protected int GRAPH_CORNER_RADIUS => Model.GRAPH_CORNER_RADIUS;

    protected long TIME_INTERVALL_START_SECONDS => Model.TIME_INTERVALL_START_SECONDS;
	protected long TIME_INTERVALL_FINISH_SECONDS => Model.TIME_INTERVALL_FINISH_SECONDS;
    protected long TIME_INTERVALL_DURATION => Model.TIME_INTERVALL_DURATION;
    protected long X_AXIS_SEGMENT_DURATION => Model.X_AXIS_SEGMENT_DURATION;

	protected int X_AXIS_SEGMENT_COUNT => Model.X_AXIS_SEGMENT_COUNT;
    protected int Y_AXIS_SEGMENT_COUNT => Model.Y_AXIS_SEGMENT_COUNT;

    protected double TASK_DESCRIPTION_GRAPH_SPAGE => Model.TASK_DESCRIPTION_GRAPH_SPACE;
    protected double TASK_DESCRIPTION_FONT_SIZE => Model.TASK_DESCRIPTION_FONT_SIZE;

    protected int TASK_GRAPH_COLUMN_COUNT => Model.TASK_GRAPH_COLUMN_COUNT;

    protected double X_AXIS_SEGMENT_SIZE => GRAPH_AREA_WIDTH / X_AXIS_SEGMENT_COUNT;
	protected double Y_AXIS_SEGMENT_SIZE => GRAPH_AREA_HEIGTH / (Y_AXIS_SEGMENT_COUNT * 1.5) * Model.TASK_GRAPH_COLUMN_COUNT;

	protected double PADDING_X => Bounds.Width * GraphPanelViewModelBase.PADDING_X_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_X_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_X_WEIGHT);
	protected double PADDING_Y => Bounds.Height * GraphPanelViewModelBase.PADDING_Y_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_Y_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_Y_WEIGHT);

	protected double GRAPH_AREA_WIDTH => Bounds.Width - 2 * PADDING_X;
	protected double GRAPH_AREA_HEIGTH => Bounds.Height - 2 * PADDING_Y;

    public const double TIMELINE_MARK_HEIGHT = 7;
    public const int MAX_TASK_DESCRIPTION_CHARS = 30;

	private bool RightMouseDown = false;
	private bool LeftMouseDown = false;

	private Rect MarkerDragRectangle;
	private Point DragOrigin;
	private Point MousePos = new(0.0,0.0);

    private ContextMenu? _contextMenu;

    #endregion fields
    public GraphPanelViewBase() : base() {
		InitializeComponent();
    }

	protected static double ArialHeightToPt(double height, double x = 1) =>
		Math.Round(Math.Log(3 * height + 1) * 3 * x + height * 0.3 * x, 2);

	public Rect GetTaskRectanlge(Database.Models.Task task, double additionalWidth, double additionalHeight, int i) {
		double proportion = X_AXIS_SEGMENT_SIZE / X_AXIS_SEGMENT_DURATION;
		double graphPosX = (task.start - TIME_INTERVALL_START_SECONDS) * proportion + PADDING_X;
		long duration = task.running ? DateTimeService.ToSeconds(DateTime.Now) - task.start : task.finish - task.start;
		double graphLength = duration * proportion;
		double width = Math.Max(graphLength, GRAPH_MINIMAL_WIDTH) + additionalWidth * 2;
		Rect res = new(
			graphPosX - additionalWidth,
			Y_AXIS_SEGMENT_SIZE * (i % (MAX_TASKS / TASK_GRAPH_COLUMN_COUNT)) * 1.5 - additionalHeight + PADDING_Y,
			width,
			Y_AXIS_SEGMENT_SIZE + additionalHeight * 2
		);
		return res;
	}

	protected abstract void DrawTimeline(DrawingContext context);

	private void DrawColumnMarkers(DrawingContext context) {
		Brush brush = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
		double x = PADDING_X + 2;
		double y = PADDING_Y + 2;
		double width = X_AXIS_SEGMENT_SIZE - 4;
		double height = GRAPH_AREA_HEIGTH - 5;
		for(int i=0; i<X_AXIS_SEGMENT_COUNT; i++) {
			if (Model.MarkedColumns[i])
				context.FillRectangle(brush, new Rect(x, y, width, height));
			x += X_AXIS_SEGMENT_SIZE;
		}
	}

	private void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i) {
		Rect rect = GetTaskRectanlge(task, 0, 0, i);
		Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Color gradientFinishColor = Color.FromArgb(20, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Brush brush = task.running
			? new LinearGradientBrush() {
				StartPoint = new RelativePoint(0.0, 0.5, RelativeUnit.Relative),
				EndPoint = new RelativePoint(1.0, 0.5, RelativeUnit.Relative),
				GradientStops = {
					new GradientStop(gradientStartColor, 0.0),
					new GradientStop(gradientFinishColor, 1.0)
				}
			}
			: new SolidColorBrush(task.DisplayColor);
		double r = Math.Min(GRAPH_CORNER_RADIUS, rect.Height / 4);
		r = Math.Min(r, rect.Width /2);
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
		context.FillRectangle(brush, new Rect(Bounds.X+PADDING_X, Bounds.Y+PADDING_Y, Bounds.Width - 2 * PADDING_X, Bounds.Height - 2 * PADDING_Y));
		DrawTimeline(context);
		DrawColumnMarkers(context);
		List<Database.Models.Task> tasks = [];
		if (DataContext is GraphPanelViewModelBase model)
			tasks = await model.GetTasksAsync();
		if (tasks != null && tasks.Count > 0) {
			double graphPosY = PADDING_Y;
			for (int i = 0; i < MAX_TASKS && i < tasks.Count; i++) {
				DrawTaskGraph(context, tasks[i], i);
			}
		}
		if (RightMouseDown) {
			Brush borderBrush = new SolidColorBrush(Color.FromArgb(200,100,100,100));
			Brush areaBrush = new SolidColorBrush(Color.FromArgb(150, 150, 220, 255));
			Pen pen = new Pen(borderBrush, 2);
			context.FillRectangle(areaBrush, MarkerDragRectangle);
			context.DrawRectangle(pen, MarkerDragRectangle);
		}
	}

	private bool IsOutsideGraphArea(Point p) {
        if (p.X < PADDING_X)
            return true;
        if (p.X > Bounds.Width - PADDING_X)
            return true;
        if (p.Y < PADDING_Y)
            return true;
        if (p.Y > Bounds.Height - PADDING_Y)
            return true;
		return false;
    }

	public async void OnClickBase(object? sender, TappedEventArgs e) {
		Console.WriteLine("base graph panel click");
		Point mousePos = e.GetPosition(this);
		if (IsOutsideGraphArea(mousePos))
			return;
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
			if (clickedTask != null)
				model.OnTaskClicked(clickedTask);
		}
		OnClick(sender, e);
	}

	protected void OnDoubleClickBase(object? sender, TappedEventArgs args) {
		Console.WriteLine("base graph panel double click");
		Point mousePos = args.GetPosition(this);
        if (IsOutsideGraphArea(mousePos))
            return;
        double offset = mousePos.X - PADDING_X;
		double clickSeconds = TIME_INTERVALL_START_SECONDS + TIME_INTERVALL_DURATION * offset / (Bounds.Width - 2 * PADDING_X);
		DateTime clickDate = new DateTime((long)clickSeconds * TimeSpan.TicksPerSecond);
		(DataContext as GraphPanelViewModelBase)?.OnDoubleClick(clickDate);
	}

	private void OnMouseMovedBase(object sender, PointerEventArgs args) {
		MousePos = args.GetCurrentPoint(this).Position;
		if (RightMouseDown) {
			MarkerDragRectangle = new Rect(
				Math.Min(MousePos.X, DragOrigin.X),
				Math.Min(MousePos.Y, DragOrigin.Y),
				Math.Abs(MousePos.X - DragOrigin.X),
				Math.Abs(MousePos.Y - DragOrigin.Y)
			);
			(DataContext as GraphPanelViewModelBase)?.OnMouseDragging(MarkerDragRectangle, GRAPH_AREA_WIDTH, PADDING_X);
			InvalidateVisual();
		} else {
			(DataContext as GraphPanelViewModelBase)?.OnMouseMoved();
		}
        OnMouseMoved(sender, args);
	}

	private void OnMousePressedBase(object sender, PointerPressedEventArgs args) {
		PointerPoint mousePoint = args.GetCurrentPoint(sender as Control);
        MousePos = mousePoint.Position;
		Console.WriteLine($"mouse pressed!");
		if (mousePoint.Properties.IsRightButtonPressed) {
			if (!RightMouseDown)
				DragOrigin = MousePos;
			RightMouseDown = true;
			MarkerDragRectangle = new Rect(
				Math.Min(MousePos.X, DragOrigin.X),
				Math.Min(MousePos.Y, DragOrigin.Y),
				Math.Abs(MousePos.X - DragOrigin.X),
				Math.Abs(MousePos.Y - DragOrigin.Y)
			);
			InvalidateVisual();
		}
		if (mousePoint.Properties.IsLeftButtonPressed)
			LeftMouseDown = true;
		DragOrigin = mousePoint.Position;
        OnMouseMoved(sender, args);
		InvalidateVisual();
	}

	private void OnMouseReleasedBase(object sender, PointerReleasedEventArgs args) {
		Console.WriteLine($"mouse released!");
		if (!args.GetCurrentPoint(sender as Control).Properties.IsRightButtonPressed) {
            if (!IsOutsideGraphArea(MousePos))
                if (RightMouseDown) {
                    ShowReasonContextMenu();
                }
			RightMouseDown = false;
		}
		if (!args.GetCurrentPoint(sender as Control).Properties.IsLeftButtonPressed)
            LeftMouseDown = false;
		OnMouseMoved(sender, args);
		InvalidateVisual();
	}

	private void ShowReasonContextMenu() {
        _contextMenu = new ContextMenu();
		_contextMenu.ItemsSource = new List<MenuItem>() {
			new() { Header = "Sick" },
			new() { Header = "School" },
			new() { Header = "Appointment" },
			new() { Header = "No Excuse" }
        };
		_contextMenu?.Open(this);
    }

	public virtual void OnClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("closed context menu!");
	}

    public virtual void OnDoubleClick(object? sender, TappedEventArgs e) { }

	public virtual void OnMouseMoved(object? sender, PointerEventArgs e) { }

	public virtual void OnMousePressed(object sender, PointerPressedEventArgs args) { }

	public virtual void OnMouseReleased(object sender, PointerReleasedEventArgs args) { }
	
    private void PreviusIntervallClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		(DataContext as GraphPanelViewModelBase)?.PreviusIntervallClick();
        InvalidateVisual();
	}

    private void FollowingIntervallClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        (DataContext as GraphPanelViewModelBase)?.FollowingIntervallClick();
        InvalidateVisual();
    }
}