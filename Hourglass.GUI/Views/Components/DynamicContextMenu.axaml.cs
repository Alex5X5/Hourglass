namespace Hourglass.GUI.Components;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Hourglass.GUI.ViewModels.Components;

public partial class DynamicContextMenu: ContextMenu, IStyleable {

    Type IStyleable.StyleKey => typeof(ContextMenu);

    public DynamicContextMenu() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}
