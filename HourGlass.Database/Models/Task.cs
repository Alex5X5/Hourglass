namespace Hourglass.Database.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Task {

	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { set; get; } = 0;
	public string description { set; get; } = "";
	public Worker? owner { set; get; }
	public Project? project { set; get; }
	public Ticket? ticket { set; get;}
	public long start { set; get; } = 0;
	public long finish { set; get; } = 0;
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
}