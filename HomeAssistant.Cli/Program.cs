using LlamaStudio;
using MicrosoftTextToSpeech;
using Recorder;
using RecorderMicrophone;
using Runner;
using SoundAudio;
using SubSystemComponent;

var appointmentFounderInRecordedAudio = async () =>
{
    Dictionary<string, string> @params = new()
    {
         {"runner_system_file_path", @"Scripts/whisper"},
         {"runner_system_file_name", @"asd3.py"},
         {"runner_system_arguements", @"C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\output.wav C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\llama_input.txt"},
         {"recorder_system_wait_for_exit_in_ms", "15000"},
         {"recorder_system_stop_automatacilly_after_silence_of_ms", "3000"},
         {"llamastudio_system_message", "You are an AI assistant who helps to date or appointments in text"},
         {"llamastudio_user_prompt", $"If you find any date or appointment in the text then answer only like this: _title_of_the_event, year:month:day:hour:minute_. If the year not known, then current year is: {DateTime.Now.Year} and month is: {DateTime.Now.Month}. If you didn't find then only answer: Not found!"},
         {"make_json_object", "true"}
    };

    var recorderSystem = SubSystemFactory<RecorderSystem>.Create(@params);
    var fasterWhisperSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderSystem);
    var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, fasterWhisperSystem);

    SubsystemPool.AddSubsystem(llamaSystem);

    await SubsystemPool.RunAllAsync(new CancellationToken());
}; // recording output and looking for dates or appointments in text

var sonnya = async () =>
{
    Dictionary<string, string> @params = new()
    {
        {"runner_system_file_path", @"Scripts/speech_recognition"},
        {"runner_system_file_name", @"recognizer.py"},
        {"runner_system_arguements", @"2000 C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\llama_input.txt"},
    };

    var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params);

    @params = new()
    {
        {"llamastudio_system_message", "You are an AI assistant. Your name is Sonya. You are funny, sarcastic, funny and your main task is to help me. You are making a point."},
    };

    var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, speechRecognitionSystem);
    var speechSynthesisSystem = SubSystemFactory<MicrosoftTextToSpeechSystem>.Create(@params, llamaSystem);

    SubsystemPool.AddSubsystem(speechSynthesisSystem);

    while (true)
    {
        await SubsystemPool.RunAllAsync(new CancellationToken());
    }
}; // ai assistant like Jarvis - English

var imre = async () =>
{
    Dictionary<string, string> @params = new()
    {
        {"recorder_system_wait_for_exit_in_ms", "2000"},
        {"recorder_system_stop_automatacilly_after_silence_of_ms", "2000"},
    };

    var recorderMicrophoneSystem = SubSystemFactory<RecorderMicrophoneSystem>.Create(@params);

    @params = new()
    {
         {"runner_system_file_path", @"Scripts/whisper"},
         {"runner_system_file_name", @"asd3.py"},
         {"runner_system_arguements", @"C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\output.wav C:\Users\sasli\source\repos\HomeAssistant\HomeAssistant.Cli\bin\Debug\net8.0\llama_input.txt False"},
    };

    var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderMicrophoneSystem);

    @params = new()
    {
        {"llamastudio_system_message", "Imrének hivnak. Egy AI asszisztens vagy, aki lényegretörően válaszol."},
    };

    var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, speechRecognitionSystem);

    @params = new()
    {
         {"runner_system_file_path", @"C:\Users\sasli\Downloads\piper_windows_amd64\piper"},
         {"runner_system_file_name", @"piper.exe"},
         {"runner_system_arguements", @"-m C:\Users\sasli\Downloads\piper_windows_amd64\piper\hu_HU-imre-medium.onnx -f C:\Users\sasli\Downloads\piper_windows_amd64\piper\output.wav --json-input "},
         {"runner_system_dependent_subsystem_output_path", llamaSystem.GetOutputPath() },
         {"runner_system_dependent_convert_output_path_to_json", "true" }
    };

    var speechSynthesisSystem = SubSystemFactory<RunnerSystem>.Create(@params, llamaSystem);

    @params = new();

    var soundAudio = SubSystemFactory<SoundAudioSystem>.Create(@params, speechSynthesisSystem);

    SubsystemPool.AddSubsystem(soundAudio);

    while (true)
    {
        await SubsystemPool.RunAllAsync(new CancellationToken());
    }
}; // ai assistant like Jarvis - Hungarian

await imre();