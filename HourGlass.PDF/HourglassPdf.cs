using Hourglass.Database.Services.Interfaces;

namespace Hourglass.PDF;

public partial class HourglassPdf {

	public static void Export(IHourglassDbService dbService) {
		Console.WriteLine("started expoting");
		string document = FileManager.LoadInput();
		List<Database.Models.Task> tasks = dbService.QueryTasksOfCurrentWeekAsync().Result;
		int dayIndex = 1;

        Dictionary<string, DayOfWeek> days = new Dictionary<string, DayOfWeek> {
            { "monday", DayOfWeek.Monday },
            { "tuesday", DayOfWeek.Tuesday },
            { "wendsday", DayOfWeek.Wednesday },
            { "thursday", DayOfWeek.Thursday },
            { "friday", DayOfWeek.Friday }
        };
		
		foreach (string dayName in days.Keys) {
			int offset = 0;
			string[] lines = ["", "", "", "", "", ""];
			List<Database.Models.Task> tasks_ = tasks.Where(x=>x.FinishDateTime.DayOfWeek == days[dayName]).ToList();
			if(tasks_.Count == 0)
				continue;
			foreach (Database.Models.Task task in tasks_) {
				string[] compiledTask = CompileTask(task);
				try {
					Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
				} catch(IndexOutOfRangeException) {
					break;
				}
				offset += compiledTask.Length;
			}
			for(int i=0; i<lines.Length; i++) {
				string query = $"{dayName}_line_{i + 1}";
                document = SetAnnotaionValue(document, query, lines[i]);
                document = SetFieldValue(document, query, lines[i]);
			}
		}
		FileManager.WriteOutput(document);
		Console.WriteLine("done exporting");
	}

	public static string[] CompileTask(Database.Models.Task task) {
		const int MAX_LINE_LENGTH = 85;
		int i = 0;
		string source = "";
		List<string> res = [];
		if (task.project != null)
			source += $"{task.project.Name}: ";
		if (task.ticket != null)
			source += $"{task.ticket.name}: ";
		source += task.description;
		while (source.Length > 0) {
			int CharacterRemoveCount;
			if (source.Length >= MAX_LINE_LENGTH)
				CharacterRemoveCount = MAX_LINE_LENGTH;
			else
				CharacterRemoveCount = source.Length;
			res.Add(source[..CharacterRemoveCount]);
			source = source[CharacterRemoveCount..source.Length];
		}
		return res.ToArray();
	}
}
