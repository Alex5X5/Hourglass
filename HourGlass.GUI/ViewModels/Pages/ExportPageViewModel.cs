namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;
using Hourglass.Util;

using System.Collections.ObjectModel;

public partial class ExportPageViewModel : PageViewModelBase {

	private readonly DateTimeService? dateTimeService;
	private readonly IPdfService? pdf;
	public override string Title => "Export";

	private PdfDocumentData pdfData;
	public ObservableCollection<TextboxItem> TableItems { get; set; }

	public string DateFromText => pdfData.DateFrom;
	public string DateToText => pdfData.DateTo;
	public string WeekCount => pdfData.Week;

    public ExportPageViewModel() : this(null, null) {
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IHourglassDbService? dbService) : this(dateTimeService, dbService, null) {
		this.dateTimeService = dateTimeService;
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IHourglassDbService? dbService, IPdfService? pdf) : base() {
		this.dateTimeService = dateTimeService;
		this.pdf = pdf;
		pdfData = pdf?.GetExportData(dateTimeService?.SelectedDay ?? DateTime.Now) ?? new PdfDocumentData();
		TableItems = [];
		for(int day = 0; day < 5; day++)
			for (int i = 0; i < PdfDocumentData.DAY_LINE_COUNT; i++) {
				int line = day * PdfDocumentData.DAY_LINE_COUNT + i;
                TableItems.Add(new DescriptionItem { RowIndex = line, Text = pdfData.Data[line][PdfDocumentData.TASK_DESCRIPTION_COLUMN] });
				TableItems.Add(new HourItem { RowIndex = line, Text = pdfData.Data[line][PdfDocumentData.HOUR_COLUMN] });
				TableItems.Add(new HourRangeItem { RowIndex = line, Text = pdfData.Data[line][PdfDocumentData.HOUR_RANGE_COLUMN] });
			}
	}

	[RelayCommand]
	private async void Import() {
		//var topLevel = TopLevel.GetTopLevel();

		//// Start async operation to open the dialog.
		//var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
		//	Title = "Open Text File",
		//	AllowMultiple = false
		//});
		Console.WriteLine("import button click! (not yet implemented)");
	}

	[RelayCommand]
	private void Export() {
		Console.WriteLine("export button click!");
		new Thread(
			() => {
				pdf?.Export(dateTimeService?.SelectedDay ?? DateTime.Now);
			}
		).Start();
	}
}

public abstract class TextboxItem {
	public int RowIndex { get; set; } = 0;

	public abstract int ColumnIndex { get; }

	public string Text { get; set; } = "";
}

public class DescriptionItem : TextboxItem {
	public override int ColumnIndex => PdfDocumentData.TASK_DESCRIPTION_COLUMN + 1;

}

public class HourItem : TextboxItem {
	public override int ColumnIndex => PdfDocumentData.HOUR_COLUMN + 1;

}

public class HourRangeItem : TextboxItem {
	public override int ColumnIndex => PdfDocumentData.HOUR_RANGE_COLUMN + 1;

}