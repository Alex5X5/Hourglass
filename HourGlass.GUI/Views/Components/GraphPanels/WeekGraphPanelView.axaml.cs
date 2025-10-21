namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.Util;

using Point = Avalonia.Point;

public partial class WeekGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => 1;

	public override int MAX_TASKS => 20;

	public override int GRAPH_CLICK_ADDITIONAL_WIDTH => 8;

	public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 5;

	public override int GRAPH_MINIMAL_WIDTH => 5;

	public override int GRAPH_CORNER_RADIUS => 5;

	public override long TIME_INTERVALL_START_SECONDS => DateTimeService.ToSeconds(DateTimeService.FloorWeek((DataContext as GraphPanelViewModelBase)?.dateTimeService?.SelectedDay ?? DateTime.Now));
	public override long TIME_INTERVALL_FINISH_SECONDS => TIME_INTERVALL_START_SECONDS + TimeSpan.SecondsPerDay * 7 - 1;

	public override int X_AXIS_SEGMENT_COUNT => 7;
	public override int Y_AXIS_SEGMENT_COUNT => MAX_TASKS;

    protected override double TASK_DESCRIPTION_GRAPH_SPAGE => 5;
    protected override double TASK_DESCRIPTION_FONT_SIZE => 10;

    public WeekGraphPanelView() : base() {
		InitializeComponent();
	}

	protected override void DrawTimeline(DrawingContext context) {
		Brush weekedDayBackground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));
		Brush todayBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 237, 166, 166));
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		double xAxisSegmentSize = (Bounds.Width - 2 * PADDING_X) / 7;
		string[] days = ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"];
		for (int i = 0; i < 7; i++) {
			double xPos = xAxisSegmentSize * i + PADDING_X;
			if (i % 7 == 5 | i % 7 == 6)
				context.FillRectangle(weekedDayBackground, new(xPos+1, PADDING_Y, xAxisSegmentSize-2, Bounds.Height - (2 * PADDING_Y)));
			if (i + 1 == (int)DateTime.Today.DayOfWeek)
				if(DateTimeService.FloorWeek((DataContext as WeekGraphPanelViewModel)!.dateTimeService.SelectedDay) == DateTimeService.FloorWeek(DateTime.Now))
				context.FillRectangle(todayBackgroundColor, new(xPos+1, PADDING_Y, xAxisSegmentSize-2, Bounds.Height - (2 * PADDING_Y)));
		}
		context.DrawLine(timeLine, new(PADDING_X, Bounds.Height - PADDING_Y), new(Bounds.Width - PADDING_X, Bounds.Height - PADDING_Y));
		for (int i = 0; i < 8; i++) {
			double xPos = (Bounds.Width - 2 * PADDING_X) * i / 7 + PADDING_X;
			context.DrawLine(hintLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, PADDING_Y));
			context.DrawLine(timeLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, Bounds.Height - PADDING_Y - TIMELINE_MARK_HEIGHT));
			if (i < 7) {
				var formattedText = new FormattedText(
					days[i],
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
			DateTime startDate = DateTimeService.FloorWeek(model.dateTimeService?.SelectedDay ?? DateTime.Now);
			DateTime finishDate = startDate.AddDays(7).AddSeconds(-1);
			OnDoubleClickBase(e, DateTimeService.ToSeconds(startDate), DateTimeService.ToSeconds(finishDate));
		}
	}
}