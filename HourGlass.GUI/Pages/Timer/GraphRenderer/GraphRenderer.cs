namespace Hourglass.GUI.Pages.Timer.GraphRenderer;

using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using HourGlass.GUI.Pages.Timer;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

public abstract class GraphRenderer : Panel {

	#region fields

	protected TimerWindow _parent;
	protected IHourglassDbService _dbService;
	public TimerWindowMode WindowMode;


	protected Bitmap image;

	public abstract int TASK_GRAPH_COLUMN_COUNT { get; }

	protected abstract int MAX_TASKS { get; }

	protected abstract int GRAPH_CLICK_ADDITIONAL_WIDTH { get; }
	protected abstract int GRAPH_CLICK_ADDITIONAL_HEIGHT { get; }

	protected abstract int GRAPH_MINIMAL_WIDTH { get; }
	protected abstract int GRAPH_CORNER_RADIUS { get; }

	protected const int PADDING_X = 50, PADDING_Y = 30;

	#endregion fields

	protected GraphRenderer(IHourglassDbService dbService, TimerWindow parent, TimerWindowMode windowMode) : this() {
		_parent = parent;
		_dbService = dbService;
		WindowMode = windowMode;
	}

	public GraphRenderer() : base() {
		image = new Bitmap(Width, Height);
		DoubleBuffered = true;
	}

	protected abstract Task<List<Database.Models.Task>> GetTasksAsync();

	protected abstract void DrawTimeline(Graphics g);

	protected abstract void DrawTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength);

	protected abstract void DrawTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY);

	protected Rectangle GetTaskRectanlge(Database.Models.Task task, long xAxisSegmentDuration, long originSecond, int xAxisSegmentCount, int yAxisSegmentCount, int additionalWidth, int additionalHeight, int minimalWidth, ref int graphPosY, int columns) {
		int xAxisSegmentSize = (image.Width - 2 * PADDING_X) / xAxisSegmentCount;
		int yAxisSegmentSize = (int)((image.Height - 2 * PADDING_Y) / (yAxisSegmentCount * 1.5));
		long duration = task.finish - task.start;
		double proportion = (double)xAxisSegmentSize / xAxisSegmentDuration;
		int graphLength = (int)Math.Floor(duration * proportion);
		int graphPosX = (int)Math.Floor((task.start - originSecond) * proportion) + PADDING_X;
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

	protected static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius) {
		GraphicsPath path = new();
		int diameter = radius * 2;
		if (diameter > rect.Width)
			diameter = rect.Width;
		path.StartFigure();
		path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
		path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
		path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
		path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
		path.CloseFigure();
		return path;
	}


	protected override void OnPaintBackground(PaintEventArgs e) { }

	protected async override void OnPaint(PaintEventArgs args) {
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
				List<Database.Models.Task> tasks = await GetTasksAsync();
				if (tasks != null && tasks.Count > 0) {
					int graphPosY = PADDING_Y;
					for (int i = 0; i < MAX_TASKS && i < tasks.Count; i++) {
						DrawTaskGraph(g, tasks[i], ref graphPosY);
					}
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
		//Console.WriteLine($"click at {mousePos}");
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
		List<Database.Models.Task>? tasks = await GetTasksAsync();
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
                        _parent.SelectedDay.Ticks / TimeSpan.TicksPerSecond,
                        24,
                        MAX_TASKS,
                        GRAPH_CLICK_ADDITIONAL_WIDTH,
                        GRAPH_CLICK_ADDITIONAL_WIDTH,
                        GRAPH_MINIMAL_WIDTH,
                        ref graphPosY,
                        1
                    ).Contains(mousePos),
				TimerWindowMode.Week =>
					GetTaskRectanlge(
                        task,
                        TimeSpan.SecondsPerDay,
                        DateTimeService.GetMondayOfWeekAtDate(_parent.SelectedDay).Ticks / TimeSpan.TicksPerSecond,
                        7,
                        MAX_TASKS,
                        GRAPH_CLICK_ADDITIONAL_WIDTH,
                        GRAPH_CLICK_ADDITIONAL_WIDTH,
                        GRAPH_MINIMAL_WIDTH,
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
                        GRAPH_CLICK_ADDITIONAL_WIDTH,
                        GRAPH_CLICK_ADDITIONAL_WIDTH,
                        GRAPH_MINIMAL_WIDTH,
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
		}
	}

    protected override void OnDoubleClick(EventArgs e) {
        Point mousePos = PointToClient(MousePosition);
		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        if (WindowMode == TimerWindowMode.Week) {
            int offset = (int)Math.Floor((mousePos.X - PADDING_X) * 7.0 / (Width - 2 * PADDING_X));
            DateTime modayOfWeek = DateTimeService.GetMondayOfWeekAtDate(_parent.SelectedDay);
            _parent.ChangeGraphRendererMode(TimerWindowMode.Day, modayOfWeek.AddDays(offset));
        } else if (WindowMode == TimerWindowMode.Month) {
            int offset = (int)Math.Floor((double)(mousePos.X - PADDING_X) * daysInCurrentMonth / (Width - 2 * PADDING_X));
            DateTime firstDayOfMonth = DateTimeService.GetFirstDayOfMonthAtDate(DateTimeService.GetFirstDayOfCurrentMonth());
            _parent.ChangeGraphRendererMode(TimerWindowMode.Week, firstDayOfMonth.AddDays(offset));
        }
    }
}
