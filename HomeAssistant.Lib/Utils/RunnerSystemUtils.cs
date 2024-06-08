using Newtonsoft.Json;
using System.Text;

namespace HomeAssistant.Lib.Utils
{
    internal class RunnerSystemUtils
    {
        public static string GetInputTextReflectsToSubsystemOutput(string outputPath, string convertOutputToJson)
        {
            if (File.Exists(outputPath))
            {
                string data = File.ReadAllText(outputPath, Encoding.UTF8);

                if (!string.IsNullOrWhiteSpace(convertOutputToJson))
                {
                    // Escape special characters and serialize to JSON
                    string json = JsonConvert.SerializeObject(new { text = data });

                    // Write JSON to a file
                    string jsonFilePath = Path.Combine(Path.GetDirectoryName(outputPath), "output.json");
                    File.WriteAllText(jsonFilePath, json, Encoding.UTF8);

                    return jsonFilePath;
                }
                return data;
            }

            return string.Empty;
        }

    }
}
