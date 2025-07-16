using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hourglass.Database.Models;

public class Ticket {

	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { set; get; }
	public string name { set; get; }
	public string description { set; get; }
	public Worker owner { set; get; }
	public Project project { set; get; }
}
