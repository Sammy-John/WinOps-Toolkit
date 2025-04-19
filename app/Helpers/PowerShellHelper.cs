using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WinOpsToolkit.Helpers
{
    public static class PowerShellHelper
    {
        public static string RunScript(string scriptPath)
        {
            if (!File.Exists(scriptPath))
                return $"Script not found: {scriptPath}";

            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\PowerShell\7\pwsh.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                var process = Process.Start(psi);

                if (process == null)
                {
                    string failMsg = "Process failed to start (returned null).";
                    LogToFile($"[{DateTime.Now}]\n{failMsg}\n---\n");
                    return failMsg;
                }

                using (process)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    string fullLog = $"[{DateTime.Now}]\nOutput:\n{output}\n\nError:\n{error}\n---\n";
                    LogToFile(fullLog);

                    return string.IsNullOrWhiteSpace(error) ? output : $"Error:\n{error}";
                }
            }
            catch (Exception ex)
            {
                string exceptionLog = $"[{DateTime.Now}]\nException: {ex}\n---\n";
                LogToFile(exceptionLog);
                return $"Exception:\n{ex.Message}";
            }

        }

        private static void LogToFile(string content)
        {
            try
            {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                Directory.CreateDirectory(logDir);

                string logPath = Path.Combine(logDir, "script_log.txt");
                File.AppendAllText(logPath, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LogToFile failed: {ex.Message}");
                MessageBox.Show($"Failed to write log: {ex.Message}", "Logging Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void SetTaskEnabled(string taskName, bool enable)
        {
            string command = enable
                ? $"Enable-ScheduledTask -TaskName \"{taskName}\""
                : $"Disable-ScheduledTask -TaskName \"{taskName}\"";

            RunScript(command);
        }

        public static (bool isEnabled, string triggerText) GetTaskScheduleStatus(string taskName)
        {
            string script = $@"
        $task = Get-ScheduledTask -TaskName '{taskName}' -ErrorAction SilentlyContinue
        if ($null -eq $task) {{
            return 'NOTFOUND'
        }}
        $enabled = ($task.State -eq 'Ready' -or $task.State -eq 'Running')
        $triggers = $task.Triggers | ForEach-Object {{
            $_.StartBoundary
        }} | Out-String

        @($enabled, $triggers)
        ";

            string output = RunScript(script);

            if (output.Contains("NOTFOUND"))
                return (false, "Task not found");

            var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Fallback if parsing fails
            if (lines.Length == 0)
                return (false, "Unknown");

            bool isEnabled = lines[0].Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            string trigger = string.Join('\n', lines.Skip(1));

            return (isEnabled, trigger);
        }



    }
}
