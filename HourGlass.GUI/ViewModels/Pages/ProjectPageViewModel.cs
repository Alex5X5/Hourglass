namespace Hourglass.GUI.ViewModels.Pages;

using Hourglass.Database.Models;

public class ProjectPageViewModel : PageViewModelBase {

	public Project SelectedProject { get; set; }
	public List<Project> AvailableProjects { get; set; }

	public ProjectPageViewModel() : base() {
		AvailableProjects = [
			new Project() { Name="test project" },
			new Project() { Name = "failing project" },
			new Project() { Name = "sucessfull project" }
		];
		SelectedProject = AvailableProjects[0];

	}
}
