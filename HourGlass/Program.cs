namespace Hourglass;

using Avalonia;
using Avalonia.ReactiveUI;

using Hourglass.GUI;

public class Program {
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	public static void Main(string[] args) =>
		BuildMainApp()
			.StartWithClassicDesktopLifetime(args);

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