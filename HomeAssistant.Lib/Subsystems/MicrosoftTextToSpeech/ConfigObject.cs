using SubSystemComponent;

namespace MicrosoftTextToSpeech
{
    internal class ConfigObject
    {
        public const string AppName = "MicrosoftTextToSpeech";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        // Shared property
        public string LlamaOutputFilePathTxt { get; set; } = "LlamaOutputFilePathTxt.txt";

    }
}
