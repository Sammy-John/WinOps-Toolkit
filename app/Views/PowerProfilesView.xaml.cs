using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WinOpsToolkit.Helpers;

namespace WinOpsToolkit.Views
{
    public partial class PowerProfilesView : UserControl
    {
        private readonly string ScriptBase = @"D:\Scripts\PowerPlans\";

        public PowerProfilesView()
        {
            InitializeComponent();
            UpdateActiveProfileText();
        }

        private void RunCreateProfiles_Click(object sender, RoutedEventArgs e)
        {
            RunScript("Create-PowerPlans.ps1");
        }

        private void RunSetTimeouts_Click(object sender, RoutedEventArgs e)
        {
            RunScript("Set-PowerPlan-Times.ps1");
        }

        private void RunSwitchToDay_Click(object sender, RoutedEventArgs e)
        {
            RunScript("Switch-To-Day.ps1");
        }

        private void RunSwitchToNight_Click(object sender, RoutedEventArgs e)
        {
            RunScript("Switch-To-Night.ps1");
        }

        private void RunAutoSwitch_Click(object sender, RoutedEventArgs e)
        {
            RunScript("AutoSwitch-ByTime.ps1");
        }

        private void RunScript(string scriptName)
        {
            string path = Path.Combine(ScriptBase, scriptName);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Script not found: {path}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string output = PowerShellHelper.RunScript(path);
            MessageBox.Show(output, $"Ran: {scriptName}", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateActiveProfileText();
        }

        private void UpdateActiveProfileText()
        {
            string? profileName = PowerProfileHelper.GetActiveProfileName();
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                ActiveProfileText.Text = $"🟢 {profileName}";
            }
            else
            {
                ActiveProfileText.Text = "⚠ Unable to determine active profile";
            }
        }
    }
}
