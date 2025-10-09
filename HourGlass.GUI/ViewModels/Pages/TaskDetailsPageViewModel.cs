namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.GUI.Views;
using Hourglass.Util;

using System.ComponentModel;

public partial class TaskDetailsPageViewModel : PageViewModelBase {

	public string DescriptionTextboxText { set; get; }
	public string ProjectTextboxText { set; get; }
	public string TicketTextboxText { set; get; }
	public string StartTextboxText { set; get; }
	public string FinishTextboxText { set; get; }

	public Project SelectedProject { get; set; }
    public List<Project> AvailableProjects { get; set; }

	public new event PropertyChangedEventHandler? PropertyChanged;

	public TaskDetailsPageViewModel() : this(null, null) {

	}

	public TaskDetailsPageViewModel(MainViewModel? controller, IServiceProvider? services) : base(controller, services) {
		AvailableProjects = [
			new Project() { Name="test project" },
			new Project() { Name = "failing project" },
			new Project() { Name = "sucessfull project" }
		];
		SelectedProject = AvailableProjects[0];
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

	public void OnLoad() {
		Console.WriteLine("loading Timer Page");
		UpdateTextFields();
	}

	public void UpdateTextFields() {
		DescriptionTextboxText = RunningTask?.description ?? "";
		StartTextboxText = RunningTask != null ? DateTimeService.ToDayAndTimeString(RunningTask.StartDateTime) : "";
		SelectedProject = RunningTask?.project ?? AvailableProjects[0];
		TicketTextboxText = RunningTask?.ticket?.description ?? "";
	}
}