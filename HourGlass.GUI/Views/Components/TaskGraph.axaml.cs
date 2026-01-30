namespace Hourglass.GUI.Views.Components;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Rendering.Composition;
using Hourglass.GUI.ViewModels.Components;

public partial class TaskGraph : UserControl {

	public double X => 10;
	public double Y => 10;


	public bool IsRemoving {
		get => (DataContext as TaskGraphViewModel)?.IsRemoving ?? false;
		set {
			if (DataContext is TaskGraphViewModel model)
				model.IsRemoving = value;
		}
	}

	public Database.Models.Task? Task {
		set; get;
	}

	public TaskGraph() : base(){
		InitializeComponent();
	}

	private void OnLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
		//if (DataContext is TaskGraphViewModel model)
			//model.PropertyChanged +=
			//	(name, args) => {
			//		if (args.PropertyName == nameof(TaskGraphViewModel.IsRemoving))
			//			if (IsRemoving && this.FindControl<Rectangle>("Rect") is { } rect) {
			//				rect.IsVisible = false;
			//				rect.
			//				InvalidateVisual();
			//			}
			//	};
	}
}