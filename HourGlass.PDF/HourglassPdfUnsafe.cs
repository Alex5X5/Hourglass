namespace Hourglass.PDF;

using Hourglass.Database.Services.Interfaces;
using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

public unsafe partial class HourglassPdfUnsafe {

	private const string LAST_SECTION_INDEXER = "eof";

	public static bool IndexersLoaded { private set; get; } = false;

	IHourglassDbService _dbService;

	private static Dictionary<string, ValueTuple<IntPtr, IntPtr>> Indexers;

	public Dictionary<string, string> InsertOperations;

	private readonly int charCount;
	private readonly char* text;

	public HourglassPdfUnsafe(IHourglassDbService dbService) {
		_dbService = dbService;
		InsertOperations = [];
		Indexers = [];
		byte* buffer = LoadInputUnsafe(out int inputFileSize);
		text = DecodeBuffer(buffer, inputFileSize, out int _charCount);
		charCount = _charCount;
		NativeMemory.Free(buffer);
		new Thread(LoadIndexers).Start();
		//LoadIndexers();
	}

	public void Dispose() {
		NativeMemory.Free(text);
	}

	private void PrintCharsBefore(char* ptr) {
        Console.Write($"chars before insert pos:");
        char* charBefore = ptr - 25;
        for (int i = 0; i < 25; i++) {
            Console.Write($"{*charBefore}");
            charBefore++;
        }
        Console.WriteLine();
    }

	public void LoadIndexers() {
		Console.WriteLine("loading indexers");
		Stopwatch stopwatch = new();
		stopwatch.Start();
		int i = 0;
		char* _content = text;
		int annotationCount = 0;
		int fieldCount = 0;
		char* previouslastSectionCharacter = text;
		while (i < charCount) {
			if (_content[0] == '%' && _content[1] == '%' && _content[2] == 'i' && _content[3] == 'n' && _content[4] == 'd' && _content[5] == 'e' && _content[6] == 'x') {
				string key = Convert.ToString(*_content);
				while (true) {
					_content++;
					i++;
					if (*_content == ' ' | *_content == '\n' | *_content == '\r' | i >= charCount)
						break;
					key += Convert.ToString(*_content);
				}
				_content++;
				i++;
				if (_content[0] == '/' && _content[1] == 'V' && _content[2] == ' ' && _content[3] == '(' && _content[4] == ')') {
					annotationCount++;
					_content += 4;
					i += 4;
					Indexers[key] = ((IntPtr)previouslastSectionCharacter, (IntPtr)_content);
					previouslastSectionCharacter = _content;
                } else if (_content[0] == '(' && _content[1] == ')' && _content[2] == ' ' && _content[3] == 'T' && _content[4] == 'j') {
					fieldCount++;
					_content += 1;
					i += 1;
					Indexers[key] = ((IntPtr)previouslastSectionCharacter, (IntPtr)_content);
					previouslastSectionCharacter = _content;
				}
			}
			_content++;
			i++;
		}
        Indexers[LAST_SECTION_INDEXER] = ((IntPtr)previouslastSectionCharacter, (IntPtr)(text+charCount));
		IndexersLoaded = true;
		stopwatch.Stop();
		Console.WriteLine("finished loading indexers");
		Console.WriteLine($"loading indexers took {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
	}

    public void Export() {
		if (!IndexersLoaded)
			return;
		Stopwatch stopwatch = new();
		stopwatch.Start();
        Console.WriteLine("started expoting unsafe");
        List<Database.Models.Task> tasks = _dbService.QueryTasksOfCurrentWeekAsync().Result;
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
            List<Database.Models.Task> tasks_ = tasks.Where(x => x.FinishDateTime.DayOfWeek == days[dayName]).ToList();
            if (tasks_.Count == 0)
                continue;
            foreach (Database.Models.Task task in tasks_) {
                string[] compiledTask = CompileTask(task);
                try {
                    Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
                } catch (IndexOutOfRangeException) {
                    break;
                }
                offset += compiledTask.Length;
            }
            for (int i = 0; i < lines.Length; i++) {
                string query = $"{dayName}_line_{i + 1}";
				BufferAnnotationValueUnsafe(query, lines[i]);
				BufferFieldValueUnsafe(query, lines[i]);
            }
        }
		SetUtilityFields();
		char* document = BuildDocument(out int documentCharCount);
		byte* resultFile = EncodeBuffer(document, documentCharCount, out int fileSize);
		WriteOutputUnsafe(resultFile, Paths.FilesPath($"Nachweise/{GetNewFileName()}"), fileSize);
        NativeMemory.Free(resultFile);
        NativeMemory.Free(document);
        InsertOperations.Clear();
        stopwatch.Stop();
		Console.WriteLine("finished exporting unsafe");
		Console.WriteLine($"exporting took {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
    }

	public static string[] CompileTask(Database.Models.Task task) {
		const int MAX_LINE_LENGTH = 85;
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
			res.Add((res.Count > 0 ? "     ":"") + source[..CharacterRemoveCount]);
			source = source[CharacterRemoveCount..source.Length];
		}
		return res.ToArray();
	}

	private void SetUtilityFields() {
		int daysDifference = (DateTime.Today - DateTimeHelper.START_DATE).Days;
		int currentWeek = (int)Math.Ceiling(daysDifference / 7.0);
        BufferAnnotationValueUnsafe("week", Convert.ToString(currentWeek));
        BufferFieldValueUnsafe("week", Convert.ToString(currentWeek));
		DateTime dayFrom = DateTimeHelper.GetMondayOfCurrentWeek();
		DateTime dayTo = DateTimeHelper.GetFridayOfCurrentWeek();
        BufferAnnotationValueUnsafe("date_from", $"{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}");
        BufferFieldValueUnsafe("date_from", $"{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}");
        BufferAnnotationValueUnsafe("date_to", $"{dayTo.Day}.{dayTo.Month}. {dayTo.Year}");
        BufferFieldValueUnsafe("date_to", $"{dayTo.Day}.{dayTo.Month}. {dayTo.Year}");
    }

	private string GetNewFileName() {
        DateTime dayFrom = DateTimeHelper.GetMondayOfCurrentWeek();
        DateTime dayTo = DateTimeHelper.GetFridayOfCurrentWeek();
		return $"Ausbildungsnachweis{DateTimeHelper.GetWeekCountSinceStart()}_{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}-{dayTo.Day}.{dayTo.Month}. {dayTo.Year}.pdf";
    }

}