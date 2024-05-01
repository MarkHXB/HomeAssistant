namespace HomeAssistant.Lib.Logger
{
    using Serilog;
    using Serilog.Core;
    using SubSystemComponent;

    public class LoggerLogicProviderSerilog : ILoggerLogicProvider
    {
        private Logger _logger;
        private Action<string> WriteConsoleMessage;
        private Action<string> WriteGreenConsoleMessage;
        private Action<string> WriteOrangeConsoleMessage;
        private Action<string> WriteRedConsoleMessage;

        public LoggerLogicProviderSerilog(string? filePath, Action<string> writeConsoleMessage = null,
            Action<string> writeGreenConsoleMessage = null, Action<string> writeOrangeConsoleMessage = null, Action<string> writeRedConsoleMessage = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = MasterSystemInfo.LogFilePath;
            }

            _logger = new LoggerConfiguration()
             .WriteTo.File(filePath)
             .Enrich.FromLogContext()
                .CreateLogger();

            WriteConsoleMessage = writeConsoleMessage;
            WriteGreenConsoleMessage = writeGreenConsoleMessage;
            WriteOrangeConsoleMessage = writeOrangeConsoleMessage;
            WriteRedConsoleMessage = writeRedConsoleMessage;
        }

        public LoggerLogicProviderSerilog(Logger logger)
        {
            _logger = logger;
        }

        public void LogSuccess(string message)
        {
            _logger.Information(message);
            WriteGreenConsoleMessage(message);
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
            WriteConsoleMessage(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
            WriteOrangeConsoleMessage(message);
        }

        public void LogCrash(string message)
        {
            _logger.Warning(message);
            WriteRedConsoleMessage(message);
        }
    }
}