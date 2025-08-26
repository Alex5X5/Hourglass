namespace Hourglass.PDF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class HourglassPdf {

	public static string SetAnnotaionValue(string document, string indexName, string value) {
			string[] lines = document.Split("\n");
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i]==$"%%index-{indexName}-annotation") {
				lines[i+1] = $"/V ({value})";
			}
		}
		string res = "";
		foreach (string s in lines)
			res += s + "\n";
		return res;
	}
	
	public static string SetFieldValue(string document, string indexName, string value) {
		string[] lines = document.Split("\n");
		for (int i = 0; i < lines.Length; i++) {
			if (lines[i] == $"%%index-{indexName}-field") {
				lines[i + 1] = $"({value}) Tj";
			}
		}
		string res = "";
		foreach (string s in lines)
			res += s + "\n";
		return res;
	}
}
