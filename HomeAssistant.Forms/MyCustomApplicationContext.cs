using HomeAssistant.Lib.Utils;
using LlamaStudio;
using RecorderMicrophone;
using Runner;
using SoundAudio;
using SubSystemComponent;
using System.Runtime.InteropServices;

namespace HomeAssistant.Forms
{
    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        private ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

        public MyCustomApplicationContext()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Tray Application";
            trayIcon.Icon = Resource1.AppIcon;
            trayIcon.Visible = true;

            contextMenuStrip.Items.Add("Cori", null, LoadCori);
            contextMenuStrip.Items.Add("Imre", null, LoadImre);
            contextMenuStrip.Items.Add("Screen Read", null, btnCapture_Click);
            contextMenuStrip.Items.Add("ToDos", null, btnAddTodo_Click);
            contextMenuStrip.Items.Add("Money Tracking", null, btnMoneyTracker_Click);
            contextMenuStrip.Items.Add("Exit", null, OnExit);
            trayIcon.ContextMenuStrip = contextMenuStrip;
        }

        private void btnMoneyTracker_Click(object? sender, EventArgs e)
        {
            using (MoneyTrackingForm form = new MoneyTrackingForm())
            {
                form.ShowDialog();
            }
        }

        private void btnAddTodo_Click(object? sender, EventArgs e)
        {
            using (TodoForm form = new TodoForm())
            {
                form.ShowDialog();
            }
        }

        private async void LoadImre(object sender, EventArgs e)
        {
            Dictionary<string, string> @params = new()
            {
                {"recorder_system_wait_for_exit_in_ms", "2000"},
                {"recorder_system_stop_automatacilly_after_silence_of_ms", "3000"},
            };

            var recorderMicrophoneSystem = SubSystemFactory<RecorderMicrophoneSystem>.Create(@params);

            @params = new()
            {
                 {"runner_system_file_path", @"Scripts/whisper"},
                 {"runner_system_file_name", @"whisper.py"},
                 {"runner_system_arguements", $@"{RecorderMicrophone.SharedObject.RecorderOutputPath} {LlamaStudio.SharedObject.LLamaInputTextFile} False hu"},
            };

            var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderMicrophoneSystem);

            @params = new()
            {
                {"llamastudio_system_message", "Te egy magyarul beszélő mesterséges intelligencia vagy."},
            };

            var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, speechRecognitionSystem);

            @params = new()
            {
                 {"runner_system_file_path", @"C:\Users\sasli\Downloads\piper_windows_amd64\piper"},
                 {"runner_system_file_name", @"piper.exe"},
                 {"runner_system_arguements", @"-m C:\Users\sasli\Downloads\piper_windows_amd64\piper\hu_HU-imre-medium.onnx -f C:\Users\sasli\Downloads\piper_windows_amd64\piper\output.wav --json-input "},
                 {"runner_system_dependent_subsystem_output_path", LlamaStudio.SharedObject.LlamaOutputFilePathTxt },
                 {"runner_system_dependent_convert_output_path_to_json", "true" }
            };

            var speechSynthesisSystem = SubSystemFactory<RunnerSystem>.Create(@params, llamaSystem);

            @params = new();

            var soundAudio = SubSystemFactory<SoundAudioSystem>.Create(@params, speechSynthesisSystem);

            SubsystemPool.AddSubsystem(soundAudio);

            await SubsystemPool.RunAllAsync(new CancellationToken());

        } // ai assistant like Jarvis - Hungarian

        private async void LoadCori(object sender, EventArgs e)
        {
            Dictionary<string, string> @params = new()
            {
                {"recorder_system_wait_for_exit_in_ms", "2000"},
                {"recorder_system_stop_automatacilly_after_silence_of_ms", "3000"},
            };

            var recorderMicrophoneSystem = SubSystemFactory<RecorderMicrophoneSystem>.Create(@params);

            recorderMicrophoneSystem.RecordWhileHoldingLAlt = true;

            @params = new()
            {
                 {"runner_system_file_path", @"Scripts/whisper"},
                 {"runner_system_file_name", @"whisper.py"},
                 {"runner_system_arguements", $@"{RecorderMicrophone.SharedObject.RecorderOutputPath} {LlamaStudio.SharedObject.LLamaInputTextFile} False en"},
            };

            var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderMicrophoneSystem);

            @params = new()
            {
                {"llamastudio_system_message", "You are an AI assistant called Cori. Answer shortly."},
            };

            var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, speechRecognitionSystem);

            @params = new()
            {
                 {"runner_system_file_path", @"C:\Users\sasli\Downloads\piper_windows_amd64\piper"},
                 {"runner_system_file_name", @"piper.exe"},
                 {"runner_system_arguements", @"-m C:\Users\sasli\Downloads\piper_windows_amd64\piper\en_GB-cori-high.onnx -f C:\Users\sasli\Downloads\piper_windows_amd64\piper\output.wav --json-input "},
                 {"runner_system_dependent_subsystem_output_path", LlamaStudio.SharedObject.LlamaOutputFilePathTxt },
                 {"runner_system_dependent_convert_output_path_to_json", "true" }
            };

            var speechSynthesisSystem = SubSystemFactory<RunnerSystem>.Create(@params, llamaSystem);

            @params = new();

            var soundAudio = SubSystemFactory<SoundAudioSystem>.Create(@params, speechSynthesisSystem);

            SubsystemPool.AddSubsystem(soundAudio);

            await SubsystemPool.RunAllAsync(new CancellationToken());
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
       
        private async void btnCapture_Click(object sender, EventArgs e)
        {     
            // Define the path where you want to save the screenshot
            string savePath = @"C:\temp\asd.png";

            using (Form1 form1 = new Form1(savePath))
            {
                form1.ShowDialog();
            }
            // run screen read - gether text from pic
            Dictionary<string, string> @params = new()
            {
                 {"runner_system_file_path", @"C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Lib\Scripts\screenread"},
                 {"runner_system_file_name", @"main.py"},
                 {"runner_system_arguements", @$"{savePath} {SharedPaths.LLamaInputScreenReaderFilePath}"},
            };
            var screenReadSystem = SubSystemFactory<RunnerSystem>.Create(@params);

            // run microphone - speech recording
            @params = new()
            {
                {"recorder_system_wait_for_exit_in_ms", "2000"},
                {"recorder_system_stop_automatacilly_after_silence_of_ms", "3000"},
            };

            var recorderMicrophoneSystem = SubSystemFactory<RecorderMicrophoneSystem>.Create(@params, screenReadSystem);

            // run whisper - speech recognition
            @params = new()
            {
                 {"runner_system_file_path", @"Scripts/whisper"},
                 {"runner_system_file_name", @"whisper.py"},
                 {"runner_system_arguements", $@"{RecorderMicrophone.SharedObject.RecorderOutputPath} {LlamaStudio.SharedObject.LLamaInputTextFile} False en"},
            };

            var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderMicrophoneSystem);

            // run llama - generate answer
            @params = new()
            {
                {"llamastudio_system_message", $"You are an AI assistant called Cori. Answer shortly! Only answer not more questions."},
                {"llamastudio_screenreader", "true"},
                //{"llamastudio_system_message", $"You are an AI assistant called Cori. Answer shortly! What is the meaning of this?"},// add the recorded text so, What is the meaning of this? and the readed text from screen as user_prompt
            };

            var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, speechRecognitionSystem);

            // run piper - generate sound for answer
            @params = new()
            {
                 {"runner_system_file_path", @"C:\Users\sasli\Downloads\piper_windows_amd64\piper"},
                 {"runner_system_file_name", @"piper.exe"},
                 {"runner_system_arguements", @"-m C:\Users\sasli\Downloads\piper_windows_amd64\piper\en_GB-cori-high.onnx -f C:\Users\sasli\Downloads\piper_windows_amd64\piper\output.wav --json-input "},
                 {"runner_system_dependent_subsystem_output_path", LlamaStudio.SharedObject.LlamaOutputFilePathTxt },
                 {"runner_system_dependent_convert_output_path_to_json", "true" }
            };

            var speechSynthesisSystem = SubSystemFactory<RunnerSystem>.Create(@params, llamaSystem);

            @params = new();

            var soundAudio = SubSystemFactory<SoundAudioSystem>.Create(@params, speechSynthesisSystem);

            SubsystemPool.AddSubsystem(soundAudio);

            await SubsystemPool.RunAllAsync(new CancellationToken());
        }
    }
}
