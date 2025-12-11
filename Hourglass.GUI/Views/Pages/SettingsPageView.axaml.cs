namespace Hourglass.GUI.Views.Pages;

using Hourglass.GUI.ViewModels.Pages;

public partial class SettingsPageView : PageViewBase {

    

	public SettingsPageView() : base() {
		InitializeComponent();
    }

    private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        Console.WriteLine("Settings Page loaded!");
        (DataContext as SettingsPageViewModel)?.OnLoad();
    }
}
