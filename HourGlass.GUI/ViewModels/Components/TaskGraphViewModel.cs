namespace Hourglass.GUI.ViewModels.Components;

using ReactiveUI;

public class TaskGraphViewModel : ViewModelBase {

	public Database.Models.Task task { set; get; }

	private bool isRemoving = false;
	public bool IsRemoving {
		set => this.RaiseAndSetIfChanged(ref isRemoving, value);
		get => isRemoving;
    }

    public int index { get; private set; }

    public TaskGraphViewModel() : this(new Database.Models.Task()) { }

	public TaskGraphViewModel(Database.Models.Task task) {
		this.task = task;
    }
}
