namespace Hourglass.GUI;

using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

using Hourglass.GUI.ViewModels.Pages;

using Microsoft.Extensions.DependencyInjection;

public class PageInstanciator {

	private readonly IServiceCollection serviceCollection = new ServiceCollection();

	public PageInstanciator(Avalonia.Application application) : this() {
		serviceCollection.AddSingleton<Func<TopLevel?>>(
			provider => 
				() => {
					if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime topWindow)
						return TopLevel.GetTopLevel(topWindow.MainWindow);
					if (application.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
						return TopLevel.GetTopLevel(singleViewPlatform.MainView);
					return null;
				}
		);
	}

	public PageInstanciator() {
	}

	public void AddCommonServiceSingleton(Type serviceType) {
		serviceCollection.AddSingleton(serviceType);
	}

	public void AddCommonServiceSingleton<RegisterT, InstanceT>(InstanceT singleton)
		where RegisterT : class
		where InstanceT : class, RegisterT {
		serviceCollection.AddSingleton<RegisterT>(singleton);
	}

	public void AddCommonServiceSingleton<RegisterT, InstanceT>() 
		where RegisterT : class 
		where InstanceT : class, RegisterT {
		serviceCollection.AddSingleton<RegisterT, InstanceT>();
	}

	public void RegisterPageTransient<T>() where T : class {
		serviceCollection.AddTransient<T>();
	}
	public void RegisterWindow<T>() where T : Window {
		serviceCollection.AddSingleton<T>();
	}

	public void RegisterPageSingleton<T>() where T : class {
		serviceCollection.AddSingleton<T>();
	}

	public IServiceProvider BuildPages() {
		return serviceCollection.BuildServiceProvider();
	}

	public void AddContentBindingType<ContentBaseT>() {
		serviceCollection.AddSingleton<Func<Type, ContentBaseT>>(
			(serviceProvider) =>
				(pageType) =>
					(ContentBaseT?)serviceProvider.GetService(pageType)
						?? throw new InvalidOperationException($"View of type {pageType?.FullName} has no registered view model")
		);
		serviceCollection.AddSingleton<ViewModelFactory<ContentBaseT>>();
	}
}

public class ViewModelFactory<ViewBaseType>(Func<Type, ViewBaseType> factory) {
	public ViewBaseType GetPageViewModel<T>(Action<T?>? afterCreation = null)
		where T : ViewBaseType {
		ViewBaseType viewModel = factory(typeof(T));
		afterCreation?.Invoke((T?)viewModel);
		return viewModel;
	}
}