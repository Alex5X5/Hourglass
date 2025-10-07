namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Media;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

using Point = Avalonia.Point;

public partial class DayGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => throw new NotImplementedException();

	public override int MAX_TASKS => 1000;

	public override int GRAPH_CLICK_ADDITIONAL_WIDTH => throw new NotImplementedException();

	public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => throw new NotImplementedException();

	public override int GRAPH_MINIMAL_WIDTH => throw new NotImplementedException();

	public override int GRAPH_CORNER_RADIUS => throw new NotImplementedException();

	public DayGraphPanelView() : this(null, null) {

	}

	public DayGraphPanelView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
	}

	protected override void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i) {
		Console.WriteLine("DayGraphPanel draw task graph");
		long todaySeconds = DateTimeService.FloorDay(DateTime.Now).Ticks / TimeSpan.TicksPerSecond;
		//long todaySeconds = nowSeconds - nowSeconds % TimeSpan.SecondsPerDay;
		Rect rect = GetTaskRectanlgeBase(task, TimeSpan.SecondsPerHour, todaySeconds, 24, MAX_TASKS, 0, 0, GRAPH_MINIMAL_WIDTH, i, 1);
		Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		//using GraphicsPath path = GetRoundedRectanglePath(rect, GRAPH_CORNER_RADIUS);
		//Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidColorBrush(task.DisplayColor);
		Brush brush = new SolidColorBrush(Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue));
		context.FillRectangle(brush, rect);
		DrawTaskDescriptionStub(context, task, rect.X, rect.Y, rect.Width);
	}

	protected override void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, double graphPosX, double graphPosY, double graphLength) {
		var rect = new Rect(100, 100, 50, 20);
		context.DrawRectangle(Background, null, rect);
		string text = "day graph panel string";
		var formattedText = new FormattedText(
			text,
			System.Globalization.CultureInfo.CurrentCulture,
			FlowDirection.LeftToRight,
			new Typeface("Arial"),
			16, // Font size
			new SolidColorBrush(Colors.Gray)
		);

		// Center the text
		var x = (Bounds.Width - formattedText.Width) / 2;
		var y = (Bounds.Height - formattedText.Height) / 2;

		context.DrawText(formattedText, new Point(100,100));
	}

	protected override void DrawTimeline(DrawingContext context) {
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		double xAxisSegmentSize = (Bounds.Width - 2 * PADDING_X) / 24;
		context.DrawLine(timeLine, new(PADDING_X, Bounds.Height - PADDING_Y), new(Bounds.Width - PADDING_X, Bounds.Height - PADDING_Y));
		for (int i = 0; i < 25; i++) {
			double xPos = (Bounds.Width - 2 * PADDING_X) * i / 24 + PADDING_X;
			context.DrawLine(hintLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, PADDING_Y));
			context.DrawLine(timeLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, Bounds.Height - PADDING_Y - TIMELINE_MARK_HEIGHT));
			if (i < 24) {
				var formattedText = new FormattedText(
					Convert.ToString(i + 1) + ":00",
					System.Globalization.CultureInfo.CurrentCulture,
					FlowDirection.LeftToRight,
					new Typeface("Arial"),
					11,
					textBrush
				);
				Point textPos = new(xPos + xAxisSegmentSize / 2.0 - formattedText.Width / 2.0, Bounds.Height - PADDING_Y + 5);
				context.DrawText(
					formattedText,
					textPos
				);
			}
		}
	}

	public override Rect GetTaskRectanlge(Database.Models.Task task, double additionalWidth, double additionalHeght, int i) =>
		GetTaskRectanlgeBase(
			task,
			DateTime.Today.Ticks / TimeSpan.TicksPerSecond,
			TimeSpan.SecondsPerDay,
			DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month),
			MAX_TASKS,
			additionalWidth,
			additionalHeght,
			GRAPH_MINIMAL_WIDTH,
			i,
			1);
}