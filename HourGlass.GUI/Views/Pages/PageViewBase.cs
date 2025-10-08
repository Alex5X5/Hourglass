using Hourglass.GUI.ViewModels;

namespace Hourglass.GUI.Views.Pages;

public abstract class PageViewBase : ViewBase {

	

	public PageViewBase() {

	}

	public PageViewBase(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
	}

}
