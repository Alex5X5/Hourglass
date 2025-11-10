//namespace Hourglass.GUI.Pages.Timer.GraphRenderer;

//using Hourglass.Database.Services.Interfaces;
//using Hourglass.Util;
//using HourGlass.GUI.Pages.Timer;
//using System.Drawing.Drawing2D;


//public class MonthGraphRenderer : GraphRenderer {

//	protected override int MAX_TASKS => 30;

//	public override int TASK_GRAPH_COLUMN_COUNT => 3;


//	protected override int GRAPH_CLICK_ADDITIONAL_WIDTH => 6;
//	protected override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 4;

//	protected override int GRAPH_MINIMAL_WIDTH => 2;
//	protected override int GRAPH_CORNER_RADIUS => 4;

//	public MonthGraphRenderer(IHourglassDbService dbService, TimerWindow timerWindow) : base(dbService, timerWindow, TimerWindowMode.Month) { }
	
//	protected async override Task<List<Database.Models.Task>> GetTasksAsync() =>
//		await _dbService.QueryTasksOfMonthAtDateAsync(_parent.SelectedDay) ?? [];

//	protected override void DrawTimeline(Graphics g) {
//		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
//		float xAxisSegmentSize = (Width - 2 * PADDING_X) / daysInCurrentMonth;
//		using Brush textBrush = new SolidBrush(Color.Black);
//		using Pen hintLines = new(new SolidBrush(Color.FromArgb(172, 172, 172)));
//		using Brush weekedDayBackground = new SolidBrush(Color.FromArgb(166, 166, 166));
//		using Brush todayBackgroundColor = new SolidBrush(Color.FromArgb(237, 166, 166));
//		using Pen timeline = new(Brushes.Black);
//		g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
//		int weekDayCounter = (int)DateTimeService.GetFirstDayOfCurrentMonth().DayOfWeek;
//		for (int i = 0; i < daysInCurrentMonth; i++) {
//            int xPos = PADDING_X + (Width - 2 * PADDING_X) * i / daysInCurrentMonth;
//            g.DrawString(
//				Convert.ToString(i + 1),
//				new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
//				textBrush,
//				new Point(xPos + (Convert.ToString(i + 1).Length == 1 ? 9 : 6), Height - (PADDING_Y / 2))
//			);
//            if (weekDayCounter % 7 == 6 | weekDayCounter % 7 == 0)
//                g.FillRectangle(weekedDayBackground, xPos, PADDING_Y, xAxisSegmentSize, Height - (2 * PADDING_Y));
//            if (weekDayCounter == DateTime.Today.Day)
//                g.FillRectangle(todayBackgroundColor, xPos, PADDING_Y, xAxisSegmentSize, Height - (2 * PADDING_Y));

//            weekDayCounter++;
//        }
//		for (int i = 0; i < daysInCurrentMonth+1; i++) {
//            int xPos = PADDING_X + (Width - 2 * PADDING_X) * i / daysInCurrentMonth;
//            g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
//			g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, Height - PADDING_Y - TIMELINE_MARK_HEIGHT);
//		}
//	}

//	protected override void DrawTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
//        using Font font = new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
//        string text;
//        if (task.description.Length > 25)
//            text = task.description[..25] + "...";
//        else
//            text = task.description;
//        float textWidth = g.MeasureString(text, font).Width;
//        if (graphLength > textWidth + 10) {
//            using (Brush brush = new SolidBrush(Color.Black))
//                g.DrawString(text, font, brush, graphPosX + 3, graphPosY - 2);
//            return;
//        }
//        if (graphPosX > textWidth + 10) {
//            using (Brush brush = new SolidBrush(Color.Black))
//                g.DrawString(text, font, brush, graphPosX - textWidth - 3, graphPosY - 2);
//            return;
//        }
//        using (Brush brush = new SolidBrush(Color.Black))
//            g.DrawString(text, font, brush, graphPosX + graphLength + 3, graphPosY - 2);
//        return;
//    }

//    protected override void DrawTaskGraph(Graphics g, Database.Models.Task task, int i) {
//		//Console.WriteLine("MonthGraphRenderer Task Graph");
//		DateTime month = DateTimeService.FloorMonth(DateTime.Now);
//        long monthSeconds = month.Ticks / TimeSpan.TicksPerSecond;
//		int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
//        Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerDay, monthSeconds, daysInCurrentMonth, MAX_TASKS, 0, 0, GRAPH_MINIMAL_WIDTH, i, 1);
//		Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
//		Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
//		using GraphicsPath path = GetRoundedRectanglePath(rect, GRAPH_CORNER_RADIUS);
//		//using Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidBrush(task.DisplayColor);
//		//g.FillPath(brush, path);
//		//DrawTaskDescriptionStub(g, task, rect.X, rect.Y, rect.Width);
//	}
//}
