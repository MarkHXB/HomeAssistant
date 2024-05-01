using SubSystemComponent;

namespace MicrosoftSpeechToText
{
    internal class ConfigObject
    {
        public const string AppName = "MicrosoftSpeechToText"; 
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);

        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");
        public string MicrosoftSpeechToTextFilePath { get; set; } = Path.Combine(AppFolder, "MicrosoftSpeechToTextFilePath.txt");

    }
}
