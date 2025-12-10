using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hourglass.Util.Services;

public class TranslatorService {

    private YmlReader ymlReader;
    Dictionary<string, string> Languages;

    private string _currentLanguageName = "";
    public string CurrentLanguageName {
        set => ChangeLanguage(value);
        get => _currentLanguageName;
    }

    public string this[string index] => ymlReader[index];

    public TranslatorService() {
        ymlReader = new();
        Languages = [];
        foreach (string path in Directory.GetFiles(PathService.LANGUAGES_DIRECTORY))
            Languages[Path.GetFileNameWithoutExtension(path)] = path;
        CurrentLanguageName = Languages.Keys.First();
    }

    private void ChangeLanguage(string value) {
        if(Languages.TryGetValue(value, out var filePath)) {
            _currentLanguageName = value;
            ymlReader.ReadFromFile(filePath);
        }
    }
}
