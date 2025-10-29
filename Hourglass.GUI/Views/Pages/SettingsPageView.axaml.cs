namespace Hourglass.GUI.Views.Pages;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;

public partial class SettingsPageView : ViewBase {

	public SettingsPageView() : base() {
		InitializeComponent();
    }

    private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        Console.WriteLine("Settings Page loaded!");
        (DataContext as SettingsPageViewModel)?.OnLoad();
    }
}
