using SubSystemComponent;

namespace LlamaStudio
{
    internal class ConfigObject
    {
        public const string AppName = "LlamaStudio";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);

        public static string ConfigFilePath = Path.Combine(AppFolder, "config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public string LlamaServerPath { get; private set; } = "http://localhost:1234/v1/chat/completions";

        public string LlamaOutputFilePathTxt { get; set; } = Path.Combine(AppFolder, "output.txt");
        public string LlamaOutputFilePathJson { get; private set; } = Path.Combine(AppFolder, "output.json");
        public string InputTextFilePath { get; private set; } = Path.Combine(AppFolder, "input.txt");
    }
}
