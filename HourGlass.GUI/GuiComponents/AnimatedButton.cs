using System.Drawing.Design;

namespace Hourglass.GUI.GuiComponents;

public class AnimatedButton:Button, IDisposable{

	private Image defaultImage;
	private Image hoverImage;
	private Image disbledImage;
	private Image pressedImage;

	private Image currentImage;

	private bool isHovered = false;
	private bool isPressed = false;

	ButtonState state;

	public AnimatedButton(
		string defaultImagePath,
		string hoverImagePath,
		string disabledImagePath,
		string pressedImagePath
	) : base() {
		defaultImage = Bitmap.FromFile(defaultImagePath);
		hoverImage = Bitmap.FromFile(hoverImagePath);
		disbledImage = Bitmap.FromFile(disabledImagePath);
		pressedImage = Bitmap.FromFile(pressedImagePath);
	}

	protected override void OnPaintBackground(PaintEventArgs arags) { }

	protected override void OnPaint(PaintEventArgs args) {
		args.Graphics.DrawImage(currentImage, new Point(0,0));
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
		if (isPressed) {
			state = ButtonState.PRESSED;
			return;
		}
		if (isHovered) {
			state = ButtonState.HOVERING;
			return;
		}
		state = ButtonState.DEFAULT;
		UpdateImage();
	}

	public void Disable() {
		state = ButtonState.DISABLED;
		UpdateImage();
	}

	private void UpdateImage() {
		currentImage = state switch {
			ButtonState.PRESSED => pressedImage,
			ButtonState.HOVERING => hoverImage,
			ButtonState.DISABLED => disbledImage,
			_ => defaultImage
		};
	}

	private enum ButtonState {
		DEFAULT,
		HOVERING,
		PRESSED,
		DISABLED
	}
}
