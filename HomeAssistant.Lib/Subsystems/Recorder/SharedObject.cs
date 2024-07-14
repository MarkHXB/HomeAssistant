using SubSystemComponent;

namespace Recorder
{
    public static class SharedObject
    {
        public const string AppName = "Recorder";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string RecorderOutputFolder = AppFolder;
    }
}
