namespace Hourglass.Util;

using Hourglass.Util.Services;
using System;

public class DateTimeService {

	public static DateTime START_DATE = GetStartDate();

	private long SelectedDayStartSeconds = ToSeconds(DateTime.Today);

	public DateTime SelectedDay {
		set => SelectedDayStartSeconds = ToSeconds(value);
		get => new(SelectedDayStartSeconds * TimeSpan.TicksPerSecond);
	}

	static DateTimeService() {
		//SettingsService.OnSettingsReload += 
		//	() => START_DATE = GetStartDate();
	}

	public static long ToSeconds(DateTime date) =>
		date.Ticks / TimeSpan.TicksPerSecond;

	public static DateTime FloorDay(DateTime date) =>
		new(date.Year, date.Month, date.Day);

	public static DateTime FloorWeek(DateTime date) =>
		FloorDay(date.AddDays((-(int)date.DayOfWeek)+1));

	public static DateTime FloorMonth(DateTime date) =>
			new(date.Year, date.Month, 1);

	public static int DaysInCurrentMonth() =>
		DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

	public static bool TodayIsDayOfWeek(DayOfWeek day) =>
		day == DateTime.Now.DayOfWeek;

	public static bool TodayIsDayOfWeek(int day) =>
		day == (int)DateTime.Now.AddDays(-1).DayOfWeek;


	private static DateTime GetStartDate() {
		DateTime date;
		try {
			date = Convert.ToDateTime(new SettingsService().GetSetting(SettingsService.START_DATE_KEY));
		} catch (FormatException) {
			date = new DateTime(2024, 8, 5);
		}
		return date;
	}

	public static DateTime? InterpretDayAndTimeString(string s) {
		try {
			int startIndex = 0;
			int finishIndex = finishIndex = s.IndexOf('.', startIndex);
			int day = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
			startIndex = finishIndex + 1;
			finishIndex = finishIndex = s.IndexOf(' ', startIndex);
			int month = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
			startIndex = finishIndex + 1;
			finishIndex = finishIndex = s.IndexOf(':', startIndex);
			int hour = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
			startIndex = finishIndex + 1;
			finishIndex = finishIndex = s.IndexOf(':', startIndex);
			int minute = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
			startIndex = finishIndex + 1;
			int second = Convert.ToInt32(s.Substring(startIndex, s.Length - startIndex));
			DateTime time = new(DateTime.Now.Year, month, day, hour, minute, second);
			return time;
		} catch (FormatException) {
			return null;
		} catch (ArgumentOutOfRangeException) {
			return null;
		}
	}

    public static DateTime? InterpretDayAndMonthAndYearString(string s) {
        try {
            int startIndex = 0;
            int finishIndex = s.IndexOf('.', startIndex);
            int day = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
            startIndex = finishIndex + 1;
            finishIndex = s.IndexOf('.', startIndex);
            int month = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
            startIndex = finishIndex + 2;
			finishIndex = s.Length;
            int year = Convert.ToInt32(s.Substring(startIndex, finishIndex - startIndex));
            DateTime time = new(year, month, day);
            return time;
        } catch (FormatException) {
            return null;
        } catch (ArgumentOutOfRangeException) {
            return null;
        }
    }

    public static string ToTimeString(DateTime time) =>
		$"{time.Hour}:{time.Minute}:{time.Second}";

	public static string ToHourMinuteString(long totalSeconds) {
		long hours = (totalSeconds % TimeSpan.SecondsPerDay) / TimeSpan.SecondsPerHour;
		long minutes = ((totalSeconds % TimeSpan.SecondsPerDay) % TimeSpan.SecondsPerHour) / TimeSpan.SecondsPerMinute;
		return (hours < 10 ? "0" + Convert.ToString(hours) : Convert.ToString(hours)) + ":" + (minutes < 10 ? "0" + Convert.ToString(minutes) : Convert.ToString(minutes));
	}

	public static string ToDayAndTimeString(DateTime time) =>
		$"{time.Day}.{time.Month}. {time.Hour}:{time.Minute}:{time.Second}";

	public static string ToDayAndMonthAndYearString(DateTime time) =>
		$"{time.Day}.{time.Month}. {time.Year}";

	public static DateTime GetMondayOfCurrentWeek() {
        return FloorWeek(DateTime.Now);
    }

	public static DateTime GetMondayOfWeekAtDate(DateTime date) =>
		FloorWeek(date);

	public static DateTime GetFridayOfCurrentWeek() =>
		GetMondayOfCurrentWeek().AddDays(4);
	
	public static DateTime GetFridayOfWeekAtDate(DateTime date) =>
		GetMondayOfWeekAtDate(date).AddDays(4);

	public static DateTime GetFirstDayOfCurrentMonth() =>
		new(DateTime.Today.Year, DateTime.Today.Month, 1);

	public static DateTime GetFirstDayOfMonthAtDate(DateTime date) =>
		new(date.Year, date.Month, 1);

	public static int GetCurrentWeekCount() =>
		GetWeekCountAtDate(DateTime.Now);

	public static int GetWeekCountAtDate(DateTime date) =>
		(int) Math.Floor((FloorWeek(date) - START_DATE).Days / 7.0) + 1;
}
