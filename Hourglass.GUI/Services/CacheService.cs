namespace Hourglass.GUI.Services;

using System;

public class CacheService {

    private Database.Models.Task? runningTask;
    public Database.Models.Task? RunningTask {
        set {
            runningTask = value;
            OnRunningTaksChanged?.Invoke(runningTask);
        }
        get => runningTask;
    }
    public event Action<Database.Models.Task?>? OnRunningTaksChanged;

    private Database.Models.Task? selectedTask;
    public Database.Models.Task? SelectedTask {
        set {
            selectedTask = value;
            OnSelectedTaksChanged?.Invoke(selectedTask);
        }
        get => selectedTask;
    }
    public event Action<Database.Models.Task?>? OnSelectedTaksChanged;

    public CacheService() {
    }
}