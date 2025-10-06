namespace Hourglass.GUI.Views;

using Avalonia.Controls;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;

public partial class MainView : ViewBase {

	public MainView() : this(null, null) {

	}

	public MainView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		DataContext = new MainViewViewModel(this, services);
		if (DataContext is MainViewViewModel viewModel)
			viewModel.ChangePage<TimerPageViewModel>();
		InitializeComponent();
	}

	private void TimerModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("timer mode button click!");
		if (DataContext is MainViewViewModel viewModel)
			viewModel.ChangePage<TimerPageViewModel>();
	}

	private void GraphModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("graph mode button click!");
		if (DataContext is MainViewViewModel viewModel)
			viewModel.ChangePage<GraphPageViewModel>();
	}

	private void ExportModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("export mode button click!");
		if (DataContext is MainViewViewModel viewModel)
			viewModel.ChangePage<ExportPageViewModel>();
	}

	private void ProjectModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("project mode button click!");
		if (DataContext is MainViewViewModel viewModel)
			viewModel.ChangePage<ProjectPageViewModel>();
	}
}
