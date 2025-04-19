using System;
using System.Diagnostics;
using System.Linq;

namespace WinOpsToolkit.Helpers
{
    public static class PowerProfileHelper
    {
        // Optional: Still useful for displaying the current active plan
        public static string? GetActiveProfileName()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powercfg",
                    Arguments = "/getactivescheme",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var line = output.Split("GUID:").Skip(1).FirstOrDefault();
            if (line != null)
            {
                var parts = line.Trim().Split("  ");
                if (parts.Length >= 2)
                {
                    return parts[1].Trim('(', ')');
                }
                else
                {
                    return line.Trim();
                }
            }

            return null;
        }
    }
}
