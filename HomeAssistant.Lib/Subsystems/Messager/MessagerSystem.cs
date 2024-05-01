using SubSystemComponent;
using System.Speech.Recognition;

namespace Messager
{
    /// <summary>
    /// Following parameters should pass:
    /// microsoft_speech_to_text_stop_recognition_on_silence_in_ms => after the amount of ms the silence detected and the recognition stopes
    /// </summary>
    public class MessagerSystem : Subsystem
    {
        private object prompt;
        private StringContent httpContent;
        private bool makeJsonObjectFromOutput;

        // Config
        private string microsoftSpeechToTextFilePath;

        // Params
        private double microsoft_speech_to_text_stop_recognition_on_silence_in_ms;

        public MessagerSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
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
            if (string.IsNullOrWhiteSpace(config.MicrosoftSpeechToTextFilePath))
            {
                throw new ArgumentNullException(nameof(config.MicrosoftSpeechToTextFilePath));
            }

            microsoftSpeechToTextFilePath = config.MicrosoftSpeechToTextFilePath;

            // Params
            Params.TryGetValue("microsoft_speech_to_text_stop_recognition_on_silence_in_ms", out string? microsoftSpeechToTextStopRecognitionOnSilenceInMs);

            double.TryParse(microsoftSpeechToTextStopRecognitionOnSilenceInMs, out microsoft_speech_to_text_stop_recognition_on_silence_in_ms);
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
           
        }

        #region Private methods

        private void SaveOutputToFile(string data)
        {
            if (string.IsNullOrWhiteSpace(microsoftSpeechToTextFilePath))
            {
                throw new ArgumentException("AppInfo.MicrosoftSpeechToTextFilePath must not be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                LogWarning("Speech recognition not recognized any voice");
                return;
            }

            File.WriteAllText(microsoftSpeechToTextFilePath, data);
        }

        public string GetOutputFromFile()
        {
            return File.ReadAllText(microsoftSpeechToTextFilePath);
        }

        #endregion
    }
}