using SubSystemComponent;

namespace RecorderMicrophone
{
    public static class SharedObject
    {
        public const string AppName = "RecorderMicrophone";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string RecorderOutputPath { get; private set; } = Path.Combine(AppFolder, "output.wav");
    }
}
