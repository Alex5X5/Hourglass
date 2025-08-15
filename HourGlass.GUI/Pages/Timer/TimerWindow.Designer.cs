using Hourglass.GUI.Pages.Timer;
using ShGame.Game.Util;
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
		TimeLabel = new Label();
		TimeTextbox = new TextBox();
		button1 = new Button();
		button2 = new Button();
		SuspendLayout();
		// 
		// StopRestartButton
		// 
		StopRestartButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StopRestartButton.Location = new Point(496, 48);
		StopRestartButton.Name = "StopRestartButton";
		StopRestartButton.Size = new Size(300, 60);
		StopRestartButton.TabIndex = 2;
		StopRestartButton.Text = "Stop and Restart";
		StopRestartButton.UseVisualStyleBackColor = true;
		StopRestartButton.Click += async (args, sender) => await StopRestartButtonClick();
		// 
		// StopButton
		// 
		StopButton.Font = new Font("Segoe UI", 22F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StopButton.Location = new Point(316, 48);
		StopButton.Name = "StopButton";
		StopButton.Size = new Size(150, 60);
		StopButton.TabIndex = 3;
		StopButton.Text = "Stop";
		StopButton.UseVisualStyleBackColor = true;
		StopButton.Click += async (args, sender)=>await StopButtonClick();
		// 
		// StartTextbox
		// 
		StartTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartTextbox.Location = new Point(604, 150);
		StartTextbox.Name = "StartTextbox";
		StartTextbox.Size = new Size(207, 38);
		StartTextbox.TabIndex = 5;
		// 
		// FinishTextbox
		// 
		FinishTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		FinishTextbox.Location = new Point(604, 210);
		FinishTextbox.Name = "FinishTextbox";
		FinishTextbox.Size = new Size(207, 38);
		FinishTextbox.TabIndex = 6;
		// 
		// GraphPanel
		// 
		GraphPanel.BackColor = SystemColors.Window;
		GraphPanel.Location = new Point(62, 337);
		GraphPanel.Name = "GraphPanel";
		GraphPanel.Size = new Size(1383, 604);
		GraphPanel.TabIndex = 7;
		GraphPanel._dbService = _dbService;
		// 
		// DescriptionLabel
		// 
		DescriptionLabel.AutoSize = true;
		DescriptionLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		DescriptionLabel.Location = new Point(866, 132);
		DescriptionLabel.Name = "DescriptionLabel";
		DescriptionLabel.Size = new Size(131, 31);
		DescriptionLabel.TabIndex = 8;
		DescriptionLabel.Text = "Description";
		// 
		// StartLabel
		// 
		StartLabel.AutoSize = true;
		StartLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartLabel.Location = new Point(533, 153);
		StartLabel.Name = "StartLabel";
		StartLabel.Size = new Size(61, 31);
		StartLabel.TabIndex = 9;
		StartLabel.Text = "Start";
		// 
		// FinishLabel
		// 
		FinishLabel.AutoSize = true;
		FinishLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		FinishLabel.Location = new Point(522, 213);
		FinishLabel.Name = "FinishLabel";
		FinishLabel.Size = new Size(73, 31);
		FinishLabel.TabIndex = 10;
		FinishLabel.Text = "Finish";
		// 
		// TicketLabel
		// 
		TicketLabel.AutoSize = true;
		TicketLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		TicketLabel.Location = new Point(106, 211);
		TicketLabel.Name = "TicketLabel";
		TicketLabel.Size = new Size(74, 31);
		TicketLabel.TabIndex = 14;
		TicketLabel.Text = "Ticket";
		// 
		// ProjectLabel
		// 
		ProjectLabel.AutoSize = true;
		ProjectLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ProjectLabel.Location = new Point(97, 151);
		ProjectLabel.Name = "ProjectLabel";
		ProjectLabel.Size = new Size(85, 31);
		ProjectLabel.TabIndex = 13;
		ProjectLabel.Text = "Project";
		// 
		// TicketTextBox
		// 
		TicketTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		TicketTextBox.Location = new Point(188, 211);
		TicketTextBox.Name = "TicketTextBox";
		TicketTextBox.Size = new Size(303, 38);
		TicketTextBox.TabIndex = 12;
		// 
		// ProjectTextBox
		// 
		ProjectTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ProjectTextBox.Location = new Point(188, 151);
		ProjectTextBox.Name = "ProjectTextBox";
		ProjectTextBox.Size = new Size(303, 38);
		ProjectTextBox.TabIndex = 11;
		// 
		// StartButton
		// 
		StartButton.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartButton.Location = new Point(136, 48);
		StartButton.Name = "StartButton";
		StartButton.Size = new Size(150, 60);
		StartButton.TabIndex = 1;
		StartButton.Text = "Start";
		StartButton.UseVisualStyleBackColor = true;
		StartButton.Click += async (args, sender)=> await StartButtonClick();
		// 
		// DescriptionTextBox
		// 
		DescriptionTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		DescriptionTextBox.Location = new Point(866, 181);
		DescriptionTextBox.Name = "DescriptionTextBox";
		DescriptionTextBox.Size = new Size(488, 125);
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
		// TimeLabel
		// 
		TimeLabel.AutoSize = true;
		TimeLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		TimeLabel.Location = new Point(528, 273);
		TimeLabel.Name = "TimeLabel";
		TimeLabel.Size = new Size(64, 31);
		TimeLabel.TabIndex = 19;
		TimeLabel.Text = "Time";
		// 
		// TimeTextbox
		// 
		TimeTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		TimeTextbox.Location = new Point(604, 270);
		TimeTextbox.Name = "TimeTextbox";
		TimeTextbox.Size = new Size(207, 38);
		TimeTextbox.TabIndex = 18;
		// 
		// button1
		// 
		button1.Location = new Point(1261, 69);
		button1.Name = "button1";
		button1.Size = new Size(54, 52);
		button1.TabIndex = 20;
		button1.Text = "Sett";
		button1.UseVisualStyleBackColor = true;
		// 
		// button2
		// 
		button2.Location = new Point(1144, 69);
		button2.Name = "button2";
		button2.Size = new Size(54, 52);
		button2.TabIndex = 21;
		button2.Text = "Sett";
		button2.UseVisualStyleBackColor = true;
		// 
		// TimerWindow
		// 
		BackColor = SystemColors.AppWorkspace;
		ClientSize = new Size(1920, 1080);
		BackgroundImage = ResizeImage(Image.FromFile(Paths.AssetsPath("Folie1.png")),Width, Height);
		Controls.Add(button2);
		Controls.Add(button1);
		Controls.Add(TimeLabel);
		Controls.Add(TimeTextbox);
		Controls.Add(SettingsButton);
		Controls.Add(DescriptionTextBox);
		Controls.Add(StartButton);
		Controls.Add(TicketLabel);
		Controls.Add(ProjectLabel);
		Controls.Add(TicketTextBox);
		Controls.Add(ProjectTextBox);
		Controls.Add(FinishLabel);
		Controls.Add(StartLabel);
		Controls.Add(DescriptionLabel);
		Controls.Add(GraphPanel);
		Controls.Add(FinishTextbox);
		Controls.Add(StartTextbox);
		Controls.Add(StopButton);
		Controls.Add(StopRestartButton);
		Name = "TimerWindow";
		Text = "Timer";
		Load += TimerWindow_Load;
		FormClosing += TimerWindow_Close;
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button StopRestartButton;
	private Button StopButton;
	private TextBox StartTextbox;
	private TextBox FinishTextbox;
	private GraphRenderer GraphPanel;
	private Label DescriptionLabel;
	private Label StartLabel;
	private Label FinishLabel;
	private Label TicketLabel;
	private Label ProjectLabel;
	private TextBox TicketTextBox;
	private TextBox ProjectTextBox;
	private Button StartButton;
	private RichTextBox DescriptionTextBox;
	private Button SettingsButton;
	private Label TimeLabel;
	private TextBox TimeTextbox;
	private Button button1;
	private Button button2;
}