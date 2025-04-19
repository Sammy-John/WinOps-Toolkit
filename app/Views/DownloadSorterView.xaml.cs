using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Diagnostics;
using WinOpsToolkit.Helpers;

namespace WinOpsToolkit.Views
{
    public partial class DownloadSorterView : UserControl
    {
        public DownloadSorterView()
        {
            InitializeComponent();
        }

        private void RunNow_Click(object sender, RoutedEventArgs e)
        {
            string scriptPath = @"D:\Scripts\DownloadSorter\Sort-Downloads.ps1";
            string output = PowerShellHelper.RunScript(scriptPath);
            MessageBox.Show(output, "Script Output", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            string logFolder = @"D:\Scripts\DownloadSorter\Logs";

            if (Directory.Exists(logFolder))
            {
                Process.Start("explorer.exe", logFolder);
            }
            else
            {
                MessageBox.Show("Log folder not found.", "Logs", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ViewConfig_Click(object sender, RoutedEventArgs e)
        {
            string configPath = @"D:\Scripts\DownloadSorter\config.json";

            if (File.Exists(configPath))
            {
                Process.Start("notepad.exe", configPath);
            }
            else
            {
                MessageBox.Show("Config file not found.", "View Config", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
