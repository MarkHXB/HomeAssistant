namespace HomeAssistant.Blazor.Components.Models
{
	public class CardModel
	{
		public CardModel(string imageUrl, string title, int maxHeight, int minWidth, int notificationCount)
		{
			ImageUrl = imageUrl;
			Title = title;
			MaxHeight = maxHeight;
			MinWidth = minWidth;
			NotificationCount = notificationCount;
		}

		public CardModel() : this("", "", 0, 0, 0) { }

		public string ImageUrl { get; set; } = "";
		public string Title { get; set; } = "";
		public int MaxHeight { get; set; } = 0;
		public int MinWidth { get; set; } = 0;
		public int NotificationCount { get; set; } = 0;
	}
}
