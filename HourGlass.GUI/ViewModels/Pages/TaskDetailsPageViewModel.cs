namespace Hourglass.GUI.ViewModels.Pages;

using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.PDF;
using Hourglass.Util;

using System.ComponentModel;
using System.Threading.Tasks;

public partial class TaskDetailsPageViewModel : PageViewModelBase, INotifyPropertyChanged {

	private IHourglassDbService dbService;
    private TimerCacheService cacheService;
	private ColorService colorService;
	private MainViewModel controller;

	public override string Title => "Task Details";


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
        get => cacheService?.SelectedTask != null ? DateTimeService.ToDayAndTimeString(cacheService.SelectedTask.StartDateTime) : "";
    }
    public string FinishTextboxText {
        set {
            if (cacheService?.SelectedTask != null) {
                DateTime finish = DateTimeService.InterpretDayAndTimeString(value) ?? cacheService.SelectedTask.FinishDateTime;
                cacheService.SelectedTask.finish = DateTimeService.ToSeconds(finish);
            }
            OnPropertyChanged(nameof(FinishTextboxText));
        }
        get => cacheService?.SelectedTask != null ? DateTimeService.ToDayAndTimeString(cacheService.SelectedTask.FinishDateTime) : "";
    }

    public bool IsContiniueButtonEnabled { get => cacheService?.SelectedTask != null; }
	public bool IsStartNewButtonEnabled { get => cacheService?.SelectedTask == null; }
	private bool didChange;
	public bool IsSaveButtonEnabled { get => didChange; }
	public bool IsStopButtonEnabled { get => cacheService?.SelectedTask?.running == true; }
    public bool IsDeleteButtonEnabled { get => true; }

	public Project SelectedProject { get; set; }
    public List<Project> AvailableProjects { get; set; }

	public new event PropertyChangedEventHandler? PropertyChanged;

	public TaskDetailsPageViewModel() : this(null, null, null, null) {
	}

	public TaskDetailsPageViewModel(IHourglassDbService dbService, MainViewModel pageController, TimerCacheService cacheService, ColorService colorService) : base() {
		this.dbService = dbService;
		this.colorService = colorService;
		this.cacheService = cacheService;
		cacheService.OnSelectedTaksChanged +=
			task => AllBindingPropertiesChanged();
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
                new Color(255, 79, 79, 79),
				null,
				new Worker { name = "new user" },
				null
			);
		AllBindingPropertiesChanged();
        controller.GoBack();
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
		AllBindingPropertiesChanged();
        controller.GoBack();
        //UpdateTextFields();
    }

	[RelayCommand]
	private async System.Threading.Tasks.Task RestartTask() {
		Console.WriteLine("start new button click! (not yet implemented)");
        await StopTask();
		await StartTask();
		cacheService.RunningTask!.description = cacheService.SelectedTask!.description;
		cacheService.RunningTask!.displayColorRed = cacheService.SelectedTask!.displayColorRed;
		cacheService.RunningTask!.displayColorGreen= cacheService.SelectedTask!.displayColorGreen;
		cacheService.RunningTask!.displayColorBlue= cacheService.SelectedTask!.displayColorBlue;
        await dbService.UpdateTaskAsync(cacheService.RunningTask);
		AllBindingPropertiesChanged();
		controller.GoBack();
    }


	[RelayCommand]
	private void DeleteTask() {
		Console.WriteLine("delete task button click!");
		if (cacheService.SelectedTask == null)
			return;
		dbService.DeleteTaskAsync(cacheService.SelectedTask);
		controller.GoBack();
	}


	[RelayCommand]
	private async System.Threading.Tasks.Task ContiniueTask() {
		Console.WriteLine("continiue task button click!");
		if (cacheService.SelectedTask == null)
			return;
		if(cacheService.RunningTask != null)
			return;
		cacheService.RunningTask = await dbService.ContiniueTaskAsync(cacheService.SelectedTask);
		controller.GoBack();
	}


	[RelayCommand]
	private void ApplyChanges() {
		if (cacheService.SelectedTask == null)
			return;
		dbService.UpdateTaskAsync(cacheService.SelectedTask);
        controller.GoBack();
    }

	[RelayCommand]
	private void Cancel() {
		controller.GoBack();
	}

	public void OnLoad() {
		Console.WriteLine("loading Task Details Page Model");
		AllBindingPropertiesChanged();
    }

	[RelayCommand]
	public void Color1Button_Click() {
		if(cacheService.SelectedTask!=null)
			cacheService.SelectedTask.DisplayColor = colorService.TASK_BACKGROUND_YELLOW;
	}

	[RelayCommand]
	public void Color2Button_Click() {
		if (cacheService.SelectedTask != null)
			cacheService.SelectedTask.DisplayColor = colorService.TASK_BACKGROUND_ORANGE;
	}

	[RelayCommand]
	public void Color3Button_Click() {
		if (cacheService.SelectedTask != null)
			cacheService.SelectedTask.DisplayColor = colorService.TASK_BACKGROUND_RED;
	}

	[RelayCommand]
	public void Color4Button_Click() {
		if (cacheService.SelectedTask != null)
			cacheService.SelectedTask.DisplayColor = colorService.TASK_BACKGROUND_LIGTH_BLUE;
	}

	[RelayCommand]
	public void Color5Button_Click() {
		if (cacheService.SelectedTask != null)
			cacheService.SelectedTask.DisplayColor = colorService.TASK_BACKGROUND_DARK_BLUE;
	}

	[RelayCommand]
	public void Color6Button_Click() {
		if (cacheService.SelectedTask != null)
			cacheService.SelectedTask.DisplayColor = colorService.TASK_BACKGROUND_DARK_GREEN;
	}

	[RelayCommand]
	public void Color7Button_Click() {
		
	}

	[RelayCommand]
	public void Color8Button_Click() {
		
	}

	[RelayCommand]
	public void Color9Button_Click() {
		
	}
}