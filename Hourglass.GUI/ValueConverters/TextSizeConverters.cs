namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;

public static class TextSizeConverters {
	/// <summary>
	/// Gets a Converter that takes a number as input and converts it into a text representation
	/// </summary>
	public static FuncValueConverter<Rect, double> TextSizeFromWidthAndHeightConverter { get; } =
		new FuncValueConverter<Rect, double>(
			rect => {
				var val = Math.Round(rect.Height * 0.6, 1);
				return Math.Max(5.0, val);
			}
		);
	
	public static FuncValueConverter<Rect, int, double> TextSizeFromWidthAndHeightConverterMultiLine { get; } =
		new FuncValueConverter<Rect, int, double>(
			(rect, lineCount) => {
				double val = Math.Round(rect.Height / lineCount, 1);
				return 30;
				return Math.Max(5.0, val);
			}
		);
}