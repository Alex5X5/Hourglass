using Hourglass.Util;

namespace Hourglass.GUI.Services;

public class SettingsCacheService {

    private string username = "";
    public string Username {
        set {
            username = value;
            OnUsernameChanged?.Invoke(nameof(Username));
        }
        get => username;
    }
    public event Action<string>? OnUsernameChanged;

    private string startDateString = "";
    public string StartDateString {
        set {
            startDateString = value;
            OnStartDateStringChanged?.Invoke(nameof(StartDateString));
        }
        get => startDateString;
    }
    public event Action<string>? OnStartDateStringChanged;

    public DateTime? StartDate {
        get {
            DateTimeService.InterpretDayAndMonthAndYearString(StartDateString);
            return new DateTime();
        }
    }
    
    private string jobName = "";
    public string JobName {
        set {
            jobName = value;
            OnJobNameChanged?.Invoke(nameof(JobName));
        }
        get => jobName;
    }
    public event Action<string>? OnJobNameChanged;
    
    
}
