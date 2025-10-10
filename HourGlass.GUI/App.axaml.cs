namespace Hourglass.GUI;

using Avalonia.Metadata;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Hourglass.Database.Services;
using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views;
using Hourglass.Util;
using Hourglass.Util.Services;

using Microsoft.Extensions.DependencyInjection;
using Hourglass.Database.Services.Interfaces;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;

public partial class App : Application {

	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		PageInstanciator instanciator = new(this);
		instanciator.AddContentBindingType<GraphPanelViewModelBase>();
		instanciator.AddContentBindingType<PageViewModelBase>();
		instanciator.AddCommonServiceSingleton<SettingsService, SettingsService>(new SettingsService());
		DateTimeService dateTimeService = new();
		instanciator.AddCommonServiceSingleton<DateTimeService, DateTimeService>(dateTimeService);
		//instanciator.AddCommonServiceSingleton<DateTimeService, DateTimeService>();

		if (!Design.IsDesignMode) {
			HourglassDbService dbService = new(dateTimeService);
			instanciator.AddCommonServiceSingleton<IHourglassDbService, HourglassDbService>(dbService);
		}
		if (!Design.IsDesignMode)
			instanciator.AddCommonServiceSingleton<IPdfService, PdfService>();

		//instanciator.AddCommonServiceSingleton<PageViewFactory, PageViewFactory>();
		instanciator.RegisterPageSingleton<MainViewModel>();
		instanciator.RegisterPageSingleton<MainWindowViewModel>();

		instanciator.RegisterPageSingleton<GraphPageViewModel>();
		instanciator.RegisterPageTransient<ExportPageViewModel>();
		instanciator.RegisterPageTransient<ProjectPageViewModel>();
		instanciator.RegisterPageTransient<TaskDetailsPageViewModel>();
		instanciator.RegisterPageTransient<TimerPageViewModel>();

		instanciator.RegisterPageTransient<DayGraphPanelViewModel>();
		instanciator.RegisterPageTransient<WeekGraphPanelViewModel>();
		instanciator.RegisterPageTransient<MonthGraphPanelViewModel>();

		var services = instanciator.BuildPages();

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new MainWindow() {
				DataContext = services.GetRequiredService<MainViewModel>()
			};
		} else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
			singleViewPlatform.MainView = new MainView() {
				DataContext = services.GetRequiredService<MainViewModel>()
			};
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
		MainViewModel mainViewModel = new MainViewModel();
		collection.AddSingleton<MainView>();

		//collection.AddTransient(
		//	serviceProvider => {
		//		Window window = serviceProvider.GetService<MainWindow>();
		//		return new MainWindowViewModel(window, serviceProvider);
		//	}
		//);

		//collection.AddTransient(serviceProvider =>
		//	new MainViewModel(serviceProvider.GetService<MainView>(), serviceProvider)
		//);

		//collection.AddTransient(serviceProvider =>
		//	new ExportPageViewModel(serviceProvider.GetService<ExportPageView>(), serviceProvider)
		//);

		//collection.AddTransient(serviceProvider =>
		//	new GraphPageViewModel(serviceProvider.GetService<GraphPageView>(), serviceProvider)
		//);

		//collection.AddTransient(serviceProvider =>
		//	new TimerPageViewModel(serviceProvider.GetService<TimerPageView>(), serviceProvider)
		//);

		//collection.AddTransient(serviceProvider =>
		//	new ProjectPageViewModel(serviceProvider.GetService<ProjectPageView>(), serviceProvider)
		//);
	}
}

