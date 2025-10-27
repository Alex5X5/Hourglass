namespace Hourglass.GUI.Views.Pages.SettingsPages;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;

public partial class VisualsSubSettingsPageView : SubSettingsPageViewBase {

    public VisualsSubSettingsPageView() : base() {
		InitializeComponent();
    }

    private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        (DataContext as VisualsSubSettingsPageViewModel)?.OnLoad();
    }
}