namespace Hourglass.GUI.Pages.Timer.GraphRenderer;

using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using HourGlass.GUI.Pages.Timer;
using System.Drawing.Drawing2D;

public class DayGraphRenderer : GraphRenderer {

	protected override int MAX_TASKS => 5;

	public override int TASK_GRAPH_COLUMN_COUNT => 1;


	protected override int GRAPH_CLICK_ADDITIONAL_WIDTH => 5;
	protected override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 2;

	protected override int GRAPH_MINIMAL_WIDTH => 8;
	protected override int GRAPH_CORNER_RADIUS => 12;

	public DayGraphRenderer(IHourglassDbService dbService, TimerWindow timerWindow) : base(dbService, timerWindow, TimerWindowMode.Day) { }

	protected async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		await _dbService.QueryTasksOfDayAtDateAsync(_parent.SelectedDay) ?? [];

	protected override void DrawTimeline(Graphics g) {
        using (Brush textBrush = new SolidBrush(Color.Black))
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
			for (int i = 0; i < 25; i++) {
				int xPos = (Width - 2 * PADDING_X) * i / 24 + PADDING_X;
				g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
				g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, Height - PADDING_Y - TIMELINE_MARK_HEIGHT);
				g.DrawString(
					Convert.ToString(i) + ":00",
					new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
					textBrush,
					new Point((Width - 2 * PADDING_X) * (i + 1) / 24, Height - PADDING_Y + 5)
				);
			}
		}
	}

	protected override void DrawTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		using Font font = new("Segoe UI", 30F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
		string text;
		if (task.description.Length > 25)
			text = task.description[..25] + "...";
		else
			text = task.description;
		float textWidth = g.MeasureString(text, font).Width;
		if (graphLength > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX + 3, graphPosY + 15);
			return;
		}
		if (graphPosX > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX - textWidth - 5, graphPosY + 15);
			return;
		}
		using (Brush brush = new SolidBrush(Color.Black))
			g.DrawString(text, font, brush, graphPosX + graphLength + 5, graphPosY + 15);
		return;
	}

	protected override void DrawTaskGraph(Graphics g, Database.Models.Task task, int i) {
		long todaySeconds = DateTimeService.FloorDay(_parent.SelectedDay).Ticks / TimeSpan.TicksPerSecond;
		//long todaySeconds = nowSeconds - nowSeconds % TimeSpan.SecondsPerDay;
		Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerHour, todaySeconds, 24, MAX_TASKS, 0, 0, GRAPH_MINIMAL_WIDTH, i, 1);
		Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		//using GraphicsPath path = GetRoundedRectanglePath(rect, GRAPH_CORNER_RADIUS);
		//using Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidBrush(task.DisplayColor);
		//g.FillPath(brush, path);
		//DrawTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
	}
}
