using Hourglass.Database.Services.Interfaces;

namespace Hourglass.PDF;

public partial class HourglassPdf {

	public static void Export(IHourglassDbService dbService) {
		string document = FileManager.LoadInput();
		List<Database.Models.Task> tasks = dbService.QueryTasksOfCurrentWeekAsync().Result;
		int dayIndex = 1;
		foreach (string dayName in new string[] { "monday", "tuesday", "wendsday", "thursday", "friday" }) {
			int offset = 0;
			string[] lines = ["", "", "", "", "", ""];
			List<Database.Models.Task> tasks_ = tasks.Where(x=>x.FinishDateTime.Day == dayIndex).ToList();
			foreach (Database.Models.Task task in tasks_) {
				string[] compiledTask = CompileTask(task);
				Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
				offset += compiledTask.Length;
			}
		}
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
