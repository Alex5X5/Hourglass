using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hourglass.PDF;

public partial class HourglassPdfUnsafe {

	const int INDEXER_COUNT = 197;

	private Dictionary<string, string> BufferedInserts;
	
 //   public static string BufferAnnotaionValueunsafe(char* document, string indexName, string value) {
	//	for (int i = 0; i < lines.Length; i++) {
	//		if (lines[i]==$"%%index-{indexName}-annotation") {
	//			lines[i+1] = $"/V ({value})";
	//		}
	//	}
	//	string res = "";
	//	foreach (string s in lines)
	//		res += s + "\n";
	//	return res;
	//}
	
	public static string BufferFieldValueUnsafe(string document, string indexName, string value) {
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

    private static unsafe byte* InsertSpace(byte* originalBuffer, int originalSize, byte* insertPoint, int spaceSize, out byte* newBuffer) {
        int offsetToInsertPoint = (int)(insertPoint - originalBuffer);
        newBuffer = (byte*)NativeMemory.Realloc(originalBuffer, (uint)(originalSize + spaceSize));
        byte* newInsertPoint = newBuffer + offsetToInsertPoint;
        int bytesToMove = originalSize - offsetToInsertPoint;
        if (bytesToMove > 0) {
            Buffer.MemoryCopy(
                newInsertPoint,                    // source: current position
                newInsertPoint + spaceSize,        // destination: 20 bytes later
                bytesToMove,                       // bytes to move
                bytesToMove                        // destination size
            );
        }

        // Clear the newly created space (optional)
        new Span<byte>(newInsertPoint, spaceSize).Clear();

        return newInsertPoint; // Return pointer to the new empty space
    }
}
