namespace Hourglass.PDF;

using Hourglass.Database.Services.Interfaces;
using Hourglass.PDF.Services.Interfaces;
using Hourglass.Util;
using Hourglass.Util.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

public unsafe partial class PdfService : IPdfService, IDisposable {

	public const int MAX_LINE_LENGTH = 85;

	private const string LAST_SECTION_INDEXER = "eof";

	public static bool IndexersLoaded { private set; get; } = false;

	private readonly IHourglassDbService _dbService;

	private readonly Dictionary<string, ValueTuple<IntPtr, IntPtr>> Indexers;

	public Dictionary<string, string> InsertOperations;

	private readonly int charCount;
	private readonly char* text;

	public PdfService(IHourglassDbService dbService) {
		_dbService = dbService;
		InsertOperations = [];
		Indexers = [];
		byte* buffer = FileService.LoadFileUnsafe(PathService.AssetsPath("output-readable-indexers.pdf"), out int inputFileSize);
		text = FileService.DecodeBufferAnsi(buffer, inputFileSize, out int _charCount);
		charCount = _charCount;
		NativeMemory.Free(buffer);
		new Thread(LoadIndexers).Start();
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
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

	private bool WaitForIndexing() {
		int counter = 0;
		while (counter < 10) {
			if (IndexersLoaded)
				return true;
			Thread.Sleep(200);
		}
		return false;
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

	public void Export(IProgressReporter progressReporter, DateTime selectedWeek) {
		if (!IndexersLoaded)
			return;
		Stopwatch totalStopwatch = new();
		totalStopwatch.Start();
		Console.WriteLine("started expoting");
		selectedWeek = DateTimeService.FloorWeek(selectedWeek);
		Stopwatch prepareContentStopwatch = new();
		prepareContentStopwatch.Start();
		Console.WriteLine("started preparing content for the document");
		List<Database.Models.Task> tasks = _dbService.QueryTasksOfWeekAtDateAsync(selectedWeek).Result;
		Dictionary<string, DayOfWeek> days = new Dictionary<string, DayOfWeek> {
			{ "monday", DayOfWeek.Monday },
			{ "tuesday", DayOfWeek.Tuesday },
			{ "wendsday", DayOfWeek.Wednesday },
			{ "thursday", DayOfWeek.Thursday },
			{ "friday", DayOfWeek.Friday }
		};
		long totalWeekSeconds = 0;
		string query = "";
		string value = "";
		const int progressUpdatesPerTask = 1;
		int totalSteps = tasks.Count * progressUpdatesPerTask;
		int currentStep = 0;
		int percentage = 0;
		foreach (string dayName in days.Keys) {
			int offset = 0;
			string[] lines = ["", "", "", "", "", ""];
			List<Database.Models.Task> tasks_ = tasks.Where(x => x.FinishDateTime.DayOfWeek == days[dayName]).ToList();
			if (tasks_.Count == 0)
				continue;
			foreach (Database.Models.Task task in tasks_) {
				if (progressReporter.IsCancellationRequested) {
					progressReporter.ReportProgress(currentStep, "Cancelling...");
					Thread.Sleep(500);
					return;
				}
				if (task.running)
					continue;
				string[] compiledTask = CompileTask(task);
				try {
					Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
					query = $"{dayName}_hour_range_{offset + 1}";
					value = DateTimeService.ToTimeString(task.StartDateTime) + " - " + DateTimeService.ToTimeString(task.FinishDateTime);
					BufferAnnotationValueUnsafe(query, value);
					BufferFieldValueUnsafe(query, value);
					query = $"{dayName}_hour_{offset + 1}";
					value = DateTimeService.ToHourMinuteString(task.finish-task.start);
					BufferAnnotationValueUnsafe(query, value);
					BufferFieldValueUnsafe(query, value);
					offset += compiledTask.Length;
				} catch (ArgumentOutOfRangeException) {
					Console.WriteLine($"ran out of empty lines while inserting {compiledTask.Length} lines for day {dayName}");
					Console.WriteLine($"description of task was:'{task.description}'");
					break;
				} catch (ArgumentException) {
					Console.WriteLine($"ran out of empty lines while inserting {compiledTask.Length} lines for day {dayName}");
					Console.WriteLine($"description of task was:'{task.description}'");
					break;
				}
				totalWeekSeconds += task.finish - task.start;
				currentStep++;
				percentage = (int)(currentStep * 100.0 / totalSteps);
				progressReporter.ReportProgress(percentage, $"Processing day {dayName}...");
			}
			for (int i = 0; i < lines.Length; i++) {
				query = $"{dayName}_line_{i + 1}";
				BufferAnnotationValueUnsafe(query, lines[i]);
				BufferFieldValueUnsafe(query, lines[i]);
			}
		}
		query = $"total_hour";
		value = DateTimeService.ToHourMinuteString(totalWeekSeconds);
		BufferAnnotationValueUnsafe(query, value);
		BufferFieldValueUnsafe(query, value);
		SetUtilityFields(selectedWeek);
		prepareContentStopwatch.Stop();
		Console.Write("finished preparing content for the document\n");
		Console.WriteLine($"preparing content took {prepareContentStopwatch.ElapsedMilliseconds / 1000.0} seconds");
		char* document = BuildDocument(out int documentCharCount);
		byte* resultFile = FileService.EncodeBufferAnsi(document, documentCharCount, out int fileSize);
		FileService.WriteFileUnsafe(resultFile, PathService.FilesPath($"Nachweise/{GetNewFileName(selectedWeek)}"), fileSize);
		NativeMemory.Free(resultFile);
		NativeMemory.Free(document);
		InsertOperations.Clear();
		totalStopwatch.Stop();
		Console.Write("finished exporting unsafe\n");
		Console.WriteLine($"exporting took {totalStopwatch.ElapsedMilliseconds / 1000.0} seconds");
		progressReporter.ReportProgress(100, "finished exporting");
	}



	public void Export(DateTime selectedWeek) {
		if (!IndexersLoaded)
			if(!WaitForIndexing())
				return;
		Stopwatch totalStopwatch = new();
		totalStopwatch.Start();
		Console.WriteLine("started expoting");
		selectedWeek = DateTimeService.FloorWeek(selectedWeek);
		Stopwatch prepareContentStopwatch = new();
		prepareContentStopwatch.Start();
		Console.WriteLine("started preparing content for the document");
		List<Database.Models.Task> tasks = _dbService.QueryTasksOfWeekAtDateAsync(selectedWeek).Result;
		Dictionary<string, DayOfWeek> days = new Dictionary<string, DayOfWeek> {
			{ "monday", DayOfWeek.Monday },
			{ "tuesday", DayOfWeek.Tuesday },
			{ "wendsday", DayOfWeek.Wednesday },
			{ "thursday", DayOfWeek.Thursday },
			{ "friday", DayOfWeek.Friday }
		};
		long totalWeekSeconds = 0;
		string query = "";
		string value = "";
		const int progressUpdatesPerTask = 1;
		int totalSteps = tasks.Count * progressUpdatesPerTask;
		int currentStep = 0;
		int percentage = 0;
		foreach (string dayName in days.Keys) {
			int offset = 0;
			string[] lines = ["", "", "", "", "", ""];
			List<Database.Models.Task> tasks_ = tasks.Where(x => x.FinishDateTime.DayOfWeek == days[dayName]).ToList();
			if (tasks_.Count == 0)
				continue;
			foreach (Database.Models.Task task in tasks_) {
				if (task.running)
					continue;
				string[] compiledTask = CompileTask(task);
				try {
					Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
					query = $"{dayName}_hour_range_{offset + 1}";
					value = DateTimeService.ToHourMinuteString(task.start) + " - " + DateTimeService.ToHourMinuteString(task.finish);
					BufferAnnotationValueUnsafe(query, value);
					BufferFieldValueUnsafe(query, value);
					query = $"{dayName}_hour_{offset + 1}";
					value = DateTimeService.ToHourMinuteString(task.finish - task.start);
					BufferAnnotationValueUnsafe(query, value);
					BufferFieldValueUnsafe(query, value);
					offset += compiledTask.Length;
				} catch (ArgumentOutOfRangeException) {
					Console.WriteLine($"ran out of empty lines while inserting {compiledTask.Length} lines for day {dayName}");
					Console.WriteLine($"description of task was:'{task.description}'");
					break;
				} catch (ArgumentException) {
					Console.WriteLine($"ran out of empty lines while inserting {compiledTask.Length} lines for day {dayName}");
					Console.WriteLine($"description of task was:'{task.description}'");
					break;
				}
				totalWeekSeconds += task.finish - task.start;
				currentStep++;
				percentage = (int)(currentStep * 100.0 / totalSteps);
			}
			for (int i = 0; i < lines.Length; i++) {
				query = $"{dayName}_line_{i + 1}";
				BufferAnnotationValueUnsafe(query, lines[i]);
				BufferFieldValueUnsafe(query, lines[i]);
			}
		}
		query = $"total_hour";
		value = DateTimeService.ToHourMinuteString(totalWeekSeconds);
		BufferAnnotationValueUnsafe(query, value);
		BufferFieldValueUnsafe(query, value);
		SetUtilityFields(selectedWeek);
		prepareContentStopwatch.Stop();
		Console.Write("finished preparing content for the document\n");
		Console.WriteLine($"preparing content took {prepareContentStopwatch.ElapsedMilliseconds / 1000.0} seconds");
		char* document = BuildDocument(out int documentCharCount);
		byte* resultFile = FileService.EncodeBufferAnsi(document, documentCharCount, out int fileSize);
		FileService.WriteFileUnsafe(resultFile, PathService.FilesPath($"Nachweise/{GetNewFileName(selectedWeek)}"), fileSize);
		NativeMemory.Free(resultFile);
		NativeMemory.Free(document);
		InsertOperations.Clear();
		totalStopwatch.Stop();
		Console.Write("finished exporting unsafe\n");
		Console.WriteLine($"exporting took {totalStopwatch.ElapsedMilliseconds / 1000.0} seconds");
	}

	public PdfDocumentData? GetExportData(DateTime selectedWeek) {
		if (!IndexersLoaded)
			if (!WaitForIndexing())
				return null;
		PdfDocumentData data = new PdfDocumentData();
		List<Database.Models.Task> tasks = _dbService.QueryTasksOfWeekAtDateAsync(selectedWeek).Result;
		Dictionary<string, DayOfWeek> days = new Dictionary<string, DayOfWeek> {
			{ "monday", DayOfWeek.Monday },
			{ "tuesday", DayOfWeek.Tuesday },
			{ "wendsday", DayOfWeek.Wednesday },
			{ "thursday", DayOfWeek.Thursday },
			{ "friday", DayOfWeek.Friday }
		};
		long totalWeekSeconds = 0;
		int dayCounter = 0;
		foreach (string dayName in days.Keys) {
			int offset = 0;
			string[] lines = ["", "", "", "", "", ""];
			string time = "";
			string hours = "";
			List<Database.Models.Task> tasks_ = tasks.Where(x => x.FinishDateTime.DayOfWeek == days[dayName]).ToList();
			if (tasks_.Count == 0)
				continue;
			foreach (Database.Models.Task task in tasks_) {
				if (task.running)
					continue;
				string[] compiledTask = CompileTaskPreview(task);
				try {
					Array.ConstrainedCopy(compiledTask, 0, lines, offset, compiledTask.Length);
					offset += compiledTask.Length;
					time = DateTimeService.ToHourMinuteString(task.start) + " - " + DateTimeService.ToHourMinuteString(task.finish);
					hours = DateTimeService.ToHourMinuteString(task.finish - task.start);
				} catch (ArgumentOutOfRangeException) {
					Console.WriteLine($"ran out of empty lines while inserting {compiledTask.Length} lines for day {dayName}");
					Console.WriteLine($"description of task was:'{task.description}'");
					break;
				} catch (ArgumentException) {
					Console.WriteLine($"ran out of empty lines while inserting {compiledTask.Length} lines for day {dayName}");
					Console.WriteLine($"description of task was:'{task.description}'");
					break;
				}
				totalWeekSeconds += task.finish - task.start;
			}
			for (int i = 0; i < 6; i++)
				data.Data[dayCounter * PdfDocumentData.DAY_LINE_COUNT + i][0] = lines[i]; 
			dayCounter++;
		}
		data.TotalTime = DateTimeService.ToHourMinuteString(totalWeekSeconds);
		data.Week = Convert.ToString(DateTimeService.GetWeekCountAtDate(selectedWeek));
		data.UserName = SettingsService.TryGetSetting(SettingsService.USER_NAME_KEY) ?? "username";
		data.JobName = SettingsService.TryGetSetting(SettingsService.JOB_NAME_KEY) ?? "job name";
        DateTime dayFrom = DateTimeService.FloorWeek(selectedWeek);
        DateTime dayTo = dayFrom.AddDays(5);
        data.DateFrom = $"{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}";
		data.DateTo = $"{dayTo.Day}.{dayTo.Month}. {dayTo.Year}";
		return data;
	}

	public void Import() {
		throw new NotImplementedException();
	}

	public static string[] CompileTask(Database.Models.Task task) {
		string source = "";
		List<string> res = [];
		if (task.project != null)
			source += $"{task.project.Name}: ";
		if (task.ticket != null)
			source += $"{task.ticket.name}: ";
		source += task.description;
		while (source.Length > 0) {
			int CharacterRemoveCount;
			if (source.Length >= MAX_LINE_LENGTH) {
				CharacterRemoveCount = MAX_LINE_LENGTH;
				while (source[CharacterRemoveCount] != ' ')
					CharacterRemoveCount--;
			}
			else
				CharacterRemoveCount = source.Length;
			res.Add((res.Count > 0 ? "     ":"") + source[..(source[CharacterRemoveCount-1]==' '? CharacterRemoveCount-1 : CharacterRemoveCount)]);
			source = source[CharacterRemoveCount..source.Length];
		}
		return res.ToArray();
	}

    public static string[] CompileTaskPreview(Database.Models.Task task) {
        string source = "";
        List<string> res = [];
        if (task.project != null)
            source += $"{task.project.Name}: ";
        if (task.ticket != null)
            source += $"{task.ticket.name}: ";
        source += task.description;
        while (source.Length > 0) {
            int CharacterRemoveCount;
            if (source.Length >= MAX_LINE_LENGTH) {
                CharacterRemoveCount = MAX_LINE_LENGTH;
                while (source[CharacterRemoveCount] != ' ')
                    CharacterRemoveCount--;
            } else
                CharacterRemoveCount = source.Length;
            res.Add((res.Count > 0 ? "     " : "") + source[..(source[CharacterRemoveCount - 1] == ' ' ? CharacterRemoveCount - 1 : CharacterRemoveCount)]);
            source = source[CharacterRemoveCount..source.Length];
        }
        return res.ToArray();
    }


    private void SetUtilityFields(DateTime selectedWeek) {
		BufferAnnotationValueUnsafe("week", Convert.ToString(DateTimeService.GetWeekCountAtDate(selectedWeek)));
		BufferFieldValueUnsafe("week", Convert.ToString(DateTimeService.GetWeekCountAtDate(selectedWeek)));
		BufferAnnotationValueUnsafe("name", SettingsService.TryGetSetting(SettingsService.USER_NAME_KEY) ?? "username");
		BufferFieldValueUnsafe("name", SettingsService.TryGetSetting(SettingsService.USER_NAME_KEY) ?? "username");
		BufferAnnotationValueUnsafe("job", SettingsService.TryGetSetting(SettingsService.JOB_NAME_KEY) ?? "job name");
		BufferFieldValueUnsafe("job", SettingsService.TryGetSetting(SettingsService.JOB_NAME_KEY) ?? "job name");
		DateTime dayFrom = DateTimeService.GetMondayOfWeekAtDate(selectedWeek);
		DateTime dayTo = DateTimeService.GetMondayOfWeekAtDate(selectedWeek);
		BufferAnnotationValueUnsafe("date_from", $"{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}");
		BufferFieldValueUnsafe("date_from", $"{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}");
		BufferAnnotationValueUnsafe("date_to", $"{dayTo.Day}.{dayTo.Month}. {dayTo.Year}");
		BufferFieldValueUnsafe("date_to", $"{dayTo.Day}.{dayTo.Month}. {dayTo.Year}");
	}

	private string GetNewFileName(DateTime selectedWeek) {
		DateTime dayFrom = DateTimeService.GetMondayOfWeekAtDate(selectedWeek);
		DateTime dayTo = DateTimeService.GetFridayOfWeekAtDate(selectedWeek);
		string path = $"Ausbildungsnachweis{DateTimeService.GetWeekCountAtDate(selectedWeek)}_{dayFrom.Day}.{dayFrom.Month}. {dayFrom.Year}-{dayTo.Day}.{dayTo.Month}. {dayTo.Year}.pdf";
		Console.WriteLine($"generated file path:{path}");
		return path;
	}
}

public class PdfDocumentData {

	public const int DAY_LINE_COUNT = 6;
	public const int WEEK_LINE_COUNT = 5 * DAY_LINE_COUNT;
	public const int DOCUMENT_FIELD_COUNT = WEEK_LINE_COUNT + 9;

	public const int USER_NAME_INDEX = WEEK_LINE_COUNT;
	public const int JOB_NAME_INDEX = WEEK_LINE_COUNT + 1;
	public const int WEEK_INDEX = WEEK_LINE_COUNT + 2;
	public const int DATE_FOM_INDEX = WEEK_LINE_COUNT + 3;
	public const int DATE_TO_INDEX = WEEK_LINE_COUNT + 4;
	public const int SICK_DAYS_INDEX = WEEK_LINE_COUNT + 5;
	public const int MISSING_DAYS_INDEX = WEEK_LINE_COUNT + 6;
	public const int TOTAL_MISSING_DAYS_INDEX = WEEK_LINE_COUNT + 7;
	public const int TOTAL_TIME_INDEX = WEEK_LINE_COUNT + 8;

	public const int TASK_DESCRIPTION_COLUMN = 0;
	public const int HOUR_COLUMN = 1;
	public const int HOUR_RANGE_COLUMN = 2;
	public const int UTILITY_DATA_COLUMN = 0;


	public string[][] Data = new string[DOCUMENT_FIELD_COUNT][];

	public Tuple<string, string, string>[] TaskData {
		get {
			List<Tuple<string, string, string>> l = [];
			for (int i = 0; i < Data.GetLength(0); i++) {
				l.Add(
					new Tuple<string, string, string>(
						Data[i][TASK_DESCRIPTION_COLUMN],
						Data[i][HOUR_COLUMN],
						Data[i][HOUR_RANGE_COLUMN]
					)
				);
			}
			return l.ToArray();
		}
	}

	public string UserName {
		set => Data[USER_NAME_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[USER_NAME_INDEX][UTILITY_DATA_COLUMN];
	}
	public string JobName {
		set => Data[JOB_NAME_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[JOB_NAME_INDEX][UTILITY_DATA_COLUMN];
	}
	public string Week {
		set => Data[WEEK_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[WEEK_INDEX][UTILITY_DATA_COLUMN];
	}
	public string DateFrom {
		set => Data[DATE_FOM_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[DATE_FOM_INDEX][UTILITY_DATA_COLUMN];
	}
	public string DateTo {
		set => Data[DATE_TO_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[DATE_TO_INDEX][UTILITY_DATA_COLUMN];
	}
	public string SickDays {
		set => Data[SICK_DAYS_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[SICK_DAYS_INDEX][UTILITY_DATA_COLUMN];
	}
	public string MmissingDays {
		set => Data[MISSING_DAYS_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[MISSING_DAYS_INDEX][UTILITY_DATA_COLUMN];
	}
	public string TotalMissingDays {
		set => Data[TOTAL_MISSING_DAYS_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[TOTAL_MISSING_DAYS_INDEX][UTILITY_DATA_COLUMN];
	}
	public string TotalTime {
		set => Data[TOTAL_TIME_INDEX][UTILITY_DATA_COLUMN] = value;
		get => Data[TOTAL_TIME_INDEX][UTILITY_DATA_COLUMN];
	}

	public PdfDocumentData() {
		for (int i = 0; i < DOCUMENT_FIELD_COUNT; i++)
			Data[i] = ["", "", "", ""];
	}
}