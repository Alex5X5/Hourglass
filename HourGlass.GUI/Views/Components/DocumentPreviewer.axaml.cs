using Avalonia;
using Avalonia.Media;

namespace Hourglass.GUI.Views.Components;

public partial class DocumentPreviewer : Avalonia.Controls.UserControl {

    public DocumentPreviewer() : base() {
        InitializeComponent();
    }

	public override void Render(DrawingContext context) {
        base.Render(context);
        double sideLength = Math.Min(Bounds.Width, Bounds.Height);
        double drawGridSize = sideLength / 12;
        Avalonia.Media.Brush outlinebrush = new SolidColorBrush(Colors.Black);
        Avalonia.Media.Pen outlinePen = new(outlinebrush) { Thickness = 1 };
        var rect = new Rect(
            (Bounds.Width - 6 * drawGridSize) / 2,
            (Bounds.Height - 8 * drawGridSize) / 2,
            drawGridSize * 6,
            drawGridSize * 8
        );
        context.DrawRectangle(
            outlinePen,
            rect
        );
    }
}