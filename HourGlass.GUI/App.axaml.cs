namespace Hourglass.GUI;

using Avalonia.Metadata;

using Avalonia;
using Avalonia.Controls;
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
		IServiceCollection commonServiceCollection = new ServiceCollection();
		commonServiceCollection.AddCommonServices();
		IServiceProvider commonServices = commonServiceCollection.BuildServiceProvider();
		IServiceCollection viewCollection = new ServiceCollection();
		viewCollection.AddViewModels();
		IServiceProvider services = viewCollection.BuildServiceProvider();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new MainWindow(commonServices);
		} else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
			singleViewPlatform.MainView = new MainView();
		}
		base.OnFrameworkInitializationCompleted();
	}

}
public static class ServiceCollectionExtensions {
	
	public static void AddCommonServices(this IServiceCollection collection) {
		SettingsService settingsService = new();
		collection.AddSingleton(settingsService);
		DateTimeService	dateTimeService = new();
		collection.AddSingleton(dateTimeService);
		if (!Design.IsDesignMode) {
			HourglassDbService dbService = new(dateTimeService);
			collection.AddSingleton(dbService);
		}
	}
	
	public static void AddViewModels(this IServiceCollection collection) {
		collection.AddTransient(
			serviceProvider => {
				Window window = serviceProvider.GetService<MainWindow>();
				return new MainWindowViewModel(window, serviceProvider);
			}
		);

		collection.AddTransient(serviceProvider =>
			new MainViewModel(serviceProvider.GetService<MainView>(), serviceProvider)
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

