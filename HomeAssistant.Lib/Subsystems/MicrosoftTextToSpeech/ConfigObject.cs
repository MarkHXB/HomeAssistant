using SubSystemComponent;

namespace MicrosoftTextToSpeech
{
    internal class ConfigObject
    {
        public const string AppName = "MicrosoftTextToSpeech";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        // Shared property
        public string LlamaOutputFilePathTxt { get; set; } = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, "LlamaStudio", "output.txt");

    }
}
