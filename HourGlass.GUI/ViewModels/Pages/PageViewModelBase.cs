using Hourglass.GUI.Views;

namespace Hourglass.GUI.ViewModels.Pages;

public abstract class PageViewModelBase : ViewModelBase {

	public Database.Models.Task? RunningTask;

	public PageViewModelBase() : this(null, null) {
	}

	public PageViewModelBase(MainViewModel? controller, IServiceProvider? services) : base(controller, services) {
	
	}

}
