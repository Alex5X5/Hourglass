namespace Hourglass.GUI.ViewModels.Components;

using Hourglass.GUI.Views.Components.GraphPanels;
using ReactiveUI;

public class TaskGraphViewModel : ViewModelBase {

	private Database.Models.Task task;
	public Database.Models.Task Task {
        set => this.RaiseAndSetIfChanged(ref task, value);
        get => task;
    }

    public string DeescriptionStub => Task.description.Length <= GraphPanelViewBase.MAX_TASK_DESCRIPTION_CHARS ? Task.description : Task.description[..GraphPanelViewBase.MAX_TASK_DESCRIPTION_CHARS] + "...";

	private bool isRemoving = false;
	public bool IsRemoving {
		set => this.RaiseAndSetIfChanged(ref isRemoving, value);
		get => isRemoving;
    }

	private int index = 0;
    public int Index {
        set => this.RaiseAndSetIfChanged(ref index, value);
        get => index;
    }

    public TaskGraphViewModel() : this(new Database.Models.Task()) { }

	public TaskGraphViewModel(Database.Models.Task task) {
		this.task = task;
    }
}
