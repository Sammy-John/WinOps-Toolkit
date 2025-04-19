using System.Windows;
using System.Windows.Controls;
using WinOpsToolkit.Views;

namespace WinOpsToolkit
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DownloadSorter_Click(object sender, RoutedEventArgs e)
        {
            ModuleContent.Content = new DownloadSorterView();
        }

        private void PowerProfiles_Click(object sender, RoutedEventArgs e)
        {
            ModuleContent.Content = new PowerProfilesView();
        }
    }
}
