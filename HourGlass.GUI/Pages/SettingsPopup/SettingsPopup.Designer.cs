//using System.Windows.Forms;

//namespace Hourglass.GUI.Pages.SettingsPopup {
//	partial class SettingsPopup {
//		/// <summary>
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.IContainer components = null;

//		/// <summary>
//		/// Clean up any resources being used.
//		/// </summary>
//		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//		protected override void Dispose(bool disposing) {
//			if (disposing && (components != null)) {
//				components.Dispose();
//			}
//			base.Dispose(disposing);
//		}

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent() {
//            NameTextbox = new TextBox();
//            NameLabel = new Label();
//            JobLabel = new Label();
//            JobTetbox = new TextBox();
//            OkButton = new Button();
//            CancelButton = new Button();
//            StartDateTextbox = new TextBox();
//            label1 = new Label();
//            trackBar1 = new TrackBar();
//            label2 = new Label();
//            label3 = new Label();
//            label4 = new Label();
//            label5 = new Label();
//            webBrowser1 = new WebBrowser();
//            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
//            SuspendLayout();
//            // 
//            // NameTextbox
//            // 
//            NameTextbox.Location = new Point(50, 33);
//            NameTextbox.Name = "NameTextbox";
//            NameTextbox.Size = new Size(238, 23);
//            NameTextbox.TabIndex = 0;
//            // 
//            // NameLabel
//            // 
//            NameLabel.AutoSize = true;
//            NameLabel.Location = new Point(50, 15);
//            NameLabel.Name = "NameLabel";
//            NameLabel.Size = new Size(60, 15);
//            NameLabel.TabIndex = 1;
//            NameLabel.Text = "Username";
//            // 
//            // JobLabel
//            // 
//            JobLabel.AutoSize = true;
//            JobLabel.Location = new Point(50, 64);
//            JobLabel.Name = "JobLabel";
//            JobLabel.Size = new Size(60, 15);
//            JobLabel.TabIndex = 2;
//            JobLabel.Text = "Job Name";
//            // 
//            // JobTetbox
//            // 
//            JobTetbox.Location = new Point(50, 82);
//            JobTetbox.Name = "JobTetbox";
//            JobTetbox.Size = new Size(238, 23);
//            JobTetbox.TabIndex = 3;
//            // 
//            // OkButton
//            // 
//            OkButton.Location = new Point(50, 270);
//            OkButton.Name = "OkButton";
//            OkButton.Size = new Size(75, 23);
//            OkButton.TabIndex = 4;
//            OkButton.Text = "OK";
//            OkButton.UseVisualStyleBackColor = true;
//            OkButton.Click += OkButton_Click;
//            // 
//            // CancelButton
//            // 
//            CancelButton.Location = new Point(213, 270);
//            CancelButton.Name = "CancelButton";
//            CancelButton.Size = new Size(75, 23);
//            CancelButton.TabIndex = 5;
//            CancelButton.Text = "Cancel";
//            CancelButton.UseVisualStyleBackColor = true;
//            CancelButton.Click += CancelButton_Click;
//            // 
//            // StartDateTextbox
//            // 
//            StartDateTextbox.Location = new Point(50, 131);
//            StartDateTextbox.Name = "StartDateTextbox";
//            StartDateTextbox.Size = new Size(238, 23);
//            StartDateTextbox.TabIndex = 7;
//            // 
//            // label1
//            // 
//            label1.AutoSize = true;
//            label1.Location = new Point(50, 113);
//            label1.Name = "label1";
//            label1.Size = new Size(58, 15);
//            label1.TabIndex = 6;
//            label1.Text = "Start Date";
//            // 
//            // trackBar1
//            // 
//            trackBar1.Location = new Point(80, 199);
//            trackBar1.Maximum = 2;
//            trackBar1.Name = "trackBar1";
//            trackBar1.Size = new Size(165, 45);
//            trackBar1.TabIndex = 8;
//            trackBar1.Value = 1;
//            // 
//            // label2
//            // 
//            label2.AutoSize = true;
//            label2.Location = new Point(50, 177);
//            label2.Name = "label2";
//            label2.Size = new Size(111, 15);
//            label2.TabIndex = 9;
//            label2.Text = "Time Export Format";
//            // 
//            // label3
//            // 
//            label3.AutoSize = true;
//            label3.Location = new Point(67, 227);
//            label3.Name = "label3";
//            label3.Size = new Size(49, 15);
//            label3.TabIndex = 10;
//            label3.Text = "detailed";
//            // 
//            // label4
//            // 
//            label4.AutoSize = true;
//            label4.BackColor = Color.Transparent;
//            label4.Location = new Point(139, 227);
//            label4.Name = "label4";
//            label4.Size = new Size(45, 15);
//            label4.TabIndex = 11;
//            label4.Text = "normal";
//            // 
//            // label5
//            // 
//            label5.AutoSize = true;
//            label5.BackColor = Color.Transparent;
//            label5.Location = new Point(205, 227);
//            label5.Name = "label5";
//            label5.Size = new Size(51, 15);
//            label5.TabIndex = 12;
//            label5.Text = "minimal";
//            // 
//            // webBrowser1
//            // 
//            webBrowser1.Location = new Point(67, 350);
//            webBrowser1.Name = "webBrowser1";
//            webBrowser1.Size = new Size(300, 300);
//            webBrowser1.TabIndex = 10;
//            // 
//            // SettingsPopup
//            // 
//            AutoScaleDimensions = new SizeF(7F, 15F);
//            AutoScaleMode = AutoScaleMode.Font;
//            ClientSize = new Size(333, 305);
//            Controls.Add(label5);
//            Controls.Add(label4);
//            Controls.Add(label3);
//            Controls.Add(label2);
//            Controls.Add(trackBar1);
//            Controls.Add(StartDateTextbox);
//            Controls.Add(label1);
//            Controls.Add(CancelButton);
//            Controls.Add(OkButton);
//            Controls.Add(JobTetbox);
//            Controls.Add(JobLabel);
//            Controls.Add(NameLabel);
//            Controls.Add(NameTextbox);
//            Name = "SettingsPopup";
//            Text = "Settings";
//            Load += SettingsPopup_Load;
//            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
//            ResumeLayout(false);
//            PerformLayout();
//        }

//        #endregion

//        private TextBox NameTextbox;
//		private Label NameLabel;
//		private Label JobLabel;
//		private TextBox JobTetbox;
//		private Button OkButton;
//		private new Button CancelButton;
//		private TextBox StartDateTextbox;
//		private Label label1;
//		private TrackBar trackBar1;
//		private Label label2;
//		private Label label3;
//		private Label label4;
//		private Label label5;
//        private WebBrowser webBrowser1;
//	}
//}