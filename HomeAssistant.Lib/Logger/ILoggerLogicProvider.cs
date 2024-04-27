namespace HomeAssistant.Lib.Logger
{
    public interface ILoggerLogicProvider
    {
        void LogInformation(string message);

        void LogWarning(string message);
    }
}
