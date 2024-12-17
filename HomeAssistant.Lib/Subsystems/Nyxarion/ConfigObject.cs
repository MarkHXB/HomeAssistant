using SubSystemComponent;

namespace Nyxarion
{
    public class ConfigObject
    {
        public const string AppName = "Nyxarion";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public static string ExtensionsLogFolderPath = Path.Combine(AppFolder, "Extensions");
    }
}