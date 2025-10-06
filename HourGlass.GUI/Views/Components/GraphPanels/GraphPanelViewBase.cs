namespace Hourglass.GUI.Views.Components.GraphPanels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;

public abstract class GraphPanelViewBase : ViewBase {

	#region fields

	protected IHourglassDbService _dbService;

	public abstract int TASK_GRAPH_COLUMN_COUNT { get; }

	public abstract int MAX_TASKS { get; }

	public abstract int GRAPH_CLICK_ADDITIONAL_WIDTH { get; }
	public abstract int GRAPH_CLICK_ADDITIONAL_HEIGHT { get; }

	public abstract int GRAPH_MINIMAL_WIDTH { get; }
	public abstract int GRAPH_CORNER_RADIUS { get; }

	protected double PADDING_X => Bounds.Width / 30;
	protected double PADDING_Y => Bounds.Height / 30;
	protected double TIMELINE_MARK_HEIGHT => 7;

	#endregion fields

	public GraphPanelViewBase() : this(null, null) {
		
	}

	public GraphPanelViewBase(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		
	}

	protected abstract void DrawTimeline(DrawingContext context);

	protected abstract void DrawTaskDescriptionStub(DrawingContext context, Database.Models.Task task, double graphPosX, double graphPosY, double graphLength);

	protected abstract void DrawTaskGraph(DrawingContext context, Database.Models.Task task, int i);

	protected Avalonia.Rect GetTaskRectanlgeBase(Database.Models.Task task, long xAxisSegmentDuration, long originSecond, int xAxisSegmentCount, int yAxisSegmentCount, double additionalWidth, double additionalHeight, double minimalWidth, int i, int columns) {
		double xAxisSegmentSize = (Bounds.Width - 2.0 * PADDING_X) / xAxisSegmentCount;
		double yAxisSegmentSize = (Bounds.Height - 2.0 * PADDING_Y) / (yAxisSegmentCount + 1.0);
		double proportion = xAxisSegmentSize / xAxisSegmentDuration;
		double graphPosX = (task.start - originSecond) * proportion + PADDING_X;
		long duration = task.finish - task.start;
		double graphLength = duration * proportion;
		double width = graphLength > minimalWidth ? graphLength : minimalWidth + additionalWidth * 2;
		Avalonia.Rect res = new(
			graphPosX - additionalWidth,
			yAxisSegmentSize * i * 1.5 - additionalHeight + PADDING_Y,
			width,
			yAxisSegmentSize + additionalHeight * 2
		);
		return res;
	}

	public abstract Rect GetTaskRectanlge(Database.Models.Task task, double additionalWidth, double additionalHeght, int i);

	public async override void Render(DrawingContext context) {
		base.Render(context);
		if (!IsVisible)
			return;
		var brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

		context.FillRectangle(brush, new Avalonia.Rect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height));
		DrawTimeline(context);
		//DrawTaskDescriptionStub(context, new() {description="blablabla"}, 100, 100, 300);

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
		List<Database.Models.Task> tasks = [];
		if(DataContext is GraphPanelViewModelBase model)
			tasks = await model.GetTasksAsync();
		if (tasks != null && tasks.Count > 0) {
			double graphPosY = PADDING_Y;
			for (int i = 0; i < MAX_TASKS && i < tasks.Count; i++) {
				DrawTaskGraph(context, tasks[i], i);
			}
		}
		//}
		//args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		//args.Graphics.DrawImage(image, 0, 0, Width, Height);
	}

	public void OnClick(object? sender, TappedEventArgs e) {
		if (DataContext is GraphPanelViewModelBase model)
			model.OnClick(sender, e);
	}

	public void OnDoubleClick(object? sender, TappedEventArgs e) {
		if (DataContext is GraphPanelViewModelBase model)
			model.OnDoubleClick(sender, e);
	}
}
