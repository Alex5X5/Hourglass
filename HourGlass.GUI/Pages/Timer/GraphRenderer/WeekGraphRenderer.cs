namespace Hourglass.GUI.Pages.Timer.GraphRenderer;

using System.Drawing.Drawing2D;

public class WeekGraphRenderer : GraphRenderer {

    protected override int MAX_TASKS => 4;

    public override int TASK_GRAPH_COUNT => 8;
    public override int TASK_GRAPH_COLUMN_COUNT => 8;


    protected override int GRAPH_CLICK_ADDITIONAL_WIDTH => 8;
    protected override int GRAPH_CLICK_ADDITIONAL_HEIGHT => 5;

    protected override int GRAPH_MINIMAL_WIDTH => 8;
    protected override int GRAPH_CORNER_RADIUS => 12;

    protected async override Task<List<Database.Models.Task>> GetTasksAsync() =>
        await _dbService.QueryTasksOfWeekAtDateAsync(_parent.SelectedDay) ?? [];

    protected override void DrawTimeline(Graphics g) {
        using (Pen hintLines = new(new SolidBrush(Color.FromArgb(170, 170, 170))))
        using (Pen timeline = new(Brushes.Black)) {
            g.DrawLine(timeline, PADDING_X, Height - PADDING_Y, Width - PADDING_X, Height - PADDING_Y);
            for (int i = 0; i < 8; i++) {
                int xPos = (Width - 2 * PADDING_X) * i / 7 + PADDING_X;
                g.DrawLine(hintLines, xPos, Height - PADDING_Y, xPos, PADDING_Y);
                g.DrawLine(timeline, xPos, Height - PADDING_Y, xPos, (int)Math.Floor(Height - PADDING_Y * 1.25));

            }
        }
        string[] days = ["Mo", "Tu", "We", "Th", "Fr", "Sa", "Su"];
        using (Brush textBrush = new SolidBrush(Color.Black))
            for (int i = 0; i < 7; i++) {
                string s = days[i];
                g.DrawString(
                    s,
                    new("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel, 0),
                    textBrush,
                    new Point((int)Math.Floor(Width * (i + 1) / 8.0), Height * 19 / 20 + 2)
                );
            }
    }

    protected override void DrawTaskDescriptionStub(Graphics g, Database.Models.Task task, int graphPosX, int graphPosY, int graphLength) {
        using Font font = new("Segoe UI", 30F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
        string text;
        if (task.description.Length > 25)
            text = task.description[..25] + "...";
        else
            text = task.description;
        float textWidth = g.MeasureString(text, font).Width;
        if (task.StartDateTime.DayOfWeek != DateTime.Now.DayOfWeek) {
            return;
        }
        if (graphLength > textWidth + 10) {
            using (Brush brush = new SolidBrush(Color.Black))
                g.DrawString(text, font, brush, graphPosX + 3, graphPosY + 6);
            return;
        }
        if (graphPosX > textWidth + 10) {
            using (Brush brush = new SolidBrush(Color.Black))
                g.DrawString(text, font, brush, graphPosX - textWidth - 5, graphPosY + 6);
            return;
        }
        using (Brush brush = new SolidBrush(Color.Black))
            g.DrawString(text, font, brush, graphPosX + graphLength + 5, graphPosY + 6);
        return;
    }

    protected override void DrawTaskGraph(Graphics g, Database.Models.Task task, ref int graphPosY) {
        long nowSeconds = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        long todaySeconds = nowSeconds - nowSeconds % TimeSpan.SecondsPerDay;
        Rectangle rect = GetTaskRectanlge(task, TimeSpan.SecondsPerHour, todaySeconds, 24, MAX_TASKS, 0, 0, GRAPH_MINIMAL_WIDTH, ref graphPosY, 1);
        Color gradientStartColor = Color.FromArgb(255, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
        Color gradientFinishColor = Color.FromArgb(0, task.displayColorRed, task.displayColorGreen, task.displayColorBlue);
        using GraphicsPath path = GetRoundedRectanglePath(rect, GRAPH_CORNER_RADIUS);
        using Brush brush = task.running ? new LinearGradientBrush(rect, gradientStartColor, gradientFinishColor, 0.0) : new SolidBrush(task.DisplayColor);
        g.FillPath(brush, path);
    }
}
