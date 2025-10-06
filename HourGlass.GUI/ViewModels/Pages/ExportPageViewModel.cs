namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.ComponentModel;

using Hourglass.GUI.Views;

public class ExportPageViewModel : PageViewModelBase {

	public ExportPageViewModel() : this(null, null) {
	}

	public ExportPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		
	}

	//public ViewState selectedGraphMode = ViewState.Day;

	//[ObservableProperty]
	//public bool _IsDayPanelVisible = false;

	//[ObservableProperty]
	//public bool _IsWeekPanelVisible = false;

	//[ObservableProperty]
	//public bool _IsMonthPanelVisible = true;

	//public enum ViewState {
	//	Day, Week, Month
	//}
}