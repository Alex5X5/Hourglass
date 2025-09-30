using Avalonia.Controls;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;

namespace Hourglass.GUI.Views;

public partial class MainWindow : Window {

	IHourglassDbService? _DbService;
	public IHourglassDbService? DbService {
		get => _DbService;
		set {
			_DbService ??= value;
			if (DataContext is MainViewViewModel viewModel)
				viewModel.DbService = value;
		}
	}

	public MainWindow() {
        InitializeComponent();
        DataContext = new MainViewViewModel();
	}

}
