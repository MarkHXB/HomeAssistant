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
     base(ConfigObject.LogFilePath, @params, dependencies)
        {

        }

        public override void Initialize()
        {
            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();

            if (string.IsNullOrWhiteSpace(config?.RecorderOutputPath))
            {
                throw new ArgumentException("Not provided audio file to analyze, please add an audio's file path to args.");
            }

            recorderOutputPath = config.RecorderOutputPath;
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            if (!File.Exists(recorderOutputPath))
            {
                throw new Exception(recorderOutputPath + " not found sound file.");
            }

            using (var waveReader = new WaveFileReader(recorderOutputPath))
            {
                using (var _waveOutEvent = new WaveOutEvent())
                {
                    // Start audio playback
                    _waveOutEvent.Init(waveReader);
                    _waveOutEvent.Play();

                    // TaskCompletionSource to await playback completion
                    var playbackCompletedTcs = new TaskCompletionSource<bool>();

                    // Event handler for playback stopped
                    _waveOutEvent.PlaybackStopped += (sender, args) =>
                    {
                        // Dispose of the WaveOutEvent when playback stops
                        _waveOutEvent.Dispose();

                        // Set the task completion
                        playbackCompletedTcs.TrySetResult(true);
                    };

                    LogInformation("Press ESC if you want to interrupt audio");
                    // Wait asynchronously until playback is finished or cancellation is requested
                    //while (true)
                    //{
                    //    if (cancellationToken.IsCancellationRequested || lookinForEscape())
                    //    {
                    //        _waveOutEvent.Stop();
                    //        LogInformation($"{nameof(SoundAudio)} cancelled by the user.");
                    //        break;
                    //    }

                    //    // Check if playback is completed
                    //    if (_waveOutEvent.PlaybackState == PlaybackState.Stopped)
                    //    {
                    //        break;
                    //    }

                    //    // Await a short delay to avoid busy waiting
                    //    await Task.Delay(100);
                    //}

                    // Await playback completion or cancellation
                    await playbackCompletedTcs.Task;
                }
            }
        }

        private bool lookinForEscape()
        {
            if (Console.In.Peek() != -1)
            {
                 if(Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    return true;
                }
            }

            return false;
        }
    }
}