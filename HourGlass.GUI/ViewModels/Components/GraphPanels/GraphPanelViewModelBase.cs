namespace Hourglass.GUI.ViewModels.Components.GraphPanels;

using Avalonia.Controls;
using Hourglass.Database.Services.Interfaces;
using Hourglass.GUI.Services;
using Hourglass.GUI.ViewModels.Pages;
using Hourglass.Util;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public abstract partial class GraphPanelViewModelBase : ViewModelBase {

    public const double GRAPH_AREA_X_WEIGHT = 28;
    public const double GRAPH_AREA_Y_WEIGHT = 28;
    public const double PADDING_X_WEIGHT = 1;
    public const double PADDING_Y_WEIGHT = 1;

	public GridLength GraphAreaXWeight { get; } = new GridLength(GRAPH_AREA_X_WEIGHT, GridUnitType.Star);
	public GridLength GraphAreaYWeight { get; } = new GridLength(GRAPH_AREA_Y_WEIGHT, GridUnitType.Star);
    public GridLength PaddingXWeight { get; } = new GridLength(PADDING_X_WEIGHT, GridUnitType.Star);
    public GridLength PaddingYWeight { get; } = new GridLength(PADDING_Y_WEIGHT, GridUnitType.Star);

    public IHourglassDbService dbService { set; get; }
	public DateTimeService dateTimeService { set; get; }
	public CacheService cacheService;

	public GraphPageViewModel panelController;
	protected MainViewModel pageController;

    public IList<MenuItemViewModel>? Items { get; set; }

    public string Title => GetTitle();

	public GraphPanelViewModelBase() : this(null, null, null, null, null) {

    }
	
	public GraphPanelViewModelBase(IHourglassDbService dbService, DateTimeService dateTimeService, GraphPageViewModel panelController, MainViewModel pageController, CacheService cacheService) : base() {
		this.dbService = dbService;
		this.dateTimeService = dateTimeService;
		this.panelController = panelController;
		this.pageController = pageController;
		this.cacheService = cacheService;
	}

	public abstract Task<List<Database.Models.Task>> GetTasksAsync();

	public virtual void OnClick(Database.Models.Task task) {
		cacheService.SelectedTask = task;
		pageController.GoToTaskdetails(task);
	}

    public abstract void OnDoubleClick(DateTime clickedTime);

    protected abstract string GetTitle();

    public abstract void PreviusIntervallClick();

    public abstract void FollowingIntervallClick();
}
