namespace Hourglass.GUI.Views.Components.GraphModeButtons;

using Avalonia.Media;

public partial class DayGraphModebutton : Avalonia.Controls.Button {
	public DayGraphModebutton() : base() {
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