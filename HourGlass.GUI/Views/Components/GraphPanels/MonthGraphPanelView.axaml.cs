namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Media;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Components.GraphPanels;

using Point = Avalonia.Point;

public partial class MonthGraphPanelView : GraphPanelViewBase {

	public override int TASK_GRAPH_COLUMN_COUNT => throw new NotImplementedException();

	protected override int MAX_TASKS => 1000;

	protected override int GRAPH_CLICK_ADDITIONAL_WIDTH => throw new NotImplementedException();

	protected override int GRAPH_CLICK_ADDITIONAL_HEIGHT => throw new NotImplementedException();

	protected override int GRAPH_MINIMAL_WIDTH => throw new NotImplementedException();

	protected override int GRAPH_CORNER_RADIUS => throw new NotImplementedException();

	public MonthGraphPanelView() : base() {
		InitializeComponent();
	}

	public MonthGraphPanelView(IHourglassDbService dbService) : base(dbService) {
		InitializeComponent();
	}

	protected override void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
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
		Console.WriteLine("DayGraphPanel draw task graph");
		//throw new NotImplementedException();
	}

	protected override void DrawTimeline(DrawingContext context) {
		context.DrawLine(new Pen(new SolidColorBrush(Colors.Green)), new(10,10), new(100,100));
		//using (Brush textBrush = new SolidBrush(Color.Black))
		//using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		//using (Pen timeline = new(Brushes.Black)) {
		//	g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
		//	for (int i = 0; i < 25; i++) {
		//		int xPos = (Width - 2 * PADDING_X) * i / 24 + PADDING_X;
		//		g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
		//		g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, Height - PADDING_Y - TIMELINE_MARK_HEIGHT);
		//		g.DrawString(
		//			Convert.ToString(i) + ":00",
		//			new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
		//			textBrush,
		//			new Point((Width - 2 * PADDING_X) * (i + 1) / 24, Height - PADDING_Y + 5)
		//		);
		//	}
		//}
	}

	protected async override Task<List<Database.Models.Task>> GetTasksAsync() {
		if (DataContext is WeekGraphPanelViewModel model)
			if (model.dbService != null)
				return await model.dbService.QueryTasksAsync();
		return [];
	}
}