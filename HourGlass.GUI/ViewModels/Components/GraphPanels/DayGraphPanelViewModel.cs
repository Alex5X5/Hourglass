using Avalonia.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;

namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

public class DayGraphPanelViewModel : GraphPanelViewModelBase {

	public DayGraphPanelViewModel() : this(null, null) {
		
	}

	public DayGraphPanelViewModel(ViewBase? model, IServiceProvider? services) : base(model, services) {
		
	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync()=>
		dbService != null ? await dbService.QueryTasksAsync() : [];

	public override void OnClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("day graph panel click");
	}

	public override void OnDoubleClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("day graph panel double click");
	}
}