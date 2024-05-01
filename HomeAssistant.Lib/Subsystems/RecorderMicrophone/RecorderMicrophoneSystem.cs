using NAudio.CoreAudioApi;
using NAudio.Wave;
using SubSystemComponent;

namespace RecorderMicrophone
{
    /// <summary>
    /// Following parameters should pass:
    /// recorder_system_wait_for_exit_in_ms => How many milliseconds should wait to stop the recording.
    /// recorder_system_stop_automatacilly_after_silence_of_ms => after ms silence the recorder stops automatically
    /// </summary>
    public class RecorderMicrophoneSystem : Subsystem
    {
        private const string AppName = nameof(RecorderMicrophoneSystem);
        private WasapiCapture _wasapiCapture;
        private WaveFileWriter _waveFileWriter;

        private string recorderOutputPath = string.Empty;

        // Parameters
        private double recorder_system_wait_for_exit_in_ms;
        private double recorder_system_stop_automatacilly_after_silence_of_ms;

        public RecorderMicrophoneSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
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

            // Parameters
            Params.TryGetValue("recorder_system_wait_for_exit_in_ms", out string? recorderSystemWaitForExitInMs);
            Params.TryGetValue("recorder_system_stop_automatacilly_after_silence_of_ms", out string? recorderSystemStopAutomatacillyAfterSilenceOfMs);

            double.TryParse(recorderSystemWaitForExitInMs, out recorder_system_wait_for_exit_in_ms);
            double.TryParse(recorderSystemStopAutomatacillyAfterSilenceOfMs, out recorder_system_stop_automatacilly_after_silence_of_ms);
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            var enumerator = new MMDeviceEnumerator();
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);

            // Create WasapiCapture to capture audio from the default input device (microphone)
            _wasapiCapture = new WasapiCapture(device);

            // Set up WaveFormat for audio capture
            WaveFormat waveFormat = new WaveFormat(44100, 16, device.AudioClient.MixFormat.Channels);

            // Set WaveFormat for capture
            _wasapiCapture.WaveFormat = waveFormat;

            // Set up a WaveFileWriter to save captured audio to a file
            _waveFileWriter = new WaveFileWriter(recorderOutputPath, waveFormat);

            // Variables to track silence detection
            bool isRecordingSilence = false;
            const int soundResetWindow = 3; // Reset the silence state if sound is detected within this window before silenceThreshold
            int consecutiveSilentFrames = 0;
            int consecutiveSoundFrames = 0;

            //Stopwatch stopwatch = Stopwatch.StartNew();stopwatch.Start();
            // Event handler for data available (audio captured)
            _wasapiCapture.DataAvailable += (sender, e) =>
            {
                // Write audio data to the file
                _waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);

                // Check audio levels for silence detection
                if (!isRecordingSilence)
                {
                    // Calculate average amplitude
                    float amplitude = CalculateAverageAmplitude(e.Buffer, e.BytesRecorded);
                    if (!float.IsNaN(amplitude))
                    {
                        //Console.WriteLine(amplitude);
                        if (amplitude < 100) // Adjust threshold as needed
                        {
                            consecutiveSilentFrames += 60;
                            consecutiveSoundFrames = 0;

                            if (consecutiveSilentFrames >= recorder_system_stop_automatacilly_after_silence_of_ms)
                            {
                                // Silence detected for the specified duration
                                isRecordingSilence = true;
                                //stopwatch.Stop();
                                //Console.WriteLine(stopwatch.ElapsedMilliseconds);
                            }
                        }
                        else
                        {
                            consecutiveSilentFrames = 0;
                            consecutiveSoundFrames++;

                            // Reset silence state if sound is detected within the reset window before silenceThreshold
                            if (consecutiveSoundFrames >= waveFormat.AverageBytesPerSecond / waveFormat.BlockAlign * soundResetWindow)
                            {
                                isRecordingSilence = false;
                            }
                            //stopwatch.Restart();
                        }
                    }
                    else
                    {
                        // Error in amplitude calculation, handle as needed
                    }
                }
            };

            // Start audio capture
            _wasapiCapture.StartRecording();

            LogInformation("Recording started.");

            try
            {
                // Wait until cancellation is requested or the timer elapses
                await WaitUntilConditionIsMet(cancellationToken, () => isRecordingSilence);
            }
            catch (Exception ex)
            {
                LogWarning(ex.Message);
            }
            finally
            {
                // Stop recording and clean up resources when the timer elapses or cancellation is requested
                _wasapiCapture.StopRecording();
                _waveFileWriter.Close();
                _waveFileWriter.Dispose();
                _wasapiCapture.Dispose();

                LogInformation("Recording has stopped.");

                LogInformation("Saved here: " + recorderOutputPath);
            }
        }

        private float CalculateAverageAmplitude(byte[] buffer, int bytesRecorded)
        {
            if (bytesRecorded == 0)
            {
                return float.NaN;
            }

            float amplitude = 0;
            for (int i = 0; i < bytesRecorded; i += 2)
            {
                short sample = BitConverter.ToInt16(buffer, i);
                amplitude += Math.Abs(sample);
            }
            amplitude /= bytesRecorded / 2;

            return amplitude;
        }


        private async Task WaitUntilConditionIsMet(CancellationToken cancellationToken, Func<bool> condition)
        {
            if (recorder_system_wait_for_exit_in_ms != 0 && condition != null)
            {
                System.Timers.Timer timer = new System.Timers.Timer(recorder_system_wait_for_exit_in_ms);
                timer.Start();

                while (!cancellationToken.IsCancellationRequested && !condition() && timer.Interval >= recorder_system_wait_for_exit_in_ms)
                {
                    // Check condition every 100 milliseconds
                    await Task.Delay(100);
                }

                timer.Stop();
                timer.Dispose();
            }
            else if (recorder_system_wait_for_exit_in_ms != 0)
            {
                System.Timers.Timer timer = new System.Timers.Timer(recorder_system_wait_for_exit_in_ms);
                timer.Start();

                while (!cancellationToken.IsCancellationRequested && timer.Interval >= recorder_system_wait_for_exit_in_ms)
                {
                    // Check condition every 100 milliseconds
                    await Task.Delay(100);
                }

                timer.Stop();
                timer.Dispose();
            }
            else if (condition != null)
            {
                while (!cancellationToken.IsCancellationRequested && !condition())
                {
                    // Check condition every 100 milliseconds
                    await Task.Delay(100);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(condition));
            }
        }
    }
}