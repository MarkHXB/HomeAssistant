namespace HomeAssistant.Blazor.Services
{
	internal class CustomGeolocationLocationChangedEventArgs : GeolocationLocationChangedEventArgs
	{
		public CustomGeolocationLocationChangedEventArgs(Location location) : base(location)
		{
		}

		public Location LastKnownLocation { get; set; }
	}
}
