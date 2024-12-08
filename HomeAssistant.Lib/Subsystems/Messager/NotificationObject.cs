namespace Messager
{
    public class NotificationObject
    {
        public NotificationObject(string subject, string body, NotificationTypes type)
        {
            Subject = subject;
            Body = body;
            Type = type;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Subject { get; set; }
        public string Body { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;
		public NotificationTypes Type { get; set; }
    }
}
