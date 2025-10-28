namespace Hourglass.GUI.Views.Pages.SettingsPages;

using Avalonia.Controls;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;

public partial class ExportSubSettingsPageView : SubSettingsPageViewBase {

    public ExportSubSettingsPageView() : base() {
		InitializeComponent();
    }

    private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        (DataContext as AboutSubSettingsPageViewModel)?.OnLoad();
    }
}