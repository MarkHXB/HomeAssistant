﻿using SubSystemComponent;
using System.Speech.Synthesis;

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

        public MicrosoftTextToSpeechSystem(params Subsystem[] dependencies) :
      base(ConfigObject.LogFilePath, dependencies)
        {

        }

        public override void Initialize()
        {
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