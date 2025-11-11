using Hourglass.GUI.ViewModels;
using Hourglass.GUI.ViewModels.Components.GraphPanels;
using Hourglass.GUI.ViewModels.Pages;
using System;

namespace Hourglass.GUI;

public class PageViewFactory(Func<Type, PageViewModelBase> factory) {
	public PageViewModelBase GetPageViewModel<T>(Action<T>? afterCreation = null)
		where T : PageViewModelBase {
		var viewModel = factory(typeof(T));
		afterCreation?.Invoke((T)viewModel);
		return viewModel;
	}
}

public class GraphPanelViewModelFactory(Func<Type, GraphPanelViewModelBase> factory) {
	public GraphPanelViewModelBase GetGraphPanelViewModel<T>(Action<T>? afterCreation = null)
		where T : GraphPanelViewModelBase {
		var viewModel = factory(typeof(T));
		afterCreation?.Invoke((T)viewModel);
		return viewModel;
	}
}

