namespace Hourglass.GUI.Views.Pages.SettingsPages;

using Avalonia.Controls;
using Avalonia.Media;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Pages.SettingsPages;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

public partial class UserDataSubSettingsPageView : SubSettingsPageViewBase {

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
        var textBox = sender as TextBox;
        if (textBox != null) {
            const double testFontSize = 12.0;

            // Measure text at test size
            var formattedText = new FormattedText(
                textBox.Text ?? "",
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBox.FontFamily),
                testFontSize,
                Brushes.Black);

            double measuredWidth = formattedText.Width;

            // Calculate required font size
            double requiredFontSize = (textBox.Bounds.Width / measuredWidth) * testFontSize;
            textBox.FontSize = 12;
            //textBox.FontSize = requiredFontSize;
        }
    }

}