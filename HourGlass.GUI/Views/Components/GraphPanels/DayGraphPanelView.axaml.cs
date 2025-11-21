namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia.Input;
using Avalonia.Media;

using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.Util;

using Point = Avalonia.Point;

public partial class DayGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => 1;

	public override int MAX_TASKS => 5;

	public override int GRAPH_CLICK_ADDITIONAL_WIDTH => 5;

	public override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 2;

	public override int GRAPH_MINIMAL_WIDTH => 8;

	public override int GRAPH_CORNER_RADIUS => 12;

	public override long TIME_INTERVALL_START_SECONDS => DateTimeService.ToSeconds(DateTimeService.FloorDay((DataContext as GraphPanelViewModelBase)?.cacheService.SelectedDay ?? DateTime.Now));
	public override long TIME_INTERVALL_FINISH_SECONDS => TIME_INTERVALL_START_SECONDS + TimeSpan.SecondsPerDay -1;

	public override int X_AXIS_SEGMENT_COUNT => 24;
	public override int Y_AXIS_SEGMENT_COUNT => MAX_TASKS;

	protected override double TASK_DESCRIPTION_GRAPH_SPAGE => 10;
	protected override double TASK_DESCRIPTION_FONT_SIZE=> 30;

    public DayGraphPanelView() : base() {
		InitializeComponent();
	}
	
	protected override void DrawTimeline(DrawingContext context) {
		Pen timeLine = new(new SolidColorBrush(Colors.Black));
		Pen hintLine = new(new SolidColorBrush(Color.FromArgb(255, 170, 170, 170)));
		Brush textBrush = new SolidColorBrush(Colors.Gray);
		context.DrawLine(timeLine, new(PADDING_X, Bounds.Height - PADDING_Y), new(Bounds.Width - PADDING_X, Bounds.Height - PADDING_Y));
		for (int i = 0; i < 25; i++) {
            double xPos = X_AXIS_SEGMENT_SIZE * i + PADDING_X;
            context.DrawLine(hintLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, PADDING_Y));
			context.DrawLine(timeLine, new Point(xPos, Bounds.Height - PADDING_Y), new Point(xPos, Bounds.Height - PADDING_Y - TIMELINE_MARK_HEIGHT));
			var formattedText = new FormattedText(
				Convert.ToString(i) + ":00",
				System.Globalization.CultureInfo.CurrentCulture,
				FlowDirection.LeftToRight,
				new Typeface("Arial"),
				ArialHeightToPt(PADDING_Y),
				textBrush
			);
			Point textPos = new(xPos - formattedText.Width / 2.0, Bounds.Height - PADDING_Y + 5);
			context.DrawText(
				formattedText,
				textPos
			);
		}
	}
}