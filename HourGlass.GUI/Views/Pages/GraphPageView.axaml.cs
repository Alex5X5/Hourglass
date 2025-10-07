using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;

using Microsoft.Extensions.DependencyInjection;

namespace Hourglass.GUI.Views.Pages;

public partial class GraphPageView : PageViewBase {

	public GraphPageView() : this(null, null) {

	}

	public GraphPageView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
		DataContext = new GraphPageViewModel(this, services);
		if (DataContext is GraphPageViewModel model_)
			model_.ChangeGraphPanel<DayGraphPanelViewModel>();
	}

	private void DayModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("day mode button click!");
		if (DataContext is GraphPageViewModel viewModel)
			viewModel.ChangeGraphPanel<DayGraphPanelViewModel>();
	}

	private void WeekModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("week mode button click!");
		if (DataContext is GraphPageViewModel viewModel)
			viewModel.ChangeGraphPanel<WeekGraphPanelViewModel>();
	}

	private void MonthModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("month mode button click!");
		if (DataContext is GraphPageViewModel viewModel)
			viewModel.ChangeGraphPanel<MonthGraphPanelViewModel>();
	}

	//public override void Render(DrawingContext context) {
	//	base.Render(context);
	//	string text = "graph page view";
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