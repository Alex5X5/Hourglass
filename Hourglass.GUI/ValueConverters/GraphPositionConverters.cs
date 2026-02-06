namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;
using Hourglass.GUI.ViewModels.Components;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using System.Collections.Generic;
using System.Globalization;

public static class TaskConverterHelper {

    public static double PaddingX(Rect bounds) =>
        bounds.Width * GraphPanelViewModelBase.PADDING_X_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_X_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_X_WEIGHT);

    public static double PaddingY(Rect bounds) =>
        bounds.Height * GraphPanelViewModelBase.PADDING_Y_WEIGHT / (GraphPanelViewModelBase.GRAPH_AREA_Y_WEIGHT + 2 * GraphPanelViewModelBase.PADDING_Y_WEIGHT);
}

public class TaskLeftConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        TaskGraphViewModel? taskModel = lst[2] as TaskGraphViewModel;
        double paddingX = TaskConverterHelper.PaddingX(parentBounds);
        double graphAreaWidth = parentBounds.Width - 2 * paddingX;
        double proportion = graphAreaWidth / panelModel?.TIME_INTERVALL_DURATION ?? 1;
        return ((taskModel?.task.start ?? 0) - (panelModel?.TIME_INTERVALL_START_SECONDS ?? 0)) * proportion + paddingX;
    }
}

public class TaskTopConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        TaskGraphViewModel? taskModel = lst[2] as TaskGraphViewModel;
        double paddingY = TaskConverterHelper.PaddingY(parentBounds);
        double graphAreaHeight = parentBounds.Height - 2 * paddingY;
        double yAxissegmentSize = graphAreaHeight / (panelModel?.Y_AXIS_SEGMENT_COUNT ?? 10 * 1.5) * panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1;
        return yAxissegmentSize * (taskModel?.index ?? 0 % (panelModel?.MAX_TASKS ?? 10 / panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1)) + paddingY;
    }
}

public class TaskWidthConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        TaskGraphViewModel? task = lst[2] as TaskGraphViewModel;
        bool running = task?.task.running ?? false;
        double paddingX = TaskConverterHelper.PaddingX(parentBounds);
        double graphAreaWidth = parentBounds.Width - 2 * paddingX;
        double proportion = graphAreaWidth / panelModel?.TIME_INTERVALL_DURATION ?? 1;
        long duration = (running ? DateTimeService.ToSeconds(DateTime.Now) : (task?.task.finish ?? 1)) - (task?.task.start ?? 0);
        double graphLength = duration * proportion;
        return Math.Max(graphLength, panelModel?.GRAPH_MINIMAL_WIDTH ?? 10);
    }
}

public class TaskHeightConverter : IMultiValueConverter {
    public object? Convert(IList<object?> lst, Type targetType, object? parameter, CultureInfo culture) {
        Rect parentBounds = (lst[0] as Rect?) ?? new Rect(0, 0, 10, 10);
        GraphPanelViewModelBase? panelModel = lst[1] as GraphPanelViewModelBase;
        double paddingY = TaskConverterHelper.PaddingY(parentBounds);
        double graphAreaHeight = parentBounds.Height - 2 * paddingY;
        return (graphAreaHeight / (panelModel?.Y_AXIS_SEGMENT_COUNT ?? 1 * 1.5) * panelModel?.TASK_GRAPH_COLUMN_COUNT ?? 1) / 1.5;
    }
}
