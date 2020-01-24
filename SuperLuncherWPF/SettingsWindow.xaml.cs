using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperLauncherWPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public string TemporaryName { get; private set; }
        public string TemporaryAppIconPath { get; private set; }
        public string TemporaryAppPath { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();

            var app = (App)Application.Current;

            AppName_Field.Text = app.Launcher.CurrentApplicationData.AppName;

            var iconUri = new Uri(app.Launcher.CurrentApplicationData.AppIconPath, UriKind.Absolute);
            AppIcon.Source = new BitmapImage(iconUri);
            AppIconPath_Field.Text = app.Launcher.CurrentApplicationData.AppIconPath;

            AppPath_Field.Text = app.Launcher.CurrentApplicationData.AppExecutablePath;
        }

        private void Top_Title(object sender, RoutedEventArgs e)
        {
            var label = sender as Label;
            label.Content = Title;
        }

        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TemporaryName = AppName_Field.Text;
            TemporaryAppIconPath = AppIconPath_Field.Text;
            TemporaryAppPath = AppPath_Field.Text;

            DialogResult = true;
            Close();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;

            AppName_Field.Text = app.Launcher.CurrentApplicationData.AppName;

            var iconUri = new Uri(app.Launcher.CurrentApplicationData.AppIconPath, UriKind.Absolute);
            AppIcon.Source = new BitmapImage(iconUri);
            AppIconPath_Field.Text = app.Launcher.CurrentApplicationData.AppIconPath;

            AppPath_Field.Text = app.Launcher.CurrentApplicationData.AppExecutablePath;
        }

        private void AppPath_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = $"Select new path for {app.Launcher.CurrentApplicationData.AppName}",
                Filter = "Application (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                AppPath_Field.Text = openFileDialog.FileName;
            }
        }

        private void AppIconPath_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = $"Select new icon for {app.Launcher.CurrentApplicationData.AppName}",
                Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var iconUri = new Uri(openFileDialog.FileName, UriKind.Absolute);
                AppIcon.Source = new BitmapImage(iconUri);

                AppIconPath_Field.Text = openFileDialog.FileName;
            }
        }
    }
}
