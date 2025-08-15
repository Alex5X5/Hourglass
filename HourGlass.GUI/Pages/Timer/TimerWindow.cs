using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Hourglass.Database.Serices.Interfaces;

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
		GraphRenderThread.Start();

		TimerUpdaterThread = new Thread(
			() => {
				while (!CanRaiseEvents)
					Thread.Sleep(10);
				while (!Disposing && !stop) {
					Thread.Sleep(500);
					try {
						if (runningTask == null) {
							if (StartTextbox == null)
								continue;
							if (StartTextbox.Disposing)
								continue;
							if (StartTextbox.InvokeRequired)
								StartTextbox?.Invoke(() => StartTextbox.Text = DateTime.Now.ToString());
							else
								StartTextbox.Text = DateTime.Now.ToString();
						} else {
							if (FinishTextbox == null)
								continue;
							if (FinishTextbox.Disposing)
								continue;
							if (FinishTextbox.InvokeRequired)
								FinishTextbox.Invoke(() => FinishTextbox.Text = DateTime.Now.ToString());
							else
								FinishTextbox.Text = DateTime.Now.ToString();
						}
					} catch(InvalidOperationException){
						
					} catch(System.ComponentModel.InvalidAsynchronousStateException){

					}
				}
			}
		);
		TimerUpdaterThread.Start();
		runningTask = _dbService.QueryCurrentTaskAsync().Result;
		if (runningTask != null) {
			StartTextbox.Text = DateTimeOffset.FromUnixTimeSeconds(runningTask.start).DateTime.ToString();
			StartButton.Enabled = false;
		} else {
			StopButton.Enabled = false;
		}
	}

	private async Task StartButtonClick() {
		IEnumerable<Hourglass.Database.Models.Project> projects = await _dbService.QueryProjectsAsync();
		Hourglass.Database.Models.Project? project = projects.FirstOrDefault(x=>x.Name == ProjectTextBox.Text);
		StartButton.Enabled = false;
		runningTask = await _dbService.StartNewTaskAsnc(
			DescriptionTextBox.Text,
			project,
			new Hourglass.Database.Models.Worker { name = "new user" },
			null
		);
		StopButton.Enabled = true;
		StartButton.Enabled = false;
	}

	private async Task StopButtonClick() {
		DateTime? startDateTime = null;
		DateTime? finishDateTime = null;
		Hourglass.Database.Models.Task? currentTask = await _dbService.QueryCurrentTaskAsync();
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
		runningTask = await _dbService.FinishCurrentTaskAsync(
			startDateTime.Value.Ticks / TimeSpan.TicksPerSecond,
			finishDateTime.Value.Ticks / TimeSpan.TicksPerSecond,
			DescriptionTextBox.Text,
			null,
			null
		);
		StopButton.Enabled = false;
		StartButton.Enabled = true;
		FinishTextbox.Text = "";
	}

	private async Task StopRestartButtonClick() {
		await StopButtonClick();
		await StartButtonClick();
	}

	private void TimerWindow_Load(object sender, EventArgs e) {

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
				wrapMode.SetWrapMode(WrapMode.TileFlipXY);
				graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
			}
		}

		return destImage;
	}
}
