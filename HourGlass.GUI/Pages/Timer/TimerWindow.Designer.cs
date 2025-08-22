using Hourglass.GUI.Pages.Timer;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimerWindow));
		StopRestartButton = new Button();
		StopButton = new Button();
		StartTextbox = new TextBox();
		FinishTextbox = new TextBox();
		TicketTextBox = new TextBox();
		ProjectTextBox = new TextBox();
		StartButton = new Button();
		DescriptionTextBox = new RichTextBox();
		SettingsButton = new Button();
		ElapsedTimeLabel = new Label();
		button1 = new Button();
		button2 = new Button();
		GraphPanel = new GraphRenderer();
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
		StopRestartButton.Location = new Point(822, 162);
		StopRestartButton.Name = "StopRestartButton";
		StopRestartButton.Size = new Size(207, 49);
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
		StopButton.Location = new Point(550, 123);
		StopButton.Name = "StopButton";
		StopButton.Size = new Size(122, 49);
		StopButton.TabIndex = 3;
		StopButton.UseVisualStyleBackColor = true;
		StopButton.Click += StopButtonClick;
		// 
		// StartTextbox
		// 
		StartTextbox.BackColor = Color.Gainsboro;
		StartTextbox.BorderStyle = BorderStyle.None;
		StartTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		StartTextbox.Location = new Point(214, 73);
		StartTextbox.Name = "StartTextbox";
		StartTextbox.Size = new Size(137, 31);
		StartTextbox.TabIndex = 5;
		// 
		// FinishTextbox
		// 
		FinishTextbox.BackColor = Color.Gainsboro;
		FinishTextbox.BorderStyle = BorderStyle.None;
		FinishTextbox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		FinishTextbox.Location = new Point(375, 73);
		FinishTextbox.Name = "FinishTextbox";
		FinishTextbox.Size = new Size(137, 31);
		FinishTextbox.TabIndex = 6;
		// 
		// TicketTextBox
		// 
		TicketTextBox.BackColor = Color.Gainsboro;
		TicketTextBox.BorderStyle = BorderStyle.None;
		TicketTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		TicketTextBox.Location = new Point(76, 269);
		TicketTextBox.Name = "TicketTextBox";
		TicketTextBox.Size = new Size(165, 31);
		TicketTextBox.TabIndex = 12;
		// 
		// ProjectTextBox
		// 
		ProjectTextBox.BackColor = Color.Gainsboro;
		ProjectTextBox.BorderStyle = BorderStyle.None;
		ProjectTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ProjectTextBox.Location = new Point(76, 174);
		ProjectTextBox.Name = "ProjectTextBox";
		ProjectTextBox.Size = new Size(172, 31);
		ProjectTextBox.TabIndex = 11;
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
		StartButton.Location = new Point(685, 162);
		StartButton.Name = "StartButton";
		StartButton.Size = new Size(122, 49);
		StartButton.TabIndex = 1;
		StartButton.UseVisualStyleBackColor = true;
		StartButton.Click += StartButtonClick;
		// 
		// DescriptionTextBox
		// 
		DescriptionTextBox.BackColor = Color.Gainsboro;
		DescriptionTextBox.BorderStyle = BorderStyle.None;
		DescriptionTextBox.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		DescriptionTextBox.Location = new Point(64, 376);
		DescriptionTextBox.Name = "DescriptionTextBox";
		DescriptionTextBox.Size = new Size(171, 191);
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
		ElapsedTimeLabel.BackColor = Color.Gainsboro;
		ElapsedTimeLabel.Font = new Font("Segoe UI", 17F, FontStyle.Regular, GraphicsUnit.Point, 0);
		ElapsedTimeLabel.Location = new Point(267, 127);
		ElapsedTimeLabel.Name = "ElapsedTimeLabel";
		ElapsedTimeLabel.Size = new Size(138, 31);
		ElapsedTimeLabel.TabIndex = 5;
		// 
		// button1
		// 
		button1.Location = new Point(1214, 346);
		button1.Name = "button1";
		button1.Size = new Size(54, 52);
		button1.TabIndex = 20;
		button1.Text = "Sett";
		button1.UseVisualStyleBackColor = true;
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
		GraphPanel.Location = new Point(356, 280);
		GraphPanel.Name = "GraphPanel";
		GraphPanel.Size = new Size(761, 396);
		GraphPanel.TabIndex = 22;
		GraphPanel._dbService = _dbService;
		// 
		// TimerWindow
		// 
		BackColor = SystemColors.AppWorkspace;
		BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
		ClientSize = new Size(1280, 720);
		Controls.Add(button2);
		Controls.Add(button1);
		Controls.Add(ElapsedTimeLabel);
		Controls.Add(SettingsButton);
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
	private Button button1;
	private Button button2;
}