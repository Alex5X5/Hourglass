namespace Hourglass.GUI.Views.Components.GraphModeButtons;

using Avalonia.Media;

using Hourglass.Util;

public partial class MonthGraphModeButtonIcon : Avalonia.Controls.UserControl {
	public MonthGraphModeButtonIcon() : base() {
		InitializeComponent();
	}

	public override void Render(DrawingContext context) {
		base.Render(context);
		int startOffset = (int)DateTimeService.GetMondayOfCurrentWeek().DayOfWeek + 1;
		double sideLength = Math.Min(Bounds.Width, Bounds.Height);
		double drawGridSize = sideLength / 22;
		for (int i = startOffset; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + startOffset; i++) {
			double xPos = i % 7 * drawGridSize * 3 + drawGridSize + Bounds.Width / 2 - 11 * drawGridSize;
			double yPos = Math.Floor(i / 7.0) * drawGridSize * 3 + drawGridSize + Bounds.Height / 2 - 7 * drawGridSize;
			Color color = Color.FromArgb(255, 255, 255, 255);
			if (i % 7 == 5 | i % 7 == 6)
				color = Color.FromArgb(255, 174, 174, 174);
			if (DateTimeService.TodayIsDayOfWeek(i))
				color = Color.FromArgb(255, 192, 0, 0);
			Brush brush = new SolidColorBrush(color);
			context.FillRectangle(brush, new(xPos, yPos, 2 * drawGridSize, 2 * drawGridSize));
		}
	}
}