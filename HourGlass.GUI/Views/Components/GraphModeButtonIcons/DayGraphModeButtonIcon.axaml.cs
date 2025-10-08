namespace Hourglass.GUI.Views.Components.GraphModeButtons;

using Avalonia;
using Avalonia.Media;

public partial class DayGraphModeButtonIcon : Avalonia.Controls.UserControl {
	public DayGraphModeButtonIcon() : base() {
		InitializeComponent();
	}

	public override void Render(DrawingContext context) {
		base.Render(context);
		double sideLength = Math.Min(Bounds.Width, Bounds.Height);
		double drawGridSize = sideLength / 12;
		Brush outlinebrush = new SolidColorBrush(Colors.Black);
		Pen outlinePen = new(outlinebrush) { Thickness = 2 };
		var rect = new Rect(
			(Bounds.Width - 6 * drawGridSize) / 2,
			(Bounds.Height - 8 * drawGridSize) / 2,
			drawGridSize * 6,
			drawGridSize * 8
		);
		context.DrawRectangle(
			outlinePen,
			rect
		);
		for(int i=1; i<6; i++)
		context.DrawLine(
			outlinePen,
			new(rect.X + drawGridSize * i, rect.Y - drawGridSize),
			new(rect.X + drawGridSize * i, rect.Y + drawGridSize)
		);
		string dayText = Convert.ToString(DateTime.Now.Day);
		var formattedText = new FormattedText(
			dayText,
			System.Globalization.CultureInfo.CurrentCulture,
			FlowDirection.LeftToRight,
			new Typeface("Arial"),
			22,
			new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))
		);
		Point textPos = new(rect.Width / 2 - formattedText.Width / 2 + rect.X, rect.Height / 2 - formattedText.Height / 2 + rect.Y);
		context.DrawText(
			formattedText,
			textPos
		);
	}
}