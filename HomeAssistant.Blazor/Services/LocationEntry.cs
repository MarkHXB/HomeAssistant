using SQLite;

namespace HomeAssistant.Blazor.Services
{
	internal class LocationEntry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
