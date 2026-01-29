namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using System.ComponentModel;

public partial class GeneralSubSettingsPageViewModel : SubSettingsPageViewModelBase {
    
    private string selectedLanguage;
    public string SelectedLanguage {
        get => selectedLanguage;
        set {
            if (value != selectedLanguage)
                HasUnsavedChanges = true;
            selectedLanguage = value;
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
        OnPropertyChanged(nameof(SelectedLanguage));
    }

    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

	public void OnLoad() {
		Console.WriteLine("loading General Sub Settings Page!");
		AllBindingPropertiesChanged();
	}
    public override void SaveSettings() {
        Console.WriteLine("[General]:save button click!");
        settingsService.Language = selectedLanguage;
        AllBindingPropertiesChanged();
    }
}