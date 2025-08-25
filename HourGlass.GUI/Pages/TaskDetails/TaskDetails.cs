using Hourglass.Database;

namespace Hourglass.GUI.Pages.TaskDetails;

public partial class TaskDetails : Form {

	private Database.Models.Task _taks;
	private HourglassDbContext _context;

	public TaskDetails(Database.Models.Task task, HourglassDbContext context) {
		_taks = task;
		_context = context;
		InitializeComponent();
	}

	private void ApplyButton_Click(object sender, EventArgs e) {
		//_taks.StartDateTime = ;
	}

	private void DeleteButton_Click(object sender, EventArgs e) {

	}

	private void EscapeButton_Click(object sender, EventArgs e) {

	}
}
