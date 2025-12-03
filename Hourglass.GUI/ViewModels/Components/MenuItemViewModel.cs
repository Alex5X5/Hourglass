namespace Hourglass.GUI.ViewModels.Components;

using ReactiveUI;

public class MenuItemViewModel : ReactiveObject {
    private string? header;
    public string? Header { get => header; set => this.RaiseAndSetIfChanged(ref header, value); }
    private object? icon;
    public object? Icon { get => icon; set => this.RaiseAndSetIfChanged(ref icon, value); }
    public IReactiveCommand? Command { get; set; }
    private object? commandParameter;
    public object? CommandParameter { get => commandParameter; set => this.RaiseAndSetIfChanged(ref commandParameter, value); }
    public IList<MenuItemViewModel>? Items { get; set; }
}
