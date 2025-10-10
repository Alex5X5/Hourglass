namespace Hourglass.GUI.ViewModels.Pages;

using Avalonia.Controls;

using CommunityToolkit.Mvvm.Input;
using Hourglass.PDF;

public partial class ExportPageViewModel : PageViewModelBase {

	private readonly PdfService? pdf;

	public ExportPageViewModel() : this(null, null) {
	}

	public ExportPageViewModel(MainViewModel? controller, IServiceProvider? services) : base(controller, services) {
		if(!Design.IsDesignMode)
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
				pdf?.Export(dateTimeService?.SelectedDay ?? DateTime.Now);
			}
		).Start();
	}
}