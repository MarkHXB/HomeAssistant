namespace Messager
{
    public class NotificationObject
    {
        public NotificationObject(string id, string title, string content, NotificationTypes type)
        {
            Id = id;
            Title = title;
            Content = content;
            Type = type;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationTypes Type { get; set; }
        public bool Notified { get; set; }
    }
}
