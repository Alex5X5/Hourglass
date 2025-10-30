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

    private DateTime startDate = DateTime.MinValue;
    public DateTime? StartDate => startDate;

    private string startDateString = DateTimeService.ToDayAndMonthAndYearString(DateTime.MinValue);
    public string StartDateString {
        set {
            startDate = DateTimeService.InterpretDayAndMonthAndYearString(startDateString) ?? startDate;
            startDateString = value;
            OnStartDateStringChanged?.Invoke(nameof(StartDateString));
        }
        get => DateTimeService.ToDayAndMonthAndYearString(startDate);
    }
    public event Action<string>? OnStartDateStringChanged;

    
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
