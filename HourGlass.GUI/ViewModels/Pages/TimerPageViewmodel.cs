namespace Hourglass.GUI.ViewModels.Pages;

using System.ComponentModel;

public class TimerPageViewmodel : ViewModelBase, INotifyPropertyChanged {

	public bool IsDayViewVisible {
		get; private set;
	}
	public bool IsWeekViewVisible {
		get; private set;
	}
	public bool IsMonthViewVisible {
		get; private set;
	}

	public TimerPageViewmodel() {

	}

	// Property changed implementation...
	public string Greeting => "Welcome to Avalonia!";
}