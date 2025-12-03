using System;

namespace Hourglass.Util.Services;

public class CacheService {

    private long SelectedDayStartSeconds = DateTimeService.ToSeconds(DateTimeService.FloorDay(DateTime.Today));
    public DateTime SelectedDay {
        set {
            SelectedDayStartSeconds = DateTimeService.ToSeconds(value);
            OnSelectedDayChanged?.Invoke(SelectedDay);
        }
        get => new(SelectedDayStartSeconds * TimeSpan.TicksPerSecond);
    }
    public event Action<DateTime?>? OnSelectedDayChanged = date => { };

    public CacheService() {

    }
}
