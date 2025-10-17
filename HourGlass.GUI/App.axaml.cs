namespace Hourglass.GUI;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.Views;
using Hourglass.Util;
using Hourglass.Util.Services;

using Hourglass.Database.Services;
using Hourglass.Database.Services.Interfaces;
using Hourglass.PDF;
using Hourglass.PDF.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Hourglass.GUI.Services;

public partial class App : Application {

	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		PathService.PrintDetailedInfo();
		PathService.ExtractFiles("Hourglass");
		PageInstanciator instanciator = new(this);
		instanciator.AddCommonServiceSingleton<SettingsService, SettingsService>(new SettingsService());
		DateTimeService dateTimeService = new();
		instanciator.AddCommonServiceSingleton<DateTimeService, DateTimeService>(dateTimeService);

		if (!Design.IsDesignMode) {
			HourglassDbService dbService = new(dateTimeService);
			instanciator.AddCommonServiceSingleton<IHourglassDbService, HourglassDbService>(dbService);
			instanciator.AddCommonServiceSingleton<IPdfService, PdfService>();
			instanciator.AddCommonServiceSingleton<CacheService, CacheService>();
		}

		instanciator.AddContentBindingType<PageViewModelBase>();
		instanciator.RegisterPageTransient<TimerPageViewModel>();
		instanciator.RegisterPageSingleton<MainViewModel>();
		instanciator.RegisterPageTransient<ExportPageViewModel>();
		instanciator.RegisterPageTransient<ProjectPageViewModel>();
		instanciator.RegisterPageTransient<TaskDetailsPageViewModel>();

		instanciator.AddContentBindingType<GraphPanelViewModelBase>();
		instanciator.RegisterPageSingleton<GraphPageViewModel>();
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

