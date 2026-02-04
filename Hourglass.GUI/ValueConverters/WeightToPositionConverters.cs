namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;
using Hourglass.Database.Models;
using Hourglass.GUI.ViewModels.Components;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using System.Threading.Tasks;

public static class WeightToPositionConverters {

	public static FuncMultiValueConverter<IList<object?>, double> WidthConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
				Rect parentBounds = (lst.ToArray<object?>()[0] as Rect?) ?? new Rect(0,0,10,10);
				GraphPanelViewModelBase? panelModel = lst.ToArray<object?>()[1] as GraphPanelViewModelBase;
				TaskGraphViewModel? taskModel = lst.ToArray<object?>()[2] as TaskGraphViewModel;
				double proportion = parentBounds.Width / panelModel?.TIME_INTERVALL_START_SECONDS ?? 1;
				double paddingX = parentBounds.Width * GraphPanelViewModelBase.PADDING_X_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_X_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_X_WEIGHT);
				return (taskModel?.task.start ?? 0 - panelModel?.TIME_INTERVALL_START_SECONDS ?? 0) * proportion + paddingX;
			}
		);

	public static FuncMultiValueConverter<IList<object?>, double> HeightConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                Rect parentBounds = (lst.ToArray<object?>()[0] as Rect?) ?? new Rect(0, 0, 10, 10);
                GraphPanelViewModelBase? panelModel = lst.ToArray<object?>()[1] as GraphPanelViewModelBase;
                double paddingY = parentBounds.Height * GraphPanelViewModelBase.PADDING_Y_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_Y_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_Y_WEIGHT);
                double graphAreaHeight = parentBounds.Height - 2* paddingY;
                return graphAreaHeight / (panelModel?.Y_AXIS_SEGMENT_COUNT ?? 1 * 1.5) * panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1;
            }
        );

	public static FuncMultiValueConverter<IList<object?>, double> TopConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                Rect parentBounds = (lst.ToArray<object?>()[0] as Rect?) ?? new Rect(0, 0, 10, 10);
                GraphPanelViewModelBase? panelModel = lst.ToArray<object?>()[1] as GraphPanelViewModelBase;
                TaskGraphViewModel? taskModel = lst.ToArray<object?>()[2] as TaskGraphViewModel;
                double paddingY = parentBounds.Height * GraphPanelViewModelBase.PADDING_Y_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_Y_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_Y_WEIGHT);
                double graphAreaHeight = parentBounds.Height - 2 * paddingY;
                double yAxissegmentSize = graphAreaHeight / (panelModel?.Y_AXIS_SEGMENT_COUNT ?? 1 * 1.5) * panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1;
                return yAxissegmentSize * (taskModel?.index ?? 0 % (panelModel?.MAX_TASKS ?? 10 / panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1)) * 1.5 + paddingY;            }
        );

	public static FuncMultiValueConverter<IList<object?>, double> LeftConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                Rect parentBounds = (lst.ToArray<object?>()[0] as Rect?) ?? new Rect(0, 0, 10, 10);
                GraphPanelViewModelBase? panelModel = lst.ToArray<object?>()[1] as GraphPanelViewModelBase;
                TaskGraphViewModel? taskModel = lst.ToArray<object?>()[2] as TaskGraphViewModel;
                double proportion = parentBounds.Width / panelModel?.TIME_INTERVALL_START_SECONDS ?? 1;
                double paddingX = parentBounds.Width * GraphPanelViewModelBase.PADDING_X_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_X_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_X_WEIGHT);
                return (taskModel?.task.start ?? 0 - panelModel?.TIME_INTERVALL_START_SECONDS ?? 0) * proportion + paddingX;
            }
		);
}