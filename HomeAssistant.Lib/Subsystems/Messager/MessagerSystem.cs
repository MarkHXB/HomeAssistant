using Newtonsoft.Json;
using Notification.Environment;
using SubSystemComponent;

namespace Messager
{
    /// <summary>
    ///  this class doesnt do anything in standalone
    ///  <para></para>
    ///  you should add a messagersystem instance to the dependency chain where you wanna use it ( call from )
    /// </summary>
    public class MessagerSystem : Subsystem
    {
        public EventHandler<NotificationEventArgs> SendMessage;

        public MessagerSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
      base(ConfigObject.LogFilePath, @params, dependencies)
        {
            SendMessage += OnSendMessage;
        }

        private async void OnSendMessage(object? sender, NotificationEventArgs e)
        {
            if (e != null && e.Content != null)
            {
                var newNotification = e.Content;
                IList<NotificationObject> notifications;

                // create json file if not exists
                if (!File.Exists(ConfigObject.NotificationsFilePath))
                {
                    File.Create(ConfigObject.NotificationsFilePath).Close();
                }

                try
                {
                    // read json messages
                    string json = await File.ReadAllTextAsync(ConfigObject.NotificationsFilePath);
                    notifications = JsonConvert.DeserializeObject<List<NotificationObject>>(json) ?? new List<NotificationObject>();

                    // append new notifications to json
                    notifications.Add(newNotification);
                    json = JsonConvert.SerializeObject(notifications);
                    await File.WriteAllTextAsync(ConfigObject.NotificationsFilePath, json);
                }
                catch (Exception ex)
                {
                    LogError($"An error occurred while adding a new notification: {ex.Message}");
                    //throw;
                }
            }
        }

        public override void Initialize()
        {
            ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
            var config = configHandler.LoadConfig<ConfigObject>();

            // Null checks
            _ = config ?? throw new ArgumentNullException(nameof(config));
        }

        public override Task TaskObject(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                //var todos = GetSubsystem(nameof(Todo.TodoSystem))?.GetOutput<IEnumerable<TodoItem>>(false) ?? new List<TodoItem>();
                //var notifications = new List<NotificationObject>();

                //foreach (var todo in todos)
                //{
                //    notifications.Add(new NotificationObject(todo.Id, todo.Title, "asd", NotificationTypes.Toast));
                //}

                //ShowNotificationsOnLocal(notifications);
            });
        }
    }
}