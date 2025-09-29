namespace Hourglass.GUI.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;

public class MainViewModel : ViewModelBase, INotifyPropertyChanged {

    public bool IsDayViewVisible { get; private set; }
    public bool IsWeekViewVisible { get; private set; }
    public bool IsMonthViewVisible { get; private set; }

	public MainViewModel() {
    
	}
    
    // Property changed implementation...
    public string Greeting => "Welcome to Avalonia!";
}

