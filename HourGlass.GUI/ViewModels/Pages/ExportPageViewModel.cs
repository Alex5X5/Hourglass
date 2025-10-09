namespace Hourglass.GUI.ViewModels.Pages;

using CommunityToolkit.Mvvm.Input;

using Hourglass.GUI.Views;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;

public partial class ExportPageViewModel : PageViewModelBase {

	private readonly IPdfService pdf;

	public ExportPageViewModel() : this(null, null) {
	}

	public ExportPageViewModel(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		pdf = new PdfService(dbService);
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
				pdf.Export(dateTimeService?.SelectedDay ?? DateTime.Now);
			}
		).Start();
	}
}