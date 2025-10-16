namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.Util;

using System.ComponentModel;

public partial class TaskDetailsPageViewModel : PageViewModelBase, INotifyPropertyChanged {

	private IHourglassDbService dbService;
    private CacheService cacheService;
    private ViewModelFactory<MainViewModel> pageFactory;
	private MainViewModel controller;


    public string DescriptionTextboxText {
        set {
            if (cacheService?.SelectedTask != null)
                cacheService.SelectedTask.description = value;
            OnPropertyChanged(nameof(DescriptionTextboxText));
        }
        get => cacheService?.SelectedTask?.description ?? "";
    }
    public string ProjectTextboxText {
        set {
            if (cacheService?.SelectedTask != null)
                cacheService.SelectedTask.description = value;
            OnPropertyChanged(nameof(ProjectTextboxText));
        }
        get => cacheService?.SelectedTask?.project?.Name ?? "";
    }
    public string TicketTextboxText {
        get => cacheService?.SelectedTask?.ticket?.name ?? "";
    }
    public string StartTextboxText {
        set {
            if (cacheService?.SelectedTask != null) {
                DateTime start = DateTimeService.InterpretDayAndTimeString(value) ?? cacheService.SelectedTask.StartDateTime;
                cacheService.SelectedTask.start = DateTimeService.ToSeconds(start);
            }
            OnPropertyChanged(nameof(StartTextboxText));
        }
        get => cacheService?.SelectedTask != null ? DateTimeService.ToTimeString(cacheService.SelectedTask.StartDateTime) : "";
    }
    public string FinishTextboxText {
        set {
            if (cacheService?.SelectedTask != null) {
                DateTime finish = DateTimeService.InterpretDayAndTimeString(value) ?? cacheService.SelectedTask.FinishDateTime;
                cacheService.SelectedTask.start = DateTimeService.ToSeconds(finish);
            }
            OnPropertyChanged(nameof(FinishTextboxText));
        }
        get => cacheService?.RunningTask != null ? DateTimeService.ToTimeString(cacheService.SelectedTask.FinishDateTime) : "";
    }

    public bool IsContiniueButtonEnabled { get => cacheService?.SelectedTask != null; }
	public bool IsStartNewButtonEnabled { get => cacheService?.SelectedTask == null; }
	private bool didChange;
	public bool IsSaveButtonEnabled { get => didChange; }
	public bool IsStopButtonEnabled { get => cacheService?.SelectedTask?.running == true; }
    public bool IsDeleteButtonEnabled { get => true; }

	private Task selectedTask;
	public Task SelectedTask {
		set {
			selectedTask = value;
            //UpdateTextFields();
        }
		get => selectedTask;
	}

	public Project SelectedProject { get; set; }
    public List<Project> AvailableProjects { get; set; }

	public new event PropertyChangedEventHandler? PropertyChanged;

	public TaskDetailsPageViewModel() : this(null, null, null) {

	}

	public TaskDetailsPageViewModel(IHourglassDbService dbService, MainViewModel pageController, CacheService cacheService) : base() {
		this.dbService = dbService;
		this.cacheService = cacheService;
		controller = pageController;
		AvailableProjects = [
			new Project() { Name = "test project" },
			new Project() { Name = "failing project" },
			new Project() { Name = "sucessfull project" }
		];
		SelectedProject = AvailableProjects[0];
	}
    private void AllBindingPropertiesChanged() {
        OnPropertyChanged(nameof(DescriptionTextboxText));
        OnPropertyChanged(nameof(StartTextboxText));
        OnPropertyChanged(nameof(FinishTextboxText));
        OnPropertyChanged(nameof(TicketTextboxText));
        OnPropertyChanged(nameof(ProjectTextboxText));
    }


    protected virtual void OnPropertyChanged(string propertyName) {
		didChange = true;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[RelayCommand]
	private async System.Threading.Tasks.Task StartTask() {
		Console.WriteLine("start task button click!");
		if(dbService!=null)
			 cacheService.RunningTask = await dbService.StartNewTaskAsnc(
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
            cacheService.RunningTask = await dbService.FinishCurrentTaskAsync(
                cacheService.RunningTask?.start ?? DateTimeService.ToSeconds(DateTime.Now),
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
        OnPropertyChanged(nameof(DescriptionTextboxText));
        OnPropertyChanged(nameof(StartTextboxText));
        OnPropertyChanged(nameof(FinishTextboxText));
        OnPropertyChanged(nameof(TicketTextboxText));
        OnPropertyChanged(nameof(ProjectTextboxText));
        //UpdateTextFields();
    }

	public void UpdateTextFields() {
		DescriptionTextboxText = SelectedTask?.description ?? "";
		StartTextboxText = SelectedTask != null ? DateTimeService.ToDayAndTimeString(SelectedTask.StartDateTime) : "";
		FinishTextboxText = SelectedTask != null ? DateTimeService.ToDayAndTimeString(SelectedTask.FinishDateTime) : "";
		SelectedProject = SelectedTask?.project ?? AvailableProjects[0];
		//TicketTextboxText = SelectedTask?.ticket?.description ?? "";
	}

	[RelayCommand]
	public void Color1Button_Click() {
		
	}
}