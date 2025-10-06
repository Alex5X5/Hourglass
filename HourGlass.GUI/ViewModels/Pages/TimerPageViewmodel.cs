using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views;

using ReactiveUI;

namespace Hourglass.GUI.ViewModels.Pages;

public class TimerPageViewModel : PageViewModelBase {

	public TimerPageViewModel() : this(null, null) {
		
	}

	public TimerPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		
	}
}