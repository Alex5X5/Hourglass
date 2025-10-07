namespace Hourglass.GUI.Views.Components.GraphModeButtons;

using Avalonia.Media;

using Hourglass.Util;

public partial class WeekGraphModeButtonIcon : Avalonia.Controls.UserControl {
	public WeekGraphModeButtonIcon() : base() {
		InitializeComponent();
	}

	public override void Render(DrawingContext context) {
		base.Render(context);
		double sideLength = Math.Min(Bounds.Width, Bounds.Height);
		double drawGridSize = sideLength / 22;
		for (int i = 0; i < 7; i++) {
			double xPos = i * drawGridSize * 3 + drawGridSize + Bounds.Width / 2 - 11 * drawGridSize;
			double yPos = Bounds.Height / 2 - 2 * drawGridSize;
			Color color = Color.FromArgb(255, 255, 255, 255);
			if (i % 7 == 5 | i % 7 == 6)
				color = Color.FromArgb(255, 174, 174, 174);
			if (i == (int)DateTime.Now.DayOfWeek - 1)
				color = Color.FromArgb(255, 192, 0, 0);
			Brush brush = new SolidColorBrush(color);
			context.FillRectangle(brush, new(xPos, yPos, 2 * drawGridSize, 4 * drawGridSize));
		}
	}
}