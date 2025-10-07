namespace Hourglass.GUI.ViewModels;

using Avalonia.Controls;

using Hourglass.GUI.Views;

using ReactiveUI;

using System;

public class MainWindowViewModel : ReactiveObject {

	public IServiceProvider? Services {
		set;
		get;
	}
	public Window? owner;

	public MainWindowViewModel() : this(null, null) {
		
	}

	public MainWindowViewModel(IServiceProvider? services) : this(null, services) {
		
	}
	
	public MainWindowViewModel(Window? owner, IServiceProvider? services) : base() {
		this.owner = owner;
		Services = services;
	}
}