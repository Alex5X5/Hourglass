namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;
using Hourglass.GUI.ViewModels.Components;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views.Components.GraphPanels;
using System.Collections.Generic;
using System.Globalization;

public class CanvasLeftConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        TaskGraphViewModel? taskModel = lst[2] as TaskGraphViewModel;
        double proportion = parentBounds.Width / panelModel?.TIME_INTERVALL_DURATION ?? 1;
        double paddingX = parentBounds.Width * GraphPanelViewModelBase.PADDING_X_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_X_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_X_WEIGHT);
        return ((taskModel?.task.start ?? 0) - (panelModel?.TIME_INTERVALL_START_SECONDS ?? 0)) * proportion + paddingX;
    }
}

public class CanvasTopConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        TaskGraphViewModel? taskModel = lst[2] as TaskGraphViewModel;
        double paddingY = parentBounds.Height * GraphPanelViewModelBase.PADDING_Y_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_Y_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_Y_WEIGHT);
        double graphAreaHeight = parentBounds.Height - 2 * paddingY;
        double yAxissegmentSize = graphAreaHeight / (panelModel?.Y_AXIS_SEGMENT_COUNT ?? 1 * 1.5) * panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1;
        return yAxissegmentSize * (taskModel?.index ?? 0 % (panelModel?.MAX_TASKS ?? 10 / panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1)) * 1.5 + paddingY;
    }
}

public class WidthConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panel = lst[1] as GraphPanelViewModelBase;
        TaskGraphViewModel? task = lst[2] as TaskGraphViewModel;
        long start = task?.task.start ?? 0;
        long finish = task?.task.finish ?? 1;
        bool running = task?.task.running ?? false;
        double paddingX = parentBounds.Width * GraphPanelViewModelBase.PADDING_X_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_X_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_X_WEIGHT);
        double proportion = (parentBounds.Width - 2 * paddingX) / panel?.TIME_INTERVALL_DURATION ?? 1;
        long duration = running ? DateTimeService.ToSeconds(DateTime.Now) - start : finish - start;
        double graphLength = duration * proportion;
        return Math.Max(graphLength, panel?.GRAPH_MINIMAL_WIDTH ?? 10);
    }
}

public class HeightConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        double paddingY = parentBounds.Height * GraphPanelViewModelBase.PADDING_Y_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_Y_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_Y_WEIGHT);
        double graphAreaHeight = parentBounds.Height - 2 * paddingY;
        return graphAreaHeight / (panelModel?.Y_AXIS_SEGMENT_COUNT ?? 1 * 1.5) * panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1;
    }
}
