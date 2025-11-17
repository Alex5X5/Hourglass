using System;

namespace Hourglass.Util.Services;

public class CacheService {

    private long SelectedDayStartSeconds = DateTimeService.ToSeconds(DateTimeService.FloorDay(DateTime.Today));
    public DateTime SelectedDay {
        set => SelectedDayStartSeconds = DateTimeService.ToSeconds(value);
        get => new(SelectedDayStartSeconds * TimeSpan.TicksPerSecond);
    }
    public event Action<DateTime?>? OnSelectedDayChanged;
}
