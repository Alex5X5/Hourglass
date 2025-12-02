namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;
using Hourglass.GUI.Services;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

public partial class ExportPageViewModel : PageViewModelBase {

	private readonly DateTimeService? dateTimeService;
	CacheService cacheService;
	private readonly IPdfService? pdf;
	private readonly MainViewModel pageController;

    public override string Title => "Export";

	private PdfDocumentData pdfData;
	public ObservableCollection<TextboxItem> TableItems { get; set; }

	public string JobNameText => pdfData.JobName;
	public string UseNameText => pdfData.UserName;

	public string DateFromText => pdfData.DateFrom;
	public string DateToText => pdfData.DateTo;
	public string WeekCount => pdfData.Week;

	public string TotalTime => pdfData.TotalTime;

	private Action<Database.Models.Task> OnTextBockClick;

    public ExportPageViewModel() : this(null) {
		
	}

	public ExportPageViewModel(DateTimeService? dateTimeService) : this(dateTimeService, null, null, null) {
		this.dateTimeService = dateTimeService;
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IPdfService? pdf, MainViewModel pageController, CacheService cacheService) : base() {
		this.dateTimeService = dateTimeService;
		this.pdf = pdf;
		this.pageController = pageController;
		this.cacheService = cacheService;
		OnTextBockClick =
			t => {
				cacheService.SelectedTask = t;
				pageController.GoToTaskdetails(t);
			};
		pdfData = pdf?.GetExportData(cacheService.SelectedDay) ?? new PdfDocumentData();
		TableItems = [];
		for(int day = 0; day < 5; day++)
			for (int i = 0; i < PdfDocumentData.DAY_LINE_COUNT; i++) {
				int line = day * PdfDocumentData.DAY_LINE_COUNT + i;
                TableItems.Add(new DescriptionItem { RowIndex = line, Text = pdfData.Data[line].Item1, Task = pdfData.Data[line].Item4 });
				TableItems.Add(new HourItem { RowIndex = line, Text = pdfData.Data[line].Item2, Task = pdfData.Data[line].Item4 });
				TableItems.Add(new HourRangeItem { RowIndex = line, Text = pdfData.Data[line].Item3, Task = pdfData.Data[line].Item4 });
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
				pdf?.Export(cacheService.SelectedDay);
			}
		).Start();
    }

    [RelayCommand]
    private void OpenExplorer() {
        Console.WriteLine("export button click!");
		Process.Start("explorer.exe", PathService.FilesPath(@"Nachweise\"));
    }

    public void OnTaskRedirect(Database.Models.Task task) {
		cacheService.SelectedTask = task;
		pageController.ChangePage<TaskDetailsPageViewModel>();
		Console.WriteLine($"redirect event for task {task}");
	}
}

public abstract partial class TextboxItem {
	public int RowIndex { get; set; } = 0;

	public abstract int ColumnIndex { get; }

	public string Text { get; set; } = "";

	public Database.Models.Task Task {
		set;
		get;
	}
}

public class DescriptionItem : TextboxItem {
	public override int ColumnIndex => 0;

}

public class HourItem : TextboxItem {
	public override int ColumnIndex => 1;

}

public class HourRangeItem : TextboxItem {
	public override int ColumnIndex => 2;
}