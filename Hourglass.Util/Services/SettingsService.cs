namespace Hourglass.Util.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class SettingsService {

    public static event Action OnSettingsReload = () => { };

    private const string FILE_NAME = "appsettings.yml";
    
    public const string USER_NAME_KEY = "name";
    public const string JOB_NAME_KEY = "job";
    public const string START_DATE_KEY = "date";

    private static bool loaded = false;

    private static Dictionary<string, string> Settings = LoadSettings();

    private static Dictionary<string, string> LoadSettings() {
        loaded = true;
        Dictionary<string, string> res = [];
        using FileStream fileHandle = File.Open(PathService.FilesPath(FILE_NAME), FileMode.OpenOrCreate);
        using StreamReader streamReader = new(fileHandle);
        string[] lines = streamReader.ReadToEnd().Split('\n');
        foreach (string line in lines) {
            string[] keyvaluePair = line.Split(':');
            if (keyvaluePair.Length == 2)
                res[keyvaluePair[0]] = keyvaluePair[1];
            if (keyvaluePair.Length > 2)
                res[keyvaluePair[0]] = string.Join(':', keyvaluePair.Skip(1));
        }
        return res;
    }

    public static void SaveSettings() {
        string[] lines = new string[Settings.Count];
        int i = 0;
        foreach (string key in Settings.Keys) {
            lines[i] = key + ":" + Settings[key];
            i++;
        }
        string res = string.Join("\n", lines);
        string path = PathService.FilesPath(FILE_NAME);
        if (!File.Exists(path))
            File.Create(path);
        using FileStream fileHandle = File.Open(path, FileMode.Truncate);
        using StreamWriter streamWriter = new(fileHandle);
        streamWriter.Write(res);
    }

    public static string GetSetting(string key) {
        Settings.TryGetValue(key, out string? setting);
        return setting ?? "";
    }

    public static string? TryGetSetting(string key) {
        Settings.TryGetValue(key, out string? setting);
        return setting;
    }

    public static void SetSetting(string key, string newValue) {
        Settings[key] = newValue;
    }

    public static void ReloadSettings() {
        SaveSettings();
        Settings = LoadSettings();
        foreach (Delegate act in OnSettingsReload.GetInvocationList())
            try {
                act.DynamicInvoke();
            } catch (Exception ex){
                Console.WriteLine("an error occurred while invoking settings reload subscribers: "+ex.Message);
            }
    }
}
