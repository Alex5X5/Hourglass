namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;
using Hourglass.Database.Models;
using Hourglass.GUI.ViewModels.Components;
using Hourglass.GUI.ViewModels.Components.GraphPanels;

public static class GridToPositionConverters {

	public static FuncMultiValueConverter<IList<object?>, double> WidthConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                if (lst.Any(item => item is AvaloniaProperty || item == AvaloniaProperty.UnsetValue)) {
                    Console.WriteLine("RowConverter: Unset binding value detected");
                    return 0.0;
                }
                Rect parentBounds = (lst.ToArray<object?>()[0] as Rect?) ?? new Rect(0,0,1,1);
                TaskGraphViewModel? task = lst.ToArray<object?>()[1] as TaskGraphViewModel;
                GraphPanelViewModelBase? panel = lst.ToArray<object?>()[2] as GraphPanelViewModelBase;
                return 100;
            }
		);

	public static FuncMultiValueConverter<IList<object?>, double> HeightConverter { get; } =
		new FuncMultiValueConverter<IList<object?>, double>(
			(lst) => {
                foreach (var item in lst)
                    Console.WriteLine(item);
                return 100;
			}
		);

    public static FuncMultiValueConverter<IList<object?>, double> RowConverter { get; } =
        new FuncMultiValueConverter<IList<object?>, double>(
            (lst) => {
                foreach (var item in lst)
                    Console.WriteLine("test");
                return 1.0;
            }
        );

    public static FuncMultiValueConverter<IList<object?>, double> ColumnConverter { get; } =
        new FuncMultiValueConverter<IList<object?>, double>(
            (lst) => {
                foreach (var item in lst)
                    Console.WriteLine(item);
                return 100.0;
            }
        );
}