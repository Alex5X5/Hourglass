namespace Hourglass.GUI.Pages.Timer;

using Hourglass.Database.Services.Interfaces;

using System.Drawing.Drawing2D;
using System.Windows.Forms;

class GraphRenderer : Panel {

#region fields
	
	private Bitmap image;
	public IHourglassDbService? _dbService;
	public TimerWindowMode _windowMode;



#endregion fields

	public GraphRenderer(IHourglassDbService dbService, TimerWindowMode windowMode) : this() {
		_dbService = dbService;
		_windowMode = windowMode;
	}

	public GraphRenderer() : base() {
		image = new Bitmap(Width, Height);
		DoubleBuffered = true;
	}

#region draw methods

	private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius) {
		GraphicsPath path = new();
		int diameter = radius * 2;
		path.StartFigure();
		path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
		path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
		path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
		path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
		path.CloseFigure();
		return path;
	}

	private void DrawTimeline(Graphics g) {
		if(_windowMode == TimerWindowMode.Day)
			DrawDayTimeline(g);
		if(_windowMode == TimerWindowMode.Week)
			DrawWeekTimeline(g);
		if(_windowMode == TimerWindowMode.Month)
			DrawMonthTimeline(g);
	}

	private void DrawDayTimeline(Graphics g) {
		using (Brush textBrush = new SolidBrush(Color.Black))
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, Width / 26, Height * 19 / 20, Width * 25 / 26, Height * 19 / 20);
			for (int i = 1; i < 26; i++) {
				g.DrawLine(timeline, Width * i / 26, Height * 19 / 20, Width * i / 26, (int)Math.Floor(Height * 18.75 / 20));
				g.DrawLine(hintLines, Width * i / 26, Height * 19 / 20, Width * i / 26, (int)Math.Floor(Height / 20.0));
				g.DrawString(
					Convert.ToString(i - 1) + ":00",
					new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
					textBrush,
					new Point(Width * i / 26 - 12, Height * 19 / 20 + 2)
				);
			}
		}
	}

	private void DrawWeekTimeline(Graphics g) {
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, Width / 16, Height * 19 / 20, Width * 15 /16, Height * 19 / 20);
			for (int i = 0; i <8; i++) {
				int xPos = (int)Math.Floor(Width * (i+0.5) /8);
				g.DrawLine(timeline, xPos, Height * 19 / 20, xPos, (int)Math.Floor(Height * 18.75 / 20));
				g.DrawLine(hintLines, xPos , Height * 19 / 20, xPos, Height / 20);
			}
		}
		string[] days = ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"];
		using (Brush textBrush = new SolidBrush(Color.Black))
			for (int i = 0; i < 7; i++) {
				string s = days[i];
				g.DrawString(
					s,
					new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
					textBrush,
					new Point((int)Math.Floor(Width * (i+1) / 8.0), Height * 19 / 20 + 2)
				);
			}
	}

	private void DrawMonthTimeline(Graphics g) {
			
	}

	private void DrawDayTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		string text;
		Font font = new("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
		if (task.description.Length > 25)
			text = task.description[..25] + "...";
		else
			text = task.description;
		float textWidth = g.MeasureString(text, font).Width;
		if (task.StartDateTime.DayOfWeek != DateTime.Now.DayOfWeek) {
			return;
		}
		if (graphLength > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX + 3, graphPosY - 5);
			return;
		}
		if (graphPosX > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX - textWidth - 5, graphPosY - 5);
			return;
		}
		using (Brush brush = new SolidBrush(Color.Black))
			g.DrawString(text, font, brush, graphPosX + graphLength + 5, graphPosY - 5);
		return;
	}

	private void DrawDayTaskGraph(Graphics g, Database.Models.Task task, int graphPosY) {
		long duration = task.finish - task.start;
		double proportion = TimeSpan.SecondsPerDay / (image.Width * 24 / 26);
		int graphLength = (int)Math.Floor(duration / proportion);
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		long today = now - (now % TimeSpan.SecondsPerDay);
		int graphPosX = (int)Math.Floor((task.start - today) / proportion);
		graphPosX += image.Width / 26;
		Rectangle rect = new(
			graphPosX,
			graphPosY,
			graphLength,
			(int)Math.Floor(image.Height / 20.0));
		using (GraphicsPath path = GetRoundedRectanglePath(rect, 5))
		using (Brush brush = new SolidBrush(Color.FromArgb(255, 122, 0)))
			g.FillPath(brush, path);
		DrawDayTaskDescriptionStub(g, task, graphPosX, graphPosY, graphLength);
	}

	private void DrawWeekTaskGraph(Graphics g, Database.Models.Task task, int graphPosY) {
		int MAX_GRAPH_COUNT = 10;
		int GRAPH_HEIGHT = (int)Math.Floor(image.Height / (MAX_GRAPH_COUNT * 1.5 + 2.0));
		long duration = task.finish - task.start;
		double proportion = TimeSpan.SecondsPerDay * 8 / image.Width;
		int graphLength = (int)Math.Floor(duration / proportion);
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		long today = now - (now % TimeSpan.SecondsPerDay);
		int graphPosX = (int)Math.Floor((task.start - today) / proportion);
		graphPosX += image.Width / 16;
		Rectangle rect = new(
			graphPosX,
			graphPosY,
			graphLength,
			(int)Math.Floor(image.Height / 30.0));
		using (GraphicsPath path = GetRoundedRectanglePath(rect, 5))
		using (Brush brush = new SolidBrush(Color.FromArgb(255, 122, 0)))
			g.FillPath(brush, path);
		DrawDayTaskDescriptionStub(g, task, graphPosX, graphPosY, graphLength);
	}

#endregion

	protected override void OnPaintBackground(PaintEventArgs e) { }

	protected override async void OnPaint(PaintEventArgs args) {
		args.Graphics.Clear(Color.Gainsboro);
		if (image.Width != Width | image.Height != Height) {
			image.Dispose();
			image = new Bitmap(Width, Height);
		}
		using (Graphics g = Graphics.FromImage(image)) {
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			g.SmoothingMode = SmoothingMode.HighQuality;
			g.Clear(Color.Gainsboro);
			
			DrawTimeline(g);
			int yPos = image.Height / 10;
			if (_dbService != null) {
				List<Database.Models.Task> tasks;
				switch (_windowMode) {
					case TimerWindowMode.Day:
						tasks = await _dbService.QueryTasksOfCurrentDayAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = (int)Math.Floor(image.Height / 20.0);
							for (int i = 0; i < 12 && i < tasks.Count; i++) {
								DrawDayTaskGraph(g, tasks[i], graphPosY);
								graphPosY += (int)Math.Floor(image.Height / 20.0);
								graphPosY += (int)Math.Floor(image.Height / 40.0);
							}
						}
						break;
					case TimerWindowMode.Week:
						tasks = await _dbService.QueryTasksOfCurrentWeekAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = (int)Math.Floor(image.Height / 30.0);
							for (int i = 0; i < 28 && i < tasks.Count; i++) {
								DrawWeekTaskGraph(g, tasks[i], graphPosY);
								graphPosY += (int)Math.Floor(image.Height / 60.0);
								graphPosY += (int)Math.Floor(image.Height / 30.0);
							}
						}
						break;
					case TimerWindowMode.Month:
						tasks = await _dbService.QueryTasksOfCurrentWeekAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = (int)Math.Floor(image.Height / 20.0);
							for (int i = 0; i < 12 && i < tasks.Count; i++) {
								DrawDayTaskGraph(g, tasks[i], graphPosY);
								graphPosY += (int)Math.Floor(image.Height / 20.0);
								graphPosY += (int)Math.Floor(image.Height / 40.0);
							}
						}
						break;
				}
			}
		}
		args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		args.Graphics.DrawImage(image, 0, 0, Width, Height);
	}

	protected async override void OnClick(EventArgs e) {
		if (_dbService == null)
			return;
		const int ADDITIONAL_HITBOX_WIDTH = 6;
		const int ADDITIONAL_HITBOX_HEIGHT = 2;
		int xAxisSegmentSize = (int)Math.Floor(image.Width / 26.0);
		int yAxisSegmentSize = (int)Math.Floor(image.Height / 20.0);
		int yGraphSpace = yAxisSegmentSize / 2;
		Point mousePos = PointToClient(MousePosition);
		Console.WriteLine($"click at {mousePos}");
		int graphPosY = yAxisSegmentSize;
		List<Database.Models.Task>? tasks = await _dbService.QueryTasksOfCurrentDayAsync();
		foreach (Database.Models.Task task in tasks) {
			long duration = task.finish - task.start;
			double proportion = xAxisSegmentSize * 24.0 / TimeSpan.SecondsPerDay;
			int graphLength = (int)Math.Floor(duration * proportion);
			long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
			long today = now - (now % TimeSpan.SecondsPerDay);
			int graphPosX = (int)Math.Floor((task.start - today) * proportion) + xAxisSegmentSize;
			Rectangle hitbox = new(
				graphPosX,
				graphPosY - ADDITIONAL_HITBOX_HEIGHT,
				graphLength + ADDITIONAL_HITBOX_WIDTH * 2,
				yAxisSegmentSize + ADDITIONAL_HITBOX_HEIGHT * 2
			);
			//using (Graphics g = Graphics.FromImage(image))
			//using (Brush b = new SolidBrush(Color.AliceBlue))		
			//	g.FillRectangle(b, hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height);

			if (hitbox.Contains(mousePos)) {
				TaskDetails.TaskDetails taskDetailsWindow = new(task, _dbService);
				taskDetailsWindow.ShowDialog();
				break;
			}
			//if (mousePos.X > graphPosX - ADDITIONAL_HITBOX_WIDTH)
			//	if (mousePos.Y > graphPosY - ADDITIONAL_HITBOX_HEIGHT)
			//		if (mousePos.X < graphPosX + graphLength + ADDITIONAL_HITBOX_WIDTH)
			//			if (mousePos.Y < graphPosY + image.Height / 20.0 + ADDITIONAL_HITBOX_HEIGHT) {
			//			}
			graphPosY += yAxisSegmentSize;
			graphPosY += yGraphSpace;
		}
	}
}
