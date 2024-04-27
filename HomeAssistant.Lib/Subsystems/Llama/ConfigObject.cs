using SubSystemComponent;

namespace LlamaStudio
{
    internal class ConfigObject
    {
        public const string AppName = "LlamaStudio";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public string LlamaOutputFilePathTxt { get; set; } = "LlamaOutputFilePathTxt.txt";
        public string LlamaServerPath { get; private set; } = "http://localhost:1234/v1/chat/completions";
        public string LlamaOutputFilePathJson { get; private set; } = "LlamaOutputFilePathJson.json";
        public string InputTextFilePath { get; private set; } = "llama_input.txt";
    }
}
