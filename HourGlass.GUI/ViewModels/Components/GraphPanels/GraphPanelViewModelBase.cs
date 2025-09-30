using Hourglass.Database.Services.Interfaces;

namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

public abstract class GraphPanelViewModelBase : ViewModelBase {
	public IHourglassDbService? dbService;
}
