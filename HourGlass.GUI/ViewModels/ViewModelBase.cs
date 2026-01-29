namespace Hourglass.GUI.ViewModels;

using ReactiveUI;
using System.ComponentModel;

public abstract class ViewModelBase : ReactiveObject {

	public ViewModelBase() : base() {
		Console.WriteLine($"constructing ViewModelBase for view model type '{GetType().Name}'");
    }
}
