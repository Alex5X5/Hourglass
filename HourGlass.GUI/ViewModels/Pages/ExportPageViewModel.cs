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
	public ObservableCollection<TextboxItem> TextboxItems { get; set; }

	public ExportPageViewModel() : this(null, null) {
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IHourglassDbService? dbService) : this(dateTimeService, dbService, null) {
		this.dateTimeService = dateTimeService;
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IHourglassDbService? dbService, IPdfService? pdf) : base() {
		this.dateTimeService = dateTimeService;
		this.pdf = pdf;
		pdfData = pdf?.GetExportData(dateTimeService?.SelectedDay ?? DateTime.Now) ?? new PdfDocumentData();
		TextboxItems = [];
		for (int i = 0; i < pdfData.Data.Length; i++) {
			TextboxItems.Add(new TextboxItem { RowIndex = i+1, Text = pdfData.Data[i][0] });
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

public class TextboxItem {
	public int RowIndex { get; set; } = 0;
	public string Text { get; set; } = "";
}