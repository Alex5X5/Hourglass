namespace Hourglass.Database.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

public class Task {

	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { set; get; } = 0;
    public string description { set; get; } = "";
	
	public bool running { set; get; } = false;
	public long start { set; get; } = 0;
	public long finish { set; get; } = 0;
	
	public Worker? owner { set; get; }
	public Project? project { set; get; }
	public Ticket? ticket { set; get;}
	
	public int displayColorRed { set; get; } = 255;
	public int displayColorGreen { set; get; } = 255;
	public int displayColorBlue { set; get; } = 255;

    [NotMapped]
	public DateTime StartDateTime {
		set => start = value.Ticks / TimeSpan.TicksPerSecond;
		get => DateTime.MinValue.AddSeconds(start);
	}

	[NotMapped]
	public DateTime FinishDateTime {
		set => finish = value.Ticks / TimeSpan.TicksPerSecond;
		get => DateTime.MinValue.AddSeconds(finish);
	}

	[NotMapped]
	public Color DisplayColor {
		set {
			displayColorRed = value.R;
			displayColorGreen = value.G;
			displayColorBlue = value.B;
		}
		get => Color.FromArgb(displayColorRed, displayColorGreen, displayColorBlue);
	}
}