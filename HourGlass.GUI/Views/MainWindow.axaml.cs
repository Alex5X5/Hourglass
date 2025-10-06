namespace Hourglass.GUI.Views;

using Avalonia.Controls;
using Hourglass.GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;


public partial class MainWindow : Window {

	public IServiceProvider? Services { set; get; }

	public MainWindow() : this(null) {

	}

	public MainWindow(IServiceProvider? services) : base() {
		Services = services;
		DataContext = Services?.GetService<MainWindowViewModel>();
        InitializeComponent();
	}
}
