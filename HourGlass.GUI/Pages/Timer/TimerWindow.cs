using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Pages.Timer;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;
using Hourglass.Util;
using Hourglass.Util.Services;

using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace HourGlass.GUI.Pages.Timer;

public partial class TimerWindow : Form {

	private readonly IHourglassDbService _dbService;

	private readonly Thread GraphRenderThread;
	private readonly Thread TimerUpdaterThread;

	private readonly PdfService Pdf;

	private readonly IPdfService pdf;

	private List<Hourglass.Database.Models.Task> VisibleTasks;
	private Hourglass.Database.Models.Task? RunningTask = null;

	private readonly Image image = Bitmap.FromFile(PathService.AssetsPath("Präsentation3.png"));
	private bool Stop = false;

	TimerWindowMode windowMode = TimerWindowMode.Day;

	public TimerWindow(IHourglassDbService dbService) {
		VisibleTasks = [];
		_dbService = dbService;
		Pdf = new PdfService(_dbService);
        InitializeComponent();

		GraphRenderThread = new Thread(
			() => {
				while (!CanRaiseEvents)
					Thread.Sleep(10);
				while (!Disposing && !Stop) {
					GraphPanel.Invalidate();
					Thread.Sleep(100);
				}
			}
		);

		TimerUpdaterThread = new Thread(UpdateTimers);
		RunningTask = _dbService.QueryCurrentTaskAsync().Result;
		if (RunningTask != null) {
			DescriptionTextBox.Text = RunningTask.description;
			SetStartTextboxText(DateTimeService.ToDayAndTimeString(RunningTask.StartDateTime));
			StartButton.Disable();
		} else {
			StopButton.Disable();
		}
	}

	#region timer update methods

	private void SetStartTextboxText(string text) {
		try {
			if (StartTextbox == null)
				return;
			if (StartTextbox.Disposing)
				return;
			if (StartTextbox.InvokeRequired)
				StartTextbox?.Invoke(() => StartTextbox.Text = text);
			else
				StartTextbox.Text = text;
		} catch (InvalidAsynchronousStateException) { }
	}

	private void SetFinishTextboxText(string text) {
		try {
			if (FinishTextbox == null)
				return;
			if (FinishTextbox.Disposing)
				return;
			if (FinishTextbox.InvokeRequired)
				FinishTextbox?.Invoke(() => FinishTextbox.Text = text);
			else
				FinishTextbox.Text = text;
		} catch(InvalidAsynchronousStateException){ }
	}

	private void SetElapsedTimeLabelText(string text) {
		try {
			if (ElapsedTimeLabel == null)
				return;
			if (ElapsedTimeLabel.Disposing)
				return;
			if (ElapsedTimeLabel.InvokeRequired)
				ElapsedTimeLabel?.Invoke(() => ElapsedTimeLabel.Text = text);
			else
				ElapsedTimeLabel.Text = text;
		} catch (InvalidAsynchronousStateException) { }
	}

	private void SetDescriptionTextboxText(string text) {
		try {
			if (DescriptionTextBox == null)
				return;
			if (DescriptionTextBox.Disposing)
				return;
			if (DescriptionTextBox.InvokeRequired)
				DescriptionTextBox?.Invoke(() => DescriptionTextBox.Text = text);
			else
				DescriptionTextBox.Text = text;
		} catch (InvalidAsynchronousStateException) { }
	}

	private void UpdateTimers() {
		while (!CanRaiseEvents)
			Thread.Sleep(10);
		while (!Disposing && !Stop) {
			try {
				if (RunningTask != null)
					SetFinishTextboxText(DateTimeService.ToDayAndTimeString(DateTime.Now));
				else
					SetStartTextboxText(DateTimeService.ToDayAndTimeString(DateTime.Now));
				if (RunningTask == null)
					continue;
				TimeSpan t = DateTime.Now.Subtract(RunningTask.StartDateTime);
				DateTime time;
				try {
					time = Convert.ToDateTime(t.ToString());
				} catch(FormatException){
					time = DateTime.Now;
				}
				SetElapsedTimeLabelText(DateTimeService.ToTimeString(time));
			} catch (InvalidOperationException) {
			}
			Thread.Sleep(200);
		}
	}

	#endregion

	#region button callbacks

	private async void StartButtonClick(object sender, EventArgs e) {
		Console.WriteLine("OnStartButtonClick");
		IEnumerable<Hourglass.Database.Models.Project> projects = await _dbService.QueryProjectsAsync();
		Hourglass.Database.Models.Project? project = projects.FirstOrDefault(x=>x.Name == ProjectTextBox.Text);
		StartButton.Enabled = false;
		RunningTask = _dbService.StartNewTaskAsnc(
			DescriptionTextBox.Text,
			project,
			new Hourglass.Database.Models.Worker { name = "new user" },
			null
		).Result;
		Task.Run(() => { Thread.Sleep(500); SetStartTextboxText(DateTimeService.ToDayAndTimeString(RunningTask.StartDateTime)); });
		StopButton.Enable();
		StartButton.Disable();
	}

	private async void StopButtonClick(object sender, EventArgs e) {
		DateTime startDateTime;
		DateTime finishDateTime;
		Hourglass.Database.Models.Task? currentTask = await _dbService.QueryCurrentTaskAsync();
		try {
			startDateTime = DateTimeService.InterpretDayAndTimeString(StartTextbox.Text) ?? DateTime.MinValue;
		} catch (FormatException) {
			if (currentTask == null) {
				startDateTime = DateTime.Now;
			} else {
				startDateTime = currentTask.StartDateTime;
			}
		}
		try {
			finishDateTime = DateTimeService.InterpretDayAndTimeString(FinishTextbox.Text) ?? DateTime.MinValue;
		} catch (FormatException) {
			finishDateTime = DateTime.Now;
		}
		RunningTask = _dbService.FinishCurrentTaskAsync(
			startDateTime.Ticks / TimeSpan.TicksPerSecond,
			finishDateTime.Ticks / TimeSpan.TicksPerSecond,
			DescriptionTextBox.Text,
			null,
			null
		).Result;
		Task.Run(() => { Thread.Sleep(500); SetFinishTextboxText(""); });
		Task.Run(() => { Thread.Sleep(500); SetElapsedTimeLabelText(""); });
		Task.Run(() => { Thread.Sleep(500); SetDescriptionTextboxText("");});
		StartButton.Enable();
		StopButton.Disable();
	}

	private void StopRestartButtonClick(object sender, EventArgs e) {
		StopButtonClick(sender, e);
		StartButtonClick(sender, e);
	}

	private void ExportButtonClick(object sender, EventArgs e) {
		Console.WriteLine("on export button click");
		ExportButton.Enabled = false;
		Task.Run(Pdf.Export)
			.ContinueWith(
				args => ExportButton.Invoke(
					()=>ExportButton.Enabled = true
				)
			);
	}

	private void ImportButtonClick(object sender, EventArgs e) {
		Console.WriteLine("import button click");
		pdf.Import();
	}

	private async void DayModeButtonButtonClick(object sender, EventArgs e) {
        Console.WriteLine("day mode button click");
		VisibleTasks = await _dbService.QueryTasksOfCurrentDayAsync();
		windowMode = TimerWindowMode.Day;
		GraphPanel.WindowMode = windowMode;
	}

	private async void WeekModeButtonButtonClick(object sender, EventArgs e) {
		Console.WriteLine("week mode button click");
        VisibleTasks = await _dbService.QueryTasksOfCurrentWeekAsync();
        windowMode = TimerWindowMode.Week;
		GraphPanel.WindowMode = windowMode;
	}

	private async void MonthModeButtonButtonClick(object sender, EventArgs e) {
		Console.WriteLine("month mode button click");
        VisibleTasks = await _dbService.QueryTasksOfCurrentWeekAsync();
        windowMode = TimerWindowMode.Month;
		GraphPanel.WindowMode = windowMode;
	}

	#endregion

	public void OnContiniueTask(Hourglass.Database.Models.Task task) {
		RunningTask = _dbService.QueryCurrentTaskAsync().Result;
		Task.Run(
			() => {
				Thread.Sleep(200);
				SetStartTextboxText(DateTimeService.ToDayAndTimeString(task.StartDateTime));
				SetDescriptionTextboxText(task.description);
			}
		);
		StopButton.Enabled = true;
		StopRestartButton.Enabled = true;
		StartButton.Enabled = false;
		
	}

	private void TimerWindow_Load(object sender, EventArgs e) {
		GraphRenderThread.Start();
		TimerUpdaterThread.Start();
	}

	private void TimerWindow_Close(object sender, EventArgs e) {
		Stop = true;
	}

	public static Bitmap ResizeImage(Image image, int width, int height) {
		var destRect = new Rectangle(0, 0, width, height);
		var destImage = new Bitmap(width, height);

		destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

		using (var graphics = Graphics.FromImage(destImage)) {
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			using (var wrapMode = new ImageAttributes()) {
				wrapMode.SetWrapMode(WrapMode.Clamp);
				graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
			}
		}
		return destImage;
	}

	protected override void OnResize(EventArgs e) {
		base.OnResize(e);
		Invalidate();
	}

	protected override void OnClick(EventArgs e) {
		Focus();
		if (RunningTask != null) {
			RunningTask.description = DescriptionTextBox.Text;
			_dbService.UpdateTaskAsync(RunningTask);
		}
	}

	public void OnDescriptionTextboxLostFocus(Object sender, EventArgs args) {
		if (RunningTask != null) {
			RunningTask.description = DescriptionTextBox.Text;
			_dbService.UpdateTaskAsync(RunningTask);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs args) { }

	protected override void OnPaint(PaintEventArgs args) {
		args.Graphics.DrawImage(image, 0, 0, ClientSize.Width, ClientSize.Height);
	}
}
