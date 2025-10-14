using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Hourglass.Util.Services;

public class InstallerService {

	public static void CheckIsInAppdata() {
		Console.WriteLine("check is in appdata");
        string expetedPath = Path.Combine(PathService.APP_DATA_DIRECTORY, PathService.GetMainEntryPointFileName());
		string actualPath = PathService.GetMainEntryPointPath();
		if (!expetedPath.Equals(actualPath)) {
			try {
				string filePath = PathService.AppDataFilesPath("last_exec_path.txt");
				if (!Directory.Exists(Path.GetDirectoryName(filePath)))
					Directory.CreateDirectory(Path.GetDirectoryName(filePath));
				if (!File.Exists(filePath))
					File.Create(filePath);
				using (FileStream file = File.OpenWrite(filePath))
				using (StreamWriter writer = new(file)) {
					writer.Write(PathService.GetMainEntryPointPath());
					writer.Flush();
				}
				CopyApplicationAndCreateShortcut();
			} catch (IOException e) {
				Console.WriteLine(e.ToString());
			}
		} else
			ClearInstallDirectoryLeftovers();
	}

	public static void ClearInstallDirectoryLeftovers() {
		string infoFilePath = PathService.AppDataFilesPath("last_exec_path");
		if (File.Exists(infoFilePath)) {
			string[] lines = File.ReadAllLines(infoFilePath);
			string oldExecutionDirectory = Path.GetDirectoryName(lines[0]);
			if (File.Exists(oldExecutionDirectory)) {
				File.Delete(oldExecutionDirectory);
				File.Delete(infoFilePath);
			}

        }

    }

	public static void CopyApplicationAndCreateShortcut() {
        string targetDirectory = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"Hourglass"
		);
		string targetExePath = Path.Combine(targetDirectory, PathService.GetMainEntryPointFileName());
		try {
			// Create the directory if it doesn't exist
			if (!Directory.Exists(targetDirectory)) {
				Directory.CreateDirectory(targetDirectory);
				Console.WriteLine($"Created directory: {targetDirectory}");
			}

			// Copy the executable to the target location
			File.Copy(PathService.GetMainEntryPointPath(), targetExePath, overwrite: true);
			Console.WriteLine("Application copied successfully!");


			string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

			using (StreamWriter writer = new StreamWriter(deskDir + "\\" + "Hourglass.exe.url")) {
				writer.WriteLine("[InternetShortcut]");
				writer.WriteLine("URL=file:///" + targetExePath);
				writer.WriteLine("IconIndex=0");
				string icon = targetExePath.Replace('\\', '/');
				writer.WriteLine("IconFile=" + icon);
			}

			// Optionally, start the copied application
			//Console.WriteLine("Starting application from new location...");
			Process.Start(targetExePath);

			// Exit this instance
			Environment.Exit(0);
		} catch (Exception ex) {
			Console.WriteLine($"Error copying application: {ex.Message}");
		}
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
