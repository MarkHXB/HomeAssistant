using SubSystemComponent;

namespace LlamaStudio
{
    public static class SharedObject
    {
        public const string AppName = "LlamaStudio";
        public static string AppFolder = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, AppName);
        public static string InputTextFilePath { get; private set; } = Path.Combine(AppFolder, "llama_input.txt");
        public static string LlamaOutputFilePathTxt { get; private set; } = Path.Combine(AppFolder, "LlamaOutputFilePathTxt.txt");

    }
}
