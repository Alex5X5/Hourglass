using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace DatabaseUtil.Generator;

public class DatabaseGenerator {

}

public class DatabaseContextFactory<EntityType> where EntityType : class  {
	public DbSet<EntityType> Items {
		get; set;
	}

	public DatabaseContextFactory() {
		
	}

	public void CreateNew(string fileName, ColumnInformation[] columns) {

	}

	public void CreateNew(string fileName, ColumnInformation[] columns, Type[] columnTypes) {
		var _dbContextOptions = new DbContextOptionsBuilder()
			   .UseSqlite("Data Source = SQLLiteDatabase.db")
			   .Options;

		Dictionary<string, string> _blakdjskf = new();
		_blakdjskf["fahlsdiujhfa"] = "tbgoilu";
	}

	public class AppDbContext(DbContextOptions options) : DbContext(options) {
		
	}
}
