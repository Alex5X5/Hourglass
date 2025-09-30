using Avalonia.Media;

using Hourglass.GUI.ViewModels.Pages;

namespace Hourglass.GUI.Views.Components.GraphModeButtons;

public partial class MonthGraphModebutton : Avalonia.Controls.Button {
	public MonthGraphModebutton() : base() {
		InitializeComponent();
	}

	public override void Render(DrawingContext context) {
		base.Render(context);
		string text = "month";
		// Create formatted text
		var formattedText = new FormattedText(
			text,
			System.Globalization.CultureInfo.CurrentCulture,
			Avalonia.Media.FlowDirection.LeftToRight,
			new Typeface("Arial"),
			14, // Font size
			Foreground
		);

		// Center the text
		var x = (Bounds.Width - formattedText.Width) / 2;
		var y = (Bounds.Height - formattedText.Height) / 2;

		// Draw the text
		context.DrawText(formattedText, new Avalonia.Point(x, y));
	}
}