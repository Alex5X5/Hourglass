namespace Hourglass.Util.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Hourglass.Util.Attributes;

public class TranslatorService {

	public static readonly TranslatorService Singleton;

	private readonly YmlReader ymlReader;
	private readonly Dictionary<string, string> Languages;

	private string _currentLanguageName = "";
	public string CurrentLanguageName {
		set => ChangeLanguage(value);
		get => _currentLanguageName;
	}

	public string? this[string index] => ymlReader[index];

	static TranslatorService() {
		Singleton = new TranslatorService();
	}

	private TranslatorService() {
		ymlReader = new();
		Languages = [];
		foreach (string path in Directory.GetFiles(PathService.LANGUAGES_DIRECTORY))
			Languages[Path.GetFileNameWithoutExtension(path)] = path;
		CurrentLanguageName = Languages.Keys.ToList()[0];
	}

	private void ChangeLanguage(string value) {
		if(Languages.TryGetValue(value, out var filePath)) {
			_currentLanguageName = value;
			ymlReader.ReadFromFile(filePath);
		}
	}

	public void TranslateAnnotatedMembers(object obj) {
		Type objectType = obj.GetType();
		foreach (PropertyInfo property in objectType.GetProperties()) {
			Attribute? propertyAttribute = property.GetCustomAttributes()
				.FirstOrDefault(x=> x.GetType() == typeof(TranslateMember));
			if (propertyAttribute is TranslateMember translateAttribute) {
				if (this[translateAttribute.TranslationKey] is string translatedValue) {
					property.SetValue(obj, translatedValue);
				} else {
					property.SetValue(obj, translateAttribute.FallbackValue);
                }
            }
		}
	}
}
