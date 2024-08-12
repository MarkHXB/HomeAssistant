namespace Notification.Environment
{
    public class NotificationObject
    {
        public NotificationObject(string title, string content, NotificationTypes type)
        {
            Title = title;
            Content = content;
            Type = type;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationTypes Type { get; set; }
        public bool Notified { get; set; }
    }
}
