namespace HourGlass.PDF;

using Hourglass.Util;

using System;
using System.Text;


public class FileManager{

	public static string LoadInput() {
		int size = (int)new FileInfo(Paths.AssetsPath("output-readable-indexers.pdf")).Length;
		byte[] buffer = new byte[size];
		using (FileStream fileHandle = File.OpenRead(Paths.AssetsPath("output-readable-indexers.pdf")))
			fileHandle.ReadExactly(buffer, 0, size);
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Encoding ansi = Encoding.GetEncoding(1252);
		return ansi.GetString(buffer);
	}

	public static void WriteOutput(string value) {
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Encoding ansi = Encoding.GetEncoding(1252);
		byte[] buffer = ansi.GetBytes(value);
		using (FileStream fileHandle = File.OpenWrite("output.pdf"))
			fileHandle.Write(buffer, 0, buffer.Length);

	}
}
