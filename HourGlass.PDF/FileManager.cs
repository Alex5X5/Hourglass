namespace HourGlass.PDF;

using System;
using System.Text;


public class FileManager
{
	public static string LoadInput() {
		int size = (int)new FileInfo("input.pdf").Length;
		byte[] buffer = new byte[size];
		using (FileStream fileHandle = File.OpenRead("input.pdf"))
			fileHandle.ReadExactly(buffer, 0, size);
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Encoding ansi = Encoding.GetEncoding(1252);
		return ansi.GetString(buffer);
	}

	public static void WriteOutput(string value) {
		//Console.WriteLine(value);
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Encoding ansi = Encoding.GetEncoding(1252);
		byte[] buffer = ansi.GetBytes(value);
		using (FileStream fileHandle = File.OpenWrite("output.pdf"))
			fileHandle.Write(buffer, 0, buffer.Length);

	}
}
