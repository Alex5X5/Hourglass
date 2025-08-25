using Hourglass.Database.Services.Interfaces;
using Hourglass.PDF;
using Hourglass.Util;

using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace HourGlass.GUI.Pages.Timer;

public partial class TimerWindow : Form {

	private readonly IHourglassDbService _dbService;

	private readonly Thread GraphRenderThread;
	private readonly Thread TimerUpdaterThread;

	private Hourglass.Database.Models.Task? runningTask = null;

	private readonly Image image = Bitmap.FromFile(Paths.AssetsPath("Präsentation3.png"));
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
			DescriptionTextBox.Text = runningTask.description;
			SetStartTextboxText(DateTimeHelper.ToDayAndTimeString(runningTask.StartDateTime));
			StartButton.Enabled = false;
		} else {
			StopButton.Enabled = false;
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
		while (!Disposing && !stop) {
			try {
				if (runningTask != null)
					SetFinishTextboxText(DateTimeHelper.ToDayAndTimeString(DateTime.Now));
				else
					SetStartTextboxText(DateTimeHelper.ToDayAndTimeString(DateTime.Now));
				if (runningTask == null)
					continue;
				TimeSpan t = DateTime.Now.Subtract(runningTask.StartDateTime);
				DateTime time;
				try {
					time = Convert.ToDateTime(t.ToString());
				} catch(FormatException){
					time = DateTime.Now;
				}
				SetElapsedTimeLabelText(DateTimeHelper.ToTimeString(time));
			} catch (InvalidOperationException) {
			}
			Thread.Sleep(200);
		}
	}

	#endregion

	#region button callbacks

	private async void StartButtonClick(object sender, EventArgs e) {
		IEnumerable<Hourglass.Database.Models.Project> projects = await _dbService.QueryProjectsAsync();
		Hourglass.Database.Models.Project? project = projects.FirstOrDefault(x=>x.Name == ProjectTextBox.Text);
		StartButton.Enabled = false;
		runningTask = _dbService.StartNewTaskAsnc(
			DescriptionTextBox.Text,
			project,
			new Hourglass.Database.Models.Worker { name = "new user" },
			null
		).Result;
		SetStartTextboxText(DateTimeHelper.ToDayAndTimeString(runningTask.StartDateTime));
		StopButton.Enabled = true;
		StartButton.Enabled = false;
	}

	private void StopButtonClick(object sender, EventArgs e) {
		DateTime startDateTime;
		DateTime finishDateTime;
		Hourglass.Database.Models.Task? currentTask = _dbService.QueryCurrentTaskAsync().Result;
		try {
			startDateTime = DateTimeHelper.InterpretDayAndTimeString(StartTextbox.Text) ?? DateTime.MinValue;
		} catch (FormatException) {
			if (currentTask == null) {
				startDateTime = DateTime.Now;
			} else {
				startDateTime = currentTask.StartDateTime;
			}
		}
		try {
			finishDateTime = DateTimeHelper.InterpretDayAndTimeString(FinishTextbox.Text) ?? DateTime.MinValue;
		} catch (FormatException) {
			finishDateTime = DateTime.Now;
		}
		runningTask = _dbService.FinishCurrentTaskAsync(
			startDateTime.Ticks / TimeSpan.TicksPerSecond,
			finishDateTime.Ticks / TimeSpan.TicksPerSecond,
			DescriptionTextBox.Text,
			null,
			null
		).Result;
		StopButton.Enabled = false;
		StartButton.Enabled = true;
		SetFinishTextboxText("");
		SetElapsedTimeLabelText("");
		SetDescriptionTextboxText("");
	}

	private void StopRestartButtonClick(object sender, EventArgs e) {
		StopButtonClick(sender, e);
		StartButtonClick(sender, e);
	}

	private void ExportButtonClick(object sender, EventArgs e) {
		Console.WriteLine("on export button click");
		HourglassPdf.Export(_dbService);
	}

	private void ImportButtonClick(object sender, EventArgs e) {
	
	}

	private void DayModeButtonButtonClick(object sender, EventArgs e) {

	}

	private void WeekModeButtonButtonClick(object sender, EventArgs e) {

	}

	private void MonthModeButtonButtonClick(object sender, EventArgs e) {

	}

	#endregion

	private void TimerWindow_Load(object sender, EventArgs e) {
		GraphRenderThread.Start();
		TimerUpdaterThread.Start();
	}

	private void TimerWindow_Close(object sender, EventArgs e) {
		stop = true;
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
	}

	public void OnDescriptionTextboxLostFocus(Object sender, EventArgs args) {
		if (runningTask != null) {
			runningTask.description = DescriptionTextBox.Text;
			_dbService.UpdateTaskAsync(runningTask);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs args) { }

	protected override void OnPaint(PaintEventArgs args) {
		args.Graphics.DrawImage(image, 0, 0, ClientSize.Width, ClientSize.Height);
	}
}
