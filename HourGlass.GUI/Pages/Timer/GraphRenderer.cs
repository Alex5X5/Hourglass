namespace Hourglass.GUI.Pages.Timer;

using Hourglass.Database.Services.Interfaces;

using System.Drawing.Drawing2D;
using System.Windows.Forms;

class GraphRenderer : Panel {

	#region fields
	
	private Bitmap image;
	public IHourglassDbService? _dbService;

	#endregion fields

	public GraphRenderer(IHourglassDbService dbService) : this() {
		_dbService = dbService;
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
		using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170,170, 170))))
		using (Pen timeline = new(Brushes.Black)) {
			g.DrawLine(timeline, Width / 26, Height * 19 / 20, Width * 25 / 26, Height * 19 / 20);
			for (int i = 1; i < 25; i++) {
				g.DrawLine(timeline, Width * i / 26, Height * 19 / 20, Width * i / 26, (int)Math.Floor(Height * 18.75 / 20));
				g.DrawLine(hintLines, Width * i / 26, Height * 19 / 20, Width * i / 26, (int)Math.Floor(Height / 20.0));
			}
		}
	}

	private void DrawTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
		string text;
		Font font = new("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
		if (task.description.Length > 15)
			text = task.description[..15] + "...";
		else
			text = task.description;
		float textWidth = g.MeasureString(text, font).Width;
		if (task.StartDateTime.DayOfWeek != DateTime.Now.DayOfWeek) {
			return;
		}
		if (graphLength > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX + 3, graphPosY + 1);
			return;
		}
		if (graphPosX > textWidth + 10) {
			using (Brush brush = new SolidBrush(Color.Black))
				g.DrawString(text, font, brush, graphPosX - textWidth - 5, graphPosY + 1);
			return;
		}
		using (Brush brush = new SolidBrush(Color.Black))
			g.DrawString(text, font, brush, graphPosX + graphLength + 5, graphPosY + 1);
		return;
	}

	private void DrawTaskGraph(Graphics g, Database.Models.Task task, int graphPosY) {
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
		DrawTaskDescriptionStub(g, task, graphPosX, graphPosY, graphLength);
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
				List<Database.Models.Task>? tasks = await _dbService.QueryTasksOfCurrentDayAsync();
				if (tasks != null && tasks.Count > 0) {
					int graphPosY = (int)Math.Floor(image.Height / 20.0);
					for (int i = 0; i < 12 && i < tasks.Count; i++) {
						DrawTaskGraph(g, tasks[i], graphPosY);
						graphPosY += (int)Math.Floor(image.Height / 20.0);
						graphPosY += (int)Math.Floor(image.Height / 40.0);
					}
				}
			}
		}
		args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		args.Graphics.DrawImage(image, 0, 0, Width, Height);
	}

	protected async override void OnClick(EventArgs e) {
		Console.WriteLine("OnClick");
		Point mousePos = PointToClient(MousePosition);
		if (_dbService == null)
			return;
		List<Database.Models.Task>? tasks = await _dbService.QueryTasksOfCurrentDayAsync();
		int graphPosY = (int)Math.Floor(image.Height / 20.0);
		foreach (Database.Models.Task task in tasks) {
			long duration = task.finish - task.start;
			double proportion = TimeSpan.SecondsPerDay / (image.Width * 24 / 26);
			int graphLength = (int)Math.Floor(duration / proportion);
			long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
			long today = now - (now % TimeSpan.SecondsPerDay);
			int graphPosX = 
				(int)Math.Floor((task.start - today) / proportion) 
					+ (int)Math.Floor(image.Height * 3 / 40.0);
			if (mousePos.X > graphPosX)
				if (mousePos.Y > graphPosY)
					if(mousePos.X < graphPosX + graphLength)
						if (mousePos.Y < graphPosY + image.Height / 20.0)
							Console.WriteLine("OnClick * 2");
			graphPosY += (int)Math.Floor(image.Height / 20.0);
			graphPosY += (int)Math.Floor(image.Height / 40.0);
		}
	}
}
