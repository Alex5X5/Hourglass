namespace Hourglass.GUI.Views.Pages;

using Avalonia.Controls;
using Avalonia.Input;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util.Attributes;

public partial class TaskDetailsPageView : PageViewBase {

	[TranslateMember("Views.Pages.Export.Buttons.Import", "Import")]
	public string ImportButtonText { get; set; } = "";

    //private bool initialDescriptionTextboxClear = true;

    public TaskDetailsPageView() : base() {
		InitializeComponent();
		//startButton.GotFocus += (sender, args) => {
		//	startButton.InvalidateVisual();
		//	Console.WriteLine("start button got focus!");
		//};
	}

	private void TextBox_GotFocus(object? sender, GotFocusEventArgs e) {
		Console.WriteLine("got focus!");
		//Console.WriteLine((DataContext as TaskDetailsPageViewModel)?.SelectedTask?.description);
		//Console.WriteLine(DescriptionTextbox.Text);
		
		//if (initialDescriptionTextboxClear) {
		//	Console.WriteLine("initial focus!");
		//	DescriptionTextbox.Clear();
		//	initialDescriptionTextboxClear = false;
		//}
	}

	private void TextBox_KeyDown(object? sender, KeyEventArgs e) {
		Console.WriteLine("key pressed!");
		if (e.Key == Key.Escape)
			TopLevel.GetTopLevel(this)?.Focus();
		if (e.Key == Key.Enter) {
			//startButton.Focus();
			//Console.WriteLine($"button is focused {startButton.IsFocused}");
		}
		//Console.WriteLine((DataContext as TaskDetailsPageViewModel)?.SelectedTask.description);
		//Console.WriteLine((DataContext as TaskDetailsPageViewModel)?.DescriptionTextboxText);
		//Console.WriteLine(DescriptionTextbox.Text);
		//DescriptionTextbox.InvalidateVisual();
	}

	private void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		Console.WriteLine("Task Details View loaded!");
		(DataContext as TaskDetailsPageViewModel)?.OnLoad();
		//Console.WriteLine(DescriptionTextbox.Text);
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