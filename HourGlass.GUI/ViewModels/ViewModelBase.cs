namespace Hourglass.GUI.ViewModels;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Views;
using Hourglass.Util;

using ReactiveUI;

public abstract class ViewModelBase : ReactiveObject {

	public IServiceProvider? Services { set; get; }
	public ViewBase? owner;
	public DateTimeService? dateTimeService;
	public IHourglassDbService? dbService;

	public ViewModelBase() : this(null, null) {
	
	}
	
	public ViewModelBase(ViewBase? owner, IServiceProvider? services) : base() {
		Console.WriteLine($"constructing ViewModelBase for view model type '{GetType().Name}' with view type'{owner?.GetType().Name ?? "NullType"}'");
		Services = services;
		dbService = (IHourglassDbService?)services?.GetService(typeof(HourglassDbService));
		dateTimeService = (DateTimeService?)services?.GetService(typeof(DateTimeService));
		this.owner = owner;
	}

	public virtual void OnFinishedRegisteringViews(List<ViewBase> views, IServiceProvider? services) {
		string viewTypeName = GetType().FullName!.Replace("ViewModel", "View");
		Type? viewType = Type.GetType(viewTypeName);
		owner = views.FirstOrDefault(x=>x.GetType().Name==viewTypeName);
	}
}
