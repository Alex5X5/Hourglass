namespace Hourglass.GUI.Views;

using Avalonia.Controls;
using Hourglass.GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;


public partial class MainWindow : Window {

	private List<ViewBase> views = [];

	public MainWindow() : this(null) {

	}

	public MainWindow(IServiceProvider? commonServices) : base() {
        InitializeComponent();
		DataContext = new MainWindowViewModel(this, commonServices);
		mainView.OnRegisteringViews(views);
		mainView.DataContext = new MainViewModel(commonServices);
		//IServiceCollection viewCollection = new ServiceCollection();
		//viewCollection.AddViewModels();
		//IServiceProvider viewModels = viewCollection.BuildServiceProvider();
	}
}
