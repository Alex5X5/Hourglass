namespace Hourglass.PDF;

using System;
using System.Collections.Generic;
using System.Linq;

using Hourglass.Database.Services.Interfaces;

public partial class HourglassPdf {

	//public static unsafe void ExportUnsafe(IHourglassDbService dbService) {
	//	Console.WriteLine("started expoting");
	//	byte* document = FileManagerUnsafe.LoadInput(out int fileSize);
	//	document = SetUtilityFieldsUnsafe(document);
	//	List<Database.Models.Task> tasks = dbService.QueryTasksOfCurrentWeekAsync().Result;
	//	Dictionary<string, DayOfWeek> days = new Dictionary<string, DayOfWeek> {
	//		{ "monday", DayOfWeek.Monday },
	//		{ "tuesday", DayOfWeek.Tuesday },
	//		{ "wendsday", DayOfWeek.Wednesday },
	//		{ "thursday", DayOfWeek.Thursday },
	//		{ "friday", DayOfWeek.Friday }
	//	};
		
	//	foreach (string dayName in days.Keys) {
	//		int offset = 0;
	//		string[] lines = ["", "", "", "", "", ""];
	//		List<Database.Models.Task> tasks_ = tasks.Where(x=>x.FinishDateTime.DayOfWeek == days[dayName]).ToList();
	//		if(tasks_.Count == 0)
	//			continue;
	//		foreach (Database.Models.Task task in tasks_) {
	//			string[] compiledTask = CompileTask(task);
	//			try {
	//				Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
	//			} catch(IndexOutOfRangeException) {
	//				break;
	//			}
	//			offset += compiledTask.Length;
	//		}
	//		for(int i=0; i<lines.Length; i++) {
	//			string query = $"{dayName}_line_{i + 1}";
	//			document = SetAnnotaionValueUnsafe(document, query, lines[i]);
	//			document = SetFieldValueUnsafe(document, query, lines[i]);
	//		}
	//	}
	//	FileManager.WriteOutput(document);
	//	Console.WriteLine("done exporting");
	//}

	public static string SetUtilityFieldsUnsafe(string document) {
		DateTime startDate = new DateTime(2024, 8, 5);
		DateTime today = DateTime.Today;
		int daysDifference = (today - startDate).Days;
		int weeksPassed = daysDifference / 7;
		int currentWeek = (int)Math.Ceiling(daysDifference / 7.0);
		document = SetFieldValue(document, "count", Convert.ToString(weeksPassed));
		document = SetAnnotaionValue(document, "count", Convert.ToString(weeksPassed));
		return document;
	}
}
