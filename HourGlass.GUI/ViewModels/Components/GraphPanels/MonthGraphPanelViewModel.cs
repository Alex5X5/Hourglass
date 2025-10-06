using Avalonia.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;

namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

public class MonthGraphPanelViewModel : GraphPanelViewModelBase {

	public MonthGraphPanelViewModel() : this(null, null) {

	}

	public MonthGraphPanelViewModel(ViewBase? model, IServiceProvider? services) : base(model, services) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksAsync() : [];

	public override void OnClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("Month Grpah panel click");
	}

	public override void OnDoubleClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("Month Grpah panel double click");
	}
}