namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;

public abstract class GraphPanelViewModelBase : ViewModelBase {

	public IHourglassDbService dbService { set; get; }
	public DateTimeService dateTimeService { set; get; }
	public TimerCacheService cacheService;

	public GraphPageViewModel panelController;
	protected MainViewModel pageController;

	public string Title => GetTitle();

	public GraphPanelViewModelBase() : this(null, null, null, null, null) {
		
	}
	
	public GraphPanelViewModelBase(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, TimerCacheService cacheService) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.panelController = panelController;
		this.pageController = pageController;
		this.cacheService = cacheService;
	}

	public abstract Task<List<Database.Models.Task>> GetTasksAsync();

	public virtual void OnClick(Database.Models.Task task) {
		cacheService.SelectedTask = task;
		pageController.GoToTaskdetails(task);
	}

	public abstract void OnDoubleClick(DateTime clickedTime);

	protected abstract string GetTitle();

	//public async void OnClickBase(Avalonia.Point mousePos, int xAxisSegmentCount, int xAxisSegmentDuration) {
	//	Console.WriteLine("base graph panel model click");
	//	List<Database.Models.Task>? tasks = await GetTasksAsync();
	//	if (controller is GraphPanelViewBase view) {
	//		bool taskClicked = false;
	//		int i = 0;
	//		foreach (Database.Models.Task task in tasks) {
	//			view.GetTaskRectanlge(task, view.GRAPH_CLICK_ADDITIONAL_WIDTH, view.GRAPH_CLICK_ADDITIONAL_HEIGHT, i).Contains(mousePos);
	//			if (taskClicked) {
	//				//TaskDetails.TaskDetailsPopup taskDetailsWindow = new(task, _dbService, _parent);
	//				//taskDetailsWindow.ShowDialog();
	//				Console.WriteLine("detected task!");
	//				break;
	//			}
	//			i++;
	//		}
	//	}
	//}

}
	