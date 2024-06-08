using SubSystemComponent;

namespace LlamaStudio
{
    public static class SharedObject
    {
        public const string AppName = "LlamaStudio";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string LLamaInputTextFile { get; private set; } = Path.Combine(AppFolder, "input.txt");
        public static string LlamaOutputFilePathTxt { get; private set; } = Path.Combine(AppFolder, "output.txt");

    }
}
