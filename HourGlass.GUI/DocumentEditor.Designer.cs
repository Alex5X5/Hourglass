namespace Zeitnachweis
{
    partial class DocumentEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			richTextBox1 = new RichTextBox();
			richTextBox2 = new RichTextBox();
			richTextBox3 = new RichTextBox();
			richTextBox4 = new RichTextBox();
			richTextBox5 = new RichTextBox();
			button1 = new Button();
			SuspendLayout();
			// 
			// richTextBox1
			// 
			richTextBox1.Dock = DockStyle.Top;
			richTextBox1.Location = new Point(0, 0);
			richTextBox1.Name = "richTextBox1";
			richTextBox1.Size = new Size(1345, 128);
			richTextBox1.TabIndex = 0;
			richTextBox1.Text = "";
			// 
			// richTextBox2
			// 
			richTextBox2.Location = new Point(621, 248);
			richTextBox2.Name = "richTextBox2";
			richTextBox2.Size = new Size(614, 128);
			richTextBox2.TabIndex = 1;
			richTextBox2.Text = "";
			// 
			// richTextBox3
			// 
			richTextBox3.Location = new Point(621, 418);
			richTextBox3.Name = "richTextBox3";
			richTextBox3.Size = new Size(614, 128);
			richTextBox3.TabIndex = 2;
			richTextBox3.Text = "";
			// 
			// richTextBox4
			// 
			richTextBox4.Location = new Point(621, 600);
			richTextBox4.Name = "richTextBox4";
			richTextBox4.Size = new Size(614, 128);
			richTextBox4.TabIndex = 3;
			richTextBox4.Text = "";
			// 
			// richTextBox5
			// 
			richTextBox5.Location = new Point(621, 773);
			richTextBox5.Name = "richTextBox5";
			richTextBox5.Size = new Size(614, 128);
			richTextBox5.TabIndex = 4;
			richTextBox5.Text = "";
			// 
			// button1
			// 
			button1.Location = new Point(90, 85);
			button1.Name = "button1";
			button1.Size = new Size(112, 34);
			button1.TabIndex = 5;
			button1.Text = "button1";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// DocumentEditor
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1345, 951);
			Controls.Add(button1);
			Controls.Add(richTextBox5);
			Controls.Add(richTextBox4);
			Controls.Add(richTextBox3);
			Controls.Add(richTextBox2);
			Controls.Add(richTextBox1);
			Name = "DocumentEditor";
			Text = "Form1";
			ResumeLayout(false);
		}

		#endregion

		private RichTextBox richTextBox1;
		private RichTextBox richTextBox2;
		private RichTextBox richTextBox3;
		private RichTextBox richTextBox4;
		private RichTextBox richTextBox5;
		private Button button1;
	}
}
