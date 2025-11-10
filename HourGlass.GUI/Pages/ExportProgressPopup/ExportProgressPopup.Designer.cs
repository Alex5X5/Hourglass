//namespace Hourglass.GUI.Pages.ExportProgressPopup;

//partial class ExportProgressPopup {
//    /// <summary>
//    /// Required designer variable.
//    /// </summary>
//    private System.ComponentModel.IContainer components = null;

//    /// <summary>
//    /// Clean up any resources being used.
//    /// </summary>
//    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//    protected override void Dispose(bool disposing) {
//        if (disposing && (components != null)) {
//            components.Dispose();
//        }
//        base.Dispose(disposing);
//    }

//    #region Windows Form Designer generated code

//    /// <summary>
//    /// Required method for Designer support - do not modify
//    /// the contents of this method with the code editor.
//    /// </summary>
//    private void InitializeComponent() {
//        progressBar = new ProgressBar();
//        CancelExportingButton = new Button();
//        statusLabel = new Label();
//        SuspendLayout();
//        // 
//        // progressBar
//        // 
//        progressBar.Location = new Point(12, 37);
//        progressBar.Name = "progressBar";
//        progressBar.Size = new Size(255, 23);
//        progressBar.TabIndex = 2;
//        // 
//        // CancelExportingButton
//        // 
//        CancelExportingButton.Location = new Point(98, 67);
//        CancelExportingButton.Name = "CancelExportingButton";
//        CancelExportingButton.Size = new Size(75, 23);
//        CancelExportingButton.TabIndex = 3;
//        CancelExportingButton.Text = "Cancel";
//        CancelExportingButton.UseVisualStyleBackColor = true;
//        CancelExportingButton.Click += CancelExportingButton_Click;
//        // 
//        // statusLabel
//        // 
//        statusLabel.AutoSize = true;
//        statusLabel.Location = new Point(117, 9);
//        statusLabel.Name = "statusLabel";
//        statusLabel.Size = new Size(69, 15);
//        statusLabel.TabIndex = 4;
//        statusLabel.Text = "exporting ...";
//        // 
//        // ExportProgressPopup
//        // 
//        ClientSize = new Size(279, 102);
//        Controls.Add(statusLabel);
//        Controls.Add(CancelExportingButton);
//        Controls.Add(progressBar);
//        Name = "ExportProgressPopup";
//        Load += ExportProgressPopup_Load;
//        ResumeLayout(false);
//        PerformLayout();
//    }

//    #endregion

//    private ProgressBar progressBar;
//    private Button CancelExportingButton;
//    private Label statusLabel;
//}