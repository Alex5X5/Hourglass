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
		PrintService ps = new PrintService();
		ps.Print(PathService.AssetsPath("output-readable-indexers.pdf"));

		BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

		//EncryptionService service = new("test"); 
		//service.EncryptFile(PathService.FilesPath("database"));

		//Application.Run(new Hourglass.GUI.Pages.LoginPopup.LoginPopup());

	}


	//#if PUBLISHED
	//		InstallerService.CheckIsInAppdata();
	//		if (!InstallerService.IsInstalled())
	//			BuildInstallerApp()
	//				.StartWithClassicDesktopLifetime(args);
	//		else
	//#endif
	//			BuildMainApp()
	//				.StartWithClassicDesktopLifetime(args);
	//}


	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI();

	public static AppBuilder BuildMainApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace()
			.UseReactiveUI();
	
	//public static AppBuilder BuildInstallerApp()
	//	=> AppBuilder.Configure<InstallerApp>()
	//		.UsePlatformDetect()
	//		.WithInterFont()
	//		.LogToTrace()
	//		.UseReactiveUI();
}