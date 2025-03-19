using Blazored.Modal;
using HomeAssistant.Blazor.Services;
using Microsoft.Extensions.Logging;

namespace HomeAssistant.Blazor
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif
			string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db");

			builder.Services.AddSingleton(new LocationDatabase(dbPath));
			builder.Services.AddSingleton<HttpClient>();
			builder.Services.AddSingleton<FirestoreService>();
			builder.Services.AddSingleton<ShoppingListService>();

			builder.Services.AddBlazoredModal();

			return builder.Build();
		}
	}
}
