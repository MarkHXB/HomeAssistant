using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SubSystemComponent;
using System.Text;
using Whisper.net;
using Whisper.net.Ggml;
using Whisper.net.Wave;

namespace MyAssistant.Whisper
{
    public class WhisperSystemUp : Subsystem
    {
        private WhisperProcessor processor;
        private MemoryStream _processedAudioFile;

        private IDictionary<int, string> _speakers = new Dictionary<int, string>();


        public WhisperSystemUp(Dictionary<string, string> @params, params Subsystem[] dependencies) : base(@params, dependencies) { }

        public override void Initialize()
        {
            ConfigObject? config = new ConfigHandler().LoadConfig<ConfigObject>();
            if (string.IsNullOrWhiteSpace(config?.WhisperModelFilePath))
            {
                throw new ArgumentException($"WhisperModelFileName must not be empty or null.");
            }

            if (!File.Exists(config.WhisperModelFilePath))
            {
                DownloadModel(config.WhisperModelFilePath, config.WhisperModel).GetAwaiter().GetResult();
            }

            var whisperFactory = WhisperFactory.FromPath(config.WhisperModelFilePath);

            Console.WriteLine("Whisper model has been successfully loaded.");

            processor = whisperFactory.CreateBuilder()
                .WithLanguage(config.WhisperUsedLanguage)
                .Build();

            Console.WriteLine("Whisper has been built.");
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            // Check if the Whisper model has been initialized
            if (processor == null)
            {
                throw new Exception("Whisper model has not been initialized. Please call Initialize method first.");
            }

            try
            {
                ConvertAudioFileToWav();

                var waveParser = new WaveParser(_processedAudioFile);
                await waveParser.InitializeAsync();
                var channels = waveParser.Channels;
                var sampleRate = waveParser.SampleRate;
                var bitsPerSample = waveParser.BitsPerSample;
                var headerSize = waveParser.DataChunkPosition;
                var frameSize = bitsPerSample / 8 * channels;

                var samples = await waveParser.GetAvgSamplesAsync(CancellationToken.None);
                // This section processes the audio file and prints the results (start time, end time and text) to the console.

                StringBuilder stringBuilder = new StringBuilder();

                await foreach (var result in processor.ProcessAsync(samples))
                {
                    // Get the wave position for the specified time interval
                    var startSample = (long)result.Start.TotalMilliseconds * sampleRate / 1000;
                    var endSample = (long)result.End.TotalMilliseconds * sampleRate / 1000;

                    // Calculate buffer size.
                    var bufferSize = (int)(endSample - startSample) * frameSize;
                    var readBuffer = new byte[bufferSize];

                    // Set fileStream position.
                    _processedAudioFile.Position = headerSize + startSample * frameSize;

                    // Read the wave data for the specified time interval, into the readBuffer.
                    await _processedAudioFile.ReadAsync(readBuffer.AsMemory());

                    // Process the readBuffer and convert to shorts.
                    var buffer = new short[bufferSize / 2];
                    for (var i = 0; i < buffer.Length; i++)
                    {
                        // Handle endianess manually and convert bytes to Int16.
                        buffer[i] = BitConverter.IsLittleEndian
                            ? (short)(readBuffer[i * 2] | (readBuffer[i * 2 + 1] << 8))
                            : (short)((readBuffer[i * 2] << 8) | readBuffer[i * 2 + 1]);
                    }

                    // Iterate in the wave data to calculate total energy in each channel, and find the channel with the maximum energy.
                    var energy = new double[channels];
                    var maxEnergy = 0d;
                    var maxEnergyChannel = 0;
                    for (var i = 0; i < buffer.Length; i++)
                    {
                        var channel = i % channels;
                        energy[channel] += Math.Pow(buffer[i], 2);

                        if (energy[channel] > maxEnergy)
                        {
                            maxEnergy = energy[channel];
                            maxEnergyChannel = channel;
                        }
                    }

                    stringBuilder.Append($"[{GetCurrentSpeakerId(maxEnergyChannel)}]: {result.Start}->{result.End}: {result.Text}.");
                    stringBuilder.AppendLine();
                }

                // Save the output to a file or use it as needed
                SaveOutputToFile(stringBuilder.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetCurrentSpeakerId(int energy)
        {
            if (!_speakers.ContainsKey(energy))
            {
                _speakers.Add(energy, $"Speaker{energy}");
            }

            _speakers.TryGetValue(energy, out string speakerId);

            return speakerId;
        }

        #region Private methods

        private void ConvertAudioFileToWav()
        {
            string audioFilePath = GetAudioFilePathToAnalyze();
            _processedAudioFile = new MemoryStream();

            MediaFoundationReader reader = new MediaFoundationReader(audioFilePath);
            var resampler = new WdlResamplingSampleProvider(reader.ToSampleProvider(), 16000);
            WaveFileWriter.WriteWavFileToStream(_processedAudioFile, resampler.ToWaveProvider16());

            // This section sets the wavStream to the beginning of the stream. (This is required because the wavStream was written to in the previous section)
            _processedAudioFile.Seek(0, SeekOrigin.Begin);
        }

        private string GetAudioFilePathToAnalyze()
        {
            if (string.IsNullOrWhiteSpace(AppInfo.RecorderOutputPath))
            {
                throw new ArgumentException("Not provided audio file to analyze, please add an audio's file path to args.");
            }

            return AppInfo.RecorderOutputPath;
        }

        static async Task DownloadModel(string fileName, GgmlType ggmlType)
        {
            Console.WriteLine($"Downloading Model {fileName}");
            using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(ggmlType);
            using var fileWriter = File.OpenWrite(fileName);
            await modelStream.CopyToAsync(fileWriter);
        }

        private void SaveOutputToFile(string data)
        {
            if (string.IsNullOrWhiteSpace(AppInfo.WhisperOutputFilePath))
            {
                throw new ArgumentException("AppInfo.WhisperOutputFilePath must not be null or empty.");
            }

            File.WriteAllText(AppInfo.WhisperOutputFilePath, data);
        }

        #endregion
    }
}
