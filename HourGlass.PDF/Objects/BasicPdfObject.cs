namespace HourGlass.PDF.Objects;

using System;

class BasicPdfObject(int offset, int objectNumber) {
	public int _offset = 0;
	public int _objectNumber = 0;
	public int _length = 0;

	public string BuildHeader() =>
		Convert.ToString(_offset) + " 0 obj\n<<\n";


	public virtual string BuildObject() => BuildHeader();
}
