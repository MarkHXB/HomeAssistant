using SubSystemComponent;

namespace Runner
{
    public static class SharedObject
    {
        public const string AppName = "RecorderMicrophone";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string RecorderOutputPath = Path.Combine(AppFolder, "WhisperedWavs");
    }
}
