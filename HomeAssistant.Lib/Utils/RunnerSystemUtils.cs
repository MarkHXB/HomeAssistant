using System.Text;

namespace HomeAssistant.Lib.Utils
{
    internal class RunnerSystemUtils
    {
        public static string GetInputTextReflectsToSubsystemOutput(string outputPath, string convertOutputToJson)
        {
            if (File.Exists(outputPath))
            {
                string data = File.ReadAllText(outputPath, encoding: Encoding.UTF8);

                if (!string.IsNullOrWhiteSpace(convertOutputToJson))
                {
                    // only for piper
                    string json = $"{{ \"text\": \"{data}\" }}";
                    string asd = Path.GetDirectoryName(outputPath);
                    File.WriteAllText(asd+"asd.json", json);
                    return asd + "asd.json";
                }
                return data;
            }

            return string.Empty;
        }
    }
}
