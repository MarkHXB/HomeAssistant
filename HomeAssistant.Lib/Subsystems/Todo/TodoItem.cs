namespace HomeAssistant.Lib.Subsystems.Todo
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsNotifiedDueDate { get; set; }
        public bool IsNotifiedReminderDate { get; set; }

        public override string ToString()
        {
            return $"{Title} - ";
        }
    }
}
