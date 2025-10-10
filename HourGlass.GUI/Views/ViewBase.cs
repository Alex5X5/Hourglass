namespace Hourglass.GUI.Views;

using Hourglass.GUI.ViewModels;

public abstract class ViewBase : Avalonia.Controls.UserControl {

	public ViewBase() {
		
	}

	public ViewBase(ViewModelBase? model, IServiceProvider? services) {
		Console.WriteLine($"constructing ViewBase for view type '{GetType().Name}' with view model type'{model?.GetType().Name ?? "NullType"}'");
		DataContext = model;
	}

	public virtual void OnRegisteringViews(List<ViewBase> views) {
		views.Add(this);
	}

	public virtual void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider services) {
	}

	//public abstract void CollectViews(IServiceCollection views);
}
