namespace Hourglass.Database.Services;

using DatabaseUtil;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class HourglassDbService : IHourglassDbService {

	DatabaseAccessor<HourglassDbContext> _accessor = 
		new(Paths.FilesPath("database"), DatabasePathFormat.FileName, null);

	public HourglassDbService() { }

	public async Task<List<Ticket>> QueryTicketsAsync()=>
		await _accessor.QueryAllAsync<Ticket>();

	public async Task<List<Project>> QueryProjectsAsync()=>
		await _accessor.QueryAllAsync<Project>();

	public async Task<List<Models.Task>> QueryTasksAsync()=>
		await _accessor.QueryAllAsync<Models.Task>();

	public async Task<Models.Task?> QueryCurrentTaskAsync() {
		List<Models.Task> tasks = await QueryTasksAsync();
		Models.Task? task = (await QueryTasksAsync())
			.Where(t=>t.finish==0)
				.MaxBy(x => x.start);
		if (task == null)
			return null;
		if (task.finish > 0)
			return null;
		return task;
	}
	
	public async Task<List<Models.Task>> QueryTasksOfCurrentHourAsync() {
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		now -= TimeSpan.SecondsPerHour;
		IEnumerable<Models.Task> all= await QueryTasksAsync();
		return all
			.Where(x => (x.finish >= now|x.finish==0))
				.ToList();
	}

	public async Task<List<Models.Task>> QueryTasksOfCurrentDayAsync() {
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		now -= TimeSpan.SecondsPerDay;
		IEnumerable<Models.Task> all = await QueryTasksAsync();
		return all
			.Where(x => x.StartDateTime.DayOfWeek == DateTime.Now.DayOfWeek)
				.ToList();
	}

	public async Task<List<Models.Task>> QueryTasksOfCurrentWeekAsync() {
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		DateTime today = DateTime.Today;
		int daysSinceMonday = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
		DateTime lastMonday = today.AddDays(-daysSinceMonday);
		IEnumerable<Models.Task> all = await QueryTasksAsync();
		return all
			.Where(x => x.StartDateTime >= lastMonday)
				.ToList();
	}

	public async Task<List<Models.Task>> QueryTasksOfCurrentMonthAsync() {
		DateTime thisMonth = new(DateTime.Now.Year, DateTime.Now.Month, 1);
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		IEnumerable<Models.Task> all = await QueryTasksAsync();
		return all
			.Where(x => x.StartDateTime >= thisMonth)
				.ToList();
	}

	public async Task<Models.Task?> StartNewTaskAsnc(string description, Project project, Worker worker, Ticket? ticket) {
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		Models.Task task = new() {
			description = description,
			project = project,
			owner = worker,
			start = now
		};
		await _accessor.AddAsync(task, false);
		return task;
	}

	public async Task<Models.Task> ContiniueTaskAsync(Models.Task taskToContiniue) { 
		Models.Task? runningTask = await QueryCurrentTaskAsync();
		if (runningTask != null)
			await FinishCurrentTaskAsync(
				runningTask.start,
				runningTask.finish,
				runningTask.description,
				runningTask.project,
				runningTask.ticket
			);
		taskToContiniue.finish = 0;
		taskToContiniue.running = true;
		await _accessor.UpdateAsync(taskToContiniue, false);
		return taskToContiniue;
	}

	public async Task<Models.Task?> FinishCurrentTaskAsync(long? start, long? finish, string description, Project? project, Ticket? ticket) {
		Models.Task? current = await QueryCurrentTaskAsync();
		if (current == null)
			return null;
		if (start != null)
			current.start = (long)start;
		current.description = description;
			current.finish = finish ?? DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		if(await _accessor.UpdateAsync(current, false))
			return null;
		return current;
	}

	public async Task<bool> UpdateTaskAsync(Models.Task updatedTask) =>
		await _accessor.UpdateAsync(updatedTask, false);

	public async Task<List<Worker>> QueryWorkersAsync()=>
		await _accessor.QueryAllAsync<Worker>();

	public async System.Threading.Tasks.Task DeleteTaskAsync(Models.Task task) =>
		await _accessor.DeleteAsync(task);
}
