namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using System.ComponentModel;

public partial class GeneralSubSettingsPageViewModel : SubSettingsPageViewModelBase {
    
    private string selectedLanguage;
    public string SelectedLanguage {
        get => selectedLanguage;
        set {
            selectedLanguage = value;
            OnSelectedLanguageChanged();
        }
    }
    public List<string> AvailableLanguages { get; set; }
    
    public override string Title => TranslatorService.Singleton["Views.Pages.Settings.General.Title"] ?? "General Settings";
        
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    public GeneralSubSettingsPageViewModel() : this(null, null, null) {
    }
    
    public GeneralSubSettingsPageViewModel(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base(dateTimeService, pageController, settingsService) {
        AvailableLanguages = TranslatorService.Singleton.AvailableTranslations.ToList();
        selectedLanguage = TranslatorService.Singleton.CurrentLanguageName;
        OnPropertyChanged(nameof(SelectedLanguage));
    }
    
    private void AllBindingPropertiesChanged() {
    }

    private void OnSelectedLanguageChanged() {
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

	public void OnLoad() {
		Console.WriteLine("loading Visuals Sub Settings Page!");
		AllBindingPropertiesChanged();
	}
    public override void SaveSettings() {
        Console.WriteLine("[General]:save button click!");
        settingsService.SetSetting(SettingsService.LANGUAGE_KEY, selectedLanguage);
        AllBindingPropertiesChanged();
    }
}