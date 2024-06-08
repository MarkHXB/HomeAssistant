using SubSystemComponent;

namespace Todo
{
    internal class ConfigObject
    {
        public const string AppName = "Todo";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public static string SQLiteDatabaseName = "store.db";
        public static string SQLiteFilePath = Path.Combine(AppFolder, SQLiteDatabaseName);
    }
}
