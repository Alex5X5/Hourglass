namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Views;
using Hourglass.PDF;
using Hourglass.Util;

using System.ComponentModel;

public partial class TimerPageViewModel : PageViewModelBase {

	private IHourglassDbService dbService;
	
	public string DescriptionString {
		set; get;
	}

	public new event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[RelayCommand]
	private async Task StartTask() {
		Console.WriteLine("OnStartButtonClick");
		IEnumerable<Database.Models.Project> projects = await dbService.QueryProjectsAsync();
		Database.Models.Project? project = projects.FirstOrDefault(x => x.Name == "");
		RunningTask = await dbService.StartNewTaskAsnc(
			DescriptionString,
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

	public TimerPageViewModel() : this(null, null) {

	}

	public TimerPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		DescriptionString = "test";
		dbService = (IHourglassDbService)services?.GetService(typeof(HourglassDbService))!;
		Console.WriteLine(this.RunningTask);
	}


}