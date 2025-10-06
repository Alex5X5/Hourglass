using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;

namespace Hourglass.GUI.Views.Pages;

public partial class TimerPageView : PageViewBase {
	
	public TimerPageView() : this(null, null) {
		
	}

	public TimerPageView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
	}
}