namespace Hourglass.GUI;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Hourglass.Database.Services;
using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.Views;
using Hourglass.GUI.Views.Pages;
using Hourglass.Util;
using Hourglass.Util.Services;

using Microsoft.Extensions.DependencyInjection;

public partial class App : Application {

	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		IServiceCollection collection = new ServiceCollection();
		collection.AddCommonServices();
		collection.AddViewModels();
		IServiceProvider services = collection.BuildServiceProvider();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new MainWindow(services);
		} else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
			singleViewPlatform.MainView = new MainView();
		}
		base.OnFrameworkInitializationCompleted();
	}

}
public static class ServiceCollectionExtensions {
	
	public static void AddCommonServices(this IServiceCollection collection) {
		if (!Avalonia.Controls.Design.IsDesignMode)
			collection.AddSingleton(new HourglassDbService());
		collection.AddSingleton(new DateTimeService());
		collection.AddSingleton(new SettingsService());
	}
	
	public static void AddViewModels(this IServiceCollection collection) {
		collection.AddTransient(serviceProvider =>
			new MainWindowViewModel(serviceProvider.GetService<MainWindow>(), serviceProvider)
		);

		collection.AddTransient(serviceProvider =>
			new MainViewViewModel(serviceProvider.GetService<MainView>(), serviceProvider)
		);

		collection.AddTransient(serviceProvider =>
			new ExportPageViewModel(serviceProvider.GetService<ExportPageView>(), serviceProvider)
		);

		collection.AddTransient(serviceProvider =>
			new GraphPageViewModel(serviceProvider.GetService<GraphPageView>(), serviceProvider)
		);

		collection.AddTransient(serviceProvider =>
			new TimerPageViewModel(serviceProvider.GetService<TimerPageView>(), serviceProvider)
		);

		collection.AddTransient(serviceProvider =>
			new ProjectPageViewModel(serviceProvider.GetService<ProjectPageView>(), serviceProvider)
		);
	}
}

