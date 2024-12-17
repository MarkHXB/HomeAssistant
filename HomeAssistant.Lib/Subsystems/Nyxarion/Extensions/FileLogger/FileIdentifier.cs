using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Nyxarion
{
	public class FileIdentifier
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

		[StructLayout(LayoutKind.Sequential)]
		private struct FILETIME
		{
			public uint dwLowDateTime;
			public uint dwHighDateTime;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct BY_HANDLE_FILE_INFORMATION
		{
			public uint FileAttributes;
			public FILETIME CreationTime;
			public FILETIME LastAccessTime;
			public FILETIME LastWriteTime;
			public uint VolumeSerialNumber;
			public uint FileSizeHigh;
			public uint FileSizeLow;
			public uint NumberOfLinks;
			public uint FileIndexHigh;
			public uint FileIndexLow;
		}

		public static (ulong VolumeSerialNumber, ulong FileIndex) GetUniqueFileId(string filePath)
		{
			using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				BY_HANDLE_FILE_INFORMATION fileInfo;
				if (GetFileInformationByHandle(fileStream.SafeFileHandle.DangerousGetHandle(), out fileInfo))
				{
					ulong fileIndex = ((ulong)fileInfo.FileIndexHigh << 32) | fileInfo.FileIndexLow;
					ulong volumeSerialNumber = fileInfo.VolumeSerialNumber;
					return (volumeSerialNumber, fileIndex);
				}
				else
				{
					throw new IOException($"Unable to get file information for {filePath}. Error: {Marshal.GetLastWin32Error()}");
				}
			} // Simplified using statement
		}

		public static ConcurrentDictionary<string, (ulong VolumeSerialNumber, ulong FileIndex)> GetUniqueFileIds(IEnumerable<string> filePaths)
		{
			var results = new ConcurrentDictionary<string, (ulong VolumeSerialNumber, ulong FileIndex)>();

			Parallel.ForEach(filePaths, filePath =>
			{
				try
				{
					var uniqueId = GetUniqueFileId(filePath);
					results.TryAdd(filePath, uniqueId);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to process {filePath}: {ex.Message}");
				}
			});

			return results;
		}
	}
}
