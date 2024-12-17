using Nyxarion;

namespace Nyxarion
{
	enum FileChangeType
	{
		Created,
		Modified,
		Renamed,
		Deleted
	}

	class File
	{
		public string Path { get; set; }
		public string Name { get; set; }
		public string Extension { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
		public bool IsDeleted { get; set; }
		public long Size { get; set; }
		public FileChangeType ChangeType { get; set; }
	}

	/// <summary>
	/// Logs file changes to a JSON file.
	/// <para>folderPaths means the paths you want to watch</para>
	/// <para>logPath means the folder path you want to use as a working directory to this extension</para>
	/// <para>logFileName means the file which will contain the changes were made from previous state to current, so its a compare</para>
	/// </summary>
	public class FileLogger : IExtension
	{
		private readonly string _logFileName;
		private readonly string _logPath;
		private readonly IEnumerable<string> _folderPaths;

		private readonly List<File> _history = new List<File>();

		public FileLogger(IEnumerable<string> folderPaths, string logPath, string logFileName)
		{
			if (string.IsNullOrWhiteSpace(logPath))
			{
				throw new ArgumentException("Log path cannot be null or empty.");
			}
			if (string.IsNullOrWhiteSpace(logFileName) || logFileName.EndsWith(".json"))
			{
				throw new ArgumentException("Log file name cannot be null or empty or it shoudnt end with json.");
			}

			_folderPaths = folderPaths;
			_logPath = logPath;
			_logFileName = logFileName;
		}

		public async Task ExecuteAsync()
		{
		}

		private async Task LogAsync(string folderName)
		{
			IList<File> history = GetScannedDirectories(folderName);

			var json = System.Text.Json.JsonSerializer.Serialize(history);
			await System.IO.File.WriteAllTextAsync(GetLogPath(folderName), json);
		}

		private IList<File> GetScannedDirectories(string folderName)
		{
			List<File> history = new List<File>();

			foreach (var folderPath in _folderPaths.Where(fp => fp == folderName))
			{
				var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
				history.AddRange(files.Select(filePath =>
				{
					var fileInfo = new FileInfo(filePath);
					return new File
					{
						Path = filePath,
						Name = Path.GetFileName(filePath),
						Extension = Path.GetExtension(filePath),
						CreatedAt = fileInfo.CreationTime,
						ModifiedAt = fileInfo.LastWriteTime,
						Size = fileInfo.Length
					};
				}));

			}

			return history;
		}

		private async Task LogChanges(string folderName)
		{
			if (!System.IO.File.Exists(GetLogPath(folderName)))
			{
				return;
			}

			var json = await System.IO.File.ReadAllTextAsync(GetLogPath(folderName));
			var history = System.Text.Json.JsonSerializer.Deserialize<List<File>>(json);

			if (history == null)
			{
				return;
			}

			IList<File> scannedDirectories = GetScannedDirectories(folderName);

			// Compare the two lists
			foreach (var file in scannedDirectories)
			{
				var existingFile = history.FirstOrDefault(f => f.Path == file.Path);
				if (existingFile == null)
				{
					// New file
					file.ChangeType = FileChangeType.Created;
					_history.Add(file);
				}
				else if (existingFile.Name != file.Name)
				{
					// File has been renamed
					file.ChangeType = FileChangeType.Renamed;
					_history.Add(file);
				}
				else
				{
					// Check if the file has been modified
					if (existingFile.ModifiedAt != file.ModifiedAt)
					{
						// File has been modified
						file.ChangeType = FileChangeType.Modified;
						_history.Add(file);
					}
				}
			}

			// Check for deleted files
			foreach (var file in history)
			{
				if (!scannedDirectories.Any(f => f.Path == file.Path))
				{
					// File has been deleted
					file.ChangeType = FileChangeType.Deleted;
					file.IsDeleted = true;
					_history.Add(file);
				}
			}

			json = System.Text.Json.JsonSerializer.Serialize(_history);

			if (!System.IO.File.Exists(Path.Combine(_logPath, _logFileName) + ".json"))
			{
				await System.IO.File.WriteAllTextAsync(Path.Combine(_logPath, _logFileName) + ".json", json);
				return;
			}

			string currentChangesInLog = await System.IO.File.ReadAllTextAsync(Path.Combine(_logPath, _logFileName) + ".json");

			List<File> changes = System.Text.Json.JsonSerializer.Deserialize<List<File>>(currentChangesInLog);
			changes.AddRange(_history);
			json = System.Text.Json.JsonSerializer.Serialize(changes);
			await System.IO.File.WriteAllTextAsync(Path.Combine(_logPath, _logFileName) + ".json", json);
		}

		private string GetLogPath(string folderName)
		{
			return Path.Combine(_logPath, $"file_logger_history_{GetExactFolderName(folderName)}.json");
		}

		private string GetExactFolderName(string folderPath)
		{
			_ = folderPath ?? throw new ArgumentNullException(nameof(folderPath));

			return folderPath.Split('\\').Last();
		}
	}
}
