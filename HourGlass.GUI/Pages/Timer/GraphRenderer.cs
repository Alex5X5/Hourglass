namespace Hourglass.GUI.Pages.Timer;

using Hourglass.Database.Serices.Interfaces;

using System.Drawing.Drawing2D;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

class GraphRenderer : Panel {

	#region fields

	//public const int VIRTUAL_WIDTH = 1400, VIRTUAL_HEIGHT = 700;
	private Bitmap image;

	private readonly Brush GROUND_COLOR = new SolidBrush(Color.FromArgb(85, 85, 90));

	public IHourglassDbService? _dbService;

	private Random rand = new Random();

	#endregion fields

	public GraphRenderer(IHourglassDbService dbService) : this() {
		_dbService = dbService;
	}

	public GraphRenderer() : base() {
		image = new Bitmap(Width, Height);
		this.DoubleBuffered = true;
	}

	protected override void OnPaintBackground(PaintEventArgs e) {

	}

	private void DrawTimeline(Graphics g) {
		using (Pen hintLines = new(Brushes.Black))
		using (Pen pen = new(Brushes.Black)) {
				g.DrawLine(pen, Width / 26, Height * 19 / 20, Width * 25 / 26, Height * 19 / 20);
			for (int i = 1; i < 25; i++) {
				g.DrawLine(pen, Width * i / 26, Height * 19 / 20, Width * i / 26, (int)Math.Floor(Height * 18.75 / 20));
				g.DrawLine(hintLines, Width * i / 26, Height * 19 / 20, Width * i / 26, (int)Math.Floor(Height / 20.0));
			}
		}

	}

	private void DrawTaskBar(Graphics g, Database.Models.Task task, int graphPosY) {
		long duration = task.finish - task.start;
		double proportion = TimeSpan.SecondsPerDay / image.Width;
		int graphLength = (int)Math.Floor(duration / proportion);
		long now = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		long today = now - (now % TimeSpan.SecondsPerDay);
		int graphPosX = (int)Math.Floor((task.start - today) / proportion);
		graphPosX += image.Width / 26;

		Rectangle rect = new(
			graphPosX,
			graphPosY,
			graphLength,
			(int)Math.Floor(image.Height / 20.0));
		using (GraphicsPath path = GetRoundedRectanglePath(rect, 5))
		using (Brush pen = new SolidBrush(Color.Black))
			g.FillPath(pen, path);
	}

	protected override async void OnPaint(PaintEventArgs e) {
		
		if (image.Width != Width | image.Height != Height) {
			image.Dispose();
			image = new Bitmap(Width, Height);
		}
		using (Graphics g = Graphics.FromImage(image)) {
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			g.SmoothingMode = SmoothingMode.HighQuality;
			g.Clear(Color.FromArgb(255, 255, 255));
			//long now = DateTime.Now.Second;
			//g.DrawString(Convert.ToString(now), new Font(FontFamily.GenericSansSerif, 10), GROUND_COLOR, 0, 0);
			DrawTimeline(g);
			int yPos = image.Height / 10;
			List<Database.Models.Task>? tasks = await _dbService?.QueryTasksOfLastDayAsync();
			if (tasks != null && tasks.Count>0) {
				int graphPosY = (int)Math.Floor(image.Height / 20.0);
				for (int i = 0; i < 10 && i < tasks.Count; i++) {
					DrawTaskBar(g, tasks[i], graphPosY);
					graphPosY += (int)Math.Floor(image.Height / 20.0);
					graphPosY += (int)Math.Floor(image.Height / 40.0);
				}
			}
		}
		e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		e.Graphics.DrawImage(image, 0, 0, Width, Height);
	}
	private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius) {
		GraphicsPath path = new GraphicsPath();

		int d = radius * 2;
		path.StartFigure();
		path.AddArc(rect.X, rect.Y, d, d, 180, 90);
		path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
		path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
		path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
		path.CloseFigure();

		return path;
	}
}
