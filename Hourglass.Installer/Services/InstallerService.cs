using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Hourglass.Util.Services;

public class InstallerService {

	public static bool IsInstalled() {
        string expetedPath = Path.Combine(PathService.APP_DATA_DIRECTORY, PathService.GetMainEntryPointFileName());
        string actualPath = PathService.GetMainEntryPointPath();
		return expetedPath.Equals(actualPath);
    }

	public static async Task CheckIsInAppdata() {
		Console.WriteLine("check is in appdata");
		if (!IsInstalled()) {
			await InstallAsync(null);
		} else
			ClearInstallDirectoryLeftovers();
	}

	public static async Task InstallAsync(IProgress<int>? progress) {
        string filePath = PathService.AppDataFilesPath("last_exec_path.txt");
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        if (!File.Exists(filePath))
                File.Create(filePath);
        progress?.Report(10);
        using (FileStream file = File.OpenWrite(filePath))
        using (StreamWriter writer = new(file)) {
            await writer.WriteAsync(PathService.GetMainEntryPointPath());
            await writer.FlushAsync();
        }
		await Task.Delay(5000);
        string targetExePath = await CopyApplicationAndCreateShortcutAsync(progress);
		progress?.Report(100);
		Console.WriteLine($"Starting Process from File:{targetExePath}");
			Process.Start(targetExePath);
            Environment.Exit(0);
    }

	private static void ClearInstallDirectoryLeftovers() {
        Console.WriteLine("clear leftovers");
        string infoFilePath = PathService.AppDataFilesPath("last_exec_path");
		if (File.Exists(infoFilePath)) {
			string[] lines = File.ReadAllLines(infoFilePath);
			string oldExecutionDirectory = Path.GetDirectoryName(lines[0]);
			if (File.Exists(oldExecutionDirectory)) {
				File.Delete(oldExecutionDirectory);
			}
        }
    }

	private static void CopyApplicationAndCreateShortcut() =>
		CopyApplicationAndCreateShortcutAsync(null).RunSynchronously();
	
    private static async Task<string> CopyApplicationAndCreateShortcutAsync(IProgress<int>? progress) {
		string targetDirectory = PathService.APP_DATA_DIRECTORY;
		string targetExePath = PathService.AppDataFilesPath(PathService.GetMainEntryPointFileName());
        try {
			// Create the directory if it doesn't exist
			if (!Directory.Exists(targetDirectory)) {
				Directory.CreateDirectory(targetDirectory);
				Console.WriteLine($"Created directory: {targetDirectory}");
			}

			using (StreamWriter writer = new StreamWriter(PathService.DesktopPath("Hourglass.exe.url"))) {
				writer.WriteLine("[InternetShortcut]");
				writer.WriteLine("URL=file:///" + targetExePath);
				writer.WriteLine("IconIndex=0");
				string icon = targetExePath.Replace('\\', '/');
				writer.WriteLine("IconFile=" + icon);
			}

			progress?.Report(20);

            // Copy the executable to the target location
            File.Copy(PathService.GetMainEntryPointPath(), targetExePath, overwrite: true);
			Console.WriteLine("Application copied successfully!");
			progress?.Report(50);
			return targetExePath;
		} catch (Exception ex) {
			Console.WriteLine($"Error copying application: {ex.Message}");
		}
		return "";
	}

	public static void ExtractFiles(string resourceNamespacePrefix) {
		// Get executing assembly
		var assembly = Assembly.GetCallingAssembly();
		var resourceNames = assembly.GetManifestResourceNames();

		// Determine output path next to the running .exe
		string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

		for (int i = 0; i < resourceNames.Length; i++) {
			// Filter resources inside the specified namespace
			if (!resourceNames[i].StartsWith(resourceNamespacePrefix))
				continue;
			string resourceName = resourceNames[i];
			while (resourceName.Split('.').Length > 2) {
				resourceName = new StringBuilder(resourceName)
					.Insert(resourceName.IndexOf('.') + 1, '\\')
						.Remove(resourceName.IndexOf('.'), 1)
							.ToString();
			}
			resourceName = exeDirectory + resourceName;
			if (File.Exists(resourceName))
				continue;
			Directory.CreateDirectory(Path.GetDirectoryName(resourceName)!);
			using Stream? resourceStream = assembly.GetManifestResourceStream(resourceNames[i]);
			if (resourceStream == null) {
				Console.WriteLine($"Failed to load resource: {resourceName}");
				continue;
			}

			using FileStream fileStream = new FileStream(resourceName, FileMode.Create, FileAccess.Write);
			resourceStream.CopyTo(fileStream);
			Console.WriteLine($"Extracted: {resourceName}");

		}
	}
}
