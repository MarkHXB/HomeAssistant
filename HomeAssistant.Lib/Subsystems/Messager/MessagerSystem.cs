using HomeAssistant.Lib.Subsystems.Todo;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using SubSystemComponent;

namespace Messager
{
    public class MessagerSystem : Subsystem
    {
        // Params

        public MessagerSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
      base(ConfigObject.LogFilePath, @params, dependencies)
        {

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
                var todos = GetSubsystem(nameof(Todo.TodoSystem))?.GetOutput<IEnumerable<TodoItem>>(false) ?? new List<TodoItem>();
                var notifications = new List<NotificationObject>();

                foreach (var todo in todos)
                {
                    notifications.Add(new NotificationObject(todo.Id, todo.Title, "asd", NotificationTypes.Toast));
                }

                ShowNotificationsOnLocal(notifications);
            });   
        }
       
        //private void AddNotification(NotificationObject notification) => AddNewNotificationToFile(notification);

        private void ShowNotificationsOnLocal(List<NotificationObject> notifications)
        {
            //var notifications = await LoadNotificationsFromFile();

            foreach (var notification in notifications)
            {
                if (!notification.Notified)
                {
                    CreateNotification(notification);

                    notification.Notified = true;
                }
            }

            //await SaveNotificationsToFile(notifications);
        }

        #region Private methods
        //private async void AddNewNotificationToFile(NotificationObject notification)
        //{
        //    try
        //    {
        //        var notifications = await LoadNotificationsFromFile();
        //        var notificationList = new List<NotificationObject>(notifications) { notification };
        //        await SaveNotificationsToFile(notificationList);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (e.g., log the error)
        //        LogError($"An error occurred while adding a new notification: {ex.Message}");
        //        throw;
        //    }
        //}

        //private async Task SaveNotificationsToFile(IEnumerable<NotificationObject> notificationObjects)
        //{
        //    try
        //    {
        //        string json = JsonConvert.SerializeObject(notificationObjects, Formatting.Indented);
        //        using (StreamWriter writer = new StreamWriter(ConfigObject.NotificationsFilePath, false))
        //        {
        //            await writer.WriteAsync(json);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (e.g., log the error)
        //        LogError($"An error occurred while saving notifications: {ex.Message}");
        //        throw;
        //    }
        //}

        //private async Task<IEnumerable<NotificationObject>> LoadNotificationsFromFile()
        //{
        //    try
        //    {
        //        using (StreamReader reader = new StreamReader(ConfigObject.NotificationsFilePath))
        //        {
        //            string json = await reader.ReadToEndAsync();
        //            return JsonConvert.DeserializeObject<IEnumerable<NotificationObject>>(json) ?? new List<NotificationObject>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (e.g., log the error)
        //        LogError($"An error occurred while loading notifications: {ex.Message}");
        //        return new List<NotificationObject>();
        //    }
        //}

        private void CreateNotification(NotificationObject notification)
        {
            switch (notification.Type)
            {
                case NotificationTypes.Toast:
                    CreateToastNotification(notification);
                    break;
                case NotificationTypes.Assistant:
                    CreateAssistantNotificationAsync(notification).ConfigureAwait(false);
                    break;
                case NotificationTypes.Remotes:
                    CreateRemotesNotificationAsync(notification).ConfigureAwait(false);
                    break;
            }
        }

        private void CreateToastNotification(NotificationObject notification)
        {
            
        }

        private async Task CreateAssistantNotificationAsync(NotificationObject notification)
        {
            // Implement assistant notification logic here
        }

        private async Task CreateRemotesNotificationAsync(NotificationObject notification)
        {
            // Implement remotes notification logic here
        }

      


        #endregion
    }
}
