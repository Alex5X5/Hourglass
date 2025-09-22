namespace Hourglass.GUI.Pages.Timer;

using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using HourGlass.GUI.Pages.Timer;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

class GraphRenderer : Panel {

	#region fields

	private TimerWindow _parent;
	public IHourglassDbService? _dbService;
	public TimerWindowMode WindowMode;


	private Bitmap image;

	private const int MAX_TASKS_PER_DAY = 4;
	private const int MAX_TASKS_PER_WEEK = MAX_TASKS_PER_DAY * 5;

	private const int DAY_GRAPH_CLICK_ADDITIONAL_WIDTH = 8, WEEK_GRAPH_CLICK_ADDITIONAL_WIDTH = 5, MONTH_GRAPH_CLICK_ADDITIONAL_WIDTH = 5;
	private const int DAY_GRAPH_CLICK_ADDITIONAL_HEIGHT = 5, WEEK_GRAPH_CLICK_ADDITIONAL_HEIGHT = 2, MONTH_GRAPH_CLICK_ADDITIONAL_HEIGHT = 2;

    private const int DAY_GRAPH_MINIMAL_WIDTH = 8, WEEK_GRAPH_MINIMAL_WIDTH = 5, MONTH_GRAPH_MINIMAL_WIDTH = 2;
	private const int DAY_GRAPH_CORNER_RADIUS = 12, WEEK_GRAPH_CORNER_RADIUS = 5, MONTH_GRAPH_CONRER_RADIUS = 2;

	private const int PADDING_X = 50, PADDING_Y = 30;

	#endregion fields

	public GraphRenderer(IHourglassDbService dbService, TimerWindowMode windowMode, TimerWindow parent) : this() {
		_parent = parent;
		_dbService = dbService;
		WindowMode = windowMode;
	}

	public GraphRenderer() : base() {
		image = new Bitmap(Width, Height);
		DoubleBuffered = true;
	}

	#region draw methods

	private Rectangle GetTaskRectanlge(Database.Models.Task task, long xAxisSegmentDuration, long originSecond, int xAxisSegmentCount, int yAxisSegmentCount, int additionalWidth, int additionalHeight, int minimalWidth, ref int graphPosY, int columns) {
		int xAxisSegmentSize = (image.Width - 2 * PADDING_X) / xAxisSegmentCount;
        int yAxisSegmentSize = (int)((image.Height - 2 * PADDING_Y) / (yAxisSegmentCount*1.5));
		long duration = task.finish - task.start;
		double proportion = (double)xAxisSegmentSize / xAxisSegmentDuration;
		int graphLength = (int)Math.Floor(duration * proportion);
		int graphPosX = (int)Math.Floor((task.start-originSecond) * proportion) + PADDING_X;
		//graphPosX += xAxisSegmentSize;
		int width = (graphLength > minimalWidth ? graphLength : minimalWidth) + additionalWidth * 2;
        Rectangle res = new(
			graphPosX - additionalWidth,
            graphPosY - additionalHeight,
            width,
			yAxisSegmentSize + additionalHeight * 2
        );
		graphPosY += yAxisSegmentSize;
		graphPosY += yAxisSegmentSize / 2;
		//using (Graphics g = Graphics.FromImage(image))
		//using (Brush b = new SolidBrush(Color.AliceBlue))
		//	g.FillRectangle(b, res.X, res.Y, res.Width, res.Height);
		return res;
	}

	private static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius) {
		GraphicsPath path = new();
		int diameter = radius * 2;
		if(diameter > rect.Width)
			diameter = rect.Width;
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
			g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
			for (int i = 0; i < 25; i++) {
				int xPos = (Width - 2 * PADDING_X) * i / 24 + PADDING_X;
				g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
				g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, (int)Math.Floor(Height - PADDING_Y * 1.25));
				g.DrawString(
					Convert.ToString(i) + ":00",
					new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
					textBrush,
					new Point((Width - 2 * PADDING_X) * (i+1) / 24, Height - PADDING_Y + 5)
				);
			}
		}
	}

	private void DrawWeekTimeline(Graphics g) {
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
			for (int i = 0; i <8; i++) {
				int xPos = (Width - 2 * PADDING_X) * i / 7 + PADDING_X;
				g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
				g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, (int)Math.Floor(Height - PADDING_Y * 1.25));

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
		int xAxisSegmentSize = (Width - 2 * PADDING_X) / daysInCurrentMonth;
		using (Brush textBrush = new SolidBrush(Color.Black))
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
			for (int i = 0; i < daysInCurrentMonth + 1; i++) {
				int xPos = (Width - 2 * PADDING_X) * i / (daysInCurrentMonth + 1) + PADDING_X;
				g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
				g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, (int)Math.Floor(Height - PADDING_Y * 1.25));
				g.DrawString(
					Convert.ToString(i),
					new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
					textBrush,
					new Point(xPos + (Convert.ToString(i).Length == 1 ? 9 : 6), Height * 19 / 20 + 2)
				);
			}
			g.DrawLine(hintLines, Width - PADDING_X, Height - PADDING_Y, Width - PADDING_X, PADDING_Y);
			g.DrawLine(timeline, Width - PADDING_X, Height - PADDING_Y, Width - PADDING_X, (int)Math.Floor(Height - PADDING_Y * 1.25));
		}
	}

	private static void DrawDayTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		using Font font = new("Segoe UI", 30F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
		string text;
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
		using Font font = new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
		string text;
		if (task.description.Length > 25)
			text = task.description[..25] + "...";
		else
			text = task.description;
		float textWidth = g.MeasureString(text, font).Width;
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
		Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerHour, todaySeconds, 24, MAX_TASKS_PER_DAY, 0, 0, DAY_GRAPH_MINIMAL_WIDTH, ref graphPosY, 1);
        Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
        Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
        using (GraphicsPath path = GetRoundedRectanglePath(rect, DAY_GRAPH_CORNER_RADIUS))
        using (Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidBrush(task.DisplayColor))
            g.FillPath(brush, path);
		DrawDayTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
	}

	private void DrawWeekTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY) {
		DateTime thisMonday = DateTimeService.GetMondayOfCurrentWeek();
		long thisWeekSeconds = thisMonday.Ticks / TimeSpan.TicksPerSecond;
		Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerDay, thisWeekSeconds, 7, MAX_TASKS_PER_WEEK, 0, 0, WEEK_GRAPH_MINIMAL_WIDTH, ref graphPosY, 1);
		Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		using (GraphicsPath path = GetRoundedRectanglePath(rect, WEEK_GRAPH_CORNER_RADIUS))
		using (Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidBrush(task.DisplayColor))
			g.FillPath(brush, path);
        DrawWeekTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
	}

	private void DrawMonthTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY) {
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		long thisMonthSeconds = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).Ticks / TimeSpan.TicksPerSecond;
        Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerDay, thisMonthSeconds, daysInCurrentMonth, daysInCurrentMonth, 0, 0, MONTH_GRAPH_MINIMAL_WIDTH, ref graphPosY, 1);
        Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
        Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
		using (GraphicsPath path = GetRoundedRectanglePath(rect, MONTH_GRAPH_CONRER_RADIUS))
        using (Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidBrush(task.DisplayColor))
            g.FillPath(brush, path);
		DrawWeekTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
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
            g.CompositingMode = CompositingMode.SourceOver;

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
							int graphPosY = PADDING_Y;
							for (int i = 0; i < MAX_TASKS_PER_DAY && i < tasks.Count; i++) {
								DrawDayTaskGraph(g, tasks[i], ref graphPosY);
							}
						}
						break;
					case TimerWindowMode.Week:
						tasks = await _dbService.QueryTasksOfCurrentWeekAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = PADDING_Y;
							for (int i = 0; i < MAX_TASKS_PER_WEEK && i < tasks.Count; i++) {
								DrawWeekTaskGraph(g, tasks[i], ref graphPosY);
							}
						}
						break;
					case TimerWindowMode.Month:
						tasks = await _dbService.QueryTasksOfCurrentMonthAsync();
						if (tasks != null && tasks.Count > 0) {
							int graphPosY = PADDING_Y;
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
		int graphPosY = PADDING_Y;
		bool taskClicked = false;
		foreach (Database.Models.Task task in tasks) {
			taskClicked = WindowMode switch {
				TimerWindowMode.Day =>
					GetTaskRectanlge(
						task,
						TimeSpan.SecondsPerHour,
						DateTime.Today.Ticks / TimeSpan.TicksPerSecond,
						24,
						MAX_TASKS_PER_DAY,
						DAY_GRAPH_CLICK_ADDITIONAL_WIDTH,
						DAY_GRAPH_CLICK_ADDITIONAL_HEIGHT,
						DAY_GRAPH_MINIMAL_WIDTH,
						ref graphPosY,
						1
					).Contains(mousePos),
				TimerWindowMode.Week =>
					GetTaskRectanlge(
						task,
						TimeSpan.SecondsPerDay,
						DateTimeService.GetMondayOfCurrentWeek().Ticks / TimeSpan.TicksPerSecond,
						7,
						MAX_TASKS_PER_WEEK,
						WEEK_GRAPH_CLICK_ADDITIONAL_WIDTH,
						WEEK_GRAPH_CLICK_ADDITIONAL_HEIGHT,
						WEEK_GRAPH_MINIMAL_WIDTH,
						ref graphPosY,
						1
					).Contains(mousePos),
				TimerWindowMode.Month =>
					GetTaskRectanlge(
						task,
						TimeSpan.SecondsPerDay,
						DateTimeService.GetFirstDayOfCurrentMonth().Ticks / TimeSpan.TicksPerSecond,
						daysInCurrentMonth,
						daysInCurrentMonth,
						MONTH_GRAPH_CLICK_ADDITIONAL_WIDTH,
						MONTH_GRAPH_CLICK_ADDITIONAL_HEIGHT,
						MONTH_GRAPH_MINIMAL_WIDTH,
						ref graphPosY,
						1
					).Contains(mousePos),
				_ => false
			};
			if (taskClicked) {
				TaskDetails.TaskDetailsPopup taskDetailsWindow = new(task, _dbService, _parent);
				taskDetailsWindow.ShowDialog();
				break;
			}
		}
		if (!taskClicked) {
			if (WindowMode == TimerWindowMode.Day) {
			} else if (WindowMode == TimerWindowMode.Week) {

                int offset = (int)Math.Floor((Width - 2.0 * PADDING_X) / 7);
				DateTime newDate = DateTimeService.GetMondayOfCurrentWeek().AddDays(offset);
			} else if (WindowMode == TimerWindowMode.Month) {

			}
		}
	}
}
