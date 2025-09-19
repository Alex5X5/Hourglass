namespace Hourglass.Util.Services;

using System.Collections.Generic;
using System.IO;

public static class SettingsService {

    public static Dictionary<string, string> ReadAppSettings() {
        Dictionary<string, string> res = [];
        using FileStream fileHandle = File.Open(PathService.FilesPath("appsettings.dat"), FileMode.OpenOrCreate);
        using StreamReader streamReader = new StreamReader(fileHandle);
        string[] lines = streamReader.ReadToEnd().Split('\n');
        foreach (string line in lines) {
            string[] keyvaluePair = line.Split(':');
            if (keyvaluePair.Length == 2)
                res[keyvaluePair[0]] = keyvaluePair[1];
        }
        return res;
    }
}
