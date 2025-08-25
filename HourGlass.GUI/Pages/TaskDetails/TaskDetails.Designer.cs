using Hourglass.Util;

namespace Hourglass.GUI.Pages.TaskDetails; 
partial class TaskDetails {
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
		ApplyButton = new Button();
		DeleteButton = new Button();
		EscapeButton = new Button();
		DescriptionTextbox = new RichTextBox();
		ProjectTextbox = new TextBox();
		TicketTextbox = new TextBox();
		DescriptionLabel = new Label();
		ProjectLabel = new Label();
		TicketLabel = new Label();
		StartLabel = new Label();
		StartTextbox = new TextBox();
		FinishLabel = new Label();
		FinishTextbox = new TextBox();
		button1 = new Button();
		button2 = new Button();
		button3 = new Button();
		button4 = new Button();
		button5 = new Button();
		button6 = new Button();
		SuspendLayout();
		// 
		// ApplyButton
		// 
		ApplyButton.Font = new Font("Segoe UI", 12F);
		ApplyButton.Location = new Point(90, 255);
		ApplyButton.Name = "ApplyButton";
		ApplyButton.Size = new Size(98, 35);
		ApplyButton.TabIndex = 0;
		ApplyButton.Text = "Apply";
		ApplyButton.UseVisualStyleBackColor = true;
		ApplyButton.Click += ApplyButton_Click;
		// 
		// DeleteButton
		// 
		DeleteButton.Font = new Font("Segoe UI", 12F);
		DeleteButton.Location = new Point(230, 255);
		DeleteButton.Name = "DeleteButton";
		DeleteButton.Size = new Size(98, 35);
		DeleteButton.TabIndex = 0;
		DeleteButton.Text = "Delete";
		DeleteButton.UseVisualStyleBackColor = true;
		DeleteButton.Click += DeleteButton_Click;
		// 
		// EscapeButton
		// 
		EscapeButton.Font = new Font("Segoe UI", 12F);
		EscapeButton.Location = new Point(370, 255);
		EscapeButton.Name = "EscapeButton";
		EscapeButton.Size = new Size(98, 35);
		EscapeButton.TabIndex = 0;
		EscapeButton.Text = "Cancel";
		EscapeButton.UseVisualStyleBackColor = true;
		EscapeButton.Click += EscapeButton_Click;
		// 
		// DescriptionTextbox
		// 
		DescriptionTextbox.Font = new Font("Segoe UI", 12F);
		DescriptionTextbox.Location = new Point(122, 31);
		DescriptionTextbox.Name = "DescriptionTextbox";
		DescriptionTextbox.Size = new Size(227, 86);
		DescriptionTextbox.TabIndex = 1;
		DescriptionTextbox.Text = "";
		// 
		// ProjectTextbox
		// 
		ProjectTextbox.Font = new Font("Segoe UI", 12F);
		ProjectTextbox.Location = new Point(122, 142);
		ProjectTextbox.Name = "ProjectTextbox";
		ProjectTextbox.Size = new Size(145, 29);
		ProjectTextbox.TabIndex = 2;
		// 
		// TicketTextbox
		// 
		TicketTextbox.Font = new Font("Segoe UI", 12F);
		TicketTextbox.Location = new Point(122, 185);
		TicketTextbox.Name = "TicketTextbox";
		TicketTextbox.Size = new Size(145, 29);
		TicketTextbox.TabIndex = 3;
		// 
		// DescriptionLabel
		// 
		DescriptionLabel.Font = new Font("Segoe UI", 12F);
		DescriptionLabel.Location = new Point(26, 34);
		DescriptionLabel.Name = "DescriptionLabel";
		DescriptionLabel.Size = new Size(89, 21);
		DescriptionLabel.TabIndex = 8;
		DescriptionLabel.Text = "Description";
		// 
		// ProjectLabel
		// 
		ProjectLabel.Font = new Font("Segoe UI", 12F);
		ProjectLabel.Location = new Point(50, 145);
		ProjectLabel.Name = "ProjectLabel";
		ProjectLabel.Size = new Size(60, 21);
		ProjectLabel.TabIndex = 7;
		ProjectLabel.Text = "Project";
		// 
		// TicketLabel
		// 
		TicketLabel.Font = new Font("Segoe UI", 12F);
		TicketLabel.Location = new Point(50, 185);
		TicketLabel.Name = "TicketLabel";
		TicketLabel.Size = new Size(60, 21);
		TicketLabel.TabIndex = 6;
		TicketLabel.Text = "Ticket";
		// 
		// StartLabel
		// 
		StartLabel.Font = new Font("Segoe UI", 12F);
		StartLabel.Location = new Point(330, 145);
		StartLabel.Name = "StartLabel";
		StartLabel.Size = new Size(60, 21);
		StartLabel.TabIndex = 10;
		StartLabel.Text = "Start";
		// 
		// StartTextbox
		// 
		StartTextbox.Font = new Font("Segoe UI", 12F);
		StartTextbox.Location = new Point(401, 137);
		StartTextbox.Name = "StartTextbox";
		StartTextbox.Size = new Size(145, 29);
		StartTextbox.TabIndex = 9;
		// 
		// FinishLabel
		// 
		FinishLabel.Font = new Font("Segoe UI", 12F);
		FinishLabel.Location = new Point(330, 185);
		FinishLabel.Name = "FinishLabel";
		FinishLabel.Size = new Size(60, 21);
		FinishLabel.TabIndex = 12;
		FinishLabel.Text = "Finish";
		// 
		// FinishTextbox
		// 
		FinishTextbox.Font = new Font("Segoe UI", 12F);
		FinishTextbox.Location = new Point(401, 185);
		FinishTextbox.Name = "FinishTextbox";
		FinishTextbox.Size = new Size(145, 29);
		FinishTextbox.TabIndex = 11;
		// 
		// button1
		// 
		button1.BackColor = Color.FromArgb(255, 128, 0);
		button1.Location = new Point(401, 37);
		button1.Name = "button1";
		button1.Size = new Size(25, 23);
		button1.TabIndex = 13;
		button1.Text = "button1";
		button1.UseVisualStyleBackColor = false;
		// 
		// button2
		// 
		button2.BackColor = Color.Firebrick;
		button2.Location = new Point(401, 94);
		button2.Name = "button2";
		button2.Size = new Size(25, 23);
		button2.TabIndex = 14;
		button2.Text = "button2";
		button2.UseVisualStyleBackColor = false;
		// 
		// button3
		// 
		button3.BackColor = Color.LightSeaGreen;
		button3.Location = new Point(451, 37);
		button3.Name = "button3";
		button3.Size = new Size(25, 23);
		button3.TabIndex = 15;
		button3.Text = "button3";
		button3.UseVisualStyleBackColor = false;
		// 
		// button4
		// 
		button4.BackColor = Color.SteelBlue;
		button4.Location = new Point(502, 37);
		button4.Name = "button4";
		button4.Size = new Size(25, 23);
		button4.TabIndex = 16;
		button4.Text = "button4";
		button4.UseVisualStyleBackColor = false;
		// 
		// button5
		// 
		button5.BackColor = Color.LimeGreen;
		button5.Location = new Point(451, 94);
		button5.Name = "button5";
		button5.Size = new Size(25, 23);
		button5.TabIndex = 17;
		button5.Text = "button5";
		button5.UseVisualStyleBackColor = false;
		// 
		// button6
		// 
		button6.BackColor = Color.Green;
		button6.FlatAppearance.BorderColor = Color.Lime;
		button6.FlatAppearance.BorderSize = 5;
		button6.Location = new Point(502, 94);
		button6.Name = "button6";
		button6.Size = new Size(25, 23);
		button6.TabIndex = 18;
		button6.Text = "button6";
		button6.UseVisualStyleBackColor = false;
		// 
		// TaskDetails
		// 
		ClientSize = new Size(593, 353);
		Controls.Add(button6);
		Controls.Add(button5);
		Controls.Add(button4);
		Controls.Add(button3);
		Controls.Add(button2);
		Controls.Add(button1);
		Controls.Add(FinishLabel);
		Controls.Add(FinishTextbox);
		Controls.Add(StartLabel);
		Controls.Add(StartTextbox);
		Controls.Add(TicketLabel);
		Controls.Add(ProjectLabel);
		Controls.Add(DescriptionLabel);
		Controls.Add(TicketTextbox);
		Controls.Add(ProjectTextbox);
		Controls.Add(DescriptionTextbox);
		Controls.Add(EscapeButton);
		Controls.Add(DeleteButton);
		Controls.Add(ApplyButton);
		Name = "TaskDetails";
		Load += TaskDetails_Load;
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private Button ApplyButton;
	private Button DeleteButton;
	private Button EscapeButton;
	private RichTextBox DescriptionTextbox;
	private TextBox ProjectTextbox;
	private TextBox TicketTextbox;
	private Label DescriptionLabel;
	private Label ProjectLabel;
	private Label TicketLabel;
	private Label StartLabel;
	private TextBox StartTextbox;
	private Label FinishLabel;
	private TextBox FinishTextbox;
	private Button button1;
	private Button button2;
	private Button button3;
	private Button button4;
	private Button button5;
	private Button button6;
}