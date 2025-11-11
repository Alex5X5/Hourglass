namespace Hourglass.GUI.ViewModels.Pages.SettingsPages;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.Util;

public abstract class SubSettingsPageViewModelBase : ViewModelBase {

	protected IHourglassDbService dbService { set; get; }
    protected DateTimeService dateTimeService { set; get; }
	protected SettingsService settingsService;


	protected MainViewModel pageController;

	public abstract string Title { get; }

	public SubSettingsPageViewModelBase() : this(null, null, null) {
		
	}
	
	public SubSettingsPageViewModelBase(DateTimeService dateTimeService, MainViewModel pageController, SettingsService settingsService) : base() {
        this.dateTimeService = dateTimeService;
		//this.settingsController = settingsController;
		this.pageController = pageController;
		this.settingsService = settingsService;
	}
}
	