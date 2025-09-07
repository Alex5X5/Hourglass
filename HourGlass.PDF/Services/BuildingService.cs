using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hourglass.PDF;

public unsafe partial class PdfService {
	
	public void BufferFieldValueUnsafe(string indexName, string value) {
		InsertOperations[$"%%index-{indexName}-field"] = value;
	}


	public void BufferAnnotationValueUnsafe(string indexName, string value) {
		InsertOperations[$"%%index-{indexName}-annotation"] = value;
	}

	public char* BuildDocument(out int finalTextLength) {
		Console.WriteLine("started building document");
        finalTextLength = charCount;
		foreach (string key in InsertOperations.Keys)
			finalTextLength += InsertOperations[key].Length;
		Console.WriteLine($"final text length will be:{finalTextLength}");
		char* buffer = (char*)NativeMemory.AllocZeroed((uint)(finalTextLength * sizeof(char)));
		char* _buffer = buffer;
		foreach (string key in Indexers.Keys) {
			uint souceCharCopyCount = (uint)((char*)Indexers[key].Item2 - (char*)Indexers[key].Item1);
			uint sourceByteCopyCount = souceCharCopyCount * sizeof(char);
			NativeMemory.Copy((char*) Indexers[key].Item1, _buffer, sourceByteCopyCount);
			_buffer += souceCharCopyCount;
			InsertOperations.TryGetValue(key, out string? val);
			if (val != null) {
				string s = InsertOperations[key];
				if(s == "")
					continue;
				int charInsertCount = s.Length;
				uint byteInsertCount = (uint) (charInsertCount * sizeof(char));
				fixed (char* insert = &InsertOperations[key].ToCharArray()[0]) {
					//PrintCharsBefore(_buffer);
					//Console.WriteLine($"Inserting\'{val}\' at document position:{_buffer - buffer} at memory position:\'{(nuint)_buffer}\' for key:\'{key}\'");
					NativeMemory.Copy(insert, _buffer, byteInsertCount);
					_buffer += charInsertCount;
				}
			}
        }
        Console.WriteLine("finished building document");
        return buffer;
    }
}
