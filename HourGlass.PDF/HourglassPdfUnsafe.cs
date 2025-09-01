namespace Hourglass.PDF;

using Hourglass.Database.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

public unsafe partial class HourglassPdfUnsafe {

	public static bool IndexersLoaded {
		private set; get;
	}

	IHourglassDbService _dbService;

	private static Dictionary<string, ValueTuple<IntPtr, IntPtr>> Indexers;

	public Dictionary<string, string> InsertOperations;

	private int charCount;
	private char* text;

	public HourglassPdfUnsafe(IHourglassDbService dbService) {
		_dbService = dbService;
		InsertOperations = [];
		byte* buffer = LoadInputUnsafe(out int inputFileSize);
		text = DecodeBuffer(buffer, inputFileSize, out int _charCount);
		charCount = _charCount;
		NativeMemory.Free(buffer);
		FindIndexers();
		//int fileSize = _fileSize;
	}

	public void Dispose() {
		throw new NotImplementedException();
	}

	public void FindIndexers() {
		Indexers ??= new();
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
					Console.WriteLine($"annotation:{key} annotationCount:{annotationCount}  pos:{(nuint)_content} offset:{(nuint)_content - (nuint)text}");
					_content += 5;
					i += 5;
					Indexers[key] = ((IntPtr)previouslastSectionCharacter, (IntPtr)_content);
					previouslastSectionCharacter = _content;
				} else if (_content[0] == '(' && _content[1] == ')' && _content[2] == ' ' && _content[3] == 'T' && _content[4] == 'j') {
					fieldCount++;
					Console.WriteLine($"field:{key} fieldCount:{fieldCount} pos:{(nuint)_content} offset:{(nuint)_content-(nuint)text}");
					_content += 0;
					i += 0;
					Indexers[key] = ((IntPtr)previouslastSectionCharacter, (IntPtr)_content);
					previouslastSectionCharacter = _content;
				} else {
					//Indexers[" "] = ((IntPtr)previouslastSectionCharacter, (nint)text + (nint)charCount*sizeof(char));
				}
				//Indexers.Add(key.TrimEnd('\r', '\n'), content_);
				//content_++;
			}
			_content++;
			i++;
		}
	}

	public void Export() {
        Console.WriteLine("started expoting unsafe");
        //string document = FileManager.LoadInput();
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
				//document = SetUtilityFieldsUnsafe(document);
				//InsertOperations["query"] = lines[i];
				BufferAnnotationValueUnsafe(query, lines[i]);
				BufferFieldValueUnsafe(query, lines[i]);
                //document = SetAnnotaionValue(document, query, lines[i]);
                //document = SetFieldValue(document, query, lines[i]);
            }
        }
		FlushDocument();
        Console.WriteLine("done exporting");
    }

	public void FlushDocument() {
		int totalInsertionTextLenth = charCount;
		foreach (string key in InsertOperations.Keys)
			totalInsertionTextLenth += InsertOperations[key].Length;
		//int bufferSize = totalInsertionTextLenth * sizeof(char);
		char* buffer = (char*)NativeMemory.Alloc((uint)(totalInsertionTextLenth * sizeof(char)));
		char* source = text;
		char* _buffer = buffer;
		foreach (string key in Indexers.Keys) {
			//nuint charCopyCount = (nuint)(Indexers[key].Item2 - Indexers[key].Item1);
			//NativeMemory.Copy(
			//	(void*)Indexers[key].Item1,
			//	_buffer,
			//	(nuint)(Indexers[key].Item2 - Indexers[key].Item1)
			//);
			uint souceCharCopyCount = (uint)Indexers[key].Item2 - (uint)Indexers[key].Item1;
			uint sourceByteCopyCount = souceCharCopyCount * sizeof(char);

            Console.WriteLine($"Offset1:{(nuint)_buffer - (nuint)buffer} total size:{totalInsertionTextLenth}");
			NativeMemory.Copy((byte*)Indexers[key].Item1, _buffer, sourceByteCopyCount);
			_buffer += souceCharCopyCount;
			Console.WriteLine($"Offset2:{(nuint)_buffer - (nuint)buffer} total size:{totalInsertionTextLenth}");
			InsertOperations.TryGetValue(key, out string? val);
			if (val != null) {
                Console.WriteLine($"Inserting\'{InsertOperations[key]}\' at document position:{(nuint)_buffer - (nuint)buffer} at memory position:\'{(nuint)_buffer}\' for key:\'{key}\'");
				string s = InsertOperations[key];
				if(s == "")
					continue;
				int charInsertCount = s.Length;
				uint byteInsertCount = (uint) (charInsertCount * sizeof(char));
				fixed (char* insert = &InsertOperations[key].ToCharArray()[0]) {
					Console.WriteLine($"copying {charInsertCount} chars or {byteInsertCount} bytes");
					NativeMemory.Copy(insert, _buffer, byteInsertCount);
					_buffer += charInsertCount;
				}
					//NativeMemory.Copy(chars, _buffer, (nuint)(charInsertCount * sizeof(char)));
				_buffer += charInsertCount;
			}

			//int copyCount = (int)(Indexers[key].Item2 -Indexers[key].Item1);

		}
        //);
        uint charCopyCount = (uint)Indexers[Indexers.Keys.ElementAt(Indexers.Keys.Count-1)].Item2 - (uint)Indexers[Indexers.Keys.ElementAt(Indexers.Keys.Count - 1)].Item1;
        uint byteCopyCount = charCopyCount * sizeof(char);

        Console.WriteLine($"Offset1:{(nuint)_buffer - (nuint)buffer} total size:{totalInsertionTextLenth}");
        NativeMemory.Copy((byte*)Indexers[Indexers.Keys.ElementAt(Indexers.Keys.Count - 1)].Item1, _buffer, byteCopyCount);
        //_buffer += byteCopyCount;

        byte* file = EncodeBuffer(buffer, totalInsertionTextLenth, out int fileSize);
		WriteOutputUnsafe(file, fileSize);
		NativeMemory.Free(file);
		NativeMemory.Free(buffer);
		InsertOperations.Clear();
        Console.WriteLine("flushed document");
        //while (i < charCount) {
        //	if (source[0] == '%' && source[1] == '%' && source[2] == 'i' && source[3] == 'n' && source[4] == 'd' && source[5] == 'e' && source[6] == 'x') {
        //		string key = Convert.ToString(*source);
        //		while (true) {
        //			source++;
        //			i++;
        //			if (*source == ' ' | *source == '\n' | *source == '\r')
        //				break;
        //			key += Convert.ToString(*source);
        //		}
        //		if (content_[0] == '/' && content_[1] == 'V' && content_[2] == ' ' && source[3] == '(' && content_[4] == ')') {
        //			annotationCount++;
        //			Console.WriteLine($"annotation:{key} {annotationCount} {(IntPtr)content_}");
        //			content_ += 3;
        //			Indexers[key] = ((IntPtr)lastSectionCharacter, (IntPtr)content_);
        //			lastSectionCharacter = content_;
        //		} else {
        //			fieldCount++;
        //			Console.WriteLine($"field:{key} {fieldCount} {(IntPtr)content_}");
        //			Indexers[key] = ((IntPtr)lastSectionCharacter, (IntPtr)content_);
        //			lastSectionCharacter = content_;

        //		}
        //		//Indexers.Add(key.TrimEnd('\r', '\n'), content_);
        //		//content_++;
        //	}
        //	content_++;
        //	i++;
        //}

        /* 
			Todo:
				inserting annotations/fields caches the new value to a dict with the indexer as the key

				flushing the cached insertion operations takes the amount of memory of the source document,
				allocates new memory of the size of the source document and all insertion values added,
				copies the document character by character and if it finds an indexer,
				appends the insertion value and continiues.
		 */
    }


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
			res.Add((res.Count > 0 ? "     ":"") + source[..CharacterRemoveCount]);
			source = source[CharacterRemoveCount..source.Length];
		}
		return res.ToArray();
	}

	public static string SetUtilityFieldsUnsafe(string document) {
		DateTime startDate = new DateTime(2024, 8, 5);
		DateTime today = DateTime.Today;
		int daysDifference = (today - startDate).Days;
		int weeksPassed = daysDifference / 7;
		int currentWeek = (int)Math.Ceiling(daysDifference / 7.0);
		//document = SetFieldValue(document, "count", Convert.ToString(weeksPassed));
		//document = SetAnnotaionValue(document, "count", Convert.ToString(weeksPassed));
		return document;
	}
}

internal unsafe struct DocumetFragmentInformation {

	public DocumetFragmentType Type;
	public char* Position;
	private string Text;
	private int size;

	public DocumetFragmentInformation( DocumetFragmentType type, char* pos) {

	}

	public void SetText(string text) {
		Text = text;
		size = Text.Length;
	}

	public string GetText() =>
		Text;

	public int GetSize() =>
		size;

	//public void Dispose() {
	//	//NativeMemory.Free();
	//}
}

internal enum DocumetFragmentType {
	ANNOTATION, FIELD
}