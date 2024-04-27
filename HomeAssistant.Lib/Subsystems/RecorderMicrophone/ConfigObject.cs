using SubSystemComponent;

namespace RecorderMicrophone
{
    internal class ConfigObject
    {
        public const string AppName = "Recorder";
        public static string ConfigFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName + "_Config.json");
        public static string LogFilePath = Path.Combine(MasterSystemInfo.LogFolderPath, $"{AppName}.log");

        public string RecorderOutputPath = "output.wav";
    }
}
