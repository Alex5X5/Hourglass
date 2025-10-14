namespace Hourglass;

using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI;
using Hourglass.PDF;
using Hourglass.Util;
using Hourglass.Util.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

public class Program {
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	public static void Main(string[] args) {

#if PUBLISHED
		InstallerService.CheckIsInAppdata();
#endif
        PathService.PrintDetailedInfo();
		PathService.ExtractFiles("Hourglass");

		BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

		//EncryptionService service = new("test"); 
		//service.EncryptFile(PathService.FilesPath("database"));

		//Application.Run(new Hourglass.GUI.Pages.LoginPopup.LoginPopup());

	}

	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI();
}