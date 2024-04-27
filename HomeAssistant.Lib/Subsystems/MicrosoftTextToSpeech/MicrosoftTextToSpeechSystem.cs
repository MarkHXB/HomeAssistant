using HomeAssistant.Lib.Logger;
using Newtonsoft.Json;
using SubSystemComponent;
using System.Speech.Synthesis;
using static LLama_gpt.DTO;

namespace MicrosoftTextToSpeech
{
    /// <summary>
    /// Following parameters should pass:
    /// None
    /// </summary>
    public class MicrosoftTextToSpeechSystem : Subsystem
    {
        // Config
        private string llamaPathText;

        public MicrosoftTextToSpeechSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) : base(@params, dependencies) { }

        public override void Initialize()
        {
            Console.WriteLine("MicrosoftTextToSpeechSystem extension initialization started...");

            LoggerLogicProviderSerilog loggerLogicProviderSerilog = new LoggerLogicProviderSerilog(ConfigObject.LogFilePath);

            LogInformation = loggerLogicProviderSerilog.LogInformation;
            LogWarning = loggerLogicProviderSerilog.LogWarning;

            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();

            // Null checks
            _ = config ?? throw new ArgumentNullException(nameof(config));

            // 1. Check configs are OK
            if (string.IsNullOrWhiteSpace(config.LlamaOutputFilePathTxt))
            {
                throw new ArgumentNullException(nameof(config.LlamaOutputFilePathTxt));
            }

            llamaPathText = config.LlamaOutputFilePathTxt;

            WriteMessageGreen("MicrosoftTextToSpeechSystem extension initialized successfully");
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            string textToSay = GetLlamaOutputFromTxt();

            if (string.IsNullOrWhiteSpace(textToSay))
            {
                LogWarning("Llama output is empty, some data needs to be prepared to synthesis the text");
                return;
            }

            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.Speak(textToSay);
        }

        public string GetLlamaOutputFromTxt()
        {
            if (string.IsNullOrWhiteSpace(llamaPathText))
            {
                throw new ArgumentException("AppInfo.LlamaOutputFilePath must not be null or empty.");
            }

            return File.ReadAllText(llamaPathText);
        }
    }
}