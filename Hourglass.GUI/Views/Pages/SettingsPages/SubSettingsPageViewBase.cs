namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.GUI.Views;

public abstract class SubSettingsPageViewBase : ViewBase {

	#region fields

	private static int MAX_TASK_DESCRIPTION_CHARS => 30;

    #endregion fields

    public SubSettingsPageViewBase() : base() {
		
	}
}
