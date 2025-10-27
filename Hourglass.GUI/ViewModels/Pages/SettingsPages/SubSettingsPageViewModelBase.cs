namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.Util;
using Hourglass.Util.Services;

public abstract class SubSettingsPageViewModelBase : ViewModelBase {

	protected IHourglassDbService dbService { set; get; }
    protected DateTimeService dateTimeService { set; get; }
    protected CacheService cacheService;
	protected SettingsService settingsService;

	protected SettingsPageViewModel settingsController;
	protected MainViewModel pageController;

	public abstract string Title { get; }

	public SubSettingsPageViewModelBase() : this(null, null, null, null, null) {
		
	}
	
	public SubSettingsPageViewModelBase(DateTimeService dateTimeService, SettingsPageViewModel settingsController, MainViewModel pageController, CacheService cacheService, SettingsService settingsService) : base() {
        this.dateTimeService = dateTimeService;
		this.settingsController = settingsController;
		this.pageController = pageController;
		this.cacheService = cacheService;
		this.settingsService = settingsService;
	}
}
	