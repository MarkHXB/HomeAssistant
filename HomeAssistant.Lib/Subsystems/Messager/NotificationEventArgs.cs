using Notification.Environment;

namespace Messager
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationObject Content { get; set; }

        private string title;
        private string description;

        public NotificationEventArgs(NotificationObject content)
        {
            Content = content;
        }

        public NotificationEventArgs(string title, string description)
        {
            this.title = title;
            this.description = description;

            Content = new NotificationObject(title, description, NotificationTypes.Toast);
        }

        public NotificationEventArgs(string title)
        {
            this.title = title;
            description = string.Empty;

            Content = new NotificationObject(title, description, NotificationTypes.Toast);
        }
    }
}
