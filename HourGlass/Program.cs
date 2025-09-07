namespace HourGlass;

using Hourglass.Database.Services.Interfaces;
using Hourglass.Database.Services;
using Hourglass.PDF;
using Hourglass.Util.Services;

public class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main() {
		PathService.PrintDetailedInfo();
		PathService.ExtractFiles("Hourglass");
		//EncryptionService service = new("test"); 
		//service.EncryptFile(PathService.FilesPath("database"));

		//Application.Run(new Hourglass.GUI.Pages.LoginPopup.LoginPopup());
		IHourglassDbService dbService = new HourglassDbService();
		Application.Run(new GUI.Pages.Timer.TimerWindow(dbService));
        
    }
}