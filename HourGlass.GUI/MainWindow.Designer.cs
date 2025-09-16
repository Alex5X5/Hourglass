namespace Hourglass.GUI {
	partial class MainWindow {
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
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel2 = new TableLayoutPanel();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			//// 
			//// tableLayoutPanel1
			//// 
			//tableLayoutPanel1.ColumnCount = 3;
			//tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
			//tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
			//tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
			//tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
			//tableLayoutPanel1.Dock = DockStyle.Fill;
			//tableLayoutPanel1.Name = "tableLayoutPanel1";
			//tableLayoutPanel1.RowCount = 3;
			//tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			//tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
			//tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
			//tableLayoutPanel1.TabIndex = 0;
			//// 
			//// tableLayoutPanel2
			//// 
			//tableLayoutPanel2.BackColor = Color.FromArgb(100, 0, 100);
			//tableLayoutPanel2.ColumnCount = 2;
			//tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			//tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			//tableLayoutPanel2.Dock = DockStyle.Fill;
			//tableLayoutPanel2.Name = "tableLayoutPanel2";
			//tableLayoutPanel2.RowCount = 2;
			//tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			//tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			//tableLayoutPanel2.TabIndex = 0;
			//// 
			//// MainWindow
			//// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(tableLayoutPanel1);
			Name = "MainWindow";
			Text = "Form1";
			tableLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private void OnResize(object sender, EventArgs e) {

		}
		private TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
	}
}