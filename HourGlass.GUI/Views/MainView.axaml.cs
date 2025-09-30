namespace Hourglass.GUI.Views;

using Avalonia.Controls;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;

public partial class MainView : UserControl {

	public MainView() {
		InitializeComponent();
		//DataContext = new MainViewViewModel();
		if (DataContext is MainViewViewModel viewModel)
			viewModel.ChangePage<TimerPageViewModel>();
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
