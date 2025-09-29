namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia.Controls;
using Avalonia.Media;
using Hourglass.Database.Services.Interfaces;
using System;


public abstract class GraphPanel : UserControl {

	#region fields

	//protected TimerWindow _parent;
	//protected IHourglassDbService _dbService;
	//public TimerWindowMode WindowMode;
	protected IHourglassDbService _dbService;


	protected Bitmap image;

	public abstract int TASK_GRAPH_COLUMN_COUNT { get; }

	protected abstract int MAX_TASKS { get; }

	protected abstract int GRAPH_CLICK_ADDITIONAL_WIDTH { get; }
	protected abstract int GRAPH_CLICK_ADDITIONAL_HEIGHT { get; }

	protected abstract int GRAPH_MINIMAL_WIDTH { get; }
	protected abstract int GRAPH_CORNER_RADIUS { get; }

	protected const int PADDING_X = 50, PADDING_Y = 30, TIMELINE_MARK_HEIGHT = 7;

	#endregion fields

	public GraphPanel() : base() {
	}

	public GraphPanel(IHourglassDbService dbService) : this() {
		_dbService = dbService;
	}

	//protected abstract Task<List<Database.Models.Task>> GetTasksAsync();

	protected abstract void DrawTimeline(DrawingContext context);

	protected abstract void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength);

	protected abstract void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i);

	protected Rectangle GetTaskRectanlge(Database.Models.Task task, long xAxisSegmentDuration, long originSecond, int xAxisSegmentCount, int yAxisSegmentCount, int additionalWidth, int additionalHeight, int minimalWidth, int i, int columns) {
		double xAxisSegmentSize = (image.Width - 2.0 * PADDING_X) / xAxisSegmentCount;
		double yAxisSegmentSize = (image.Height - 2.0 * PADDING_Y) / (yAxisSegmentCount + 1.0);
		double proportion = xAxisSegmentSize / xAxisSegmentDuration;
		int graphPosX = (int)Math.Floor((task.start - originSecond) * proportion) + PADDING_X;
		long duration = task.finish - task.start;
		int graphLength = (int)Math.Floor(duration * proportion);
		int width = (graphLength > minimalWidth ? graphLength : minimalWidth) + additionalWidth * 2;
		Rectangle res = new(
			graphPosX - additionalWidth,
			(int)(yAxisSegmentSize * i * 1.5) - additionalHeight + PADDING_Y,
			width,
			(int)(yAxisSegmentSize) + additionalHeight * 2
		);
		//using (Graphics g = Graphics.FromImage(image))
		//using (Brush b = new SolidBrush(Color.AliceBlue))
		//	g.FillRectangle(b, res.X, res.Y, res.Width, res.Height);
		return res;
	}

	public override void Render(DrawingContext context) {
		base.Render(context);
		var brush = new SolidColorBrush(Color.FromArgb(255, 200, 40, 150)); // Adjust thickness if necessary
		context.FillRectangle(brush, new(MAX_TASKS, 10, 10, 10));
		DrawTaskDescriptionStub(context, new() {description="blablabla"}, 100, 100, 300);
		//args.Graphics.Clear(Color.Gainsboro);

		//if (image.Width != Width | image.Height != Height) {
		//	image.Dispose();
		//	image = new Bitmap((int)Width, (int)Height, PixelFormat.Format32bppArgb);
		//}
		//using (Graphics g = Graphics.FromImage(image)) {
		//	g.SmoothingMode = SmoothingMode.AntiAlias;
		//	g.InterpolationMode = InterpolationMode.HighQualityBicubic;
		//	g.PixelOffsetMode = PixelOffsetMode.HighQuality;
		//	g.CompositingMode = CompositingMode.SourceOver;

		//	g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

		//	g.SmoothingMode = SmoothingMode.HighQuality;
		//	//g.Clear(Color.Gainsboro);

		//	DrawTimeline(g);
		//	if (_dbService != null) {
		//		List<Database.Models.Task> tasks = await GetTasksAsync();
		//		if (tasks != null && tasks.Count > 0) {
		//			int graphPosY = PADDING_Y;
		//			for (int i = 0; i < MAX_TASKS && i < tasks.Count; i++) {
		//				DrawTaskGraph(g, tasks[i], i);
		//			}
		//		}
		//	}
		//}
		//args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		//args.Graphics.DrawImage(image, 0, 0, Width, Height);
	}
}
