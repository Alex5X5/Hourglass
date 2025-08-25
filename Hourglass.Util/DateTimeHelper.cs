namespace Hourglass.Util; 

public static class DateTimeHelper {
	
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
		}
	}

	public static string ToTimeString(DateTime time) =>
		$"{time.Hour}:{time.Minute}:{time.Second}";

	public static string ToDayAndTimeString(DateTime time) =>
		$"{time.Day}.{time.Month} {time.Hour}:{time.Minute}:{time.Second}";
}
