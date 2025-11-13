namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia.Input;
using Avalonia.Media;

using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.Util;

using Point = Avalonia.Point;

public partial class MonthGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => 2;

	public override int MAX_TASKS => 70;

	public override int GRAPH_CLICK_ADDITIONAL_WIDTH => 6;

	public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 4;

	public override int GRAPH_MINIMAL_WIDTH => 2;

	public override int GRAPH_CORNER_RADIUS => 4;

	public override long TIME_INTERVALL_START_SECONDS => DateTimeService.ToSeconds(DateTimeService.FloorMonth((DataContext as GraphPanelViewModelBase)?.dateTimeService?.SelectedDay ?? DateTime.Now));
	public override long TIME_INTERVALL_FINISH_SECONDS => TIME_INTERVALL_START_SECONDS + TimeSpan.SecondsPerDay * DateTimeService.DaysInCurrentMonth() - 1;

	public override int X_AXIS_SEGMENT_COUNT => DateTimeService.DaysInCurrentMonth();
	public override int Y_AXIS_SEGMENT_COUNT => MAX_TASKS;

    protected override double TASK_DESCRIPTION_GRAPH_SPAGE => 5;
    protected override double TASK_DESCRIPTION_FONT_SIZE => 10;

    public MonthGraphPanelView() : base() {
		InitializeComponent();
	}

	protected override void DrawTimeline(DrawingContext context) {
		Brush weekedDayBackground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));
		Brush todayBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 237, 166, 166));
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		int daysInCurrentMonth = DateTimeService.DaysInCurrentMonth();
		double xAxisSegmentSize = (Bounds.Width - 2 * PADDING_X) / daysInCurrentMonth;
        int weekDayCounter = (int)DateTimeService.GetFirstDayOfCurrentMonth().DayOfWeek;
        for (int i = 0; i < daysInCurrentMonth; i++) {
            double xPos = X_AXIS_SEGMENT_SIZE * i + PADDING_X;
            if (weekDayCounter % 7 == 6 | weekDayCounter % 7 == 0)
                context.FillRectangle(weekedDayBackground, new(xPos + 1, PADDING_Y, xAxisSegmentSize - 2, Bounds.Height - (2 * PADDING_Y)));
            if (i == DateTime.Today.Day - 1)
                context.FillRectangle(todayBackgroundColor, new(xPos + 1, PADDING_Y, xAxisSegmentSize - 2, Bounds.Height - (2 * PADDING_Y)));
            weekDayCounter++;
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
        context.DrawLine(timeLine, new(PADDING_X, Bounds.Height - PADDING_Y), new(Bounds.Width - PADDING_X, Bounds.Height - PADDING_Y));
		for (int i = 0; i < daysInCurrentMonth + 1; i++) {
			double xPos = X_AXIS_SEGMENT_SIZE * i + PADDING_X;
			context.DrawLine(hintLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, PADDING_Y));
			context.DrawLine(timeLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, Bounds.Height - PADDING_Y - TIMELINE_MARK_HEIGHT));
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