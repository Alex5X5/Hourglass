namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;

using System.ComponentModel;

public partial class TaskDetailsPageViewModel : PageViewModelBase, INotifyPropertyChanged {

	private IHourglassDbService dbService;
	private DateTimeService dateTimeService;
	private ViewModelFactory<MainViewModel> pageFactory;
	private MainViewModel controller;

	public string DescriptionTextboxText { set; get; }
	public string ProjectTextboxText { set; get; }
	public string TicketTextboxText { set; get; }
	public string StartTextboxText { set; get; }
	public string FinishTextboxText { set; get; }

	public bool IsContiniueButtonEnabled { set; get; }
	public bool IsStartNewButtonEnabled { set; get; }
	public bool IsSaveButtonEnabled { set; get; }
	public bool IsStopButtonEnabled { set; get; }
	public bool IsDeleteButtonEnabled { set; get; }

	private Task selectedTask;
	public Task SelectedTask {
		set {
			selectedTask = value;
			UpdateTextFields();
		}
		get => selectedTask;
	}

	public Project SelectedProject { get; set; }
    public List<Project> AvailableProjects { get; set; }

	public new event PropertyChangedEventHandler? PropertyChanged;

	public TaskDetailsPageViewModel() : this(null, null, null) {

	}

	public TaskDetailsPageViewModel(IHourglassDbService dbService, DateTimeService dateTimeService, MainViewModel pageController) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		controller = pageController;
		AvailableProjects = [
			new Project() { Name = "test project" },
			new Project() { Name = "failing project" },
			new Project() { Name = "sucessfull project" }
		];
		SelectedProject = AvailableProjects[0];
		UpdateTextFields();
	}

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[RelayCommand]
	private async System.Threading.Tasks.Task StartTask() {
		Console.WriteLine("start task button click!");
		if(dbService!=null)
			RunningTask = await dbService.StartNewTaskAsnc(
				DescriptionTextboxText,
				null,
				new Worker { name = "new user" },
				null
			);
		UpdateTextFields();
		//await Task.Run(
		//	() => {
		//		Thread.Sleep(100);
		//		if (RunningTask != null)
		//			SetTextBoxTextSafely(StartTextbox, DateTimeService.ToDayAndTimeString(RunningTask.StartDateTime));
		//	}
		//);
		//StopButton.Enable();
		//StartButton.Disable();
	}

	[RelayCommand]
	private async System.Threading.Tasks.Task StopTask() {
		Console.WriteLine("stop task button click!");
		if(dbService!=null)
			RunningTask = await dbService.FinishCurrentTaskAsync(
				RunningTask?.start ?? DateTimeService.ToSeconds(DateTime.Now),
				DateTimeService.ToSeconds(DateTime.Now),
				DescriptionTextboxText,
				SelectedProject,
				null
			);
		UpdateTextFields();
	}

	[RelayCommand]
	private void RestartTask() {
		Console.WriteLine("restart task button click! (not yet implemented)");
		UpdateTextFields();
	}


	[RelayCommand]
	private void DeleteTask() {
		Console.WriteLine("delete task button click!");
		dbService.DeleteTaskAsync(SelectedTask);
		controller.GoBack();
		UpdateTextFields();
	}


	[RelayCommand]
	private void ApplyChanges() {
		Console.WriteLine("apply changes button click!");
		Console.WriteLine($"task description:{DescriptionTextboxText}");
		Console.WriteLine($"start time text:{StartTextboxText}");
		Console.WriteLine($"finish time text:{FinishTextboxText}");
		Console.WriteLine($"project name:{SelectedProject.Name}");
		Console.WriteLine($"ticket name:{TicketTextboxText}");

		Task newTask = new() {
			Id = SelectedTask.Id,
			description = DescriptionTextboxText,
			StartDateTime = DateTimeService.InterpretDayAndTimeString(StartTextboxText) ?? SelectedTask.StartDateTime,
			FinishDateTime = DateTimeService.InterpretDayAndTimeString(FinishTextboxText) ?? SelectedTask.FinishDateTime,
			owner = SelectedTask.owner,
			project = SelectedTask.project,
			ticket = SelectedTask.ticket,
			displayColorBlue = SelectedTask.displayColorBlue,
			displayColorGreen = SelectedTask.displayColorGreen,
			displayColorRed = SelectedTask.displayColorRed,
			running = SelectedTask.running,
		};
		dbService.UpdateTaskAsync(newTask);
		controller.GoBack();
		//UpdateTextFields();
	}

	[RelayCommand]
	private void Cancel() {
		controller.GoBack();
	}

	public void OnLoad() {
		Console.WriteLine("loading Task Details Page Model");
		UpdateTextFields();
	}

	public void UpdateTextFields() {
		DescriptionTextboxText = SelectedTask?.description ?? "";
		StartTextboxText = SelectedTask != null ? DateTimeService.ToDayAndTimeString(SelectedTask.StartDateTime) : "";
		FinishTextboxText = SelectedTask != null ? DateTimeService.ToDayAndTimeString(SelectedTask.FinishDateTime) : "";
		SelectedProject = SelectedTask?.project ?? AvailableProjects[0];
		TicketTextboxText = SelectedTask?.ticket?.description ?? "";
		OnPropertyChanged(nameof(DescriptionTextboxText));
		OnPropertyChanged(nameof(StartTextboxText));
		OnPropertyChanged(nameof(FinishTextboxText));
		OnPropertyChanged(nameof(SelectedProject));
		OnPropertyChanged(nameof(TicketTextboxText));
	}

	[RelayCommand]
	public void Color1Button_Click() {
		
	}
}