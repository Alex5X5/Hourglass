namespace Hourglass.Util;

using System;
using System.IO;
using System.Reflection;
using System.Text;

public static class Paths {

	public static void ExtractFiles(string resourceNamespacePrefix) {
		// Get executing assembly
		var assembly = Assembly.GetCallingAssembly();
		var resourceNames = assembly.GetManifestResourceNames();

		// Determine output path next to the running .exe
		string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

		for(int i=0; i<resourceNames.Length; i++) {
			// Filter resources inside the specified namespace
			if (!resourceNames[i].StartsWith(resourceNamespacePrefix))
				continue;
			string resourceName = resourceNames[i];
			while (resourceName.Split('.').Length>2) {
				resourceName = new StringBuilder(resourceName)
					.Insert(resourceName.IndexOf('.')+1, '\\')
						.Remove(resourceName.IndexOf('.'), 1)
							.ToString();
			}
			resourceName=exeDirectory+resourceName;
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

	public static string AssetsPath(string fileName) {
		string assemplyName = Assembly.GetExecutingAssembly().GetName().Name+".dll";
		string assemblyLocation = Assembly.GetExecutingAssembly().Location;
		string trimedName = assemblyLocation.Trim(assemplyName.ToCharArray());
		trimedName = Path.Combine(trimedName+ @"\Hourglass\Assets\", fileName);
		//return Path.Combine(trimedName, fileName);
		return trimedName;
	} 
}
