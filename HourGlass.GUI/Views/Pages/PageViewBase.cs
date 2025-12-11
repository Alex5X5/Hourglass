using Hourglass.GUI.ViewModels;

namespace Hourglass.GUI.Views.Pages;

public abstract class PageViewBase : ViewBase {

	

	public PageViewBase() : this(null) {

	}

	public PageViewBase(ViewModelBase? model) : base(model) {
	}

}
