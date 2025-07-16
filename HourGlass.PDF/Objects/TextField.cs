namespace HourGlass.PDF.Objects;

class TextField(int offset, int objectNumber) : BasicPdfObject(offset, objectNumber) {

	string content = "";

	public override string BuildObject() {
		return BuildHeader();


	}
}
