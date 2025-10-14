namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;
using Hourglass.Util;

public partial class ExportPageViewModel : PageViewModelBase {

	private readonly DateTimeService? dateTimeService;
	private readonly IPdfService? pdf;

	public ExportPageViewModel() : this(null, null) {
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IHourglassDbService? dbService) : base() {
		this.dateTimeService = dateTimeService;
	}

	public ExportPageViewModel(DateTimeService? dateTimeService, IHourglassDbService? dbService, IPdfService pdf) : base() {
		this.dateTimeService = dateTimeService;
		this.pdf = pdf;
	}

	[RelayCommand]
	private void Import() {
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