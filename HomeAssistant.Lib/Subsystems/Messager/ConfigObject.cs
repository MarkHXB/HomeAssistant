using SubSystemComponent;

namespace Messager
{
    internal class ConfigObject
    {
        public const string AppName = "Messager";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public static string NotificationsFilePath = Path.Combine(AppFolder, "notifications.json");

        // Shared property
        public string LlamaOutputFilePathTxt { get; set; } = Path.Combine(AppFolder, "LlamaOutputFilePathTxt.txt");

    }
}