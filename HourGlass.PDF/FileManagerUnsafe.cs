namespace Hourglass.PDF;

using Hourglass.Util;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


public unsafe class FileManagerUnsafe {

	public static bool IndexersLoaded { private set; get; }
	public static UnsafePointerDictionary Indexers;

	public static void FindIndexers(char* content, int charCount) {
		Indexers?.ForKeys(s => NativeMemory.Free(Indexers.Get<char>(s)));
		Indexers = new();
		int i = 0;
		char* content_ = content;
		while (i < charCount) {
			if(content_[0] == '%' && content_[1] == '%' && content_[2] == 'i' && content_[3] == 'n' && content_[4] == 'd' && content_[5] == 'e' && content_[6] == 'x') {
				string key =  Convert.ToString(*content_);
				while(*content_!=' ' && *content_ != '\n' && i < charCount) {
					key += Convert.ToString(*(++content_));
					i++;
                }
				Indexers.Add(key.TrimEnd('\r', '\n'), content_);
			}
			content_++;
			i++;
		}
	}

	public static byte* LoadInput(out int fileSize) {
		fileSize = (int)new FileInfo(Paths.AssetsPath("output-readable-indexers.pdf")).Length;
		byte* file = (byte*)NativeMemory.Alloc((uint)fileSize);
		using (FileStream fs = new FileStream(Paths.AssetsPath("output-readable-indexers.pdf"), FileMode.Open, FileAccess.Read))
			fs.ReadExactly(new Span<byte>(file, fileSize));
		return file;
	}

	public static void WriteOutput(char* content, int size) {
		byte* file = (byte*)NativeMemory.Alloc((uint)size);
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Encoding ansi = Encoding.GetEncoding(1252);
		ansi.GetBytes(content, size, file, size);
		using (FileStream fileHandle = File.OpenWrite("output.pdf"))
			fileHandle.Write(new Span<byte>(file, size));
	}

	public static char* DecodeCharacters(byte* buffer, int fileSize, out int charCount) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding ansi = Encoding.GetEncoding(1252);
        charCount = ansi.GetCharCount(buffer, fileSize);
        //charCount = fileSize / sizeof(char);
        char* chars = (char*)NativeMemory.Alloc((uint)(charCount * sizeof(char)));
        //char* chars = (char*)NativeMemory.Alloc((uint)fileSize);
        ansi.GetChars(buffer, fileSize, chars, charCount);
        return chars;
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
