namespace Hourglass.GUI.Pages.ExportProgressPopup;

partial class ExportProgressPopup {
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
        InfoTextbox = new RichTextBox();
        progressBar1 = new ProgressBar();
        SuspendLayout();
        // 
        // InfoTextbox
        // 
        InfoTextbox.Font = new Font("Segoe UI", 12F);
        InfoTextbox.Location = new Point(12, 12);
        InfoTextbox.Name = "InfoTextbox";
        InfoTextbox.Size = new Size(255, 136);
        InfoTextbox.TabIndex = 1;
        InfoTextbox.Text = "";
        InfoTextbox.TextChanged += DescriptionTextbox_TextChanged;
        // 
        // progressBar1
        // 
        progressBar1.Location = new Point(12, 155);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(255, 23);
        progressBar1.TabIndex = 2;
        progressBar1.Click += progressBar1_Click;
        // 
        // ExportProgressPopup
        // 
        ClientSize = new Size(279, 190);
        Controls.Add(progressBar1);
        Controls.Add(InfoTextbox);
        Name = "ExportProgressPopup";
        Load += ExportProgressPopup_Load;
        ResumeLayout(false);
    }

    #endregion
    private RichTextBox InfoTextbox;
    private ProgressBar progressBar1;
}