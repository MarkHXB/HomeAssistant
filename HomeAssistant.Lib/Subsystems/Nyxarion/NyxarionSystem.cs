using SubSystemComponent;

namespace Nyxarion
{
	/// <summary>
	/// file_logger_folder_path_list<string> : 
	/// </summary>
	public class NyxarionSystem : Subsystem
	{
		// Fields
		private string _extensionsLogFolderPath;

		// Extensions
		private IExtension fileLoggerExtension;
		private IExtension foreGroundExtension;

		// params

		public NyxarionSystem(params Subsystem[] dependencies) :
	  base(ConfigObject.LogFilePath, dependencies)
		{
		}

		public override void Initialize()
		{
			ConfigHandler configHandler = new ConfigHandler(ConfigObject.ConfigFilePath);
			var config = configHandler.LoadConfig<ConfigObject>();

			// Null checks
			_ = config ?? throw new ArgumentNullException(nameof(config));

			// Get configs
			_extensionsLogFolderPath = ConfigObject.ExtensionsLogFolderPath;

			// Validate configs
			if (!Directory.Exists(_extensionsLogFolderPath))
			{
				Directory.CreateDirectory(_extensionsLogFolderPath);
			}

			// Init FileLogger
			IEnumerable<string> folderPaths = GetParameter<IEnumerable<string>>(Params.FileLoggerFolderPathList);
			if (folderPaths == null)
			{
				throw new ArgumentNullException("file_logger_folder_path_list");
			}

			fileLoggerExtension = new FileLogger(folderPaths, _extensionsLogFolderPath);

			// Init ForeGround
			foreGroundExtension = new ForeGroundWindowLogger("foreground_logger.json", _extensionsLogFolderPath);
		}

		public override async Task TaskObject(CancellationToken cancellationToken)
		{
			var tasks = new List<Task>
			{
				Task.Run(async () =>
				{
					await fileLoggerExtension.ExecuteAsync(cancellationToken);
				}, cancellationToken),
				Task.Run(async () =>
				{
					await foreGroundExtension.ExecuteAsync(cancellationToken);
				}, cancellationToken)
			};

			await Task.WhenAll(tasks);
		}
	}
}