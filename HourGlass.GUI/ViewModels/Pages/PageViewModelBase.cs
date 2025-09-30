using Hourglass.Database.Services.Interfaces;

namespace Hourglass.GUI.ViewModels.Pages;

public abstract class PageViewModelBase : ViewModelBase {

	public IHourglassDbService? dbService;

}
