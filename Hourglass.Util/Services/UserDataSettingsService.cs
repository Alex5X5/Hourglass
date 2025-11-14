namespace Hourglass.Util.Services;

using System;

public partial class SettingsService {

	public string Username {
        set {
            SetSetting(USER_NAME_KEY, value);
            OnUsernameChanged?.Invoke(nameof(JobName));
        }
        get => GetSetting(JOB_NAME_KEY);
    }
	public event Action<string>? OnUsernameChanged;

	private DateTime startDate = DateTime.MinValue;
	private string startDateString = DateTimeService.ToDayAndMonthAndYearString(DateTime.MinValue);
	
	public DateTime StartDate {
		set {
			SetSetting(START_DATE_KEY, DateTimeService.ToDayAndMonthAndYearString(value));
            OnStartDateChanged?.Invoke(nameof(StartDate));
        }
		get => DateTimeService.InterpretDayAndMonthAndYearString(GetSetting(START_DATE_KEY)) ?? DateTime.MinValue;
	}

	public string StartDateString {
        set {
			SetSetting(START_DATE_KEY, value);
            OnStartDateChanged?.Invoke(nameof(StartDate));
        }
        get => GetSetting(START_DATE_KEY);

    }

	public event Action<string>? OnStartDateChanged;

	public string JobName {
		set {
			SetSetting(JOB_NAME_KEY, value);
			OnJobNameChanged?.Invoke(nameof(JobName));
		}
		get => GetSetting(JOB_NAME_KEY);
	}

	public event Action<string>? OnJobNameChanged;
}
