using Avalonia.Media;

namespace Hourglass.GUI.Views.Components.GraphModeButtons;

public partial class DayGraphModebutton : Avalonia.Controls.UserControl{
	public DayGraphModebutton() : base() {
		InitializeComponent();
	}

	public override void Render(DrawingContext context) {
		context.FillRectangle(Avalonia.Media.Brushes.Aqua, new Avalonia.Rect(10, 10, 10, 10));
	}
}