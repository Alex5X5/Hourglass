using Hourglass.GUI.GuiComponents;
using Hourglass.GUI.Pages.Timer;
using Hourglass.Util;
using Hourglass.Util.Services;
using System.Windows.Forms;

namespace HourGlass.GUI.Pages.Timer; 

public partial class TimerWindow {
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent() {
		StopRestartButton = new Button();
		StopButton = new AnimatedButton(
			PathService.AssetsPath("button-1-normal.png"),
			PathService.AssetsPath("button-1-hover.png"),
			PathService.AssetsPath("button-1-disabled.png"),
			PathService.AssetsPath("button-1-pressed.png"),
			new(683, 156, 155, 58)
		);
		StartButton = new AnimatedButton(
			PathService.AssetsPath("button-1-normal.png"),
			PathService.AssetsPath("button-1-hover.png"),
			PathService.AssetsPath("button-1-disabled.png"),
			PathService.AssetsPath("button-1-pressed.png"),
			new(861, 205, 155, 58)
		);
		ExportButton = new AnimatedButton(
			PathService.AssetsPath("button-1-normal.png"),
			PathService.AssetsPath("button-1-hover.png"),
			PathService.AssetsPath("button-1-disabled.png"),
			PathService.AssetsPath("button-1-pressed.png"),
			new(83, 676, 155, 58)
		);
		ImportButton = new AnimatedButton(
			PathService.AssetsPath("button-1-normal.png"),
			PathService.AssetsPath("button-1-hover.png"),
			PathService.AssetsPath("button-1-disabled.png"),
			PathService.AssetsPath("button-1-pressed.png"),
			new(83, 748, 155, 58)
		);
		DayModeButton = new AnimatedButton(
			PathService.AssetsPath("button-2-normal.png"),
			PathService.AssetsPath("button-2-hover.png"),
			PathService.AssetsPath("button-2-disabled.png"),
			PathService.AssetsPath("button-2-pressed.png"),
			new(1513, 403, 72, 72)
		);
		WeekModeButton = new AnimatedButton(
			PathService.AssetsPath("button-2-normal.png"),
			PathService.AssetsPath("button-2-hover.png"),
			PathService.AssetsPath("button-2-disabled.png"),
			PathService.AssetsPath("button-2-pressed.png"),
			new(1513, 522, 72, 72)
		);
		MonthModeButton = new AnimatedButton(
			PathService.AssetsPath("button-2-normal.png"),
			PathService.AssetsPath("button-2-hover.png"),
			PathService.AssetsPath("button-2-disabled.png"),
			PathService.AssetsPath("button-2-pressed.png"),
			new(1513, 641, 72, 72)
		);
		StartTextbox = new TextBox();
		FinishTextbox = new TextBox();
		GraphPanel = new GraphRenderer();
		DescriptionLabel = new Label();
		StartLabel = new Label();
		FinishLabel = new Label();
		TicketLabel = new Label();
		ProjectLabel = new Label();
		TicketTextBox = new TextBox();
		ProjectTextBox = new TextBox();
		DescriptionTextBox = new RichTextBox();
		SettingsButton = new Button();
		ElapsedTimeLabel = new Label();
		//DayModeButton = new Button();
		//WeekModeButton = new Button();
		//MonthModeButton = new Button();
		button2 = new Button();
		GraphPanel = new GraphRenderer(_dbService, windowMode, this);
		SuspendLayout();
		// 
		// StopRestartButton
		// 
		StopRestartButton.BackColor = Color.Transparent;
		StopRestartButton.FlatAppearance.BorderSize = 0;
		StopRestartButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
		StopRestartButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
		StopRestartButton.FlatStyle = FlatStyle.Flat;
		StopRestartButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StopRestartButton.ForeColor = Color.Transparent;
		StopRestartButton.Location = new Point(1047, 207);
		StopRestartButton.Name = "StopRestartButton";
		StopRestartButton.Size = new Size(255, 53);
		StopRestartButton.TabIndex = 2;
		StopRestartButton.UseVisualStyleBackColor = true;
		StopRestartButton.Click += StopRestartButtonClick;
		// 
		// StopButton
		// 
		StopButton.Text = "Stop";
		StopButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StopButton.ForeColor = Color.White;
		StopButton.Click += StopButtonClick;
		// 
		// StartButton
		// 
		StartButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartButton.ForeColor = Color.White;
		StartButton.Text = "Start";
		StartButton.Click += StartButtonClick;
		// 
		// StartTextbox
		// 
		StartTextbox.BackColor = Color.Gainsboro;
		StartTextbox.BorderStyle = BorderStyle.None;
		StartTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartTextbox.Location = new Point(250, 91);
		StartTextbox.Name = "StartTextbox";
		StartTextbox.Size = new Size(137, 31);
		StartTextbox.TabIndex = 5;
		// 
		// FinishTextbox
		// 
		FinishTextbox.BackColor = Color.Gainsboro;
		FinishTextbox.BorderStyle = BorderStyle.None;
		FinishTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		FinishTextbox.Location = new Point(460, 91);
		FinishTextbox.Name = "FinishTextbox";
		FinishTextbox.Size = new Size(137, 31);
		FinishTextbox.TabIndex = 6;
		// 
		// TicketTextBox
		// 
		TicketTextBox.BackColor = Color.Gainsboro;
		TicketTextBox.BorderStyle = BorderStyle.None;
		TicketTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		TicketTextBox.Location = new Point(74, 294);
		TicketTextBox.Name = "TicketTextBox";
		TicketTextBox.Size = new Size(172, 31);
		TicketTextBox.TabIndex = 12;
		// 
		// ProjectTextBox
		// 
		ProjectTextBox.BackColor = Color.Gainsboro;
		ProjectTextBox.BorderStyle = BorderStyle.None;
		ProjectTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ProjectTextBox.Location = new Point(74, 197);
		ProjectTextBox.Name = "ProjectTextBox";
		ProjectTextBox.Size = new Size(172, 31);
		ProjectTextBox.TabIndex = 11;
		// 
		// DescriptionTextBox
		// 
		DescriptionTextBox.BackColor = Color.Gainsboro;
		DescriptionTextBox.BorderStyle = BorderStyle.None;
		DescriptionTextBox.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
		DescriptionTextBox.Location = new Point(55, 413);
		DescriptionTextBox.Name = "DescriptionTextBox";
		DescriptionTextBox.Size = new Size(230, 191);
		DescriptionTextBox.TabIndex = 16;
		DescriptionTextBox.Text = "";
		DescriptionTextBox.ScrollBars = RichTextBoxScrollBars.None;
		DescriptionTextBox.LostFocus += OnDescriptionTextboxLostFocus;
		// 
		// SettingsButton
		// 
		SettingsButton.Location = new Point(1410, 13);
		SettingsButton.Name = "SettingsButton";
		SettingsButton.Size = new Size(75, 75);
		SettingsButton.TabIndex = 17;
		SettingsButton.Text = "Sett";
		SettingsButton.UseVisualStyleBackColor = true;
        SettingsButton.Click += SettingsButtonClick;
        // 
        // ElapsedTimeLabel
        // 
        ElapsedTimeLabel.BackColor = Color.FromArgb(255,166,166,166);
		ElapsedTimeLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ElapsedTimeLabel.Location = new Point(321, 163);
		ElapsedTimeLabel.Name = "ElapsedTimeLabel";
		ElapsedTimeLabel.Size = new Size(138, 31);
		ElapsedTimeLabel.TabIndex = 5;
		// 
		// DayModeButton
		// 
		DayModeButton.Click += DayModeButtonButtonClick;
		DayModeButton.AddOnPaintEvent(DayModeButtonPaint);
		// 
		// WeekModeButton
		// 
		WeekModeButton.Click += WeekModeButtonButtonClick;
		WeekModeButton.AddOnPaintEvent(WeekModeButtonPaint);
		// 
		// MonthModeButton
		// 
		MonthModeButton.Click += MonthModeButtonButtonClick;
		MonthModeButton.AddOnPaintEvent(MonthModeButtonPaint);
		// 
		// button2
		// 
		button2.Location = new Point(1108, 12);
		button2.Name = "button2";
		button2.Size = new Size(54, 52);
		button2.TabIndex = 21;
		button2.Text = "Sett";
		button2.UseVisualStyleBackColor = true;
		// 
		// GraphPanel
		// 
		GraphPanel.Location = new Point(430, 350);
		GraphPanel.Name = "GraphPanel";
		GraphPanel.Size = new Size(996, 494);
		GraphPanel.TabIndex = 22;
		GraphPanel._dbService = _dbService;
		// 
		// ExportButton
		// 
		ExportButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ExportButton.ForeColor = Color.White;
		ExportButton.Text = "Export";
		ExportButton.Click += ExportButtonClick;
		// 
		// ImportButton
		// 
		ImportButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ImportButton.ForeColor = Color.White;
		ImportButton.Text = "Import";
		ImportButton.Click += ImportButtonClick;
		// 
		// TimerWindow
		// 
		BackColor = SystemColors.AppWorkspace;
		FormBorderStyle = FormBorderStyle.FixedSingle;
		ClientSize = new Size(1600, 900);
		Controls.Add(DayModeButton);
		Controls.Add(WeekModeButton);
		Controls.Add(MonthModeButton);
		Controls.Add(ElapsedTimeLabel);
		Controls.Add(SettingsButton);
		Controls.Add(StartButton);
		Controls.Add(ImportButton);
		Controls.Add(ExportButton);
		Controls.Add(StartButton);
		Controls.Add(TicketTextBox);
		Controls.Add(ProjectTextBox);
		Controls.Add(DescriptionTextBox);
		Controls.Add(FinishTextbox);
		Controls.Add(StartTextbox);
		Controls.Add(StopButton);
		Controls.Add(StopRestartButton);
		Controls.Add(GraphPanel);
		Name = "TimerWindow";
		Text = "Timer";
		FormClosing += TimerWindow_Close;
		Load += TimerWindow_Load;
		ResumeLayout(false);
		PerformLayout();
	}

    #endregion

    private Button StopRestartButton;
	private AnimatedButton StopButton;
	private TextBox StartTextbox;
	private TextBox FinishTextbox;
	private GraphRenderer GraphPanel;
	private TextBox TicketTextBox;
	private TextBox ProjectTextBox;
	private AnimatedButton StartButton;
	private RichTextBox DescriptionTextBox;
	private Button SettingsButton;
	private Label ElapsedTimeLabel;
	private Label DescriptionLabel;
	private Label StartLabel;
	private Label FinishLabel;
	private Label TicketLabel;
	private Label ProjectLabel;
	private Button button2;
	private AnimatedButton DayModeButton;
	private AnimatedButton WeekModeButton;
	private AnimatedButton MonthModeButton;
	private AnimatedButton ExportButton;
	private AnimatedButton ImportButton;
}