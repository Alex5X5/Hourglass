namespace Hourglass.GUI.Views;

using Hourglass.GUI.ViewModels;

public class ViewBase : Avalonia.Controls.UserControl {

	public IServiceProvider? Services { set; get; }

	public ViewBase() {
		
	}

	public ViewBase(ViewModelBase? model, IServiceProvider? services) {
		Console.WriteLine($"constructing ViewBase for view type '{GetType().Name}' with view model type'{model?.GetType().Name ?? "NullType"}'");
		Services = services;
		DataContext = model;
	}
}
