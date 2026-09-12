namespace MCSRemoteAccess.Bot.Services;

using System.Diagnostics;
using System.Runtime.InteropServices;

public class PythonRunnerService
{
    public async Task<string> RunScriptAsync(string scriptPath, string arguments)
    {
        string pythonExecutable = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "python" : "python3";

        var startInfo = new ProcessStartInfo
        {
            FileName = pythonExecutable,
            Arguments = $"\"{scriptPath}\" {arguments}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();

        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> errorTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(outputTask, errorTask);
        await process.WaitForExitAsync();

        string result = await outputTask;
        string error = await errorTask;

        return !string.IsNullOrWhiteSpace(error) ? $"Python Error: {error.Trim()}" : result.Trim();
    }
}