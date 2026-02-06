namespace Hourglass.GUI;

using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

public class PageInstanciator {

	private readonly IServiceCollection serviceCollection = new ServiceCollection();
	//private readonly Dictionary<Type, IScopeController> runningScopes;

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

	public void RegisterWindow<T>() where T : Window {
		serviceCollection.AddSingleton<T>();
	}

	public void RegisterPageSingleton<T>() where T : class {
		serviceCollection.AddSingleton<T>();
	}

	public void RegisterPageTransient<T>() where T : class {
		serviceCollection.AddTransient<T>();
    }

    public void RegisterPageScoped<T, ScopeOwnerT>() where T : class {
		serviceCollection.AddScoped<T>();
    }

    public void AddScopeController<OwnerT>() where OwnerT : class {
        serviceCollection.AddSingleton<Func<Type, IServiceScope>>(
            (serviceProvider) =>
                (controllerType) => {
                    IServiceScope scope = serviceProvider.CreateScope();
                    return scope;
                }
        );
        serviceCollection.AddTransient<ScopeController<OwnerT>>(
            serviceProvider=>new ScopeController<OwnerT>(serviceProvider.CreateScope())
        );

        //return (IServiceScope)scope?.Scope?.GetService(typeof(OwnerT))
        //	?? throw new InvalidOperationException($"View model of type {controllerType?.FullName} has no registered view model");
        //}
        //serviceProvider => {
        //	IServiceScope scope = serviceProvider.CreateScope();
        //	ScopeController<OwnerT> controller = new ScopeController<OwnerT>(serviceProvider.GetService<Func<Type, IServiceScope>>());
        //	runningScopes.Add(controller);
        //	return controller;
        //}

    }

    public void RegisterComponentTransient<ComponentT>() where ComponentT : class {
		serviceCollection.AddTransient<ComponentT>();
		serviceCollection.AddSingleton<Func<ComponentT>>(
			(serviceProvider) => 
				() => serviceProvider.GetService<ComponentT>() ?? Activator.CreateInstance<ComponentT>()
		);
        serviceCollection.AddSingleton<ComponentViewModelFactory<ComponentT>>();
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

	public void AddContentBindingTypeScoped<ContentBaseT>() {
		serviceCollection.AddSingleton<Func<Type, ContentBaseT>>(
			(serviceProvider) =>
				(pageType) =>
					(ContentBaseT?)serviceProvider.GetService(pageType)
						?? throw new InvalidOperationException($"View of type {pageType?.FullName} has no registered view model")
		);
        serviceCollection.AddScoped<ViewModelFactory<ContentBaseT>>();
    }

    public IServiceProvider BuildPages() {
        return serviceCollection.BuildServiceProvider();
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

public class ComponentViewModelFactory<ComponentT>(Func<ComponentT> factory) {

    public ComponentT GetComponentViewModel(
		Action<ComponentT?>? afterCreation = null,
		Dictionary<string, object?>? data = null
	) {
        ComponentT viewModel = factory();
		if(data != null) {
			PropertyInfo[] properties = typeof(ComponentT).GetProperties();
			FieldInfo[] fields = typeof(ComponentT).GetFields();
            foreach (string key in data.Keys) {
				PropertyInfo? property = properties.FirstOrDefault(x => x.Name == key);
				if(property != null) {
					property?.SetValue(viewModel, data[key]);
					continue;
				}
				fields.FirstOrDefault(x => x.Name == key)?.SetValue(viewModel, data[key]);
			}
		}
        afterCreation?.Invoke(viewModel);
        return viewModel;
    }
}

public class ScopeController<OwnerT>(IServiceScope scope) : IScopeController where OwnerT : class {

	private IServiceScope _scope = scope;

	public IServiceScope Scope => _scope;

    public void DisposeScope() {
		_scope.Dispose();
    }
}

public interface IScopeController {
	public IServiceScope Scope { get; }
	public void DisposeScope();
}

public class TopLevelSupplier(){
	
}