namespace Hourglass.GUI.Pages.ExportProgressPopup;

using Hourglass.Database.Services.Interfaces;
using HourGlass.GUI.Pages.Timer;
using Microsoft.AspNetCore.Http.HttpResults;

public partial class ExportProgressPopup : Form {

    private bool isCancelled = false;
    private bool isFinished = false;
    public bool IsCancelled => isCancelled;

    public ExportProgressPopup() {
        InitializeComponent();
    }

    protected override void OnFormClosing(FormClosingEventArgs e) {
        if (!isCancelled && !isFinished) {
            e.Cancel = true; // Prevent closing unless cancelled
        }
        base.OnFormClosing(e);
    }

    private void ExportProgressPopup_Load(object sender, EventArgs e) {

    }

    public void UpdateProgress(int percentage, string? status = null) {
        if (InvokeRequired) {
            Invoke(new Action<int, string>(UpdateProgress), percentage, status);
            return;
        }
        int _percentage = Math.Max(0, Math.Min(100, percentage));
        progressBar.Value = _percentage;
        if (!string.IsNullOrEmpty(status)) {
            statusLabel.Text = status;
        }
        if (percentage >= 100)
            OnFinished();
    }

    private void OnFinished() {
        isFinished = true;
        progressBar.Visible = false;
        CancelExportingButton.Text = "Ok";
        //Thread.Sleep(1000);
    }

    private void CancelExportingButton_Click(object sender, EventArgs e) {
        if (isFinished) {
            Close();
            return;
        }
        isCancelled = true;
        CancelExportingButton.Enabled = false;
        statusLabel.Text = "Cancelling...";
    }
}
