using HomeAssistant.Lib.Logger;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SubSystemComponent;
using System.Text;
using Whisper.net;
using Whisper.net.Ggml;

namespace Whisper
{
    /// <summary>
    /// Following parameters should pass:
    /// None
    /// </summary>
    public class WhisperSystem : Subsystem
    {
        private WhisperProcessor processor;
        private MemoryStream _processedAudioFile;

        // Config
        private string recorderOutputPath;
        private string whisperModelFilePath;
        private string whisperOutputFilePath;
        private string whisperUsedLanguage;
        private GgmlType whisperModel;

        public WhisperSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
                  base(@params, dependencies)
        { }
        public override void Initialize()
        {
            LoggerLogicProviderSerilog loggerLogicProviderSerilog = new LoggerLogicProviderSerilog(ConfigObject.LogFilePath);

            LogInformation = loggerLogicProviderSerilog.LogInformation;
            LogWarning = loggerLogicProviderSerilog.LogWarning;

            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            ConfigObject? config = configHandler.LoadConfig<ConfigObject>();

            whisperModelFilePath = config.WhisperModelFilePath;
            recorderOutputPath = config.RecorderOutputPath;
            whisperOutputFilePath = config.WhisperOutputFilePath;
            whisperUsedLanguage = config.WhisperUsedLanguage;
            whisperModel = config.WhisperModel;

            LogInformation("Whisper has been built.");
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            await DownloadModel(whisperModelFilePath, whisperModel);

            var whisperFactory = WhisperFactory.FromPath(whisperModelFilePath, useGpu: true);

            LogInformation("Whisper model has been successfully loaded.");

            processor = whisperFactory.CreateBuilder()
                .WithLanguage(whisperUsedLanguage)
                .Build();


            // 1. Analyze wav
            ConvertAudioFileToWav();

            if (_processedAudioFile == null)
            {
                throw new Exception("Must Build first the WhisperBox to initialize the model");
            }
            // 2. Generate output
            LogInformation("Text analysis has been started.");
            string output = await PrepareProcessedText();
            LogInformation("Text analysis has been finished.");

            // 3. Save
            SaveOutputToFile(output);
        }

        #region Private methods

        private async Task<string> PrepareProcessedText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            await foreach (var result in processor.ProcessAsync(_processedAudioFile))
            {
                stringBuilder.Append(result.Start);
                stringBuilder.Append("->");
                stringBuilder.Append(result.End);
                stringBuilder.Append(":");
                stringBuilder.Append("\n");
                stringBuilder.Append(result.Text);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        private void ConvertAudioFileToWav()
        {
            _processedAudioFile = new MemoryStream();

            MediaFoundationReader reader = new MediaFoundationReader(recorderOutputPath);
            var resampler = new WdlResamplingSampleProvider(reader.ToSampleProvider(), 16000);
            WaveFileWriter.WriteWavFileToStream(_processedAudioFile, resampler.ToWaveProvider16());

            // This section sets the wavStream to the beginning of the stream. (This is required because the wavStream was written to in the previous section)
            _processedAudioFile.Seek(0, SeekOrigin.Begin);
        }

        private async Task DownloadModel(string fileName, GgmlType ggmlType)
        {
            if(!File.Exists(fileName)) 
            {
                using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(ggmlType);
                using var fileWriter = File.OpenWrite(fileName);
                await modelStream.CopyToAsync(fileWriter);
            }    
        }

        private void SaveOutputToFile(string data)
        {
            if (string.IsNullOrWhiteSpace(whisperOutputFilePath))
            {
                throw new ArgumentException("whisperModelFilePath must not be null or empty.");
            }

            File.WriteAllText(whisperOutputFilePath, data);
        }

        #endregion
    }
}
