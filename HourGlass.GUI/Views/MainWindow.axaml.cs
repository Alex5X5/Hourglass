namespace Hourglass.GUI.Views;

using Avalonia.Controls;
using Hourglass.GUI.ViewModels;


public partial class MainWindow : Window {

	private List<ViewBase> views = [];

	public MainWindow() : this(null) {

	}

	public MainWindow(MainWindowViewModel? model) : base() {
        InitializeComponent();
		//DataContext = model;
		//mainView.OnRegisteringViews(views);
		//mainView.DataContext = new MainViewModel(commonServices);
		//IServiceCollection viewCollection = new ServiceCollection();
		//viewCollection.AddViewModels();
		//IServiceProvider viewModels = viewCollection.BuildServiceProvider();
	}
}
