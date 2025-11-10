//namespace Hourglass.GUI.Pages.TaskDetails; 

//using Hourglass.Util;

//partial class TaskDetailsPopup {
//	/// <summary>
//	/// Required designer variable.
//	/// </summary>
//	private System.ComponentModel.IContainer components = null;

//	/// <summary>
//	/// Clean up any resources being used.
//	/// </summary>
//	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//	protected override void Dispose(bool disposing) {
//		if (disposing && (components != null)) {
//			components.Dispose();
//		}
//		base.Dispose(disposing);
//	}

//    #region Windows Form Designer generated code

//    /// <summary>
//    /// Required method for Designer support - do not modify
//    /// the contents of this method with the code editor.
//    /// </summary>
//    private void InitializeComponent() {
//        ApplyButton = new Button();
//        DeleteButton = new Button();
//        EscapeButton = new Button();
//        DescriptionTextbox = new RichTextBox();
//        ProjectTextbox = new TextBox();
//        TicketTextbox = new TextBox();
//        DescriptionLabel = new Label();
//        ProjectLabel = new Label();
//        TicketLabel = new Label();
//        StartLabel = new Label();
//        StartTextbox = new TextBox();
//        FinishLabel = new Label();
//        FinishTextbox = new TextBox();
//        ColorOrangeButton = new Button();
//        ColorRedButton = new Button();
//        ColorLightBlueButton = new Button();
//        ColorDarkBlueButton = new Button();
//        ColorLightGreenButton = new Button();
//        ColorDarkGreenButton = new Button();
//        ContiniueButton = new Button();
//        RestartButton = new Button();
//        SuspendLayout();
//        // 
//        // ApplyButton
//        // 
//        ApplyButton.Font = new Font("Segoe UI", 12F);
//        ApplyButton.Location = new Point(242, 249);
//        ApplyButton.Name = "ApplyButton";
//        ApplyButton.Size = new Size(98, 35);
//        ApplyButton.TabIndex = 0;
//        ApplyButton.Text = "Apply";
//        ApplyButton.UseVisualStyleBackColor = true;
//        ApplyButton.Click += ApplyButton_Click;
//        // 
//        // DeleteButton
//        // 
//        DeleteButton.Font = new Font("Segoe UI", 12F);
//        DeleteButton.Location = new Point(138, 249);
//        DeleteButton.Name = "DeleteButton";
//        DeleteButton.Size = new Size(98, 35);
//        DeleteButton.TabIndex = 0;
//        DeleteButton.Text = "Delete";
//        DeleteButton.UseVisualStyleBackColor = true;
//        DeleteButton.Click += DeleteButtonClick;
//        // 
//        // EscapeButton
//        // 
//        EscapeButton.Font = new Font("Segoe UI", 12F);
//        EscapeButton.Location = new Point(346, 249);
//        EscapeButton.Name = "EscapeButton";
//        EscapeButton.Size = new Size(98, 35);
//        EscapeButton.TabIndex = 0;
//        EscapeButton.Text = "Cancel";
//        EscapeButton.UseVisualStyleBackColor = true;
//        EscapeButton.Click += EscapeButtonClick;
//        // 
//        // DescriptionTextbox
//        // 
//        DescriptionTextbox.Font = new Font("Segoe UI", 12F);
//        DescriptionTextbox.Location = new Point(122, 31);
//        DescriptionTextbox.Name = "DescriptionTextbox";
//        DescriptionTextbox.Size = new Size(227, 86);
//        DescriptionTextbox.TabIndex = 1;
//        DescriptionTextbox.Text = "";
//        // 
//        // ProjectTextbox
//        // 
//        ProjectTextbox.Font = new Font("Segoe UI", 12F);
//        ProjectTextbox.Location = new Point(122, 142);
//        ProjectTextbox.Name = "ProjectTextbox";
//        ProjectTextbox.Size = new Size(145, 29);
//        ProjectTextbox.TabIndex = 2;
//        // 
//        // TicketTextbox
//        // 
//        TicketTextbox.Font = new Font("Segoe UI", 12F);
//        TicketTextbox.Location = new Point(122, 185);
//        TicketTextbox.Name = "TicketTextbox";
//        TicketTextbox.Size = new Size(145, 29);
//        TicketTextbox.TabIndex = 3;
//        // 
//        // DescriptionLabel
//        // 
//        DescriptionLabel.Font = new Font("Segoe UI", 12F);
//        DescriptionLabel.Location = new Point(26, 34);
//        DescriptionLabel.Name = "DescriptionLabel";
//        DescriptionLabel.Size = new Size(89, 21);
//        DescriptionLabel.TabIndex = 8;
//        DescriptionLabel.Text = "Description";
//        // 
//        // ProjectLabel
//        // 
//        ProjectLabel.Font = new Font("Segoe UI", 12F);
//        ProjectLabel.Location = new Point(50, 145);
//        ProjectLabel.Name = "ProjectLabel";
//        ProjectLabel.Size = new Size(60, 21);
//        ProjectLabel.TabIndex = 7;
//        ProjectLabel.Text = "Project";
//        // 
//        // TicketLabel
//        // 
//        TicketLabel.Font = new Font("Segoe UI", 12F);
//        TicketLabel.Location = new Point(50, 185);
//        TicketLabel.Name = "TicketLabel";
//        TicketLabel.Size = new Size(60, 21);
//        TicketLabel.TabIndex = 6;
//        TicketLabel.Text = "Ticket";
//        // 
//        // StartLabel
//        // 
//        StartLabel.Font = new Font("Segoe UI", 12F);
//        StartLabel.Location = new Point(330, 145);
//        StartLabel.Name = "StartLabel";
//        StartLabel.Size = new Size(60, 21);
//        StartLabel.TabIndex = 10;
//        StartLabel.Text = "Start";
//        // 
//        // StartTextbox
//        // 
//        StartTextbox.Font = new Font("Segoe UI", 12F);
//        StartTextbox.Location = new Point(401, 137);
//        StartTextbox.Name = "StartTextbox";
//        StartTextbox.Size = new Size(145, 29);
//        StartTextbox.TabIndex = 9;
//        // 
//        // FinishLabel
//        // 
//        FinishLabel.Font = new Font("Segoe UI", 12F);
//        FinishLabel.Location = new Point(330, 185);
//        FinishLabel.Name = "FinishLabel";
//        FinishLabel.Size = new Size(60, 21);
//        FinishLabel.TabIndex = 12;
//        FinishLabel.Text = "Finish";
//        // 
//        // FinishTextbox
//        // 
//        FinishTextbox.Font = new Font("Segoe UI", 12F);
//        FinishTextbox.Location = new Point(401, 185);
//        FinishTextbox.Name = "FinishTextbox";
//        FinishTextbox.Size = new Size(145, 29);
//        FinishTextbox.TabIndex = 11;
//        // 
//        // ColorOrangeButton
//        // 
//        ColorOrangeButton.BackColor = TASK_BACKGROUND_ORANGE;
//        ColorOrangeButton.Location = new Point(401, 37);
//        ColorOrangeButton.Name = "ColorOrangeButton";
//        ColorOrangeButton.Size = new Size(25, 23);
//        ColorOrangeButton.TabIndex = 13;
//        ColorOrangeButton.UseVisualStyleBackColor = false;
//        ColorOrangeButton.Click += ColorOrangeButton_Click;
//        // 
//        // ColorRedButton
//        // 
//        ColorRedButton.BackColor = TASK_BACKGROUND_RED;
//        ColorRedButton.Location = new Point(401, 94);
//        ColorRedButton.Name = "ColorRedButton";
//        ColorRedButton.Size = new Size(25, 23);
//        ColorRedButton.TabIndex = 14;
//        ColorRedButton.UseVisualStyleBackColor = false;
//        ColorRedButton.Click += ColorRedButton_Click;
//        // 
//        // ColorLightBlueButton
//        // 
//        ColorLightBlueButton.BackColor = TASK_BACKGROUND_LIGTH_BLUE;
//        ColorLightBlueButton.Location = new Point(451, 37);
//        ColorLightBlueButton.Name = "ColorLightBlueButton";
//        ColorLightBlueButton.Size = new Size(25, 23);
//        ColorLightBlueButton.TabIndex = 15;
//        ColorLightBlueButton.UseVisualStyleBackColor = false;
//        ColorLightBlueButton.Click += ColorLightBlueButton_Click;
//        // 
//        // ColorDarkBlueButton
//        // 
//        ColorDarkBlueButton.BackColor = TASK_BACKGROUND_DARK_BLUE;
//        ColorDarkBlueButton.Location = new Point(502, 37);
//        ColorDarkBlueButton.Name = "ColorDarkBlueButton";
//        ColorDarkBlueButton.Size = new Size(25, 23);
//        ColorDarkBlueButton.TabIndex = 16;
//        ColorDarkBlueButton.UseVisualStyleBackColor = false;
//        ColorDarkBlueButton.Click += ColorDarkBlueButton_Click;
//        // 
//        // ColorLightGreenButton
//        // 
//        ColorLightGreenButton.BackColor = TASK_BACKGROUND_LIGHT_GREEN;
//        ColorLightGreenButton.Location = new Point(451, 94);
//        ColorLightGreenButton.Name = "ColorLightGreenButton";
//        ColorLightGreenButton.Size = new Size(25, 23);
//        ColorLightGreenButton.TabIndex = 17;
//        ColorLightGreenButton.UseVisualStyleBackColor = false;
//        ColorLightGreenButton.Click += ColorLightGreenButton_Click;
//        // 
//        // ColorDarkGreenButton
//        // 
//        ColorDarkGreenButton.BackColor = TASK_BACKGROUND_DARK_GREEN;
//        ColorDarkGreenButton.Location = new Point(502, 94);
//        ColorDarkGreenButton.Name = "ColorDarkGreenButton";
//        ColorDarkGreenButton.Size = new Size(25, 23);
//        ColorDarkGreenButton.TabIndex = 18;
//        ColorDarkGreenButton.UseVisualStyleBackColor = false;
//        ColorDarkGreenButton.Click += ColorDarkGreenButton_Click;
//        // 
//        // ContiniueButton
//        // 
//        ContiniueButton.Font = new Font("Segoe UI", 12F);
//        ContiniueButton.Location = new Point(293, 290);
//        ContiniueButton.Name = "ContiniueButton";
//        ContiniueButton.Size = new Size(98, 35);
//        ContiniueButton.TabIndex = 0;
//        ContiniueButton.Text = "Continiue";
//        ContiniueButton.UseVisualStyleBackColor = true;
//        ContiniueButton.Click += ContiniueButtonClick;
//        // 
//        // RestartButton
//        // 
//        RestartButton.Font = new Font("Segoe UI", 12F);
//        RestartButton.Location = new Point(189, 290);
//        RestartButton.Name = "RestartButton";
//        RestartButton.Size = new Size(98, 35);
//        RestartButton.TabIndex = 19;
//        RestartButton.Text = "Start New";
//        RestartButton.UseVisualStyleBackColor = true;
//        RestartButton.Click += StartNewButton_Click;
//        // 
//        // TaskDetailsPopup
//        // 
//        ClientSize = new Size(593, 353);
//        Controls.Add(RestartButton);
//        Controls.Add(ColorDarkGreenButton);
//        Controls.Add(ColorLightGreenButton);
//        Controls.Add(ColorDarkBlueButton);
//        Controls.Add(ColorLightBlueButton);
//        Controls.Add(ColorRedButton);
//        Controls.Add(ColorOrangeButton);
//        Controls.Add(FinishLabel);
//        Controls.Add(FinishTextbox);
//        Controls.Add(StartLabel);
//        Controls.Add(StartTextbox);
//        Controls.Add(TicketLabel);
//        Controls.Add(ProjectLabel);
//        Controls.Add(DescriptionLabel);
//        Controls.Add(TicketTextbox);
//        Controls.Add(ProjectTextbox);
//        Controls.Add(DescriptionTextbox);
//        Controls.Add(EscapeButton);
//        Controls.Add(ContiniueButton);
//        Controls.Add(DeleteButton);
//        Controls.Add(ApplyButton);
//        Name = "TaskDetailsPopup";
//        Load += TaskDetails_Load;
//        ResumeLayout(false);
//        PerformLayout();
//    }

//    #endregion

//    private Button ApplyButton;
//	private Button DeleteButton;
//	private Button EscapeButton;
//	private RichTextBox DescriptionTextbox;
//	private TextBox ProjectTextbox;
//	private TextBox TicketTextbox;
//	private Label DescriptionLabel;
//	private Label ProjectLabel;
//	private Label TicketLabel;
//	private Label StartLabel;
//	private TextBox StartTextbox;
//	private Label FinishLabel;
//	private TextBox FinishTextbox;
//	private Button ColorOrangeButton;
//	private Button ColorRedButton;
//	private Button ColorLightBlueButton;
//	private Button ColorDarkBlueButton;
//	private Button ColorLightGreenButton;
//	private Button ColorDarkGreenButton;
//    private Button ContiniueButton;
//    private Button RestartButton;
//}