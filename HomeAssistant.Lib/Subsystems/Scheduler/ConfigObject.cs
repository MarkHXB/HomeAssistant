using SubSystemComponent;

namespace Scheduler
{
    internal class ConfigObject
    {
        public const string AppName = "Scheduler";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log"); 

        public int MaxRunsPerMinute = 5;
    }
}
