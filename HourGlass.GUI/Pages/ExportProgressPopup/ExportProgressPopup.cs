namespace Hourglass.GUI.Pages.ExportProgressPopup;

using Hourglass.Database.Services.Interfaces;
using HourGlass.GUI.Pages.Timer;

public partial class ExportProgressPopup : Form {

    public static readonly Color TASK_BACKGROUND_ORANGE = Color.FromArgb(255, 128, 0);
    public static readonly Color TASK_BACKGROUND_RED = Color.Firebrick;
    public static readonly Color TASK_BACKGROUND_LIGTH_BLUE = Color.LightSeaGreen;
    public static readonly Color TASK_BACKGROUND_DARK_BLUE = Color.SteelBlue;
    public static readonly Color TASK_BACKGROUND_LIGHT_GREEN = Color.LimeGreen;
    public static readonly Color TASK_BACKGROUND_DARK_GREEN = Color.Green;

    private readonly Database.Models.Task _task;
    private readonly IHourglassDbService _dbService;
    private readonly TimerWindow _parent;

    private Color previousColor;

    public ExportProgressPopup() {
        InitializeComponent();
    }

    private void ExportProgressPopup_Load(object sender, EventArgs e) {
        if (_task != null) {
            previousColor = _task.DisplayColor;
            InfoTextbox.Text = _task.description;
        }
    }

    private void DescriptionTextbox_TextChanged(object sender, EventArgs e) {

    }

    private void progressBar1_Click(object sender, EventArgs e) {

    }
}
