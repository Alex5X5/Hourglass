namespace Hourglass.GUI.Views;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util.Attributes;

public partial class MainView : ViewBase {

    [TranslateMember("Views.Pages.TaskDetails.Labels.Start", "Start")]
    public string StartLabelText { get; set; } = "";

    [TranslateMember("Views.Pages.TaskDetails.Labels.Stop", "Finish")]
    public string FinishLabelText { get; set; } = "";

    public MainView() : this(null) {

	}

	public MainView(MainViewModel model) : base() {
		InitializeComponent();

		//DataContext = model;
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
