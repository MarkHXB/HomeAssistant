using NAudio.Wave;
using SubSystemComponent;
using System.Timers;

namespace Recorder
{
    /// <summary>
    /// Following parameters should pass:
    /// recorder_system_wait_for_exit_in_ms => How many milliseconds should wait to stop the recording.
    /// recorder_system_stop_automatacilly_after_silence_of_ms => after ms silence the recorder stops automatically
    /// </summary>
    public class RecorderSystem : Subsystem
    {
        private WasapiLoopbackCapture _wasapiLoopback;
        private WaveFileWriter _waveFileWriter;

        private string recorderOutputPath = string.Empty;
        private string recorderOutputPathFileName = string.Empty;

        // Parameters
        private double recorder_system_wait_for_exit_in_ms;
        private double recorder_system_stop_automatacilly_after_silence_of_ms;

        public RecorderSystem( params Subsystem[] dependencies) :
     base(ConfigObject.LogFilePath, dependencies)
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

            recorderOutputPathFileName = config.RecorderOutputPath;

			// Parameters
			recorder_system_wait_for_exit_in_ms = GetParameter<double>("recorder_system_wait_for_exit_in_ms");
			recorder_system_stop_automatacilly_after_silence_of_ms = GetParameter<double>("recorder_system_stop_automatacilly_after_silence_of_ms");
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            recorderOutputPath = $"{recorderOutputPathFileName}_{DateTime.Now.Ticks}.wav";

            // Create WasapiLoopbackCapture to capture system audio
            _wasapiLoopback = new WasapiLoopbackCapture();

            // Set up WaveFormat for audio capture
            WaveFormat waveFormat = new WaveFormat(44100, 16, 2);

            // Set WaveFormat for capture
            _wasapiLoopback.WaveFormat = waveFormat;

            // Set up a WaveFileWriter to save captured audio to a file
            _waveFileWriter = new WaveFileWriter(recorderOutputPath, waveFormat);


            // Variables to track silence detection
            bool isRecordingSilence = recorder_system_stop_automatacilly_after_silence_of_ms == 0;
            const int soundResetWindow = 3; // Reset the silence state if sound is detected within this window before silenceThreshold
            int consecutiveSilentFrames = 0;
            int consecutiveSoundFrames = 0;

            //Stopwatch stopwatch = Stopwatch.StartNew();stopwatch.Start();
            // Event handler for data available (audio captured)
            _wasapiLoopback.DataAvailable += (sender, e) =>
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
            _wasapiLoopback.StartRecording();

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
                _wasapiLoopback.StopRecording();
                _waveFileWriter.Close();
                _waveFileWriter.Dispose();
                _wasapiLoopback.Dispose();

                //string filename = Path.GetFileNameWithoutExtension(recorderOutputPath);
                //filename = "processed_" + filename+ ".wav";
                //string path = Path.GetDirectoryName(recorderOutputPath);
                //File.Move(recorderOutputPath, Path.Combine(path, filename));

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
            if (recorder_system_wait_for_exit_in_ms != 0 && recorder_system_stop_automatacilly_after_silence_of_ms != 0)
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
                await Task.Delay((int)recorder_system_wait_for_exit_in_ms);             
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