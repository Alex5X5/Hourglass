using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hourglass.Database.Models;

public class Worker {

	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long id { set; get; } = 0;
	public string name { set; get; } = "";
}
