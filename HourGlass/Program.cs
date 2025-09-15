namespace HourGlass;

using Hourglass.Database.Services.Interfaces;
using Hourglass.Database.Services;
using Hourglass.PDF;
using Hourglass.Util.Services;
using Hourglass.Util;

public class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main() {

		//int i = 1;
		//DateTime n = DateTimeService.START_DATE;
		//while (n <= DateTime.Now) {
		//	Console.WriteLine($"{i}st week starts at {n}");
		//	i++;
		//	n = n.AddDays(7);
		//}
		//return;

		PathService.PrintDetailedInfo();
		PathService.ExtractFiles("Hourglass");
		//EncryptionService service = new("test"); 
		//service.EncryptFile(PathService.FilesPath("database"));

		//Application.Run(new Hourglass.GUI.Pages.LoginPopup.LoginPopup());
		IHourglassDbService dbService = new HourglassDbService();
		Application.Run(new GUI.Pages.Timer.TimerWindow(dbService));
        
    }
}