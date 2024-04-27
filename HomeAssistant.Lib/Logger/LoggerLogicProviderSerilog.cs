namespace HomeAssistant.Lib.Logger
{
    using Serilog;
    using Serilog.Core;
    using SubSystemComponent;

    public class LoggerLogicProviderSerilog : ILoggerLogicProvider
    {
        private Logger _logger;

        public LoggerLogicProviderSerilog()
        {
            string logFile = MasterSystemInfo.LogFilePath;

            _logger = new LoggerConfiguration()
             .WriteTo.File(logFile)
             .Enrich.FromLogContext()
                .CreateLogger();
        }
        public LoggerLogicProviderSerilog(string filePath)
        {
            _logger = new LoggerConfiguration()
             .WriteTo.File(filePath)
             .Enrich.FromLogContext()
                .CreateLogger();
        }

        public LoggerLogicProviderSerilog(Logger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }
    }
}
