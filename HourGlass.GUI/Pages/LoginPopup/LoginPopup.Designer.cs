namespace Hourglass.GUI.Pages.LoginPopup {
	partial class LoginPopup {
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
			textBox1 = new TextBox();
			LoginButton = new Button();
			InfoLabel = new Label();
			SuspendLayout();
			// 
			// textBox1
			// 
			textBox1.Location = new Point(87, 131);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(152, 23);
			textBox1.TabIndex = 0;
			// 
			// LoginButton
			// 
			LoginButton.Location = new Point(129, 170);
			LoginButton.Name = "LoginButton";
			LoginButton.Size = new Size(75, 23);
			LoginButton.TabIndex = 2;
			LoginButton.Text = "Ok";
			LoginButton.UseVisualStyleBackColor = true;
			LoginButton.Click += LoginButton_Click;
			// 
			// InfoLabel
			// 
			InfoLabel.AutoSize = true;
			InfoLabel.Location = new Point(127, 99);
			InfoLabel.Name = "InfoLabel";
			InfoLabel.Size = new Size(87, 15);
			InfoLabel.TabIndex = 3;
			InfoLabel.Text = "Enter Password";
			// 
			// LoginPopup
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(338, 278);
			Controls.Add(InfoLabel);
			Controls.Add(LoginButton);
			Controls.Add(textBox1);
			Name = "LoginPopup";
			Text = "Login";
			Load += LoginPopup_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox textBox1;
		private Button LoginButton;
		private Label InfoLabel;
	}
}