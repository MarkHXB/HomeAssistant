using SQLite;

namespace HomeAssistant.Blazor.Services
{
	internal class LocationDatabase
	{
		private readonly SQLiteAsyncConnection _database;

		public LocationDatabase(string dbPath)
		{
			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<LocationEntry>().Wait();
		}

		public Task<int> SaveLocationAsync(LocationEntry location)
		{
			return _database.InsertAsync(location);
		}

		public Task<List<LocationEntry>> GetPendingLocationsAsync()
		{
			return _database.Table<LocationEntry>().ToListAsync();
		}

		public Task<int> DeleteLocationAsync(LocationEntry location)
		{
			return _database.DeleteAsync(location);
		}

		public Task<LocationEntry> GetLastKnownLocationAsync()
		{
			return _database.Table<LocationEntry>().OrderByDescending(l => l.Timestamp).FirstOrDefaultAsync();
		}

		public Task<int> UpdateLocationAsync(LocationEntry location)
		{
			return _database.UpdateAsync(location);
		}
	}
}
