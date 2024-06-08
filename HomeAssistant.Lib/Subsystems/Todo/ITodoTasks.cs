namespace HomeAssistant.Lib.Subsystems.Todo
{
    public interface ITodoTasks
    {
        Task GetAsync(string id);
        Task GetAllAsync();
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(string id);
    }
}
