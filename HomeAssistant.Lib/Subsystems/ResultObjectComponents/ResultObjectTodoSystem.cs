using HomeAssistant.Lib.Subsystems.Todo;

namespace ResultObjectComponents
{
    public class ResultObjectTodoSystem : ResultObject    
    {
        public TodoCommands Command { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsNotifiedDueDate { get; set; }
        public bool IsNotifiedReminderDate { get; set; }
    }
}
