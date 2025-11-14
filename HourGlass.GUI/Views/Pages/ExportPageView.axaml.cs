using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Hourglass.GUI.ViewModels.Pages;
using System.Diagnostics;

namespace Hourglass.GUI.Views.Pages;

public partial class ExportPageView : PageViewBase {

	public ExportPageView() : base() {
		InitializeComponent();
	}
    
    private void TextBox_Focused(object sender, GotFocusEventArgs e) {
        if (sender is TextBox textBox && textBox.DataContext is TextboxItem item)
            if(item.Task!=null)
                (DataContext as ExportPageViewModel)?.OnTaskRedirect(item.Task);
    }
}

    