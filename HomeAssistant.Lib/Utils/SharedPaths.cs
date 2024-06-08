using SubSystemComponent;

namespace HomeAssistant.Lib.Utils
{
    public static class SharedPaths
    {
        public static string ScreenReaderOutputFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, "ScreenReader", "output.txt");
        public static string LlamaInputTextFilePath = Path.Combine(MasterSystemInfo.Appdata, MasterSystemInfo.AppName, "LlamaStudio", "input.txt");
    
        public static string GetSubsystemOutputPath(string subsystemName)
        {
            if (string.IsNullOrWhiteSpace(subsystemName))
            {
                throw new ArgumentNullException(subsystemName);
            }

            return string.Empty;
        }
    }
}
