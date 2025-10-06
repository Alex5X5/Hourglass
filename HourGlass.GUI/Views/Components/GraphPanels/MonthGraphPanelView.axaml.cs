namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Media;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels;
using Hourglass.Util;

using Point = Avalonia.Point;

public partial class MonthGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => throw new NotImplementedException();

	public override int MAX_TASKS => 1000;

	public override int GRAPH_CLICK_ADDITIONAL_WIDTH => throw new NotImplementedException();

	public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => throw new NotImplementedException();

	public override int GRAPH_MINIMAL_WIDTH => throw new NotImplementedException();

	public override int GRAPH_CORNER_RADIUS => throw new NotImplementedException();

	public MonthGraphPanelView() : this(null, null) {

	}

	public MonthGraphPanelView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
	}

	protected override void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, double graphPosX, double graphPosY, double graphLength) {
		// Draw rectangle background
		var rect = new Rect(100, 100, 50, 20);
		context.DrawRectangle(Background, null, rect);
		string text = "month graph panel string";
		// Create formatted text
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

		// Draw the text
		context.DrawText(formattedText, new Point(100,100));
	}

	protected override void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i) {
		Console.WriteLine("Month draw task graph");
	}

	protected override void DrawTimeline(DrawingContext context) {
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		double xAxisSegmentSize = (Bounds.Width - 2 * PADDING_X) / daysInCurrentMonth;
		context.DrawLine(timeLine, new(PADDING_X, Bounds.Height - PADDING_Y), new(Bounds.Width - PADDING_X, Bounds.Height - PADDING_Y));
		for (int i = 0; i < daysInCurrentMonth+1; i++) {
			double xPos = (Bounds.Width - 2 * PADDING_X) * i / daysInCurrentMonth + PADDING_X;
			context.DrawLine(hintLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, PADDING_Y));
			context.DrawLine(timeLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, Bounds.Height - PADDING_Y - TIMELINE_MARK_HEIGHT));
		}
		for (int i = 0; i < daysInCurrentMonth; i++) {
			double xPos = (Bounds.Width - 2 * PADDING_X) * i / daysInCurrentMonth + PADDING_X;
			var formattedText = new FormattedText(
				Convert.ToString(i+1),
				System.Globalization.CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				new Typeface("Arial"),
				13,
				textBrush
			);
			Point textPos = new(xPos + xAxisSegmentSize / 2.0 - formattedText.Width / 2.0, Bounds.Height - PADDING_Y + 5);
			context.DrawText(
				formattedText,
				textPos
			);
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