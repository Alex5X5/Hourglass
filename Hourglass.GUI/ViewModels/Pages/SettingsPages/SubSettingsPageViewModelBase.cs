namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.Util;

public abstract class SubSettingsPageViewModelBase : ViewModelBase {

	public IHourglassDbService dbService { set; get; }
	public DateTimeService dateTimeService { set; get; }
	public CacheService cacheService;

	protected SettingsPageViewModel settingsController;
	protected MainViewModel pageController;

	public abstract string Title { get; }

	public SubSettingsPageViewModelBase() : this(null, null, null, null, null) {
		
	}
	
	public SubSettingsPageViewModelBase(IHourglassDbService dbService, DateTimeService dateTimeService, SettingsPageViewModel settingsController, MainViewModel pageController, CacheService cacheService) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.settingsController = settingsController;
		this.pageController = pageController;
		this.cacheService = cacheService;
	}
}
	