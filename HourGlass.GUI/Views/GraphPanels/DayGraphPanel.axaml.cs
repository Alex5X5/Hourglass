namespace Hourglass.GUI.Views.GraphPanels;

using Avalonia.Controls;
using Avalonia.Media;

public partial class DayGraphPanel : UserControl {
    public DayGraphPanel() {
        InitializeComponent();
    }

	public override void Render(DrawingContext context) {
		base.Render(context);
		// Draw here.
		var brush = new SolidColorBrush(Color.FromArgb(255, 200, 40, 150)); // Adjust thickness if necessary
		context.FillRectangle(brush, new(10, 10, 10, 10));
	}
}