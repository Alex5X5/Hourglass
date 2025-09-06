namespace Hourglass.GUI.Pages.TaskDetails;

using Hourglass.Database;
using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using HourGlass.GUI.Pages.Timer;

public partial class TaskDetailsPopup : Form
{
    public static readonly Color TASK_BACKGROUND_ORANGE = Color.FromArgb(255, 128, 0);
    public static readonly Color TASK_BACKGROUND_RED= Color.Firebrick;
    public static readonly Color TASK_BACKGROUND_LIGTH_BLUE= Color.LightSeaGreen;
    public static readonly Color TASK_BACKGROUND_DARK_BLUE= Color.SteelBlue;
    public static readonly Color TASK_BACKGROUND_LIGHT_GREEN = Color.LimeGreen;
    public static readonly Color TASK_BACKGROUND_DARK_GREEN = Color.Green;

    private readonly Database.Models.Task _task;
    private readonly IHourglassDbService _dbService;
    private readonly TimerWindow _parent;

    private Color previousColor;

    public TaskDetailsPopup(Database.Models.Task task, IHourglassDbService dbService, TimerWindow parent) {
        _task = task;
        _dbService = dbService;
        _parent = parent;
        InitializeComponent();
    }

    private void ApplyButton_Click(object sender, EventArgs e) {
        Database.Models.Task newTask = new() {
            Id = _task.Id,
            description = DescriptionTextbox.Text,
            StartDateTime = DateTimeService.InterpretDayAndTimeString(StartTextbox.Text) ?? _task.StartDateTime,
            FinishDateTime = DateTimeService.InterpretDayAndTimeString(FinishTextbox.Text) ?? _task.FinishDateTime,
            owner = _task.owner,
            project = _task.project,
            ticket = _task.ticket,
            displayColorBlue = _task.displayColorBlue,
            displayColorGreen = _task.displayColorGreen,
            displayColorRed = _task.displayColorRed,
            running = _task.running,
        };
        _dbService.UpdateTaskAsync(newTask);
        Close();
    }

    private void DeleteButtonClick(object sender, EventArgs e) {
        _dbService.DeleteTaskAsync(_task);
        Close();
    }

    private void EscapeButtonClick(object sender, EventArgs e) {
        _task.DisplayColor = previousColor;
        _dbService.UpdateTaskAsync(_task);
        Close();
    }

    private void ContiniueButtonClick(object sender, EventArgs e) {
        _dbService.ContiniueTaskAsync(_task);
        _parent.OnContiniueTask(_task);
        Close();
    }

    private void TaskDetails_Load(object sender, EventArgs e) {
        if (_task != null) {
            previousColor = _task.DisplayColor;
            DescriptionTextbox.Text = _task.description;
            StartTextbox.Text = DateTimeService.ToDayAndTimeString(_task.StartDateTime);
            FinishTextbox.Text = DateTimeService.ToDayAndTimeString(_task.FinishDateTime);
        }
    }

    private void ColorOrangeButton_Click(object sender, EventArgs e) {
        _task.DisplayColor = TASK_BACKGROUND_ORANGE;
        _dbService.UpdateTaskAsync(_task);
    }

    private void ColorLightBlueButton_Click(object sender, EventArgs e) {
        _task.DisplayColor = TASK_BACKGROUND_LIGTH_BLUE;
        _dbService.UpdateTaskAsync(_task);
    }

    private void ColorDarkBlueButton_Click(object sender, EventArgs e)  {
        _task.DisplayColor = TASK_BACKGROUND_DARK_BLUE;
        _dbService.UpdateTaskAsync(_task);
    }

    private void ColorRedButton_Click(object sender, EventArgs e)  {
        _task.DisplayColor = TASK_BACKGROUND_RED;
        _dbService.UpdateTaskAsync(_task);
    }

    private void ColorLightGreenButton_Click(object sender, EventArgs e) {
        _task.DisplayColor = TASK_BACKGROUND_LIGHT_GREEN;
        _dbService.UpdateTaskAsync(_task);
    }

    private void ColorDarkGreenButton_Click(object sender, EventArgs e)  {
        _task.DisplayColor = TASK_BACKGROUND_DARK_GREEN;
        _dbService.UpdateTaskAsync(_task);
    }
}
