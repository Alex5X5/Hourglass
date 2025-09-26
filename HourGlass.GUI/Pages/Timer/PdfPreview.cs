using Hourglass.Database.Services.Interfaces;
using HourGlass.GUI.Pages.Timer;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Hourglass.GUI.Pages.Timer;

internal class PdfPreview : Panel {

	protected Bitmap image;

	protected TimerWindow _parent;
	protected IHourglassDbService _dbService;

	public PdfPreview(IHourglassDbService dbService, TimerWindow parent) : base() {
		_parent = parent;
		_dbService = dbService;
		image = new Bitmap(Width, Height);
		DoubleBuffered = true;
	}

	protected override void OnPaint(PaintEventArgs args) {
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

            using (Brush b = new SolidBrush(Color.FromArgb(100,255,10)))
                g.FillRectangle(b, 10, 10, 10, 10);

        }
        args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        args.Graphics.DrawImage(image, 0, 0, Width, Height);
    }

}
