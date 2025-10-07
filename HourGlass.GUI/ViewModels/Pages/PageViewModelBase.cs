using Hourglass.GUI.Views;

namespace Hourglass.GUI.ViewModels.Pages;

public abstract class PageViewModelBase : ViewModelBase {

	public Database.Models.Task? RunningTask;
	
	public PageViewModelBase() : this(null, null) {
	}

	public PageViewModelBase(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		
	}

}
