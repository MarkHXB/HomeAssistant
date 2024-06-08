using LlamaStudio;
using MicrosoftTextToSpeech;
using Recorder;
using RecorderMicrophone;
using ResultObjectComponents;
using Runner;
using SoundAudio;
using SubSystemComponent;
using Todo;

var appointmentFounderInRecordedAudio = async () =>
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
    //var schedulerSystem = SubSystemFactory<SchedulerSystem>.Create(@params, recorderSystem); // should place the end of the chain
    //schedulerSystem.AssignAction(MessagerSystem.Run);
    SubsystemPool.AddSubsystem(llamaSystem);
    await SubsystemPool.RunAllAsync(new CancellationToken());

}; // recording output and looking for dates or appointments in text

var sonnya = async () =>
{
    Dictionary<string, string> @params = new()
    {
        {"runner_system_file_path", @"Scripts/speech_recognition"},
        {"runner_system_file_name", @"recognizer.py"},
        {"runner_system_arguements", $@"2000 {LlamaStudio.SharedObject.LLamaInputTextFile}"},
    };

    var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params);

    @params = new()
    {
        {"llamastudio_system_message", "Some meta information about me:\r\n{\r\n my home location: 47.5960659,19.340621,\r\n my favourite food: hamburger,\r\n my favourite music genre: dubstep\r\n}\r\nInstruction:\r\nYou have direct access to command line on windows, so answer only with code or commands because the whole answer goes into bash. Always start the command or code with: Code:"},
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

    while (true)
    {
        await SubsystemPool.RunAllAsync(new CancellationToken());
    }
}; // ai assistant like Jarvis - Hungarian

var cori = async () =>
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

    while (true)
    {
        await SubsystemPool.RunAllAsync(new CancellationToken());
    }
}; // ai assistant like Jarvis - English

var prgoramMaker = async () =>
{
    Dictionary<string, string> @params = new()
    {
        {"recorder_system_wait_for_exit_in_ms", "2000"},
        {"recorder_system_stop_automatacilly_after_silence_of_ms", "3000"},
        {"recorder_system_watch_words", "Hey Cori"},
    };

    var recorderMicrophoneSystem = SubSystemFactory<RecorderMicrophoneSystem>.Create(@params);

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

    SubsystemPool.AddSubsystem(llamaSystem);

    while (true)
    {
        await SubsystemPool.RunAllAsync(new CancellationToken());
    }
};

var todoTest = async () =>
{
    Dictionary<string, string> @params = new()
    {
        // {"todo_system_id", "9cc4c819-58fb-45fb-8c81-7091c94a93c6"},
        //{"todo_system_command", "ADD"},
        //{"todo_system_new_item_title", "Test2"},
        //{ "todo_system_new_item_duetodate", "2024.06.06."},
        //{"todo_system_new_item_reminderdate", "2024.06.05."},
        //{"todo_system_new_item_iscompleted", "fa"},
    };
    //Dictionary<string, string> @params = new()
    //{
    //    { "todo_system_command", "GETALL"},
    //};

    var todoSystem = SubSystemFactory<TodoSystem>.Create(@params);

    SubsystemPool.AddSubsystem(todoSystem);

    await SubsystemPool.RunAllAsync(new CancellationToken());
};
//await todoTest();
//var featureBase = new FeatureBase();
//await featureBase.Run(featureBase.EventReminder);

var passive_cori = async () =>
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
         {"runner_system_arguements", $@"{RecorderMicrophone.SharedObject.RecorderOutputPath} {LlamaStudio.SharedObject.LLamaInputTextFile} False en"},
    };

    var speechRecognitionSystem = SubSystemFactory<RunnerSystem>.Create(@params, recorderMicrophoneSystem);

    @params = new()
    {
        {"llamastudio_system_message", "You are an AI assistant called Cori. Answer only ResultObject if you finish. If you find the text relevant to any descriptions then please make a resultobject based on rules.\n" +
        "Topics:" +
        "Description:\r\nA ToDo application allows users to manage their tasks by adding, editing, and deleting items. It provides a clear and organized interface to track and prioritize daily activities efficiently.\r\nCommands:\r\n[\r\nGET, GETALL, UPDATE, DELETE, ADD\r\n]\r\nStruct:\r\n(\r\npublic string Id { get; set; }\r\npublic string Title { get; set; }\r\npublic DateTime DueDate { get; set; }\r\npublic DateTime ReminderDate { get; set; }\r\npublic bool IsCompleted { get; set; }\r\npublic bool IsNotifiedDueDate { get; set; }\r\npublic bool IsNotifiedReminderDate { get; set; }\r\n)\r\nResultObject:\r\n(\r\ntodo_system_command=[COMMAND]\r\ntodo_system_id=[Id]\r\ntodo_system_new_item_title=[Title]\r\ntodo_system_new_item_duetodate=[DueDate]\r\ntodo_system_new_item_reminderdate=[ReminderDate]\r\ntodo_system_new_item_iscompleted=[IsComplete]\r\n)"},
    };

    var llamaSystem = SubSystemFactory<LlamaStudioSystem>.Create(@params, speechRecognitionSystem);

    @params = new();

    var resultObjectResolverSystem = SubSystemFactory<ResultObjectResolverSystem>.Create(@params, llamaSystem);

    @params = new();

    var resultObjectHandlerSystem = SubSystemFactory<ResultObjectHandlerSystem>.Create(@params, resultObjectResolverSystem);

    SubsystemPool.AddSubsystem(resultObjectHandlerSystem);

    await SubsystemPool.RunAllAsync(new CancellationToken());
}; // ai assistant like Jarvis - English

await passive_cori();