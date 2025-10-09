namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Models;
using Hourglass.GUI.Views;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class TimerPageViewModel : PageViewModelBase {

	public string DescriptionTextboxText { set; get; } = "description ...";
	public string ProjectTextboxText { set; get; } = "a project";
	public string TicketTextboxText { set; get; } = "a ticket";
	public string StartTextboxText { set; get; } = "started at";
	public string FinishTextboxText { set; get; } = "finished at";

	public Project SelectedFont { get; set; }
    public List<Project> FontFamilies { get; set; }

	public new event PropertyChangedEventHandler? PropertyChanged;

	public TimerPageViewModel() : this(null, null) {

	}

	public TimerPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		FontFamilies = [
			new Project() { Name="test project" },
			new Project() { Name = "failing project" },
			new Project() { Name = "sucessfull project" }
		];
		SelectedFont = FontFamilies[0];
	}

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[RelayCommand]
	private async System.Threading.Tasks.Task StartTask() {
		Console.WriteLine("OnStartButtonClick");
		IEnumerable<Database.Models.Project> projects = await dbService?.QueryProjectsAsync() ?? [];
		Database.Models.Project? project = projects.FirstOrDefault(x => x.Name == "");
		RunningTask = await dbService.StartNewTaskAsnc(
			DescriptionTextboxText,
			project,
			new Database.Models.Worker { name = "new user" },
			null
		);
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
	private void StopTask() {
		Console.WriteLine("stopping ccurrent task!");
	}

	[RelayCommand]
	private void RestartTask() {
		Console.WriteLine("restarting task!");
	}
}