//namespace Hourglass.GUI.Pages;

//public class TimerPage : IPage {

//	private Button StartButton;
//	private Button StopButton;
//	private Button StopRestartButton;

//	private Label ProjectLabel;
//	private Label TicketLabel;
//	private Label StartLabel;
//	private Label FinishLabel;
//	private Label DescriptionLabel;

//	private RichTextBox DescriptionBox;

//	private Panel DiagrammPanel;

//	public TimerPage() : base () {
//		StartButton = new Button();
//		StartButton.Visible = true;
//		StopButton = new();
//		StopRestartButton = new();
//		ProjectLabel = new();
//		TicketLabel = new();
//		StartLabel = new();
//		FinishLabel = new();
//		DescriptionLabel = new();
//		DescriptionBox = new();
//		DiagrammPanel = new();
//		BackColor = Color.FromArgb(100, 100, 100);
//		ApplyLayout();
//	}

//	public override void SetupCenterPanel(TableLayoutPanel panel) {
//		panel.BackColor = Color.FromArgb(0, 200, 100);
//		for(int i=0; i<32; i++)
//				ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3.125F));
//		for(int i=0; i<20; i++)
//				ColumnStyles.Add(new RowStyle(SizeType.Percent, 5.0F));
//		//StartButton.Parent = panel;
//		StartButton.AutoSize = true;
//		//StartButton.BackColor = Color.FromArgb(255, 0, 0);
//		StartButton.Dock = DockStyle.Fill;
//		StartButton.Margin = new Padding(0,0,0,0);
//		StartButton.Size = new(100, 100);
//		StartButton.Location = new(0,0);
//		Controls.Add(StartButton, 0, 0);
//		SetRowSpan(StartButton, 4);
//		SetColumnSpan(StartButton, 2);

//		StopRestartButton.Visible = true;
//		StopRestartButton.AutoSize = true;
//		//StopRestartButton.Location = new Point(0,0);
//		StopRestartButton.Name = "StopRestartButton";
//		//StopRestartButton.Size = new Size(153, 34);
//		StopRestartButton.Dock = DockStyle.Fill;
//		StopRestartButton.TabIndex = 1;
//		StopRestartButton.Text = "Stop and Restart";
//		StopRestartButton.UseVisualStyleBackColor = true;
//		SetRowSpan(StopRestartButton, 2);
//		SetColumnSpan(StopRestartButton, 2);
//		Controls.Add(StopRestartButton);

//		//StopRestartButton.Click += StopRestartButton_Click;

//		Controls.Add(StopButton, 5, 2);
//		SetRowSpan(StopButton, 2);
//		SetColumnSpan(StopButton, 2);

//		Controls.Add(StopRestartButton, 8, 2);
//		SetRowSpan(StopRestartButton, 2);
//		SetColumnSpan(StopRestartButton, 2);

//		Controls.Add(StartButton, 2, 2);
//		SetRowSpan(StartButton, 2);
//		SetColumnSpan(StartButton, 2);

//		Controls.Add(StopButton, 5, 2);
//		Controls.Add(StopRestartButton);
//		Controls.Add(ProjectLabel);
//		Controls.Add(TicketLabel);
//		Controls.Add(StartLabel);
//		Controls.Add(FinishLabel);
//		Controls.Add(DescriptionLabel);
//	}
//}
