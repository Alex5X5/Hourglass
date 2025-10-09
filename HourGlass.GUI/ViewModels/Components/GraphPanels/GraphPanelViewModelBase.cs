namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;

public abstract class GraphPanelViewModelBase : ViewModelBase {

	protected GraphPageViewModel? graphPageViewModel;

	public GraphPanelViewModelBase() : this(null, null, null) {

	}

	public GraphPanelViewModelBase(MainViewModel? controller, GraphPageViewModel? panelController, IServiceProvider? services) : base(controller, services) {
		graphPageViewModel = panelController;
	}

	public async virtual Task<List<Database.Models.Task>> GetTasksAsync() =>
		await dbService.QueryTasksAsync() ?? [];

	public abstract void OnClick(Database.Models.Task task);

	public abstract void OnDoubleClick(DateTime clickedTime);

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
	