using Moq;
using Moq.Protected;
using Runner;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

[TestClass]
public class RunnerSystemTests
{
    private string filePath = @"C:\path\to\test";
    private string fileName = "test.exe";
    private ILogger<RunnerSystem> loggerMock;

    [Setup]
    public void Setup()
    {
        loggerMock = new Mock<ILogger<RunnerSystem>>();
    }

    [TestMethod]
    public async Task ExecuteExeFileAsync_ExecutesProcessCorrectly()
    {
        // Arrange
        var system = new RunnerSystem(new Dictionary<string, string>(), new Subsystem[0])
        {
            FilePath = filePath,
            fileName = fileName
        };
        system.LogInformation = loggerMock.Object;

        using (var processMock = new Mock<Process>())
        {
            var startInfo = new ProcessStartInfo(fileName)
            {
                WorkdingDirectory = filePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            processMock.Setup(p => p.StartInfo).Returns(startInfo);
            processMock.Protected().Setup<int>("Start", ItExpr.IsAny<ProcessStartInfo>(), ItExpr.IsAny<WaitHandle>())
                    .Verifiable();

            system.OutputDataReceived = OutputDataReceived;
            system.ErrorDataReceived = ErrorDataReceived;

            // Act
            await system.ExecuteExeFileAsync(CancellationToken.None);

            // Assert
            processMock.Protected().Verify("Start", Times.Once);
        }
    }

    [TestMethod]
    public async Task ExecuteExeFileAsync_LogsOutput()
    {
        // Arrange
        var system = new RunnerSystem(new Dictionary<string, string>(), new Subsystem[0])
        {
            FilePath = filePath,
            fileName = fileName
        };
        system.LogInformation = loggerMock.Object;

        using (var processMock = new Mock<Process>())
        {
            var outputData = "Test output";
            processMock.Setup(p => p.BeginOutputReadLine()).Returns(() => OutputDataReceived).Verifiable();
            processMock.Setup(p => p.OutputDataReceived)
                    .AddCallback((sender, e) => e.Data = outputData);
            system.OutputDataReceived = OutputDataReceived;
            system.ErrorDataReceived = ErrorDataReceived;

            // Act
            await system.ExecuteExeFileAsync(CancellationToken.None);

            // Assert
            loggerMock.Verify(l => l.LogInformation(It.Is<Func<string, string>>(f => f("@{fileName} [ OUTPUT ]: {data}") == It.IsAny<Action<string>>())), Times.Once());
            loggerMock.Verify(l => l.LogInformation(It.Is<Func<string, string>>(f => f("@{fileName} [ OUTPUT ]: {data}") == It.Is(x => x("Test output")))), Times.Once());
        }
    }

    // Repeat the tests for other methods (ExecuteDotNetFileAsync and ExecutePythonScriptAsync)

    private void OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        e.Data = null;
    }

    private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        e.Data = null;
    }
}