namespace HomeAssistant.Blazor.Services
{
	public interface ILocationService
	{
		Task<Location> GetCurrentLocationAsync();
		void StartBackgroundTracking();
		void StopBackgroundTracking();
	}
}
