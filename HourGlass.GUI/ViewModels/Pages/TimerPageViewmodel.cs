namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.GUI.Views;

using ReactiveUI;

using System.ComponentModel;
using System.Reactive;

public partial class TimerPageViewModel : PageViewModelBase {

	public string DescriptionTextboxText { set; get; } = "description ...";
	public string ProjectTextboxText { set; get; } = "a project";
	public string TicketTextboxText { set; get; } = "a ticket";
	public string StartTextboxText { set; get; } = "started at";
	public string FinishTextboxText { set; get; } = "finished at";

	private bool descriptionTextboxEdited = false;


	public new event PropertyChangedEventHandler? PropertyChanged;

	public TimerPageViewModel() : this(null, null) {

	}

	public TimerPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
	}

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[RelayCommand]
	private async Task StartTask() {
		Console.WriteLine("OnStartButtonClick");
		IEnumerable<Database.Models.Project> projects = await dbService.QueryProjectsAsync();
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

	public ReactiveCommand<Unit, Unit> GotFocusCommand { get; }
		= ReactiveCommand.Create(() => { });

	[RelayCommand]
	private void StopTask() {
		Console.WriteLine("stopping ccurrent task!");
	}

	[RelayCommand]
	private void RestartTask() {
		Console.WriteLine("restarting task!");
	}

	[RelayCommand]
	private void OnFocus() {
		Console.WriteLine("restarting task!");
	}




}