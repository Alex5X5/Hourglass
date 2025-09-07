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
		//ConfigurationManager configuration = (ConfigurationManager)new ConfigurationBuilder()
		//	.SetBasePath(Directory.GetCurrentDirectory())
		//		.AddJsonFile(Paths.AssetsPath("settings.json"), false, true)
		//			;
		//.AddJsonFile(Paths.AssetsPath($"settings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json"), false, true)
		//.AddEnvironmentVariables()
		//var migrationName = "InitialCreate"; // or any name you want
		//var process = new Process {
		//	StartInfo = new ProcessStartInfo {
		//		FileName = "dotnet",
		//		Arguments = $"ef migrations add {migrationName} " +
		//					"--project ../Hourglass.Database/Hourglass.Database.csproj " +
		//					"--startup-project ../Hourglass/Hourglass.csproj " +
		//					"--output-dir Migrations",
		//		RedirectStandardOutput = true,
		//		RedirectStandardError = true,
		//		UseShellExecute = false,
		//		CreateNoWindow = true,
		//		WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "Hourglass.Database")
		//	}
		//};

		IHourglassDbService dbService = new HourglassDbService();

		//BasicDatabaseAcessor<Item> accessor = new("test.db", DatabasePathFormat.FileName, null);
		//FileManager.writeOutput("test");
		//ApplicationConfiguration.Initialize();

		//string document = FileManager.LoadInput();

		//document = Builder.SetAnnotaionValue(
		//	document,
		//	"moday_hour_range_1",
		//	"test"

		//);
		//document = Builder.SetFieldValue(
		//	document,
		//	"moday_hour_range_1",
		//	"mh2"

		//);

		//FileManager.WriteOutput(document);
		//Application.SetHighDpiMode(HighDpiMode.SystemAware);
		//Application.EnableVisualStyles();
		//Application.SetCompatibleTextRenderingDefault(false);

		Application.Run(new GUI.Pages.Timer.TimerWindow(dbService));
        
    }
}