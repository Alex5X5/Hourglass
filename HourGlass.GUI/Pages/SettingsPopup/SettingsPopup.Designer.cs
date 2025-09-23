namespace Hourglass.GUI.Pages.SettingsPopup {
    partial class SettingsPopup {
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
            NameTextbox = new TextBox();
            NameLabel = new Label();
            JobLabel = new Label();
            JobTetbox = new TextBox();
            OkButton = new Button();
            CancelButton = new Button();
            StartDateTextbox = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // NameTextbox
            // 
            NameTextbox.Location = new Point(50, 46);
            NameTextbox.Name = "NameTextbox";
            NameTextbox.Size = new Size(238, 23);
            NameTextbox.TabIndex = 0;
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.Location = new Point(50, 28);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(60, 15);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "Username";
            // 
            // JobLabel
            // 
            JobLabel.AutoSize = true;
            JobLabel.Location = new Point(50, 92);
            JobLabel.Name = "JobLabel";
            JobLabel.Size = new Size(60, 15);
            JobLabel.TabIndex = 2;
            JobLabel.Text = "Job Name";
            // 
            // JobTetbox
            // 
            JobTetbox.Location = new Point(50, 110);
            JobTetbox.Name = "JobTetbox";
            JobTetbox.Size = new Size(238, 23);
            JobTetbox.TabIndex = 3;
            // 
            // OkButton
            // 
            OkButton.Location = new Point(50, 233);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 4;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(213, 233);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(75, 23);
            CancelButton.TabIndex = 5;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // textBox1
            // 
            StartDateTextbox.Location = new Point(50, 172);
            StartDateTextbox.Name = "textBox1";
            StartDateTextbox.Size = new Size(238, 23);
            StartDateTextbox.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(50, 154);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 6;
            label1.Text = "Start Date";
            // 
            // SettingsPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 268);
            Controls.Add(StartDateTextbox);
            Controls.Add(label1);
            Controls.Add(CancelButton);
            Controls.Add(OkButton);
            Controls.Add(JobTetbox);
            Controls.Add(JobLabel);
            Controls.Add(NameLabel);
            Controls.Add(NameTextbox);
            Name = "SettingsPopup";
            Text = "SettingsPopup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox NameTextbox;
        private Label NameLabel;
        private Label JobLabel;
        private TextBox JobTetbox;
        private Button OkButton;
        private new Button CancelButton;
        private TextBox StartDateTextbox;
        private Label label1;
    }
}