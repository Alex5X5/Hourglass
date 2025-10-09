namespace Hourglass.GUI.ViewModels;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Views;
using Hourglass.Util;

using ReactiveUI;

public abstract class ViewModelBase : ReactiveObject {

	public IServiceProvider? Services { set; get; }
	public MainViewModel? controller;
	public DateTimeService? dateTimeService;
	public IHourglassDbService? dbService;

	public ViewModelBase() : this(null, null) {
	
	}
	
	public ViewModelBase(MainViewModel? controller, IServiceProvider? services) : base() {
		Console.WriteLine($"constructing ViewModelBase for view model type '{GetType().Name}' with view type'{this.controller?.GetType().Name ?? "NullType"}'");
		Services = services;
		dbService = (IHourglassDbService?)services?.GetService(typeof(HourglassDbService));
		dateTimeService = (DateTimeService?)services?.GetService(typeof(DateTimeService));
		this.controller = controller;
	}
}
