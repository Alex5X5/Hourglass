namespace HourGlass.PDF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Document {

	private string TraineeName = "", DayFrom = "", DayTo = "";

	private string MontagLine1 = "", MontagLine2 = "", MontagLine3 = "", MontagLine4 = "", MontagLine5 = "", MontagLine6;
	private string MontagTime1 = "", MontagTime2 = "", MontagTime3 = "", MontagTime4 = "", MontagTime5 = "", MontagTime6;

	private string DienstagLine1 = "", DienstagLine2 = "", DienstagLine3 = "", DienstagLine4 = "", DienstagLine5 = "", DienstagLine6;
	private string DienstagTime1 = "", DienstagTime2 = "", DienstagTime3 = "", DienstagTime4 = "", DienstagTime5 = "", DienstagTime6;

	private string MittwochLine1 = "", MittwochLine2 = "", MittwochLine3 = "", MittwochLine4 = "", MittwochLine5 = "", MittwochLine6;
	private string MittwochTime1 = "", MittwochTime2 = "", MittwochTime3 = "", MittwochTime4 = "", MittwochTime5 = "", MittwochTime6;

	private string DonnerstagLine1 = "", DonnerstagLine2 = "", DonnerstagLine3 = "", DonnerstagLine4 = "", DonnerstagLine5 = "", DonnerstagLine6;
	private string DonnerstagTime1 = "", DonnerstagTime2 = "", DonnerstagTime3 = "", DonnerstagTime4 = "", DonnerstagTime5 = "", DonnerstagTime6;

	private string FreitagLine1 = "", FreitagLine2 = "", FreitagLine3 = "", FreitagLine4 = "", FreitagLine5 = "", FreitagLine6;
	private string FreitagTime1 = "", FreitagTime2 = "", FreitagTime3 = "", FreitagTime4 = "", FreitagTime5 = "", FreitagTime6;

	private string MontagTotalTime = "", DienstagTotalTime = "", MittwochTotalTime = "", DonnerstagTotalTime = "", FreitagTotalTime;
}
