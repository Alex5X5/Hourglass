using Hourglass.GUI.Pages.Timer;
using Hourglass.Util;

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
		StopButton = new Button();
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
		StartButton = new Button();
		DescriptionTextBox = new RichTextBox();
		SettingsButton = new Button();
		ElapsedTimeLabel = new Label();
		DayModeButton = new Button();
		WeekModeButton = new Button();
		MonthModeButton = new Button();
		button2 = new Button();
		GraphPanel = new GraphRenderer();
		ExportButton = new Button();
		ImportButton = new Button();
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
		StopButton.BackColor = Color.Transparent;
		StopButton.FlatAppearance.BorderSize = 0;
		StopButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
		StopButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
		StopButton.FlatStyle = FlatStyle.Flat;
		StopButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StopButton.ForeColor = Color.Transparent;
		StopButton.Location = new Point(688, 158);
		StopButton.Name = "StopButton";
		StopButton.Size = new Size(146, 55);
		StopButton.TabIndex = 3;
		StopButton.UseVisualStyleBackColor = true;
		StopButton.Click += StopButtonClick;
		// 
		// StartButton
		// 
		StartButton.BackColor = Color.Transparent;
		StartButton.FlatAppearance.BorderSize = 0;
		StartButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
		StartButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
		StartButton.FlatStyle = FlatStyle.Flat;
		StartButton.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartButton.ForeColor = Color.Transparent;
		StartButton.Location = new Point(867, 207);
		StartButton.Name = "StartButton";
		StartButton.Size = new Size(146, 55);
		StartButton.TabIndex = 1;
		StartButton.UseVisualStyleBackColor = true;
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
		DescriptionTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		DescriptionTextBox.Location = new Point(55, 413);
		DescriptionTextBox.Name = "DescriptionTextBox";
		DescriptionTextBox.Size = new Size(210, 191);
		DescriptionTextBox.TabIndex = 16;
		DescriptionTextBox.Text = "";
		// 
		// SettingsButton
		// 
		SettingsButton.Location = new Point(1364, 69);
		SettingsButton.Name = "SettingsButton";
		SettingsButton.Size = new Size(54, 52);
		SettingsButton.TabIndex = 17;
		SettingsButton.Text = "Sett";
		SettingsButton.UseVisualStyleBackColor = true;
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
		DayModeButton.Location = new Point(1214, 350);
		DayModeButton.Name = "DayModeButton";
		DayModeButton.Size = new Size(54, 52);
		DayModeButton.TabIndex = 20;
		DayModeButton.Text = "Day";
		DayModeButton.UseVisualStyleBackColor = true;
		// 
		// WeekModeButton
		// 
		WeekModeButton.Location = new Point(1214, 439);
		WeekModeButton.Name = "WeekModeButton";
		WeekModeButton.Size = new Size(54, 52);
		WeekModeButton.TabIndex = 20;
		WeekModeButton.Text = "Week";
		WeekModeButton.UseVisualStyleBackColor = true;
		// 
		// MonthModeButton
		// 
		MonthModeButton.Location = new Point(1214, 530);
		MonthModeButton.Name = "MonthModeButton";
		MonthModeButton.Size = new Size(54, 52);
		MonthModeButton.TabIndex = 20;
		MonthModeButton.Text = "Month";
		MonthModeButton.UseVisualStyleBackColor = true;
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
		//ExportButton.BackColor = Color.Transparent;
		//ExportButton.FlatAppearance.BorderSize = 0;
		//ExportButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
		//ExportButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
		//ExportButton.FlatStyle = FlatStyle.Flat;
		ExportButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ExportButton.ForeColor = Color.Transparent;
		ExportButton.Location = new Point(83, 595);
		ExportButton.Name = "ExportButton";
		ExportButton.Size = new Size(122, 49);
		ExportButton.TabIndex = 23;
		ExportButton.UseVisualStyleBackColor = true;
		ExportButton.Click += ExportButtonClick;
		// 
		// ImportButton
		// 
		//ImportButton.BackColor = Color.Transparent;
		//ImportButton.FlatAppearance.BorderSize = 0;
		//ImportButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
		//ImportButton.FlatStyle = FlatStyle.Flat;
		ImportButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ImportButton.ForeColor = Color.Transparent;
		ImportButton.Location = new Point(83, 654);
		ImportButton.Name = "ImportButton";
		ImportButton.Size = new Size(122, 49);
		ImportButton.TabIndex = 24;
		ImportButton.UseVisualStyleBackColor = true;
		// 
		// TimerWindow
		// 
		BackColor = SystemColors.AppWorkspace;
		FormBorderStyle = FormBorderStyle.FixedSingle;
		ClientSize = new Size(1600, 900);
		//Controls.Add(button2);
		//Controls.Add(DayModeButton);
		//Controls.Add(WeekModeButton);
		//Controls.Add(MonthModeButton);
		Controls.Add(ElapsedTimeLabel);
		Controls.Add(SettingsButton);
		Controls.Add(StartButton);
		//Controls.Add(ImportButton);
		//Controls.Add(ExportButton);
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
	private Button StopButton;
	private TextBox StartTextbox;
	private TextBox FinishTextbox;
	private GraphRenderer GraphPanel;
	private TextBox TicketTextBox;
	private TextBox ProjectTextBox;
	private Button StartButton;
	private RichTextBox DescriptionTextBox;
	private Button SettingsButton;
	private Label ElapsedTimeLabel;
	private Label DescriptionLabel;
	private Label StartLabel;
	private Label FinishLabel;
	private Label TicketLabel;
	private Label ProjectLabel;
	private Button button2;
	private Button DayModeButton;
	private Button WeekModeButton;
	private Button MonthModeButton;
	private Button ExportButton;
	private Button ImportButton;
}