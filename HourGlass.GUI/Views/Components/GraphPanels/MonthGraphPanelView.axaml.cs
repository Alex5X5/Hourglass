namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia.Media;
using Avalonia;

using Hourglass.GUI.ViewModels.Components.GraphPanels;

public partial class MonthGraphPanelView : GraphPanelViewBase {

	public MonthGraphPanelView() : base() {
		base.InitializeComponent();
		InitializeComponent();
	}

	protected override void DrawTimeline(DrawingContext context) {
		DateTime selectedMonth = DateTimeService.FloorMonth((DataContext as GraphPanelViewModelBase)!.cacheService.SelectedDay);
		bool selectedIsThisMonth = selectedMonth == DateTimeService.FloorMonth(DateTime.Now);
		Brush weekedDayBackground = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
		Brush todayBackgroundColor = new SolidColorBrush(Color.FromArgb(255, 237, 166, 166));
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		int daysInCurrentMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);
		double xAxisSegmentSize = (Bounds.Width - 2 * PADDING_X) / daysInCurrentMonth;
		int weekDayCounter = (int)selectedMonth.DayOfWeek;
        double textSize = Math.Round(PADDING_Y * 0.7, 1);
        for (int i = 0; i < daysInCurrentMonth; i++) {
			double xPos = X_AXIS_SEGMENT_SIZE * i + PADDING_X;
			if (weekDayCounter % 7 == 6 | weekDayCounter % 7 == 0)
				context.FillRectangle(weekedDayBackground, new(xPos + 1, PADDING_Y, xAxisSegmentSize - 2, Bounds.Height - (2 * PADDING_Y)));
			if(selectedIsThisMonth)
				if (i == DateTime.Today.Day - 1)
					context.FillRectangle(todayBackgroundColor, new(xPos + 1, PADDING_Y, xAxisSegmentSize - 2, Bounds.Height - (2 * PADDING_Y)));
			weekDayCounter++;
			var formattedText = new FormattedText(
				Convert.ToString(i + 1),
				System.Globalization.CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				new Typeface("Arial"),
				textSize,
				textBrush
			);
			Point textPos = new(xPos + xAxisSegmentSize / 2.0 - formattedText.Width / 2.0, Bounds.Height - (PADDING_Y * 0.85));
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
}