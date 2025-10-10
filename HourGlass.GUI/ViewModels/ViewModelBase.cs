namespace Hourglass.GUI.ViewModels;

using ReactiveUI;

public abstract class ViewModelBase : ReactiveObject {
	
	public ViewModelBase() : base() {
		Console.WriteLine($"constructing ViewModelBase for view model type '{GetType().Name}'");

		//Services = services;
		//dbService = (IHourglassDbService?)services?.GetService(typeof(HourglassDbService));
		//dateTimeService = (DateTimeService?)services?.GetService(typeof(DateTimeService));
		//this.controller = controller;
	}
}
