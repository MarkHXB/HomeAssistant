using HomeAssistant.Lib.Utils;
using SubSystemComponent;
using System.Diagnostics;

namespace Runner
{
    /// <summary>
    /// Following parameters should pass:
    /// <para>runner_system_file_path => only the folder path without the file</para>
    /// <para>runner_system_file_name => the filename you want to run</para>
    /// <para>runner_system_arguements => arguements which are passed to the run file</para>
    /// <para>runner_system_dependent_subsystem_output_path => different subsystem's output file path to read as input to process</para>
    /// </summary>
    public class RunnerSystem : Subsystem
    {
        private string? filePath;
        private string? fileName;
        private string? args;
        private string? dependentSubsystemOutputPath;
        private string? convertOutputToJson;

        public RunnerSystem(Dictionary<string, string> @params, params Subsystem[] dependencies) :
            base(ConfigObject.LogFilePath, @params, dependencies)
        {

        }

        public override void Initialize()
        {
            Params.TryGetValue("runner_system_file_path", out filePath);
            Params.TryGetValue("runner_system_file_name", out fileName);
            Params.TryGetValue("runner_system_arguements", out args);
            Params.TryGetValue("runner_system_dependent_subsystem_output_path", out dependentSubsystemOutputPath);
            Params.TryGetValue("runner_system_dependent_convert_output_path_to_json", out convertOutputToJson);
        }

        public override async Task TaskObject(CancellationToken cancellationToken)
        {
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".exe":
                    await ExecuteExeFileAsync(cancellationToken);
                    break;
                case ".dll":
                    await ExecuteDotNetFileAsync(cancellationToken);
                    break;
                case ".py":
                    await ExecutePythonScriptAsync(cancellationToken);
                    break;
                default:
                    throw new ArgumentException($"Unsupported file extension '{Path.GetExtension(filePath)}'.");
            }
        }

        private async Task ExecuteExeFileAsync(CancellationToken cancellationToken)
        {
            if (CurrentPlatform == SupportedPlatforms.ANDROID)
                throw new NotSupportedException("Executing .EXE files is not supported on Android.");

            string path = Path.Combine(filePath, fileName);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            string cmd = string.Empty;

            if (!string.IsNullOrWhiteSpace(dependentSubsystemOutputPath))
            {
                string jsonPath = RunnerSystemUtils.GetInputTextReflectsToSubsystemOutput(dependentSubsystemOutputPath, convertOutputToJson);
                if (!string.IsNullOrWhiteSpace(jsonPath))
                {
                    cmd = $"{path} {args} < {jsonPath}";
                }
            }
            else
            {
                cmd = $"{path} {args}";
            }

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c \"{cmd}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                try
                {
                    process.Start();

                    OutputDataReceived(process);
                    ErrorDataReceived(process);

                    await process.WaitForExitAsync(cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    process.Kill();
                    throw;
                }
                catch (Exception ex) { }

            }
        }

        private async Task ExecuteDotNetFileAsync(CancellationToken cancellationToken)
        {
            if (CurrentPlatform == SupportedPlatforms.ANDROID)
                throw new NotSupportedException("Executing .NET files is not supported on Android.");

            string path = Path.Combine(filePath, fileName);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = path;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true; // Add redirection to standard error stream
                process.StartInfo.CreateNoWindow = true;
                try
                {
                    process.Start();

                    OutputDataReceived(process);
                    ErrorDataReceived(process);

                    await process.WaitForExitAsync(cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    process.Kill();
                    throw;
                }
            }
        }

        private async Task ExecutePythonScriptAsync(CancellationToken cancellationToken)
        {
            string path = Path.Combine(filePath, fileName);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "python";
                process.StartInfo.Arguments = path + " " + args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true; // Add redirection to standard error stream
                process.StartInfo.CreateNoWindow = true;
                try
                {
                    process.Start();

                    OutputDataReceived(process);
                    ErrorDataReceived(process);

                    await process.WaitForExitAsync(cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    process.Kill();
                    throw;
                }
            }
        }

        private void OutputDataReceived(Process process)
        {
            process.BeginOutputReadLine();

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null && LogInformation != null)
                {
                    LogInformation($"@{fileName} [ OUTPUT ]: {args.Data}");
                }
            };
        }

        private void ErrorDataReceived(Process process)
        {
            process.BeginErrorReadLine();

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null && LogWarning != null)
                {
                    LogWarning($"@{fileName} [ ERROR ]: {args.Data}");
                }
            };
        }
    }
}

