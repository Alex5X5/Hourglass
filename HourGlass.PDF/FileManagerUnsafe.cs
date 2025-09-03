namespace Hourglass.PDF;

using Hourglass.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


public partial class HourglassPdfUnsafe {

	public static unsafe char* DecodeBuffer(byte* buffer, int fileSize, out int charCount) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding encoding = Encoding.GetEncoding(1252);
		int bytesPerChar = encoding.GetByteCount(['a']);
		charCount = fileSize / bytesPerChar;
		char* chars = (char*)NativeMemory.Alloc((uint)(charCount * sizeof(char)));
		charCount = encoding.GetChars(buffer, fileSize, chars, charCount);
		return chars;
	}

	public static unsafe byte* EncodeBuffer(char* chars, int charCount, out int fileSize) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding encoding = Encoding.GetEncoding(1252);
        fileSize = charCount * sizeof(char);
        byte* buffer = (byte*)NativeMemory.Alloc((uint)fileSize);
		fileSize = encoding.GetBytes(chars, charCount, buffer, fileSize);
		return buffer;
	}

	public static unsafe byte* LoadInputUnsafe(out int fileSize) {
		fileSize = (int)new FileInfo(Paths.AssetsPath("output-readable-indexers.pdf")).Length;
		byte* file = (byte*)NativeMemory.Alloc((uint)fileSize);
		using (FileStream fs = new FileStream(Paths.AssetsPath("output-readable-indexers.pdf"), FileMode.Open, FileAccess.Read))
			fs.ReadExactly(new Span<byte>(file, fileSize));
		return file;
	}

	public static unsafe void WriteOutputUnsafe(byte* content, string filename, int size) {
		using (FileStream fileHandle = File.OpenWrite("output.pdf"))
			fileHandle.Write(new Span<byte>(content, size));
	}
}

public unsafe class UnsafePointerDictionary {
	private readonly Dictionary<string, IntPtr> _dict = [];

	public void Add<T>(string key, T* pointer) where T : unmanaged {
		_dict[key] = (IntPtr)pointer;
	}

	public T* Get<T>(string key) where T : unmanaged {
		return _dict.TryGetValue(key, out IntPtr ptr) ? (T*)ptr : null;
	}

	public void ForKeys(Action<string> action) {
		foreach(string key in _dict.Keys)
			action(key);
	}
}
