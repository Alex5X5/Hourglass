namespace Hourglass.GUI.ViewModels.Pages;

public abstract class PageViewModelBase : ViewModelBase {

	public Database.Models.Task? RunningTask;
	
	public PageViewModelBase() : base() {
			
	}

}
