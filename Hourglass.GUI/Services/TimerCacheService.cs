namespace Hourglass.GUI.Services;

using Hourglass.Database.Services.Interfaces;

using System;

public class TimerCacheService {

    private Database.Models.Task? runningTask;
    public Database.Models.Task? RunningTask {
        set {
            runningTask = value?.Clone();
            OnRunningTaksChanged?.Invoke(runningTask);
        }
        get => runningTask;
    }
    public event Action<Database.Models.Task?>? OnRunningTaksChanged;

    private Database.Models.Task? selectedTask;
    public Database.Models.Task? SelectedTask {
        set {
            selectedTask = value?.Clone();
            OnSelectedTaksChanged?.Invoke(selectedTask);
        }
        get => selectedTask;
    }
    public event Action<Database.Models.Task?>? OnSelectedTaksChanged;

    public TimerCacheService(IHourglassDbService dbService) {
        RunningTask = dbService.QueryCurrentTaskAsync().Result;
    }
}