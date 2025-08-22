using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;

using System.Linq;

namespace HourGlass.GUI.Pages.Timer;

public partial class TimerWindow : Form {

	private readonly IHourglassDbService _dbService;

	private readonly Thread GraphRenderThread;
	private readonly Thread TimerUpdaterThread;

	private Hourglass.Database.Models.Task? runningTask = null;

	private bool stop = false;

	public TimerWindow(IHourglassDbService dbService) {
		_dbService = dbService;

		InitializeComponent();

		GraphRenderThread = new Thread(
			() => {
				while (!CanRaiseEvents)
					Thread.Sleep(10);
				while (!Disposing && !stop) {
					GraphPanel.Invalidate();
					Thread.Sleep(100);
				}
			}
		);

		TimerUpdaterThread = new Thread(UpdateTimers);
		runningTask = _dbService.QueryCurrentTaskAsync().Result;
		if (runningTask != null) {
			StartTextbox.Text = FormatDateTime(runningTask.StartDateTime);
			StartButton.Enabled = false;
		} else {
			StopButton.Enabled = false;
		}
	}

	private string FormatDateTime(DateTime time) =>
		$"{time.Day}.{time.Month} {time.Hour}:{time.Minute}:{time.Second}";

	private void UpdateTimers() {
		while (!CanRaiseEvents)
			Thread.Sleep(10);
		while (!Disposing && !stop) {
			Thread.Sleep(200);
			try {
				if (runningTask == null)
					if (StartTextbox != null)
						if (!StartTextbox.Disposing)
							if (StartTextbox.InvokeRequired) {
								StartTextbox?.Invoke(
									() => StartTextbox.Text = FormatDateTime(DateTime.Now)
								);
							} else {
								StartTextbox.Text = FormatDateTime(DateTime.Now);
							}
				if (runningTask == null)
					continue;
				if (FinishTextbox != null)
					if (!FinishTextbox.Disposing)
						if (FinishTextbox.InvokeRequired) {
							FinishTextbox.Invoke(
								() => FinishTextbox.Text = FormatDateTime(DateTime.Now)
							);
						} else {
							FinishTextbox.Text = FormatDateTime(DateTime.Now);
						}
				if (runningTask == null)
					continue;
				if (ElapsedTimeLabel != null)
					if (!ElapsedTimeLabel.Disposing)
						if (ElapsedTimeLabel.InvokeRequired) {
							TimeSpan t = DateTime.Now.Subtract(runningTask.StartDateTime);
							ElapsedTimeLabel.Invoke(
								() => {
									DateTime time = Convert.ToDateTime(DateTime.Now.Subtract(runningTask.StartDateTime).ToString());
									ElapsedTimeLabel.Text = $"{time.Hour}:{time.Minute}:{time.Second}";
								}
							);
						} else {
							DateTime time = Convert.ToDateTime(DateTime.Now.Subtract(runningTask.StartDateTime).ToString());
							ElapsedTimeLabel.Text = $"{time.Hour}:{time.Minute}:{time.Second}";
						}
			} catch (InvalidOperationException) {
			} catch (System.ComponentModel.InvalidAsynchronousStateException) {
			}
		}
	}
	
	private void StartButtonClick(object sender, EventArgs e) {
		IEnumerable<Hourglass.Database.Models.Project> projects = _dbService.QueryProjectsAsync().Result;
		Hourglass.Database.Models.Project? project = projects.FirstOrDefault(x=>x.Name == ProjectTextBox.Text);
		StartButton.Enabled = false;
		runningTask = _dbService.StartNewTaskAsnc(
			DescriptionTextBox.Text,
			project,
			new Hourglass.Database.Models.Worker { name = "new user" },
			null
		).Result;
		StopButton.Enabled = true;
		StartButton.Enabled = false;
	}

	private void StopButtonClick(object sender, EventArgs e) {
		DateTime? startDateTime = null;
		DateTime? finishDateTime = null;
		Hourglass.Database.Models.Task? currentTask = _dbService.QueryCurrentTaskAsync().Result;
		try {
			startDateTime = DateTime.Parse(StartTextbox.Text);
		} catch (FormatException) {
			if (currentTask == null) {
				startDateTime = DateTime.Now;
			} else {
				startDateTime = DateTimeOffset.FromUnixTimeSeconds(currentTask.start).DateTime;
			}
		}
		try {
			finishDateTime = DateTime.Parse(FinishTextbox.Text);
		} catch (FormatException) {
			finishDateTime = DateTime.Now;
		}
		runningTask = _dbService.FinishCurrentTaskAsync(
			startDateTime.Value.Ticks / TimeSpan.TicksPerSecond,
			finishDateTime.Value.Ticks / TimeSpan.TicksPerSecond,
			DescriptionTextBox.Text,
			null,
			null
		).Result;
		StopButton.Enabled = false;
		StartButton.Enabled = true;
		if (FinishTextbox.InvokeRequired)
			FinishTextbox.Invoke(() => FinishTextbox.Text = "");
		else
			FinishTextbox.Text = "";
		if (ElapsedTimeLabel.InvokeRequired)
			ElapsedTimeLabel.Invoke(() => ElapsedTimeLabel.Text = "");
		else
			ElapsedTimeLabel.Text = "";
	}

	private void StopRestartButtonClick(object sender, EventArgs e) {
		StopButtonClick(sender, e);
		StartButtonClick(sender, e);
	}

	private void TimerWindow_Load(object sender, EventArgs e) {
		GraphRenderThread.Start();
		TimerUpdaterThread.Start();
	}

	private void TimerWindow_Close(object sender, EventArgs e) {
		stop = true;
	}

	protected override void OnPaintBackground(PaintEventArgs args) {
		using (Graphics g = args.Graphics) {
			g.Clear(Color.White);
			Image m = Bitmap.FromFile(Paths.AssetsPath("Background2.png"));
			g.DrawImage(
				Bitmap.FromFile(Paths.AssetsPath("Background2.png")),
				new Point(0,0)
			);
		}
	}
}
