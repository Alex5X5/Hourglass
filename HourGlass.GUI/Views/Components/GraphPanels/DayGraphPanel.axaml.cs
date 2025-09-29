namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Media;

using Hourglass.Database.Services.Interfaces;

using Point = Avalonia.Point;

public partial class DayGraphPanel : GraphPanel {

	public override int TASK_GRAPH_COLUMN_COUNT => throw new NotImplementedException();

	protected override int MAX_TASKS => 1000;

	protected override int GRAPH_CLICK_ADDITIONAL_WIDTH => throw new NotImplementedException();

	protected override int GRAPH_CLICK_ADDITIONAL_HEIGHT => throw new NotImplementedException();

	protected override int GRAPH_MINIMAL_WIDTH => throw new NotImplementedException();

	protected override int GRAPH_CORNER_RADIUS => throw new NotImplementedException();

	public DayGraphPanel() : base() {
		InitializeComponent();
	}

	public DayGraphPanel(IHourglassDbService dbService) : base(dbService) {
		InitializeComponent();
	}

	protected override void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		// Draw rectangle background
		var rect = new Rect(100, 100, 50, 20);
		context.DrawRectangle(Background, null, rect);
		string text = "a looon string";
		// Create formatted text
		var formattedText = new FormattedText(
			text,
			System.Globalization.CultureInfo.CurrentCulture,
			FlowDirection.LeftToRight,
			new Typeface("Arial"),
			16, // Font size
			Foreground
		);

		// Center the text
		var x = (Bounds.Width - formattedText.Width) / 2;
		var y = (Bounds.Height - formattedText.Height) / 2;

		// Draw the text
		context.DrawText(formattedText, new Point(x, y));
	}

	protected override void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i) {
		//throw new NotImplementedException();
	}

	protected override void DrawTimeline(DrawingContext context) {
		//throw new NotImplementedException();
	}

	//protected async override Task<List<Database.Models.Task>> GetTasksAsync() {
		
	//	return await Task.Run(()=>new List<Database.Models.Task>());
	//	//throw new NotImplementedException();
	//}
}