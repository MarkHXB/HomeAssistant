using HomeAssistant.Lib.Logger;
using Newtonsoft.Json;
using SubSystemComponent;
using System.Text;
using static LLama_gpt.DTO;

namespace LlamaStudio
{
    /// <summary>
    /// Following parameters should pass:
    /// <para>llamastudio_system_message => only the folder path without the file</para>
    /// <para>llamastudio_userPrompt => the filename you want to run</para>
    /// <para>(Optional) make_json_object => If further analysis set to before output and convert it to json object then save it to .json</para>
    /// </summary>
    public class LlamaStudioSystem : Subsystem
    {
        private object prompt;
        private StringContent httpContent;
        private bool makeJsonObjectFromOutput;

        // Config
        private string llamaServerPath;
        private string llamaPathText;
        private string inputTextFilePath;
        private string llamaOutputFilePathJson;

        // Parameters
        private string? llamastudio_systemMessage;
        private string? llamastudio_userPrompt;
        private string? make_json_object;

        public LlamaStudioSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
             base(ConfigObject.LogFilePath, @params, dependencies)
        {

        }

        public override void Initialize()
        {
            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();

            // Null checks
            _ = config ?? throw new ArgumentNullException(nameof(config));

            // 1. Check configs are OK
            if (string.IsNullOrWhiteSpace(config.LlamaServerPath))
            {
                throw new ArgumentNullException(nameof(config.LlamaServerPath));
            }

            llamaServerPath = config.LlamaServerPath;
            llamaPathText = config.LlamaOutputFilePathTxt;
            inputTextFilePath = config.InputTextFilePath;
            llamaOutputFilePathJson = config.LlamaOutputFilePathJson;

            Params.TryGetValue("llamastudio_system_message", out llamastudio_systemMessage);
            Params.TryGetValue("llamastudio_user_prompt", out llamastudio_userPrompt);
            Params.TryGetValue("make_json_object", out make_json_object);

            if (string.IsNullOrWhiteSpace(llamastudio_systemMessage))
            {
                throw new ArgumentNullException(nameof(llamastudio_systemMessage));
            }

            bool.TryParse(make_json_object, out makeJsonObjectFromOutput);
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(llamastudio_userPrompt))
            {
                // try to get from file
                if (!File.Exists(inputTextFilePath))
                {
                    LogWarning(nameof(inputTextFilePath) + " is missing");
                }

                llamastudio_userPrompt = File.ReadAllText(inputTextFilePath);

                prompt = new
                {
                    messages = new[]
               {
                    new { role = "system", content = llamastudio_systemMessage },
                    new { role = "user", content = llamastudio_userPrompt },
                },
                    temperature = 0.7,
                    max_tokens = -1,
                    stream = false
                };

                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(prompt);
                httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                await RunLlamaExtension();

                llamastudio_userPrompt = string.Empty;

                return;
            }

            string whisperOutput = GetWhisperOutput();

            if (string.IsNullOrWhiteSpace(whisperOutput))
            {
                throw new ArgumentNullException(nameof(whisperOutput));
            }

            byte[] bytes = Encoding.Default.GetBytes(whisperOutput);
            whisperOutput = Encoding.UTF8.GetString(bytes);

            if (!makeJsonObjectFromOutput)
            {
                prompt = new
                {
                    messages = new[]
                {
                    new { role = "system", content = llamastudio_systemMessage },
                    new { role = "user", content = llamastudio_userPrompt },
                    new { role = "user", content = whisperOutput }
                },
                    temperature = 0.7,
                    max_tokens = -1,
                    stream = false
                };

                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(prompt);
                httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            }
            else // in this scenario the user answer should be the output from the last session ( above ) 
            {
                string llamaOutput = "Just write a json object from this and no say anything else: " + GetLlamaOutputFromTxt();

                prompt = new
                {
                    messages = new[]
                {
                    new { role = "system", content = llamastudio_systemMessage },
                    new { role = "user", content = llamaOutput },
                },
                    temperature = 0.7,
                    max_tokens = -1,
                    stream = false
                };

                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(prompt);
                httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            }

            await RunLlamaExtension();
        }

        private async Task RunLlamaExtension()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Making the POST request to the local server
                    var response = await httpClient.PostAsync(llamaServerPath, httpContent);

                    // Checking if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Printing the response content
                        var responseBody = await response.Content.ReadAsStringAsync();
                        SaveOutputToFile(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get response: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                    }
                }

                WriteMessageGreen("LLama extension finished successfully");
            }
            catch (Exception ex)
            {
                WriteMessageRed("LLama extension finished with errors!");
                throw ex; // replace this with logger
            }
        }

        public string GetLlamaOutputFromTxt()
        {
            if (string.IsNullOrWhiteSpace(llamaPathText))
            {
                throw new ArgumentException("AppInfo.LlamaOutputFilePath must not be null or empty.");
            }

            string output = File.ReadAllText(llamaPathText);

            File.WriteAllText(llamaPathText, "");

            return output;
        }

        #region Private methods


        private string GetWhisperOutput()
        {
            if (File.Exists(inputTextFilePath))
            {
                return File.ReadAllText(inputTextFilePath);
            }

            return string.Empty;
        }

        private void SaveOutputToFile(string data)
        {
            if (string.IsNullOrWhiteSpace(llamaPathText))
            {
                throw new ArgumentException("AppInfo.LlamaOutputFilePath must not be null or empty.");
            }

            ChatCompletion completion = JsonConvert.DeserializeObject<ChatCompletion>(data);

            if (completion != null && makeJsonObjectFromOutput)
            {
                File.WriteAllText(llamaOutputFilePathJson, completion.Choices[0].Message.Content);
            }
            else if(completion != null)
            {
                File.WriteAllText(llamaPathText, completion.Choices[0].Message.Content);
            }
            else
            {
                WriteMessageRed("Data not provided for LLama");
            }
        }

        #endregion
    }
}