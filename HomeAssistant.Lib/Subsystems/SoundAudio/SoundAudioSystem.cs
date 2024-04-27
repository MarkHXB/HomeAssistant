using HomeAssistant.Lib.Logger;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SubSystemComponent;

namespace SoundAudio
{
    /// <summary>
    /// Following parameters should pass:
    /// recorder_system_wait_for_exit_in_ms => How many milliseconds should wait to stop the recording.
    /// recorder_system_stop_automatacilly_after_silence_of_ms => after ms silence the recorder stops automatically
    /// </summary>
    public class SoundAudioSystem : Subsystem
    {
        private string recorderOutputPath = string.Empty;

        public SoundAudioSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
            base(@params, dependencies)
        { }

        public override void Initialize()
        {
            LoggerLogicProviderSerilog loggerLogicProviderSerilog = new LoggerLogicProviderSerilog(ConfigObject.LogFilePath);

            LogInformation = loggerLogicProviderSerilog.LogInformation;
            LogWarning = loggerLogicProviderSerilog.LogWarning;

            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();

            if (string.IsNullOrWhiteSpace(config?.RecorderOutputPath))
            {
                throw new ArgumentException("Not provided audio file to analyze, please add an audio's file path to args.");
            }

            recorderOutputPath = config.RecorderOutputPath;
        }

        public override Task TaskObject(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (!File.Exists(recorderOutputPath))
                {
                    throw new Exception(recorderOutputPath + " not found sound file.");
                }

                // Create WasapiLoopbackCapture to capture system audio
                using (var waveReader = new WaveFileReader(recorderOutputPath))
                {
                    using (var _waveOutEvent = new WaveOutEvent())
                    {
                        // Start audio playback
                        _waveOutEvent.Init(waveReader);
                        _waveOutEvent.Play();

                        // Event handler for playback stopped
                        _waveOutEvent.PlaybackStopped += (sender, args) =>
                        {
                            // Dispose of the WaveOutEvent when playback stops
                            _waveOutEvent.Dispose();
                        };

                        //// Wait until playback is finished or cancellation is requested
                        while (_waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                _waveOutEvent.Stop();
                                break;
                            }
                            Thread.Sleep(10);
                        }
                    }
                }
            });
        }
    }
}