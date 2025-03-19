using Newtonsoft.Json;

namespace Nyxarion
{
	class File
	{
		public string Folder { get; set; }
		public string Name { get; set; }
		public WatcherChangeTypes ChangeTypes { get; set; }
		public DateTime ChangeDate { get; set; }
	}

	public class FileLogger(IEnumerable<string> folderPathList, string logFolderPath) : IExtension
	{
		public IEnumerable<string> FolderPathList { get; } = folderPathList;
		public string LogFolderPath { get; } = Path.Combine(logFolderPath, "file_logger_changes.json");

		private static readonly object _lock = new object();

		public async Task ExecuteAsync(CancellationToken? cancellationToken = null)
		{
			if (!cancellationToken.HasValue)
			{
				throw new ArgumentNullException(nameof(cancellationToken));
			}

			var tasks = FolderPathList.Select(folderPath => Task.Run(() =>
			{
				using var watcher = new FileSystemWatcher(folderPath);

				watcher.NotifyFilter = NotifyFilters.Attributes
									 | NotifyFilters.CreationTime
									 | NotifyFilters.DirectoryName
									 | NotifyFilters.FileName
									 | NotifyFilters.LastAccess
									 | NotifyFilters.LastWrite
									 | NotifyFilters.Security
									 | NotifyFilters.Size;

				watcher.Changed += OnChanged;
				watcher.Created += OnCreated;
				watcher.Deleted += OnDeleted;
				watcher.Renamed += OnRenamed;

				watcher.Filter = "*.pdf";
				watcher.IncludeSubdirectories = true;
				watcher.EnableRaisingEvents = true;

				while (!cancellationToken.Value.IsCancellationRequested)
				{
					Thread.Sleep(1000);
				}
			}, cancellationToken.Value));

			await Task.WhenAll(tasks);
		}

		private async void OnChanged(object sender, FileSystemEventArgs e) => await LogChange(e.FullPath, WatcherChangeTypes.Changed);

		private async void OnCreated(object sender, FileSystemEventArgs e) => await LogChange(e.FullPath, WatcherChangeTypes.Created);

		private async void OnDeleted(object sender, FileSystemEventArgs e) => await LogChange(e.FullPath, WatcherChangeTypes.Deleted);

		private async void OnRenamed(object sender, RenamedEventArgs e) => await LogChange(e.FullPath, WatcherChangeTypes.Renamed);

		private async Task LogChange(string filePath, WatcherChangeTypes changeType)
		{
			File file = new File
			{
				Folder = Path.GetDirectoryName(filePath),
				Name = GetLogFileName(filePath),
				ChangeTypes = changeType,
				ChangeDate = DateTime.Now
			};

			await AppendChangeToFile(file);
		}

		private string GetLogFileName(string filePath)
		{
			return filePath.Split('\\').Last();
		}

		private async Task AppendChangeToFile(File change)
		{
			List<File> changes = new List<File>();

			lock (_lock)
			{
				if (!System.IO.Directory.Exists(logFolderPath))
				{
					Directory.CreateDirectory(logFolderPath);
				}

				if (System.IO.File.Exists(LogFolderPath))
				{
					var data = System.IO.File.ReadAllText(LogFolderPath);
					changes = JsonConvert.DeserializeObject<List<File>>(data) ?? new List<File>();
				}

				changes.Add(change);

				 System.IO.File.WriteAllText(LogFolderPath, JsonConvert.SerializeObject(changes));
			}			
		}
	}
}
