using SubSystemComponent;

namespace SoundAudio
{
    internal class ConfigObject
    {
        public const string AppName = "SoundAudio";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public string RecorderOutputPath = "C:\\Users\\sasli\\Downloads\\piper_windows_amd64\\piper\\output.wav";
    }
}
