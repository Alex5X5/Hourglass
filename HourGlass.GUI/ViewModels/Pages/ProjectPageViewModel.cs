using Hourglass.GUI.Views;

namespace Hourglass.GUI.ViewModels.Pages;

public class ProjectPageViewModel : PageViewModelBase {

	public ProjectPageViewModel() : this(null, null) {

	}

	public ProjectPageViewModel(MainViewModel? controller, IServiceProvider? services) : base(controller, services) {

	}
}
