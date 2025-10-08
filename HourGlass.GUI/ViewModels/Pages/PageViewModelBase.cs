using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Views;

namespace Hourglass.GUI.ViewModels.Pages;

public abstract class PageViewModelBase : ViewModelBase {

	public Database.Models.Task? RunningTask;
	public IHourglassDbService? dbService;


	public PageViewModelBase() : this(null, null) {
	}

	public PageViewModelBase(ViewBase? owner, IServiceProvider? services) : base(owner, services) {
		dbService = (IHourglassDbService?)services?.GetService(typeof(HourglassDbService));
		RunningTask = dbService?.QueryCurrentTaskAsync().Result;
	}

}
