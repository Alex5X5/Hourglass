namespace Hourglass.GUI.Pages.Timer;

using Hourglass.Database.Services.Interfaces;

using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

class GraphRenderer : Panel {

	#region fields

	private Bitmap image;

	private readonly Brush GROUND_COLOR = new SolidBrush(Color.FromArgb(85, 85, 90));

	public IHourglassDbService? _dbService;
	#endregion fields

	public GraphRenderer(IHourglassDbService dbService) : this() {
		_dbService = dbService;
	}

	public GraphRenderer() : base() {
		image = new Bitmap(Width, Height);
		DoubleBuffered = true;
		Click += OnClick;
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

	private void DrawTaskMarker(Graphics g, Database.Models.Task task, int graphPosY) {
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
		string text;
		if (task.description.Length > 15)
			text = task.description.Substring(0, 15);
		else
			text = task.description;
		using (Brush brush = new SolidBrush(Color.Black))
			g.DrawString(text, new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel, 0), brush, graphPosX + 3, graphPosY + 3);
	}

	protected override async void OnPaint(PaintEventArgs e) {
		e.Graphics.Clear(Color.Gainsboro);
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
			
			//using (GraphicsPath path = GetRoundedRectanglePath(new Rectangle(0, 0, image.Width, image.Height), 55))
			//using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
			//	g.FillPath(brush, path);
			DrawTimeline(g);
			int yPos = image.Height / 10;
			if (_dbService != null) {
				List<Database.Models.Task>? tasks = await _dbService.QueryTasksOfCurrentDayAsync();
				if (tasks != null && tasks.Count > 0) {
					int graphPosY = (int)Math.Floor(image.Height / 20.0);
					for (int i = 0; i < 10 && i < tasks.Count; i++) {
						DrawTaskMarker(g, tasks[i], graphPosY);
						graphPosY += (int)Math.Floor(image.Height / 20.0);
						graphPosY += (int)Math.Floor(image.Height / 40.0);
					}
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

	private void OnClick(object? sender, EventArgs e) {
		
	}
}
