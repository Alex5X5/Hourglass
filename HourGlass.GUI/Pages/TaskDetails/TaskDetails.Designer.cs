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
		DescriptionTextbox.Location = new Point(210, 20);
		DescriptionTextbox.Name = "DescriptionTextbox";
		DescriptionTextbox.Size = new Size(227, 86);
		DescriptionTextbox.TabIndex = 1;
		DescriptionTextbox.Text = _task.description;
		// 
		// ProjectTextbox
		// 
		ProjectTextbox.Font = new Font("Segoe UI", 12F);
		ProjectTextbox.Location = new Point(122, 142);
		ProjectTextbox.Name = "ProjectTextbox";
		ProjectTextbox.Size = new Size(145, 29);
		ProjectTextbox.TabIndex = 2;
		ProjectTextbox.Text = _task.project != null ? _task.project.Name : "";
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
		DescriptionLabel.Location = new Point(114, 23);
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
		StartTextbox.Text = DateTimeHelper.ToDayAndTimeString(_task.StartDateTime);
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
		FinishTextbox.Text = DateTimeHelper.ToDayAndTimeString(_task.FinishDateTime);
		// 
		// TaskDetails
		// 
		ClientSize = new Size(593, 353);
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
		Name = "Task Details";
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
}