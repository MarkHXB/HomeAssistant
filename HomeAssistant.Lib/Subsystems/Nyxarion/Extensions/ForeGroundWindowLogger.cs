
using System.Runtime.InteropServices;
using System.Text;

namespace Nyxarion
{
	public class ForeGroundWindowLogger : IExtension
	{
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		private readonly string _logFileName;
		private readonly string _logPath;

		public ForeGroundWindowLogger(string logFileName, string logPath)
		{
			if(string.IsNullOrWhiteSpace(logPath))
			{
				throw new ArgumentException("Log path cannot be null or empty.");
			}
			if(string.IsNullOrWhiteSpace(logFileName))
			{
				throw new ArgumentException("Log file name cannot be null or empty.");
			}

			_logFileName = logFileName;
			_logPath = logPath;
		}

		public async Task ExecuteAsync()
		{
			string activeProcess = GetActiveProcess();

			var json = System.Text.Json.JsonSerializer.Serialize(activeProcess);
			await System.IO.File.WriteAllTextAsync(Path.Combine(_logPath, _logFileName), json);
		}

		private string GetActiveProcess()
		{
			IntPtr hWnd = GetForegroundWindow();
			StringBuilder windowText = new StringBuilder(256);
			if (GetWindowText(hWnd, windowText, 256) > 0)
			{
				return windowText.ToString();
			}

			return string.Empty;
		}
	}
}
