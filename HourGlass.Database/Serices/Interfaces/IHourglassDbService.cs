namespace Hourglass.Database.Services.Interfaces;

using Hourglass.Database.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IHourglassDbService {


	public Task<Models.Task?> QueryCurrentTaskAsync();

	public Task<Models.Task?> StartNewTaskAsnc(string description, Project project, Worker worker, Ticket? ticket);

	public Task<bool> UpdateTaskAsync(Models.Task updatedTask);

	public System.Threading.Tasks.Task DeleteTaskAsync(Models.Task updatedTask);

	public Task<bool> ContiniueTaskAsync(Models.Task updatedTask);

	public Task<List<Models.Task>> QueryTasksAsync();

	public Task<List<Models.Task>> QueryTasksOfCurrentHourAsync();

	public Task<List<Models.Task>> QueryTasksOfCurrentDayAsync();

	public Task<List<Models.Task>> QueryTasksOfCurrentWeekAsync();

	public Task<Models.Task?> FinishCurrentTaskAsync(long? start, long? finish, string description, Project? project, Ticket? ticket);

	public Task<List<Models.Ticket>> QueryTicketsAsync();

	public Task<List<Models.Project>> QueryProjectsAsync();

	public Task<List<Models.Worker>> QueryWorkersAsync();

}
