namespace Hourglass.PDF;

using Hourglass.Util;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


public unsafe class FileManagerUnsafe {

	public static byte* LoadInputUnsafe(out int fileSize) {
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
		//ansi.GetBytes(content, size, file, size);
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
