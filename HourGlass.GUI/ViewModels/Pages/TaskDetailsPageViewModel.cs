namespace Hourglass.GUI.ViewModels.Pages;

using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.Util;

using System.ComponentModel;

public partial class TaskDetailsPageViewModel : PageViewModelBase, INotifyPropertyChanged {

	private IHourglassDbService dbService;
    private CacheService cacheService;
	private ColorService colorService;
	private MainViewModel controller;

	public override string Title => "Task Details";

	private readonly Task temporaryTask;

	public string DescriptionTextboxText {
        set {
            if (cacheService?.SelectedTask != null)
                temporaryTask.description = value;
            OnPropertyChanged(nameof(DescriptionTextboxText));
        }
        get => cacheService?.SelectedTask?.description ?? "";
    }
    public string ProjectTextboxText {
        set {
            if (cacheService?.SelectedTask != null)
                temporaryTask.description = value;
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
                DateTime start = DateTimeService.InterpretDayAndTimeString(value) ?? temporaryTask.StartDateTime;
                temporaryTask.start = DateTimeService.ToSeconds(start);
            }
            OnPropertyChanged(nameof(StartTextboxText));
        }
        get => cacheService?.SelectedTask != null ? DateTimeService.ToDayAndMonthAndTimeString(temporaryTask.StartDateTime) : "";
    }
    public string FinishTextboxText {
        set {
            if (cacheService?.SelectedTask != null) {
                DateTime finish = DateTimeService.InterpretDayAndTimeString(value) ?? temporaryTask.FinishDateTime;
                temporaryTask.finish = DateTimeService.ToSeconds(finish);
            }
            OnPropertyChanged(nameof(FinishTextboxText));
        }
        get => DateTimeService.ToDayAndMonthAndTimeString(temporaryTask.FinishDateTime);
    }

    public bool IsContiniueButtonEnabled { get => cacheService.RunningTask == null; }
	public bool IsStartNewButtonEnabled { get => cacheService.RunningTask == null; }
	private bool didChange;
	public bool IsSaveButtonEnabled { get => didChange; }
	public bool IsStopButtonEnabled { get => cacheService.RunningTask != null; }
    public bool IsDeleteButtonEnabled { get => true; }

	public new event PropertyChangedEventHandler? PropertyChanged;

	public TaskDetailsPageViewModel() : this(null, null, null, null) {
	}

	public TaskDetailsPageViewModel(IHourglassDbService dbService, MainViewModel pageController, CacheService cacheService, ColorService colorService) : base() {
		this.dbService = dbService;
		this.controller = pageController;
		this.cacheService = cacheService;
		this.colorService = colorService;
        temporaryTask = cacheService.RunningTask?.Clone() ?? new Task();
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
                cacheService.RunningTask?.DisplayColor ?? new Color(255, 79, 79, 79),
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
			if(temporaryTask?.running ?? false)
				cacheService.RunningTask = await dbService.FinishCurrentTaskAsync(
                    temporaryTask.start,
					DateTimeService.ToSeconds(DateTime.Now),
					DescriptionTextboxText,
					null,
					null
				);
        controller.GoBack();
    }

	[RelayCommand]
	private async System.Threading.Tasks.Task RestartTask() {
		Console.WriteLine("start new button click! (not yet implemented)");
        await StopTask();
		await StartTask();
		cacheService.RunningTask!.description = temporaryTask!.description;
		cacheService.RunningTask!.displayColorRed = temporaryTask!.displayColorRed;
		cacheService.RunningTask!.displayColorGreen= temporaryTask!.displayColorGreen;
		cacheService.RunningTask!.displayColorBlue= temporaryTask!.displayColorBlue;
        await dbService.UpdateTaskAsync(cacheService.RunningTask);
		controller.GoBack();
    }


	[RelayCommand]
	private void DeleteTask() {
		Console.WriteLine("delete task button click!");
		if (temporaryTask == null)
			return;
		dbService.DeleteTaskAsync(temporaryTask);
		controller.GoBack();
	}


	[RelayCommand]
	private async System.Threading.Tasks.Task ContiniueTask() {
		Console.WriteLine("continiue task button click!");
		if (temporaryTask == null)
			return;
		if(cacheService.RunningTask != null)
			return;
		cacheService.RunningTask = await dbService.ContiniueTaskAsync(temporaryTask);
		controller.GoBack();
	}


	[RelayCommand]
	private void ApplyChanges() {
		if (temporaryTask == null)
			return;
		dbService.UpdateTaskAsync(temporaryTask);
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
		if(temporaryTask!=null)
			temporaryTask.DisplayColor = colorService.TASK_BACKGROUND_YELLOW;
	}

	[RelayCommand]
	public void Color2Button_Click() {
		if (temporaryTask != null)
			temporaryTask.DisplayColor = colorService.TASK_BACKGROUND_ORANGE;
	}

	[RelayCommand]
	public void Color3Button_Click() {
		if (temporaryTask != null)
			temporaryTask.DisplayColor = colorService.TASK_BACKGROUND_RED;
	}

	[RelayCommand]
	public void Color4Button_Click() {
		if (temporaryTask != null)
			temporaryTask.DisplayColor = colorService.TASK_BACKGROUND_LIGTH_BLUE;
	}

	[RelayCommand]
	public void Color5Button_Click() {
		if (temporaryTask != null)
			temporaryTask.DisplayColor = colorService.TASK_BACKGROUND_DARK_BLUE;
	}

	[RelayCommand]
	public void Color6Button_Click() {
		if (temporaryTask != null)
			temporaryTask.DisplayColor = colorService.TASK_BACKGROUND_DARK_GREEN;
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