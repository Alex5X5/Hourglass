namespace Hourglass.GUI.Pages.TaskDetails;

using Hourglass.Database;
using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;

public partial class TaskDetails : Form {

	private readonly Database.Models.Task _task;
	private readonly IHourglassDbService _dbService;

	public TaskDetails(Database.Models.Task task, IHourglassDbService dbService) {
		_task = task;
		_dbService = dbService;
		InitializeComponent();
	}

	private void ApplyButton_Click(object sender, EventArgs e) {
		//_taks.StartDateTime = ;
		Database.Models.Task newTask = new() {
			Id = _task.Id,
			description = DescriptionTextbox.Text,
			StartDateTime = DateTimeHelper.InterpretDayAndTimeString(StartTextbox.Text) ?? _task.StartDateTime,
			FinishDateTime = DateTimeHelper.InterpretDayAndTimeString(FinishTextbox.Text) ?? _task.FinishDateTime,
			owner = _task.owner,
			project = _task.project,
			ticket = _task.ticket
		};
		_dbService.UpdateTaskAsync(newTask);
		Close();
	}

	private void DeleteButtonClick(object sender, EventArgs e) {
		_dbService.DeleteTaskAsync(_task);
		Close();
    }

    private void EscapeButtonClick(object sender, EventArgs e) {
        Close();
    }

    private void ContiniueButtonClick(object sender, EventArgs e) {
		_dbService.ContiniueTaskAsync(_task);
        Close();
    }

    private void TaskDetails_Load(object sender, EventArgs e) {
		if (_task != null) {
			DescriptionTextbox.Text = _task.description;
			StartTextbox.Text = DateTimeHelper.ToDayAndTimeString(_task.StartDateTime);
			FinishTextbox.Text = DateTimeHelper.ToDayAndTimeString(_task.FinishDateTime);
		}
	}
}
