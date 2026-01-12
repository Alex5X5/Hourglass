namespace Hourglass.GUI.Views;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util.Attributes;

public partial class MainView : ViewBase {

    [TranslateMember("Views.MainView.Buttons.Timer", "Timer")]
    public string TimerModeButtonText { get; set; } = "";

    [TranslateMember("Views.MainView.Buttons.Graphs", "Graphs")]
    public string GraphModeButtonText { get; set; } = "";

    [TranslateMember("Views.MainView.Buttons.Export", "Export")]
    public string ExportModeButtonText { get; set; } = "";


	public MainView() : base() {
		InitializeComponent();
		if (DataContext is MainViewModel viewModel) {
			viewModel.ChangePage<TimerPageViewModel>();
		}
	}

	private void TimerModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("timer mode button click!");
		if (DataContext is MainViewModel viewModel)
			viewModel.ChangePage<TimerPageViewModel>();
	}

	private void GraphModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("graph mode button click!");
		if (DataContext is MainViewModel viewModel)
			viewModel.ChangePage<GraphPageViewModel>();
	}

	private void ExportModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("export mode button click!");
		if (DataContext is MainViewModel viewModel)
			viewModel.ChangePage<ExportPageViewModel>();
	}

	private void ProjectModeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("project mode button click!");
		if (DataContext is MainViewModel viewModel)
			viewModel.ChangePage<ProjectPageViewModel>();
	}
}
