using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Views;

namespace Hourglass.GUI.ViewModels.Pages;

public abstract class PageViewModelBase : ViewModelBase {

	public PageViewModelBase() : this(null, null) {
	}

	public PageViewModelBase(ViewBase? owner, IServiceProvider? services) : base(owner, services) {

	}

}
