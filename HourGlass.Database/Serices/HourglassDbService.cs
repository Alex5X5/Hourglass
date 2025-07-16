namespace Hourglass.Database.Serices;

using DatabaseUtil;

using Hourglass.Database.Models;
using Hourglass.Database.Serices.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class HourglassDbService : IHourglassDbService {

	DatabaseAccessor<HourglassDbContext> _accessor = new("Databse.sqlite", DatabasePathFormat.FileName, null);

	public HourglassDbService() {
		var res = _accessor.QueryAllAsync<Models.Task>().Result;
	}

	public async Task<List<Ticket>> QueryTicketsAsync()=>
		await _accessor.QueryAllAsync<Ticket>();

	public async Task<List<Project>> QueryProjectsAsync()=>
		await _accessor.QueryAllAsync<Project>();

	public async Task<List<Models.Task>> QueryTasksAsync()=>
		await _accessor.QueryAllAsync<Models.Task>();

	public async Task<Models.Task?> QueryCurrentTaskAsync() {
		Models.Task? task= (await QueryTasksAsync()).MaxBy(x => x.start);
		if (task == null)
			return null;
		if (task.finish > 0)
			return null;
		return task;
	}

	public async Task<List<Models.Task>> QueryTasksOfLastHourAsync() {
		long now = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		now -= TimeSpan.SecondsPerHour;
		IEnumerable<Models.Task> all= await QueryTasksAsync();
		return all.Where(x => (x.finish >= now|x.finish==0)).ToList();
	}

	public async Task<List<Models.Task>> QueryTasksOfLastDayAsync() {
		long now = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		now -= TimeSpan.SecondsPerDay;
		IEnumerable<Models.Task> all = await QueryTasksAsync();
		return all.Where(x => x.finish >= now).ToList();
	}

	public async Task<Models.Task?> StartNewTaskAsnc(string description, Project project, Worker worker, Ticket? ticket) {
		long now = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		Models.Task task = new() {
			description = description,
			project = project,
			owner = worker,
			start = now
		};
		await _accessor.AddAsync<Models.Task>(task, false);
		return task;
	}

	public async Task<bool> ContiniueTaskAsync(Models.Task updatedTask) =>
		await _accessor.UpdateAsync<Models.Task>(updatedTask, false);

	public async Task<Models.Task?> FinishCurrentTaskAsync(long? start, long? finish, string description, Project? project, Ticket? ticket) {
		Models.Task? current = await QueryCurrentTaskAsync();
		if (current == null)
			return null;

		if (start != null)
			current.start = (long)start;
		current.finish = finish ?? System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
		if(await _accessor.UpdateAsync<Models.Task>(current, false))
			return null;
		return current;
	}

	public async Task<bool> UpdateTaskAsync(Models.Task updatedTask) =>
		await _accessor.UpdateAsync<Models.Task>(updatedTask, false);



	public async Task<List<Worker>> QueryWorkersAsync()=>
		await _accessor.QueryAllAsync<Worker>();

}
