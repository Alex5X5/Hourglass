namespace Hourglass.GUI.ViewModels.Components;

using Avalonia.Controls;

using System.ComponentModel;

public class TaskGraphViewModel : INotifyPropertyChanged {

	public readonly Database.Models.Task task;

	private readonly long intervallStart;
	private readonly long intervallDuration;


	private bool isRemoving = false;
	public bool IsRemoving {
		set {
			isRemoving = value;
			OnPropertyChanged(nameof(IsRemoving));
		}
		get => isRemoving;
	}

	private ColumnDefinition beforeTaskColumnWeigth;
	public ColumnDefinition BeforeTaskColumnWeigth {
		get => beforeTaskColumnWeigth;
	}

	private ColumnDefinition afterTaskColumnWeigth;
	public ColumnDefinition AfterTaskColumnWeigth {
		get => afterTaskColumnWeigth;
	}

	private ColumnDefinition taskColumnWeigth;
	public ColumnDefinition TaskColumnWeigth {
		get => taskColumnWeigth;
	}

	public double Row { get; private set; }
	public double Column { get; private set; }

	public int index { get; private set; }


	public event PropertyChangedEventHandler? PropertyChanged = (a, s) => { };

	public TaskGraphViewModel() : this(new Database.Models.Task(), 1, 0, 0) { }

	public TaskGraphViewModel(Database.Models.Task task, long intervallDuration, long intervallStart, int row) {
		this.task = task;
		Row = row;
		Column = 0;
		this.intervallStart = intervallStart;
		this.intervallDuration = intervallDuration;
		CalculateColumnWeights();
	}

	private void CalculateColumnWeights() {
		long offset = task.start - intervallStart;
		beforeTaskColumnWeigth = new ColumnDefinition(1000.0 / intervallDuration * offset, GridUnitType.Star);
		taskColumnWeigth = new ColumnDefinition(1000.0 / intervallDuration * task.Duration, GridUnitType.Star);
		afterTaskColumnWeigth = new ColumnDefinition(1000.0 / intervallDuration * (intervallDuration - offset - task.Duration), GridUnitType.Star);
	}

	private void ColumnsChanged() {
		OnPropertyChanged(nameof(BeforeTaskColumnWeigth));
		OnPropertyChanged(nameof(TaskColumnWeigth));
		OnPropertyChanged(nameof(AfterTaskColumnWeigth));
	}

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
