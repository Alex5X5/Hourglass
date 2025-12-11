namespace Hourglass.GUI.Views.Pages.SettingsPages;

using Avalonia.Controls;

using Hourglass.GUI.ViewModels.Pages.SettingsPages;
using Hourglass.Util.Attributes;

public partial class UserDataSubSettingsPageView : SubSettingsPageViewBase {

    [TranslateMember("Views.Pages.Setting.UserData.Labels.Username", "Username")]
    public string UsernameLabelText { get; set; }

    [TranslateMember("Views.Pages.Setting.UserData.Labels.StartDate", "StartDate")]
    public string StartDateLabelText { get; set; }
    
    [TranslateMember("Views.Pages.Setting.UserData.Labels.JobName", "JobName")]
    public string JobNameLabelText { get; set; }


    public UserDataSubSettingsPageView() : base() {
		InitializeComponent();
    }

    private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        (DataContext as UserDataSubSettingsPageViewModel)?.OnLoad();
    }

    private void AnyInput_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
        (DataContext as UserDataSubSettingsPageViewModel)?.AnyInput_LostFocus();
    }

    private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e) {
        
    }

}