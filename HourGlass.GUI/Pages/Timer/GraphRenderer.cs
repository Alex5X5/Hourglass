namespace Hourglass.GUI.Pages.Timer;

using Hourglass.Database.Services.Interfaces;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

class GraphRenderer : Panel {

#region fields
	
	private Bitmap image;
	public IHourglassDbService? _dbService;
	public TimerWindowMode WindowMode;

	private const int MAX_TASKS_PER_DAY = 6;
	private const int MAX_TASKS_PER_WEEK = MAX_TASKS_PER_DAY * 5;

	#endregion fields

	public GraphRenderer(IHourglassDbService dbService, TimerWindowMode windowMode) : this() {
		_dbService = dbService;
		WindowMode = windowMode;
	}

	public GraphRenderer() : base() {
		image = new Bitmap(Width, Height);
		DoubleBuffered = true;
	}

	#region draw methods

	private Rectangle GetTaskRectanlge(Database.Models.Task task, long xAxisSegmentDuration, long originSecond, int xAxisSegmentCount, int yAxisSegmentCount, int additionalWidth, int additionalHeight, ref int graphPosY) {
		int xAxisSegmentSize = image.Width / (xAxisSegmentCount + 2);
        int yAxisSegmentSize = image.Height / (yAxisSegmentCount + 2);
		int yGraphSpace = yAxisSegmentSize / 2;
		long duration = task.finish - task.start;
		double proportion = (double)xAxisSegmentSize / xAxisSegmentDuration;
		int graphLength = (int)Math.Floor(duration * proportion);
		int graphPosX = (int)Math.Floor((task.start-originSecond) * proportion);
		graphPosX += xAxisSegmentSize;
		graphPosY += (int)(yAxisSegmentSize / 2);
		Rectangle res = new(
			graphPosX - additionalWidth,
            graphPosY - additionalHeight,
            graphLength + additionalWidth * 2,
			yAxisSegmentSize + additionalHeight * 2
        );
		graphPosY += yAxisSegmentSize;
		//using (Graphics g = Graphics.FromImage(image))
		//using (Brush b = new SolidBrush(Color.AliceBlue))
		//	g.FillRectangle(b, res.X, res.Y, res.Width, res.Height);
		return res;
	}

	private static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius) {
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
		if(WindowMode == TimerWindowMode.Day)
			DrawDayTimeline(g);
		if(WindowMode == TimerWindowMode.Week)
			DrawWeekTimeline(g);
		if(WindowMode == TimerWindowMode.Month)
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
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		int xAxisSegmentSize = Width / (daysInCurrentMonth + 2);
		using (Brush textBrush = new SolidBrush(Color.Black))
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, xAxisSegmentSize, Height * 19 / 20, xAxisSegmentSize*(daysInCurrentMonth+2), Height * 19 / 20);
			for (int i = 1; i < daysInCurrentMonth; i++) {
				g.DrawLine(timeline, xAxisSegmentSize * i, Height * 19 / 20, xAxisSegmentSize * i, (int)Math.Floor(Height * 18.75 / 20));
				g.DrawLine(hintLines, xAxisSegmentSize * i, Height * 19 / 20, xAxisSegmentSize * i, (int)Math.Floor(Height / 20.0));
				g.DrawString(
					Convert.ToString(i),
					new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
					textBrush,
					new Point(xAxisSegmentSize * i + (Convert.ToString(i).Length == 1 ? 9 : 6), Height * 19 / 20 + 2)
				);
			}
		}
	}

	private static void DrawDayTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		string text;
		Font font = new("Segoe UI", 30F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
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
				g.DrawString(text, font, brush, graphPosX + 3, graphPosY + 6);
			return;
		}
		if (graphPosX > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX - textWidth - 5, graphPosY + 6);
			return;
		}
		using (Brush brush = new SolidBrush(Color.Black))
			g.DrawString(text, font, brush, graphPosX + graphLength + 5, graphPosY + 6);
		return;
	}

	private static void DrawWeekTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		string text;
		Font font = new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
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
				g.DrawString(text, font, brush, graphPosX + 3, graphPosY - 1 );
			return;
		}
		if (graphPosX > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX - textWidth - 3, graphPosY - 1 );
			return;
		}
		using (Brush brush = new SolidBrush(Color.Black))
			g.DrawString(text, font, brush, graphPosX + graphLength + 3, graphPosY - 1 );
		return;
	}

	private void DrawDayTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY) {
		long nowSeconds = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		long todaySeconds = nowSeconds - (nowSeconds % TimeSpan.SecondsPerDay);
		Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerHour, todaySeconds, 24, MAX_TASKS_PER_DAY, 0, 0, ref graphPosY);
		using (GraphicsPath path = GetRoundedRectanglePath(rect, 20))
		using (Brush brush = new SolidBrush(task.DisplayColor))
			g.FillPath(brush, path);
		DrawDayTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
	}

	private void DrawWeekTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY) {
		int daysSinceMonday = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
		long thisWeekSeconds = DateTime.Today.AddDays(-daysSinceMonday).Ticks / TimeSpan.TicksPerSecond;
		Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerDay, thisWeekSeconds, 7, MAX_TASKS_PER_WEEK, 0, 0, ref graphPosY);
		using (GraphicsPath path = GetRoundedRectanglePath(rect, 5))
		using (Brush brush = new SolidBrush(task.DisplayColor))
			g.FillPath(brush, path);
        DrawWeekTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
	}

	private void DrawMonthTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY) {
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		long thisMonthSeconds = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).Ticks / TimeSpan.TicksPerSecond;
		Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerDay, thisMonthSeconds, daysInCurrentMonth, daysInCurrentMonth, 0, 0, ref graphPosY);
		using (GraphicsPath path = GetRoundedRectanglePath(rect, 2))
		using (Brush brush = new SolidBrush(task.DisplayColor))
			g.FillPath(brush, path);
	}

#endregion

	protected override void OnPaintBackground(PaintEventArgs e) { }

	protected override async void OnPaint(PaintEventArgs args) {
		args.Graphics.Clear(Color.Gainsboro);
		if (image.Width != Width | image.Height != Height) {
			image.Dispose();
			image = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
		}
		using (Graphics g = Graphics.FromImage(image)) {
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingMode = CompositingMode.SourceOver; // This preserves background

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			g.SmoothingMode = SmoothingMode.HighQuality;
			g.Clear(Color.Gainsboro);

			DrawTimeline(g);
			if (_dbService != null) {
				List<Database.Models.Task> tasks;
				switch (WindowMode) {
					case TimerWindowMode.Day:
						tasks = await _dbService.QueryTasksOfCurrentDayAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = 0;
							for (int i = 0; i < MAX_TASKS_PER_DAY && i < tasks.Count; i++) {
								DrawDayTaskGraph(g, tasks[i], ref graphPosY);
							}
						}
						break;
					case TimerWindowMode.Week:
						tasks = await _dbService.QueryTasksOfCurrentWeekAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = 0;
							for (int i = 0; i < MAX_TASKS_PER_WEEK && i < tasks.Count; i++) {
								DrawWeekTaskGraph(g, tasks[i], ref graphPosY);
							}
						}
						break;
					case TimerWindowMode.Month:
						tasks = await _dbService.QueryTasksOfCurrentWeekAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = 0;
							for (int i = 0; i < 100 && i < tasks.Count; i++) {
								DrawMonthTaskGraph(g, tasks[i], ref graphPosY);
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
		Point mousePos = PointToClient(MousePosition);
		Console.WriteLine($"click at {mousePos}");
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		List<Database.Models.Task>? tasks = WindowMode switch {
			TimerWindowMode.Day => await _dbService.QueryTasksOfCurrentDayAsync(),
			TimerWindowMode.Week => await _dbService.QueryTasksOfCurrentWeekAsync(),
			TimerWindowMode.Month => await _dbService.QueryTasksOfCurrentWeekAsync(),
			_=> []
		};
		using (Graphics g = Graphics.FromImage(image))
			g.Clear(Color.Gainsboro);
		int graphPosY = 0;
		foreach (Database.Models.Task task in tasks) {
			bool clicked = WindowMode switch {
				TimerWindowMode.Day =>
					GetTaskRectanlge(
						task,
						TimeSpan.SecondsPerHour,
						DateTime.Today.Ticks / TimeSpan.TicksPerSecond,
						24,
						MAX_TASKS_PER_DAY,
						10,
						5,
						ref graphPosY
					).Contains(mousePos),
				TimerWindowMode.Week =>
					GetTaskRectanlge(
						task,
						TimeSpan.SecondsPerDay,
						DateTime.Today.AddDays(-((7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7)).Ticks / TimeSpan.TicksPerSecond,						7,
						MAX_TASKS_PER_WEEK,
						5,
						2,
						ref graphPosY
					).Contains(mousePos),
				TimerWindowMode.Month =>
					GetTaskRectanlge(
						task,
						TimeSpan.SecondsPerDay,
						new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).Ticks / TimeSpan.TicksPerSecond,
						daysInCurrentMonth,
						MAX_TASKS_PER_WEEK * daysInCurrentMonth,
						2,
						2,
						ref graphPosY
					).Contains(mousePos),
				_ => false
			};
			if (clicked) {
				TaskDetails.TaskDetails taskDetailsWindow = new(task, _dbService);
				taskDetailsWindow.ShowDialog();
				break;
			}
		}
	}
}
