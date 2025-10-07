namespace Hourglass.GUI.ViewModels;

using Hourglass.GUI.Views;

using ReactiveUI;

public abstract class ViewModelBase : ReactiveObject {

	public IServiceProvider? Services { set; get; }
	public ViewBase? owner;

	public ViewModelBase() : this(null, null) {
	
	}
	
	public ViewModelBase(ViewBase? owner, IServiceProvider? services) : base() {
		Console.WriteLine($"constructing ViewModelBase for view model type '{GetType().Name}' with view type'{owner?.GetType().Name ?? "NullType"}'");
		this.owner = owner;
		Services = services;
	}

	public virtual void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider? services) {
		string viewTypeName = GetType().FullName!.Replace("ViewModel", "View");
		Type? viewType = Type.GetType(viewTypeName);
		owner = views.FirstOrDefault(x=>x.GetType().Name==viewTypeName);
	}
}
