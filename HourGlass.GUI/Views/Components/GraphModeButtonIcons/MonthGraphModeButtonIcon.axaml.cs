namespace Hourglass.GUI.Views.Components.GraphModeButtons;

using Avalonia.Media;
using Hourglass.Util;

public partial class MonthGraphModeButtonIcon : Avalonia.Controls.UserControl {
	public MonthGraphModeButtonIcon() : base() {
		InitializeComponent();
	}

	public override void Render(DrawingContext context) {
		base.Render(context);
		int startOffset = (int)DateTimeService.GetMondayOfCurrentWeek().DayOfWeek;
		double sideLength = Math.Min(Bounds.Width, Bounds.Height);
		double drawGridSize = sideLength / 22;
        int daysInCurrentMonth = DateTimeService.DaysInCurrentMonth();
        int weekDayCounter = (int)DateTimeService.GetFirstDayOfCurrentMonth().DayOfWeek - 1;
        int today = weekDayCounter + DateTime.Today.Day -1;
        for (int i = 0; i < daysInCurrentMonth; i++) {
            double xPos = weekDayCounter % 7 * drawGridSize * 3 + drawGridSize + Bounds.Width / 2 - 11 * drawGridSize;
            double yPos = Math.Floor(weekDayCounter / 7.0) * drawGridSize * 3 + drawGridSize + Bounds.Height / 2 - 7 * drawGridSize;
            Color color = Color.FromArgb(255, 255, 255, 255);
            if (weekDayCounter % 7 == 5 | weekDayCounter % 7 == 6)
                color = Color.FromArgb(255, 174, 174, 174);
            if (weekDayCounter == today)
                color = Color.FromArgb(255, 192, 0, 0);
            Brush brush = new SolidColorBrush(color);
            context.FillRectangle(brush, new(xPos, yPos, 2 * drawGridSize, 2 * drawGridSize)); weekDayCounter++;
        }
	}
}