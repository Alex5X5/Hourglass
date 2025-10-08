using Avalonia.Interactivity;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;

namespace Hourglass.GUI.Views.Pages;

public partial class TimerPageView : PageViewBase {

	private bool initialDescriptionTextboxClear = true;
	
	public TimerPageView() : this(null, null) {
		
	}

	public TimerPageView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent(); 
	}

	private void TextBox_GotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e) {
		Console.WriteLine("got focus!");
		if (initialDescriptionTextboxClear) {
			Console.WriteLine("initial focus!");
			DescriptionTextbox.Clear();
			initialDescriptionTextboxClear = false;
		}
	}
}