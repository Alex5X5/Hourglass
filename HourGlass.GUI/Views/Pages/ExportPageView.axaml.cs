namespace Hourglass.GUI.Views.Pages;

using Avalonia.Controls;
using Avalonia.Media;

using Hourglass.GUI.ViewModels;
using Hourglass.PDF.Services.Interfaces;

public partial class ExportPageView : PageViewBase {

	public ExportPageView() : this(null, null) {

	}

	public ExportPageView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
	}

	//public override void Render(DrawingContext context) {
	//	base.Render(context);
	//	string text = "export page view";
	//	// Create formatted text
	//	var formattedText = new FormattedText(
	//		text,
	//		System.Globalization.CultureInfo.CurrentCulture,
	//		Avalonia.Media.FlowDirection.LeftToRight,
	//		new Typeface("Arial"),
	//		16, // Font size
	//		Foreground
	//	);

	//	// Center the text
	//	var x = (Bounds.Width - formattedText.Width) / 2;
	//	var y = (Bounds.Height - formattedText.Height) / 2;

	//	// Draw the text
	//	context.DrawText(formattedText, new Avalonia.Point(x, y));
	//}
}