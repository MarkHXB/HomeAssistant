using SubSystemComponent;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Messager
{
	/// <summary>
	/// pushbullet_api_key: <ExampleKeyGoesHere>
	/// gmail_password_key: <ExampleKeyGoesHere>
	/// </summary>
	public class MessagerSystem : Subsystem
	{
		private ConcurrentBag<NotificationObject> notifications = new ConcurrentBag<NotificationObject>();

		// params
		private string pushBulletApiKey;
		private string gmailPasswordKey;
		private string historyFilePath;

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

			// Validate history file
			if (string.IsNullOrWhiteSpace(ConfigObject.NotificationsFilePath))
			{
				throw new ArgumentNullException("History file path is missing.");
			}

			// Get configs
			historyFilePath = ConfigObject.NotificationsFilePath;

			// Get params
			pushBulletApiKey = GetParam<string>(nameof(Messager.Params.PushBulletApiKey), string.Empty);
			gmailPasswordKey = GetParam<string>(nameof(Messager.Params.GmailPasswordKey), string.Empty);

			// Validate params
			if (string.IsNullOrWhiteSpace(pushBulletApiKey) || string.IsNullOrWhiteSpace(gmailPasswordKey))
			{
				throw new ArgumentNullException("Missing required parameters.");
			}
		}

		public override Task TaskObject(CancellationToken cancellationToken)
		{
			return Task.Run(async () =>
			{
				foreach (var notification in notifications)
				{
					switch (notification.Type)
					{
						case NotificationTypes.PushBullet:
							await SendNotification(notification);
							break;
						case NotificationTypes.Gmail:
							SendMail(notification);
							break;
						default:
							throw new Exception("Unknown notification type.");
					}

					// Add to history
					await AddToHistoryAsync(notification);

					// Remove from list
					notifications.TryTake(out _);
				}

				await RemoveOutdatedNotificationsAsync();
			});
		}

		public void CreateNotification(NotificationObject notification)
		{
			notifications.Add(notification);
		}

		private async Task AddToHistoryAsync(NotificationObject notification)
		{
			if(File.Exists(historyFilePath))
			{
				var history = await File.ReadAllTextAsync(historyFilePath);
				var historyList = System.Text.Json.JsonSerializer.Deserialize<List<NotificationObject>>(history) ?? new List<NotificationObject>();
				historyList.Add(notification);
				var json = System.Text.Json.JsonSerializer.Serialize(historyList);
				await File.WriteAllTextAsync(historyFilePath, json);
			}
			else
			{
				var json = System.Text.Json.JsonSerializer.Serialize(new List<NotificationObject> { notification });
				await File.WriteAllTextAsync(historyFilePath, json);
			}
		}

		private async Task RemoveOutdatedNotificationsAsync()
		{
			if (File.Exists(historyFilePath))
			{
				var history = await File.ReadAllTextAsync(historyFilePath);
				var historyList = System.Text.Json.JsonSerializer.Deserialize<List<NotificationObject>>(history) ?? new List<NotificationObject>();
				historyList = historyList.Where(x => x.Date > DateTime.Now.AddDays(-7)).ToList();
				var json = System.Text.Json.JsonSerializer.Serialize(historyList);
				await File.WriteAllTextAsync(historyFilePath, json);
			}
		}

		private async Task SendNotification(NotificationObject notification)
		{
			string apiKey = pushBulletApiKey;

			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Access-Token", apiKey);
				var data = new
				{
					type = "note",
					title = notification.Subject,
					body = notification.Body
				};

				string jsonData = System.Text.Json.JsonSerializer.Serialize(data);
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

				var response = await client.PostAsync("https://api.pushbullet.com/v2/pushes", content);
				Console.WriteLine(await response.Content.ReadAsStringAsync());
			}
		}

		private void SendMail(NotificationObject notification)
		{
			try
			{
				// Sender email address
				string senderEmail = "bkmrk74@gmail.com";
				string senderPassword = gmailPasswordKey;

				// Configure SMTP client
				SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
				{
					Credentials = new NetworkCredential(senderEmail, senderPassword),
					EnableSsl = true
				};

				// Email content
				MailMessage mail = new MailMessage
				{
					From = new MailAddress(senderEmail),
					Subject = notification.Subject,
					Body = notification.Body,
					IsBodyHtml = false
				};

				// Recipient
				mail.To.Add("bakonyimark8@gmail.com");

				// Send the email
				smtpClient.Send(mail);
				Console.WriteLine("Email sent successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send email. Error: {ex.Message}");
			}
		}
	}
}