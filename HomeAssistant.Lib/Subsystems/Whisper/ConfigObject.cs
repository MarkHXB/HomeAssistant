using SubSystemComponent;
using Whisper.net.Ggml;

namespace Whisper
{
    internal class ConfigObject
    {
        public const string AppName = "Whisper";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public GgmlType WhisperModel { get; private set; } = GgmlType.Medium;
        public string? WhisperModelFilePath { get; private set; } = "ggml-medium.bin";
        public string WhisperUsedLanguage { get; private set; } = "auto";
        public string WhisperOutputFilePath { get; private set; } = "whisper_output.txt";

        // Shared property
        public string RecorderOutputPath = "output.wav";
    }
}
