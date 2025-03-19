
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace Nyxarion
{
	class Log
	{
		public string Process { get; set; }
	}

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

		public async Task ExecuteAsync(CancellationToken? cancellationToken = null)
		{
			if (!cancellationToken.HasValue)
			{
				throw new ArgumentNullException(nameof(cancellationToken));
			}

			while (!cancellationToken.Value.IsCancellationRequested)
			{
				string activeProcess = GetActiveProcess();
				var log = new Log
				{
					Process = activeProcess
				};
					
				if (!System.IO.File.Exists(Path.Combine(_logPath, _logFileName)))
				{
					System.IO.File.Create(Path.Combine(_logPath, _logFileName)).Close();
				}

				var data = System.IO.File.ReadAllText(Path.Combine(_logPath, _logFileName));
				List<Log> history = JsonConvert.DeserializeObject<List<Log>>(data) ?? new List<Log>();
				history.Add(log);
				string content = JsonConvert.SerializeObject(history);

				await System.IO.File.AppendAllTextAsync(Path.Combine(_logPath, _logFileName), content); 
				
				Thread.Sleep(1000);
			}	
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
