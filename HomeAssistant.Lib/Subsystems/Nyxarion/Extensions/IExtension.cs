namespace Nyxarion
{
	public interface IExtension
	{
		Task ExecuteAsync(CancellationToken? cancellationToken = null);
	}
}
