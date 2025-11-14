namespace Hourglass.Database.Services;

using Avalonia.Media;
using DatabaseUtil;

using Hourglass.Database.Models;
using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using Hourglass.Util.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class HourglassDbService : IHourglassDbService {

	DatabaseAccessor<HourglassDbContext> _accessor = 
		new(PathService.FilesPath("database"), DatabasePathFormat.FileName, null);

	public async Task<List<Ticket>> QueryTicketsAsync()=>
		await _accessor.QueryAllAsync<Ticket>();

	public async Task<List<Project>> QueryProjectsAsync()=>
		await _accessor.QueryAllAsync<Project>();

	public async Task<List<Models.Task>> QueryTasksAsync()=>
		await _accessor.QueryAllAsync<Models.Task>();

	public async Task<List<Models.Task>> QueryTasksInIntervallAsync(long intervallStartSeconds, long intervallFinishSeconds) =>
		(await _accessor.QueryAllAsync<Models.Task>())
			.Where(x => x.start >= intervallStartSeconds && x.finish <= intervallFinishSeconds)
                .OrderBy(p => p.start)
					.ToList();

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
	
	public async Task<List<Models.Task>> QueryTasksOfHourAtDateAsync(DateTime date) {
		long seconds = date.Ticks / TimeSpan.TicksPerSecond;
		seconds -= TimeSpan.SecondsPerHour;
		IEnumerable<Models.Task> all= await QueryTasksAsync();
		return all
			.Where(x => (x.finish >= seconds|x.finish==0))
				.ToList();
	}
	
	public async Task<List<Models.Task>> QueryTasksOfCurrentHourAsync() =>
		await QueryTasksOfHourAtDateAsync(DateTime.Now);

	public async Task<List<Models.Task>> QueryTasksOfDayAtDateAsync(DateTime date) {
        DateTime start = DateTimeService.FloorDay(date);
        DateTime finfish = start.AddDays(1);
        return await QueryTasksInIntervallAsync(DateTimeService.ToSeconds(start), DateTimeService.ToSeconds(finfish));
    }

	public async Task<List<Models.Task>> QueryTasksOfCurrentDayAsync() =>
		await QueryTasksOfDayAtDateAsync(DateTime.Now);

	public async Task<List<Models.Task>> QueryTasksOfWeekAtDateAsync(DateTime date) {
		DateTime start = DateTimeService.FloorWeek(date);
		DateTime finfish = start.AddDays(7);
		return await QueryTasksInIntervallAsync(DateTimeService.ToSeconds(start), DateTimeService.ToSeconds(finfish));
	}

	public async Task<List<Models.Task>> QueryTasksOfCurrentWeekAsync() =>
		await QueryTasksOfWeekAtDateAsync(DateTime.Now);

	public async Task<List<Models.Task>> QueryTasksOfMonthAtDateAsync(DateTime date) {
        DateTime start = DateTimeService.FloorMonth(date);
        DateTime finfish = start.AddDays(DateTime.DaysInMonth(date.Year, date.Month));
        return await QueryTasksInIntervallAsync(DateTimeService.ToSeconds(start), DateTimeService.ToSeconds(finfish));
    }

	public async Task<List<Models.Task>> QueryTasksOfCurrentMonthAsync() =>
		await QueryTasksOfMonthAtDateAsync(DateTime.Now);


	public async Task<Models.Task> StartNewTaskAsnc(string description, Color color, Project? project, Worker worker, Ticket? ticket) {
		long now = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
		Models.Task task = new() {
			DisplayColor = color,
			description = description,
			owner = worker,
			project = project,
			running = true,
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
		current.running = false;
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
