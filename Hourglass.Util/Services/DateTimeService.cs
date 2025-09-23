namespace Hourglass.Util;

using Hourglass.Util.Services;
using System;

public static class DateTimeService {

	public static DateTime START_DATE = GetStartDate();

	static DateTimeService() {
		SettingsService.OnSettingsReload += 
			() => START_DATE = GetStartDate();
	}

	private static DateTime GetStartDate() {
		try {
			var val = Convert.ToDateTime(SettingsService.GetSetting(SettingsService.START_DATE_KEY));
			return val;
		} catch (FormatException) {
			var val = new DateTime(2024, 8, 5);
			return val;
		}
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

	public static string ToTimeString(DateTime time) =>
		$"{time.Hour}:{time.Minute}:{time.Second}";

	public static string ToHourMinuteString(long totalSeconds) {
		long hours = totalSeconds / TimeSpan.SecondsPerHour;
		long minutes = (long)Math.Ceiling((totalSeconds % TimeSpan.SecondsPerHour) / (float)TimeSpan.SecondsPerMinute);
		return (hours < 10 ? "0" + Convert.ToString(hours) : Convert.ToString(hours)) + ":" + (minutes < 10 ? "0" + Convert.ToString(minutes) : Convert.ToString(minutes));
	}

	public static string ToDayAndTimeString(DateTime time) =>
		$"{time.Day}.{time.Month} {time.Hour}:{time.Minute}:{time.Second}";

	public static DateTime GetMondayOfCurrentWeek() {
		DateTime today = DateTime.Today;
		int daysSinceMonday = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
		return today.AddDays(-daysSinceMonday);
	}

	public static DateTime GetMondayOfWeekAtDate(DateTime date) {
		int daysSinceMonday = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
		return date.AddDays(-daysSinceMonday);
	}
	
	public static DateTime GetFridayOfCurrentWeek() =>
		GetMondayOfCurrentWeek().AddDays(4);
	
	public static DateTime GetFridayOfWeekAtDate(DateTime date) =>
		GetMondayOfWeekAtDate(date).AddDays(4);

	public static DateTime GetFirstDayOfCurrentMonth() =>
		new(DateTime.Today.Year, DateTime.Today.Month, 1);

	public static DateTime GetFirstDayOfMonthAtDate(DateTime date) =>
		new(date.Year, date.Month, 1);

	public static int GetCurrentWeekCount() =>
		(int) Math.Floor((DateTime.Today - START_DATE).Days / 7.0) + 1;

	public static int GetWeekCountAtDate(DateTime date) =>
		(int) Math.Floor((DateTime.Today - date).Days / 7.0) + 1;
}
