namespace Hourglass.GUI.Views.Pages;

using Hourglass.GUI.ViewModels;

public partial class ProjectPageView : PageViewBase {

	public ProjectPageView() : this(null, null) {

	}

	public ProjectPageView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
	}

	//public override void Render(DrawingContext context) {
	//	base.Render(context);
	//	string text = "project page view";
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