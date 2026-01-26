namespace Hourglass;

using Avalonia;
using Avalonia.ReactiveUI;

using Hourglass.GUI;
using Hourglass.Util.Services;
using System.Globalization;

public class Program {
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	public static void Main(string[] args) {

        PathService.PrintDetailedInfo();
		PathService.ExtractFiles("Hourglass");

		BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

		//EncryptionService service = new("test"); 
		//service.EncryptFile(PathService.FilesPath("database"));
	}

	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI();
}