using HomeAssistant.Lib.Subsystems.Todo;
using Messager;
using SubSystemComponent;
using System.Data.SQLite;

namespace Todo
{
    /// <summary>
    /// Following parameters should pass:
    /// todo_system_command <para></para>
    ///        todo_system_id <para></para>
    ///       todo_system_new_item_title <para></para>
    ///     todo_system_new_item_duetodate <para></para>
    ///       todo_system_new_item_reminderdate <para></para>
    ///    todo_system_new_item_iscompleted <para></para>
    ///
    /// </summary>
    public class TodoSystem : Subsystem, ITodoTasks
    {
        private string ConnectionString => $"Data Source={ConfigObject.SQLiteFilePath};Version=3;";

        // Output
        public IList<TodoItem> TodoItems { get; private set; }


        // params
        private TodoCommands _command;
        private string? _todoId;
        private string? _newTodoTitle;
        private bool _newTodoIsCompleted;
        private DateTime _newTodoDueToDate;
        private DateTime _newTodoReminderDate;

        private MessagerSystem? _messagerSystem;

        public TodoSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
    base(ConfigObject.LogFilePath, @params, dependencies)
        {
            TodoItems = new List<TodoItem>();
        }

        public override void Initialize()
        {
            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();

            // Parameters
            Params.TryGetValue("todo_system_command", out string? _todoCommand);
            Params.TryGetValue("todo_system_id", out _todoId);
            Params.TryGetValue("todo_system_new_item_title", out _newTodoTitle);
            Params.TryGetValue("todo_system_new_item_duetodate", out string? todoSystemDueToDate);
            Params.TryGetValue("todo_system_new_item_reminderdate", out string? todoSystemReminderDate);
            Params.TryGetValue("todo_system_new_item_iscompleted", out string? todoSystemIsCompleted);

            Enum.TryParse(_todoCommand, out _command);

            DateTime.TryParse(todoSystemDueToDate, out _newTodoDueToDate);
            DateTime.TryParse(todoSystemReminderDate, out _newTodoReminderDate);
            bool.TryParse(todoSystemIsCompleted, out _newTodoIsCompleted);

            _messagerSystem = GetSubsystem<MessagerSystem>();
            _messagerSystem?.SendMessage.Invoke(this, new NotificationEventArgs("Asd", "asd"));

            InitializeDatabase();
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            switch (_command)
            {
                case TodoCommands.GET:
                    await GetAsync(_todoId);
                    break;
                case TodoCommands.GETALL:
                    await GetAllAsync();
                    break;
                case TodoCommands.DELETE:
                    await DeleteAsync(_todoId);
                    break;
                case TodoCommands.UPDATE:
                    var alterItem = new TodoItem { Id = _todoId, Title = _newTodoTitle, DueDate = _newTodoDueToDate, ReminderDate = _newTodoReminderDate, IsCompleted = _newTodoIsCompleted };
                    await UpdateAsync(alterItem);
                    break;
                case TodoCommands.ADD:
                    var newItem = new TodoItem { Title = _newTodoTitle, DueDate = _newTodoDueToDate, ReminderDate = _newTodoReminderDate };
                    await AddAsync(newItem);
                    break;
            }
        }

        public async Task GetAsync(string? id)
        {
            TodoItems.Clear();

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT Id, Title, DueDate, ReminderDate, IsCompleted, IsNotifiedDueDate, IsNotifiedReminderDate FROM TodoItems WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            TodoItems.Add(new TodoItem
                            {
                                Title = reader.GetString(1),
                                DueDate = reader.GetDateTime(2),
                                ReminderDate = reader.GetDateTime(3),
                                IsCompleted = reader.GetBoolean(4),
                                IsNotifiedDueDate = reader.GetBoolean(5),
                                IsNotifiedReminderDate = reader.GetBoolean(6),
                            });
                        }
                    }
                }
            }

            AddOutput<IEnumerable<TodoItem>>(TodoItems);
        }

        public async Task GetAllAsync()
        {
            TodoItems.Clear();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT Id, Title, DueDate, ReminderDate, IsCompleted, IsNotifiedDueDate, IsNotifiedReminderDate FROM TodoItems";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        TodoItems.Add(new TodoItem
                        {
                            Id = reader.GetString(0),
                            Title = reader.GetString(1),
                            DueDate = reader.GetDateTime(2),
                            ReminderDate = reader.GetDateTime(3),
                            IsCompleted = reader.GetBoolean(4),
                            IsNotifiedDueDate = reader.GetBoolean(5),
                            IsNotifiedReminderDate = reader.GetBoolean(6),
                        });
                    }
                }
            }

            AddOutput<IEnumerable<TodoItem>>(TodoItems);
        }

        public async Task AddAsync(TodoItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                throw new ArgumentNullException(nameof(item));
            }
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string insertQuery = "INSERT INTO TodoItems (Id, Title, DueDate, ReminderDate, IsCompleted, IsNotifiedDueDate, IsNotifiedReminderDate) VALUES (@Id, @Title, @DueDate, @ReminderDate, @IsCompleted, @IsNotifiedDueDate, @IsNotifiedReminderDate)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", item.Id ?? Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@DueDate", item.DueDate);
                    command.Parameters.AddWithValue("@ReminderDate", item.ReminderDate);
                    command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);
                    command.Parameters.AddWithValue("@IsNotifiedDueDate", item.IsNotifiedDueDate);
                    command.Parameters.AddWithValue("@IsNotifiedReminderDate", item.IsNotifiedReminderDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(TodoItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                throw new ArgumentNullException($"{nameof(item.Title)}");
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string updateQuery = "UPDATE TodoItems SET Title = @Title, DueDate = @DueDate, ReminderDate = @ReminderDate, IsCompleted = @IsCompleted, IsNotifiedDueDate = @IsNotifiedDueDate, IsNotifiedReminderDate = @IsNotifiedReminderDate WHERE Id = @Id";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", item.Id);
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@DueDate", item.DueDate);
                    command.Parameters.AddWithValue("@ReminderDate", item.ReminderDate);
                    command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted ? 1 : 0);
                    command.Parameters.AddWithValue("@IsNotifiedDueDate", item.IsNotifiedDueDate ? 1 : 0);
                    command.Parameters.AddWithValue("@IsNotifiedReminderDate", item.IsNotifiedReminderDate ? 1 : 0);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                await connection.OpenAsync();

                string deleteQuery = "DELETE FROM TodoItems WHERE Id = @Id";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS TodoItems (
                        Id TEXT PRIMARY KEY,
                        Title TEXT NOT NULL,
                        DueDate DATETIME NOT NULL,
                        ReminderDate DATETIME NOT NULL,
                        IsCompleted BOOLEAN NOT NULL,
                        IsNotifiedDueDate BOOLEAN NOT NULL,
                        IsNotifiedReminderDate BOOLEAN NOT NULL
                    );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        internal IEnumerable<TodoItem> GetTodos()
        {
            var now = DateTime.Now;

            foreach (var todo in TodoItems)
            {
                if (now >= todo.ReminderDate || now >= todo.DueDate)
                {
                    yield return todo;
                }
            }
        }
    }
}