namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;
using Hourglass.Database.Models;
using Hourglass.GUI.ViewModels.Components;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views.Components.GraphPanels;

public static class GridToPositionConverters {

	public static FuncMultiValueConverter<IList<object?>, double> WidthConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                TaskGraphViewModel? task = lst.ToArray<object?>()[0] as TaskGraphViewModel;
                GraphPanelViewBase? panel = lst.ToArray<object?>()[1] as GraphPanelViewBase;
                long start = task?.task.start ?? 0;
                long finish = task?.task.finish ?? 1;
                bool running = task?.task.running ?? false;
                double proportion = panel?.GRAPH_AREA_WIDTH ?? 1 / panel?.TIME_INTERVALL_DURATION ?? 1;
                double graphPosX = (start - panel?.TIME_INTERVALL_START_SECONDS ?? 1) * proportion + panel?.PADDING_X ?? 0;
                long duration = (task?.task.running ?? false) ? DateTimeService.ToSeconds(DateTime.Now) - (task?.task.start ?? 0) : (task?.task.finish ?? 1) - (task?.task.start ?? 0);
                double graphLength = duration * proportion;
                return Math.Max(graphLength, panel?.GRAPH_MINIMAL_WIDTH ?? 10) * 2;
            }
		);

	public static FuncMultiValueConverter<IList<object?>, double> HeightConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                TaskGraphViewModel? task = lst.ToArray<object?>()[0] as TaskGraphViewModel;
                GraphPanelViewBase? panel = lst.ToArray<object?>()[1] as GraphPanelViewBase;
                return panel?.Y_AXIS_SEGMENT_SIZE ?? 10;
            }
		);

    public static FuncMultiValueConverter<IList<object?>, double> XConverter { get; } =
        new FuncMultiValueConverter<IList<object?>, double>(
            (lst) => {
                TaskGraphViewModel? task = lst.ToArray<object?>()[0] as TaskGraphViewModel;
                GraphPanelViewBase? panel = lst.ToArray<object?>()[1] as GraphPanelViewBase;
                return panel?.Y_AXIS_SEGMENT_SIZE ?? 10;
            }
        );

    public static FuncMultiValueConverter<IList<object?>, double> YConverter { get; } =
        new FuncMultiValueConverter<IList<object?>, double>(
            (lst) => {
                foreach (var item in lst)
                    Console.WriteLine(item);
                return 100.0;
            }
        );
}