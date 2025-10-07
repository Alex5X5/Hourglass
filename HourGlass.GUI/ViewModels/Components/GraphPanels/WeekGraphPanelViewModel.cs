using Avalonia.Input;

using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Components.GraphPanels;

namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

public class WeekGraphPanelViewModel : GraphPanelViewModelBase {

	public WeekGraphPanelViewModel() : this(null, null) {

	}

	public WeekGraphPanelViewModel(ViewBase? model, IServiceProvider? services) : base(model, services) {

	}

	public async override Task<List<Database.Models.Task>> GetTasksAsync() =>
		dbService != null ? await dbService.QueryTasksOfWeekAtDateAsync(dateTimeService.SelectedDay) : [];

	public override void OnClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("Week graph panel click");
	}

	public override void OnDoubleClick(object? sender, TappedEventArgs e) {
		Console.WriteLine("Week graph panel double click");
	}
}