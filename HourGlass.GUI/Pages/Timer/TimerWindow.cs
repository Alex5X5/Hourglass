using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Pages.ExportProgressPopup;
using Hourglass.GUI.Pages.SettingsPopup;
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

    private bool invokeInProgress = false;
	private bool stopInvoking = false;
	public bool ShuttingDown { get { return stopInvoking; } }

    private readonly IPdfService pdf;

	private Hourglass.Database.Models.Task? RunningTask = null;

	private readonly Image image = Bitmap.FromFile(PathService.AssetsPath("Präsentation3.png"));
	private bool Stop = false;

    public DateTime SelectedWeek {
        set => SelectedWeekStartSeconds = (int)(value.Ticks / TimeSpan.TicksPerSecond);
        get => new(SelectedWeekStartSeconds);
    }

	public DateTime SelectedDay {
        set => SelectedDayStartSeconds = (int)(value.Ticks / TimeSpan.TicksPerSecond);
        get => new(SelectedDayStartSeconds);
    }

    private int SelectedWeekStartSeconds = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerSecond);
	private int SelectedDayStartSeconds = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerSecond);

    TimerWindowMode windowMode = TimerWindowMode.Day;

	public TimerWindow(IHourglassDbService dbService) {
		_dbService = dbService;
		pdf = new PdfService(_dbService);
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
			SetTextBoxTextSafely(StartTextbox, DateTimeService.ToDayAndTimeString(RunningTask.StartDateTime));
            SetTextBoxTextSafely(DescriptionTextBox, RunningTask.description);
			StartButton.Disable();
		} else {
			StopButton.Disable();
		}
	}

    #region timer update methods

    private void SetLabelTextSafely(Label label, string text) {
            if (Disposing)
                return;
            if (label == null)
                return;
            if (label.Disposing)
                return;
            if (label.InvokeRequired) {
                if (stopInvoking != true) {
                    invokeInProgress = true;
                    label?.Invoke(() => label.Text = text);
                    invokeInProgress = false;
                }
                return;
            } else
                label.Text = text;
    }

    private void SetTextBoxTextSafely(TextBox label, string text) {
            if (Disposing)
                return;
            if (label == null)
                return;
            if (label.Disposing)
                return;
            if (label.InvokeRequired) {
                if (stopInvoking != true) {
                    invokeInProgress = true;
                    label?.Invoke(() => label.Text = text);
                    invokeInProgress = false;
                }
                return;
            } else
                label.Text = text;
    }

    private void SetTextBoxTextSafely(RichTextBox label, string text) {
            if (Disposing)
                return;
            if (label == null)
                return;
            if (label.Disposing)
                return;
            if (label.InvokeRequired) {
                if (stopInvoking != true) {
                    invokeInProgress = true;
                    label?.Invoke(() => label.Text = text);
                    invokeInProgress = false;
                }
                return;
            } else
                label.Text = text;
    }

	private void UpdateTimers() {
		while (!CanRaiseEvents)
			Thread.Sleep(10);
		while (!Disposing && !Stop) {
			try {
				if (RunningTask != null)
					SetTextBoxTextSafely(FinishTextbox, DateTimeService.ToDayAndTimeString(DateTime.Now));
				else
					SetTextBoxTextSafely(StartTextbox, DateTimeService.ToDayAndTimeString(DateTime.Now));
				if (RunningTask == null)
					continue;
				TimeSpan t = DateTime.Now.Subtract(RunningTask.StartDateTime);
				DateTime time;
				try {
					time = Convert.ToDateTime(t.ToString());
				} catch(FormatException){
					time = DateTime.Now;
				}
				SetLabelTextSafely(ElapsedTimeLabel, DateTimeService.ToTimeString(time));
			} catch (InvalidOperationException) { }
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
		RunningTask = await _dbService.StartNewTaskAsnc(
			DescriptionTextBox.Text,
			project,
			new Hourglass.Database.Models.Worker { name = "new user" },
			null
		);
		await Task.Run(
			() => {
				Thread.Sleep(100);
				if(RunningTask!=null)
					SetTextBoxTextSafely(StartTextbox, DateTimeService.ToDayAndTimeString(RunningTask.StartDateTime));
			}
		);
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
		RunningTask = await _dbService.FinishCurrentTaskAsync(
			startDateTime.Ticks / TimeSpan.TicksPerSecond,
			finishDateTime.Ticks / TimeSpan.TicksPerSecond,
			DescriptionTextBox.Text,
			null,
			null
		);
		await Task.Run(
			() => {
				Thread.Sleep(100);
				SetTextBoxTextSafely(FinishTextbox, "");
                SetTextBoxTextSafely(DescriptionTextBox, "");
                SetLabelTextSafely(ElapsedTimeLabel, "");
            }
		);
		StartButton.Enable();
		StopButton.Disable();
	}

    private void SettingsButtonClick(object sender, EventArgs e) {
		Console.WriteLine("showing settings");
        SettingsPopup popup =  new SettingsPopup();
		popup.ShowDialog();
    }

    private void StopRestartButtonClick(object sender, EventArgs e) {
		StopButtonClick(sender, e);
		StartButtonClick(sender, e);
	}

	private void ExportButtonClick(object sender, EventArgs e) {
		Console.WriteLine("on export button click");
		ExportButton.Enabled = false;
		ExportProgressPopup popup = new();
		ProgressReporter progressReporter = new(popup);
		popup.Show(this);
		new Thread(
			() => {
				pdf.Export(progressReporter);
				Invoke(
						() => {
							ExportButton.Enabled = true;
						});
			}
		).Start();
    }

	private void ImportButtonClick(object sender, EventArgs e) {
		Console.WriteLine("import button click");
	}

	private void DayModeButtonButtonClick(object sender, EventArgs e) {
        Console.WriteLine("day mode button click");
		windowMode = TimerWindowMode.Day;
		GraphPanel.WindowMode = windowMode;
	}

	private void WeekModeButtonButtonClick(object sender, EventArgs e) {
		Console.WriteLine("week mode button click");
        windowMode = TimerWindowMode.Week;
		GraphPanel.WindowMode = windowMode;
	}

	private void MonthModeButtonButtonClick(object sender, EventArgs e) {
		Console.WriteLine("month mode button click");
        windowMode = TimerWindowMode.Month;
		GraphPanel.WindowMode = windowMode;
	}

	#endregion

	public void OnContiniueTask(Hourglass.Database.Models.Task task) {
		RunningTask = task;
		Task.Run(
			() => {
				Thread.Sleep(200);
				SetTextBoxTextSafely(StartTextbox, DateTimeService.ToDayAndTimeString(task.StartDateTime));
				SetTextBoxTextSafely(DescriptionTextBox, task.description);
			}
		);
		StopButton.Enable();
		StopRestartButton.Enabled = true;
		StartButton.Disable();
		
	}

	private void TimerWindow_Load(object sender, EventArgs e) {
		GraphRenderThread.Start();
		TimerUpdaterThread.Start();
	}

	private async void TimerWindow_Close(object sender, FormClosingEventArgs e) {
		Stop = true;
        if (invokeInProgress) {
            e.Cancel = true;
            stopInvoking = true;
            await Task.Factory.StartNew(
				() => {
					while (invokeInProgress) ;
				}
			);
            Close();
        }
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

	private void DayModeButtonPaint(PaintEventArgs args) {
		using (Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
			args.Graphics.FillRectangle(brush, new(18, 18, 36, 36));
		using (Brush brush = new SolidBrush(Color.FromArgb(255, 200, 200, 200)))
			args.Graphics.FillRectangle(brush, new(20, 20, 32, 32));
		string dayText = Convert.ToString(DateTime.Now.Day);
		using (Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
			args.Graphics.DrawString(
				dayText,
				new("Segoe UI", 26F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
				brush,
				new Point(dayText.Length == 1 ? 25 : 18, 17)
			);
	}

	private void WeekModeButtonPaint(PaintEventArgs args) {
		int currentDay = DateTimeService.GetMondayOfCurrentWeek().DayOfWeek switch {
			DayOfWeek.Monday => 0,
			DayOfWeek.Tuesday => 1,
			DayOfWeek.Wednesday => 2,
			DayOfWeek.Thursday => 3,
			DayOfWeek.Friday => 4,
			DayOfWeek.Sunday => 5,
			_ => 6
		};
		int squareIntervall = 8;
		for (int i = 0; i < 7; i++) {
			int xPos = i * squareIntervall+8;
			int yPos = 33;
			Color color = Color.FromArgb(255, 230, 230, 230);
			if (i % 7 == 5 | i % 7 == 6)
				color = Color.FromArgb(255, 174, 174, 174);
			if (i == DateTime.Now.Day)
				color = Color.FromArgb(255, 192, 0, 0);
			if(i==currentDay)
				color = Color.FromArgb(255, 192, 0, 0);
			using Brush brush = new SolidBrush(color);
			args.Graphics.FillRectangle(brush, new(xPos, yPos, 6, 6));
		}
	}

	private void MonthModeButtonPaint(PaintEventArgs args) {
		int startOffset = DateTimeService.GetMondayOfCurrentWeek().DayOfWeek switch {
			DayOfWeek.Monday => 0,
			DayOfWeek.Tuesday => 1,
			DayOfWeek.Wednesday => 2,
			DayOfWeek.Thursday => 3,
			DayOfWeek.Friday => 4,
			DayOfWeek.Saturday => 5,
			_ => 6
		};
		int squareIntervall = 7;
		for (int i = startOffset; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + startOffset; i++) {
			int xPos = (i % 7) * squareIntervall + 13;
			int yPos = (int)Math.Floor(i / 7.0) * squareIntervall + 20;
			Color color = Color.FromArgb(255, 230, 230, 230);
			if (i % 7 == 5 | i % 7 == 6)
				color = Color.FromArgb(255, 174, 174, 174);
			if (i == DateTime.Now.Day - 1)
				color = Color.FromArgb(255, 192, 0, 0);
			using Brush brush = new SolidBrush(color);
			args.Graphics.FillRectangle(brush, new(xPos, yPos, 5, 5));
		}
	}
}
