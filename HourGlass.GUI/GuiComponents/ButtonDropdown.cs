using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Hourglass.GUI.GuiComponents;

public class ButtoDropdown:Panel, IDisposable{

	private Image defaultImage;
	private Image hoverImage;
	private Image disbledImage;
	private Image pressedImage;

	private Image currentImage;

	private bool isEnabled = true;
	private bool isHovered = false;
	private bool isPressed = false;

	private List<Action<PaintEventArgs>> additionalPaintActions = new();

	ButtonState state;

	public ButtoDropdown(
		string defaultImagePath,
		string hoverImagePath,
		string disabledImagePath,
		string pressedImagePath,
		Rectangle size
	) : base() {
		Size = new(size.Width, size.Height);
		Location = new Point(size.X,size.Y);
		SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		BackColor = Color.Transparent;


		defaultImage = Bitmap.FromFile(defaultImagePath);
		hoverImage = Bitmap.FromFile(hoverImagePath);
		disbledImage = Bitmap.FromFile(disabledImagePath);
		pressedImage = Bitmap.FromFile(pressedImagePath);
		UpdateImage();
	}

	private void DrawParentBackground(Graphics graphics) {
		if (Parent != null) {
			using var bitmap = new Bitmap(Width, Height);
			using (var g = Graphics.FromImage(bitmap)) {
				g.TranslateTransform(-Left, -Top);
				var paintArgs = new PaintEventArgs(g, new Rectangle(Left, Top, Width, Height));
				InvokePaint(Parent, paintArgs);
			}
			graphics.DrawImage(bitmap, 0, 0);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs arags) { }

	protected override void OnPaint(PaintEventArgs args) {
		DrawParentBackground(args.Graphics);
		args.Graphics.CompositingMode = CompositingMode.SourceOver;
		args.Graphics.CompositingQuality = CompositingQuality.HighQuality;
		args.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		args.Graphics.SmoothingMode = SmoothingMode.HighQuality;
		args.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

		using (var wrapMode = new ImageAttributes()) {
			wrapMode.SetWrapMode(WrapMode.Clamp);
			args.Graphics.DrawImage(currentImage, 0, 0, Size.Width, Size.Height);
		}
		using(var brush = new SolidBrush(ForeColor))
		args.Graphics.DrawString(Text, Font, brush, Width/2-args.Graphics.MeasureString(Text, Font).Width/2, Height / 2 - args.Graphics.MeasureString(Text, Font).Height / 2);
		foreach(var action in additionalPaintActions)
			action.Invoke(args);
	}

	protected override void OnMouseDown(MouseEventArgs mevent) {
		base.OnMouseDown(mevent);
		isPressed = true;
		UpdateImage();
	}

	protected override void OnMouseUp(MouseEventArgs mevent) {
		base.OnMouseUp(mevent);
		isPressed = false;
		UpdateImage();
	}

	protected override void OnMouseEnter(EventArgs e) {
		isHovered = true;
		UpdateImage();
	}

	protected override void OnMouseLeave(EventArgs e) {
		isHovered = false;
		isPressed = false;
		UpdateImage();
	}

	public void AddOnPaintEvent(Action<PaintEventArgs> paintFunction) {
		additionalPaintActions.Add(paintFunction);
	}

	public new void Dispose() {
		GC.SuppressFinalize(this);
		base.Dispose();
		defaultImage.Dispose();
		hoverImage.Dispose();
		disbledImage.Dispose();
		pressedImage.Dispose();
	}

	public void Enable() {
		isEnabled = true;
		Enabled = true;
		UpdateImage();
	}

	public void Disable() {
		isEnabled = false;
		Enabled = false;
		UpdateImage();
	}

	private void UpdateState() {
		state = ButtonState.DEFAULT;
		if (isHovered)
			state = ButtonState.HOVERING;
		if (isPressed)
			state = ButtonState.PRESSED;
		if (!isEnabled)
			state = ButtonState.DISABLED;
	}

	private void UpdateImage() {
		UpdateState();
		currentImage = state switch {
			ButtonState.PRESSED => pressedImage,
			ButtonState.HOVERING => hoverImage,
			ButtonState.DISABLED => disbledImage,
			_ => defaultImage
		};
		Invalidate();
	}

	private enum ButtonState {
		DEFAULT,
		HOVERING,
		PRESSED,
		DISABLED
	}
}
