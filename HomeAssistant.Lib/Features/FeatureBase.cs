using HomeAssistant.Lib.Features.Features;
using LlamaStudio;
using Messager;
using Recorder;
using Runner;
using Scheduler;
using SubSystemComponent;
using Todo;

namespace HomeAssistant.Lib.Features
{
    public enum ExecuteAction
    {
        RUN
    }

    public class FeatureBase
    {
        private Subsystem AppointmentFounderInRecordedAudioSubsysetm()
        {
            Dictionary<string, string> @params = new()
             {
                  {"runner_system_file_path", @"Scripts/whisper"},
                  {"runner_system_file_name", @"whisper.py"},
                  {"runner_system_arguements", @"C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\output.wav C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\llama_input.txt"},
                  {"recorder_system_wait_for_exit_in_ms", "5000"},
                  {"recorder_system_stop_automatacilly_after_silence_of_ms", "1000"},
                  {"llamastudio_system_message", "You are an AI assistant who helps to date or appointments in text"},
                  {"llamastudio_user_prompt", $"If you find any date or appointment in the text then answer only like this: _title_of_the_event, year:month:day:hour:minute_. If the year not known, then current year is: {DateTime.Now.Year} and month is: {DateTime.Now.Month}. If you didn't find then only answer: Not found!"},
                  {"make_json_object", "true"}
             };

            var recorderSystem = SubSystemFactory<RecorderSystem>.Create(@params);
            var fasterWhisperSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderSystem);

            var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, fasterWhisperSystem);

            return llamaSystem;
        }

        /// <summary>
        /// this feature is not works with api, because it is platform specific due to local microphone recording
        /// </summary>
        private Feature AppointmentFounderInRecordedAudio
        {
            get
            {
                return new Feature
                {
                    Name = "Appointment Founder",
                    Notifications = 0,
                    Subsystem = AppointmentFounderInRecordedAudioSubsysetm(),
                    Output = string.Empty,
                };
            }
        }

        private Subsystem EventReminderSubsystem()
        {
            Dictionary<string, string> @params = new()
            {
                { "todo_system_command", "GETALL"},
            };

            var todoSystem = SubSystemFactory<TodoSystem>.Create(@params);
            var messagerSystem = SubSystemFactory<MessagerSystem>.Create(@params, todoSystem);

            return messagerSystem;
        }

        public Feature EventReminder
        {
            get
            {
                return new Feature
                {
                    Name = "Event reminder",
                    Notifications = 0,
                    Subsystem = EventReminderSubsystem(),
                    Output = string.Empty,
                };
            }
        }

        public IList<Feature> Features { get; private set; }

        public FeatureBase()
        {
            Features = new List<Feature>
            {
                //AppointmentFounderInRecordedAudio,
                EventReminder
            };
        }

        public async Task Run(Feature feature, CancellationToken cancellationToken = default, int cyclicOfRunningInMilliseconds = 0)
        {
            SubsystemPool.AddSubsystem(feature.Subsystem);

            if(cyclicOfRunningInMilliseconds > 10) // 10ms to avoid high cpu usage
            {
                while (true)
                {
                    await SubsystemPool.RunAllAsync(cancellationToken);
                    feature.Output = LlamaStudio.SharedObject.LlamaOutputFilePathTxt;

                    Thread.Sleep(cyclicOfRunningInMilliseconds);
                }
            }
            else
            {
                await SubsystemPool.RunAllAsync(cancellationToken);  
                //a messagert tedd subsystemme, hogy egymas utan lehesen hasznalni oket, igy a messagersystem mar a friss todokkal tud dolgozni, tovabba ne nyiss public methodokat subsystemekre, mert ez a lenyege hogy fuggetlen
            }
        }
    }
}
