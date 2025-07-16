using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseUtil.Generator.MigrationGenerator;

public class MigrationInformation : IDisposable {

	private bool disposedValue;
	private DbContext _context;
	private readonly DbContextOptions<DbContext> _dbContextOptions;
	Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder builder;
	public DbContext DatabaseContext => _context;

	//public MigratingDatabaseFixture() {
		//_dbContextOptions = new DbContextOptionsBuilder<BlogContext>()
	//	   .UseSqlite("Data Source = SQLLiteDatabase.db")
	//	   .Options;

	//	_context = new BlogContext(_dbContextOptions);
	//}

	//public async Task SetMigration(string migrationName) {
	//	IMigrator migrator = DatabaseContext..GetService<IMigrator>();
	//	await migrator.MigrateAsync(migrationName);
	//}

	public void RunScript(string fileLocation) {
		var script = File.ReadAllText(fileLocation);
		_context.Database.ExecuteSqlRaw(script);
	}

	public void Dispose(bool disposing) {
		if (!disposedValue) {
			if (disposing) {
				// This should delete the test database.
				//_context.con.ExecuteSqlRaw(@"delete from ""Blogs""");
			}
			disposedValue = true;
		}
	}

	void IDisposable.Dispose() {
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
