using HomeAssistant.Lib.Subsystems.Todo;
using SubSystemComponent;
using System.Text.RegularExpressions;

namespace ResultObjectComponents
{
    public class ResultObjectResolverSystem : Subsystem
    {
        private string? resultObjectInput;

        public ResultObjectResolverSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
            base(ConfigObject.LogFilePath, @params, dependencies)
        {

        }

        public override void Initialize()
        {
            
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            resultObjectInput = GetSubsystem("LlamaStudioSystem")?.GetOutput<string>(false);

            if (string.IsNullOrWhiteSpace(resultObjectInput))
            {
                throw new ArgumentNullException(nameof(resultObjectInput));
            }

            if (resultObjectInput.Contains("todo_system"))
            {
                var resultObjectTodoSystem = new ResultObjectTodoSystem();
                var values = ExtractValues("todo_system", resultObjectInput);

                values.TryGetValue("todo_system_command", out string command);
                values.TryGetValue("todo_system_id", out string id);
                values.TryGetValue("todo_system_new_item_title", out string title);
                values.TryGetValue("todo_system_new_item_duetodate", out string duedate);
                values.TryGetValue("todo_system_new_item_reminderdate", out string reminderdate);

                Enum.TryParse(command, out TodoCommands todoCommand);
                DateTime.TryParse(duedate, out DateTime dueDate);
                DateTime.TryParse(reminderdate, out DateTime reminderDate);

                resultObjectTodoSystem.Command = todoCommand;
                resultObjectTodoSystem.Id = id;
                resultObjectTodoSystem.Title = title;
                resultObjectTodoSystem.ReminderDate = reminderDate;
                resultObjectTodoSystem.DueDate = dueDate;

                AddOutput(resultObjectTodoSystem);


            }
        }

        public static Dictionary<string, string> ExtractValues(string subSystem, string input)
        {
            // Unescape the string to handle escape characters
            string unescapedInput = Regex.Unescape(input);

            // Regular expression to match key-value pairs
            string pattern = $@"(?<key>{subSystem}_[^=\n]+)=(?<value>.*?)(?=\n{subSystem}_|$)";

            // Create a dictionary to hold the extracted values
            Dictionary<string, string> values = new Dictionary<string, string>();

            // Find all matches
            MatchCollection matches = Regex.Matches(unescapedInput, pattern);

            // Loop through matches and add to dictionary
            foreach (Match match in matches)
            {
                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;
                values[key] = value;
            }

            return values;
        }
    }
}

