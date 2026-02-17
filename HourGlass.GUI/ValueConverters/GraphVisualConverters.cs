namespace Hourglass.GUI.ValueConverters;

using Avalonia;
using Avalonia.Data.Converters;
using System.Collections.Generic;
using System.Globalization;


public class RectangleRoundConverter : IValueConverter {

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
		Rect bounds = (value as Rect?) ?? new Rect(0, 0, 10, 10);
		return Math.Min(bounds.Height * 0.25, bounds.Width / 2);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
		throw new NotImplementedException();
	}
}