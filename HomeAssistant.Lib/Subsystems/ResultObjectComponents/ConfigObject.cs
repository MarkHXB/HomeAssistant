using SubSystemComponent;

namespace ResultObjectComponents
{
    internal class ConfigObject
    {
        public const string AppName = "ResultObjectHandler";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");
    }
}
