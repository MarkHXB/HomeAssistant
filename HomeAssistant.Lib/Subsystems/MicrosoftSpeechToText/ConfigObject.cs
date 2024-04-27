using SubSystemComponent;

namespace MicrosoftSpeechToText
{
    internal class ConfigObject
    {
        public const string AppName = "MicrosoftSpeechToText";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");
        public string MicrosoftSpeechToTextFilePath { get; set; } = "MicrosoftSpeechToTextFilePath.txt";

    }
}
