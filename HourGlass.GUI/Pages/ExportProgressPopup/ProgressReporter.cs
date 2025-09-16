using Hourglass.PDF.Services.Interfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Hourglass.GUI.Pages.ExportProgressPopup;

public class ProgressReporter(ExportProgressPopup popup) : IProgressReporter {

    private readonly ExportProgressPopup popup = popup;

    public bool IsCancellationRequested => popup.IsCancelled;

    public void ReportProgress(int percentage, string? status = null) {
        popup.UpdateProgress(percentage, status);
    }

}
