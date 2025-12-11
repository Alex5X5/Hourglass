namespace Hourglass.GUI.Views;

using Hourglass.GUI.ViewModels;

public abstract class ViewBase : Avalonia.Controls.UserControl {

	public ViewBase() : base() {
		TranslatorService.Singleton.TranslateAnnotatedMembers(this);
	}

	public ViewBase(ViewModelBase? model) : this() {
        Console.WriteLine($"constructing ViewBase for view type '{GetType().Name}' with view model type'{model?.GetType().Name ?? "NullType"}'");
		DataContext = model;
	}

	//public abstract void CollectViews(IServiceCollection views);
}
