using SubSystemComponent;

namespace Runner
{
    internal class ConfigObject
    {
        public const string AppName = "Runner";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");
    }
}
