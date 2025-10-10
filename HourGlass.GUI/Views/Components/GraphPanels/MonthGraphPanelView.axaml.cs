namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.Util;

using Point = Avalonia.Point;

public partial class MonthGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => 2;

	public override int MAX_TASKS => 30;

	public override int GRAPH_CLICK_ADDITIONAL_WIDTH => 6;

	public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 4;

	public override int GRAPH_MINIMAL_WIDTH => 2;

	public override int GRAPH_CORNER_RADIUS => 4;

	public override long TIME_INTERVALL_START_SECONDS => DateTimeService.ToSeconds(DateTimeService.FloorMonth((DataContext as GraphPanelViewModelBase)?.dateTimeService?.SelectedDay ?? DateTime.Now));
	public override long TIME_INTERVALL_FINISH_SECONDS => TIME_INTERVALL_START_SECONDS + TimeSpan.SecondsPerDay * DateTimeService.DaysInCurrentMonth() - 1;

	public override int X_AXIS_SEGMENT_COUNT => DateTimeService.DaysInCurrentMonth();
	public override int Y_AXIS_SEGMENT_COUNT => MAX_TASKS;


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
		context.DrawText(formattedText, new Point(100, 100));
	}

	protected override void DrawTimeline(DrawingContext context) {
		Brush weekedDayBackground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));
		Brush todayBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 237, 166, 166));
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		int daysInCurrentMonth = DateTimeService.DaysInCurrentMonth();
		double xAxisSegmentSize = (Bounds.Width - 2 * PADDING_X) / daysInCurrentMonth;
		for (int i = 0; i < 7; i++) {
			double xPos = xAxisSegmentSize * i + PADDING_X;
			if (i % 7 == 5 | i % 7 == 6)
				context.FillRectangle(weekedDayBackground, new(xPos + 1, PADDING_Y, xAxisSegmentSize - 2, Bounds.Height - (2 * PADDING_Y)));
			if (i + 1 == (int)DateTime.Today.DayOfWeek)
				context.FillRectangle(todayBackgroundColor, new(xPos + 1, PADDING_Y, xAxisSegmentSize - 2, Bounds.Height - (2 * PADDING_Y)));
		}
		context.DrawLine(timeLine, new(PADDING_X, Bounds.Height - PADDING_Y), new(Bounds.Width - PADDING_X, Bounds.Height - PADDING_Y));
		for (int i = 0; i < daysInCurrentMonth + 1; i++) {
			double xPos = (Bounds.Width - 2 * PADDING_X) * i / daysInCurrentMonth + PADDING_X;
			context.DrawLine(hintLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, PADDING_Y));
			context.DrawLine(timeLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, Bounds.Height - PADDING_Y - TIMELINE_MARK_HEIGHT));
			if (i < daysInCurrentMonth) {
				var formattedText = new FormattedText(
					Convert.ToString(i + 1),
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
	}

	public override void OnDoubleClick(object? sender, TappedEventArgs e) {
		if (DataContext is GraphPanelViewModelBase model) {
			DateTime startDate = DateTimeService.FloorMonth(model.dateTimeService?.SelectedDay ?? DateTime.Now);
			DateTime finishDate = startDate.AddMonths(1).AddSeconds(-1);
			OnDoubleClickBase(e, DateTimeService.ToSeconds(startDate), DateTimeService.ToSeconds(finishDate));
		}
	}
}