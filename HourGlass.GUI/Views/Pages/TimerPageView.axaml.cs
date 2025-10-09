namespace Hourglass.GUI.Views.Pages;

using Avalonia.Controls;
using Avalonia.Input;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;

public partial class TimerPageView : PageViewBase {

	private bool initialDescriptionTextboxClear = true;
	
	public TimerPageView() : this(null, null) {
		
	}

	public TimerPageView(ViewModelBase? model, IServiceProvider? services) : base(model, services) {
		InitializeComponent();
		startButton.GotFocus += (sender, args) => {
			startButton.InvalidateVisual();
			Console.WriteLine("start button got focus!");
		};
	}

	private void TextBox_GotFocus(object? sender, Avalonia.Input.GotFocusEventArgs e) {
		Console.WriteLine("got focus!");
		if (initialDescriptionTextboxClear) {
			Console.WriteLine("initial focus!");
			DescriptionTextbox.Clear();
			initialDescriptionTextboxClear = false;
		}
	}

	private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e) {
		Console.WriteLine("key pressed!");
		if (e.Key == Key.Escape)
			TopLevel.GetTopLevel(this)?.Focus();
		if (e.Key == Key.Enter) {
			startButton.Focus();
			Console.WriteLine($"button is focused {startButton.IsFocused}");
		}
	}

	private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("Timer Page loaded!");
		(DataContext as TimerPageViewModel)?.OnLoad();
	}

	//private void TextBox_KeyDown(object? sender, Avalonia. e) {
	//	Console.WriteLine("got focus!");
	//	if (initialDescriptionTextboxClear) {
	//		Console.WriteLine("initial focus!");
	//		DescriptionTextbox.Clear();
	//		initialDescriptionTextboxClear = false;
	//	}
	//}
}