namespace Hourglass.Database.Services;

using Avalonia;
using Avalonia.Controls;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
			.Where(x => x.start >= intervallStartSeconds && x.start <= intervallFinishSeconds)
                .Where(x => x.finish >= intervallStartSeconds && x.finish <= intervallFinishSeconds)
					.OrderBy(p => p.start)
						.ToList();

	public async Task<Models.Task?> QueryCurrentTaskAsync() {
		List<Models.Task> tasks = await QueryTasksAsync();
		Models.Task? task = (await QueryTasksAsync())
			.Where(t=>t.running)
				.MaxBy(x => x.start);
		return task;
	}
	
	public async Task<List<Models.Task>> QueryTasksOfHourAtDateAsync(DateTime date) {
        return await QueryTasksInIntervallAsync(
			DateTimeService.ToSeconds(DateTimeService.FloorHour(date)),
			DateTimeService.ToSeconds(DateTimeService.FloorHour(date).AddHours(1))
		);
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
			start = now,
			finish = now
		};
		await _accessor.AddAsync(task, false);
		return task;
    }

    private async Task<Models.Task> CreateIntervallBlockingTaskAsync(BlockedTimeIntervallType type, DateTime date, long duration) {
		string reason = type switch {
			BlockedTimeIntervallType.Vacant => "Urlaub",
			BlockedTimeIntervallType.Holiday => "Feiertag",
			BlockedTimeIntervallType.Sick => "Krank",
			BlockedTimeIntervallType.NoExcuse => "Unentschuldigt",
			BlockedTimeIntervallType.None => "",
			_ => ""
		};
        Models.Task task = new() {
            description = reason,
            blocksTime = type,
            owner = null,
            project = null,
            running = false,
            StartDateTime = date,
            FinishDateTime = date.AddSeconds(duration)
        };
        await _accessor.AddAsync(task, false);
        return task;
    }

	public async Task<Models.Task> CreateHourBlockingTaskAsync(BlockedTimeIntervallType type, DateTime date) =>
		await CreateIntervallBlockingTaskAsync(type, date, TimeSpan.SecondsPerHour);

    public async Task<Models.Task> CreateDayBlockingTaskAsync(BlockedTimeIntervallType type, DateTime date) =>
        await CreateIntervallBlockingTaskAsync(type, date, TimeSpan.SecondsPerDay);

    public async Task<Models.Task?> QueryIntervallBlockingTaskAsync(Models.Task task) {
		DateTime date = task.StartDateTime;
        return (await QueryTasksAsync())
            .Where(x => x.blocksTime != BlockedTimeIntervallType.None)
                .Where(x => x.StartDateTime != DateTimeService.FloorHour(date))
					.Where(x => x.StartDateTime != DateTimeService.FloorDay(date))
						.Where(x => x.StartDateTime != DateTimeService.FloorWeek(date))
							.FirstOrDefault();
    }

    public async Task<string?> GetHourBlockedMessageAsync(DateTime date) {
        long seconds = DateTimeService.ToSeconds(DateTimeService.FloorHour(date));
        return (await QueryAllIntervallBlockingTasksAsync())
            .FirstOrDefault(x => x.start == seconds)?.description;
    }

    public async Task<string?> GetDayBlockedMessageAsync(DateTime date) {
        long seconds = DateTimeService.ToSeconds(DateTimeService.FloorDay(date));
        return (await QueryAllIntervallBlockingTasksAsync())
            .FirstOrDefault(x => x.start == seconds)?.description;
    }

    public async Task<string?> GetWeekBlockedMessageAsync(DateTime date) {
        long seconds = DateTimeService.ToSeconds(DateTimeService.FloorWeek(date));
        return (await QueryAllIntervallBlockingTasksAsync())
            .FirstOrDefault(x => x.start == seconds)?.description;
    }

    private async Task<IEnumerable<Models.Task>> QueryAllIntervallBlockingTasksAsync() =>
        (await QueryTasksAsync())
            .Where(x => x.blocksTime != BlockedTimeIntervallType.None);

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
		taskToContiniue.finish = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
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
