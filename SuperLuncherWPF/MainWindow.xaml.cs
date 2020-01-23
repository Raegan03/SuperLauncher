using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SuperLauncher.Data;

namespace SuperLauncherWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var app = Application.Current as App;
            applicationsList.ItemsSource = app.Launcher.ApplicationsData;

            bool noApp = app.Launcher.IsApplicationsListEmpty;
            CoverPanel.Visibility = noApp ? Visibility.Visible : Visibility.Hidden;
            WebBrowser.Visibility = noApp ? Visibility.Hidden : Visibility.Visible;

            sessionsList.ItemsSource = app.Launcher.SessionsData;
            UpdateInfoView();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //private void Fullscreen_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        //    var image = FindLogicalChildren<Image>(sender as DependencyObject)?.FirstOrDefault();
        //    if (image == null)
        //        return;

        //    image.Source = WindowState == WindowState.Normal
        //        ? new BitmapImage(new Uri("/SuperLuncherWPF;component/Media/Images/Fullscreen.png", UriKind.Relative))
        //        : new BitmapImage(new Uri("/SuperLuncherWPF;component/Media/Images/Normal.png", UriKind.Relative));
        //}

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Top_Title(object sender, RoutedEventArgs e)
        {
            var label = sender as Label;
            label.Content = Application.Current.MainWindow.Title;
        }

        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void SelectApplication(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;

            var app = Application.Current as App;
            app.Launcher.SelectApplication((Guid)fe.Tag);

            UpdateInfoView();
        }

        private void AddNewApplication(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select new application for launcher",
                Filter = "Application (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var app = Application.Current as App;
                app.Launcher.AddNewApplication(openFileDialog.FileName);
            }

            UpdateInfoView();
        }

        private void SessionPlaceholder(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Launcher.AddNewSession(app.Launcher.CurrentApplicationData.AppGUID);
        }

        private void UpdateInfoView()
        {
            var app = Application.Current as App;
            bool noApp = app.Launcher.IsApplicationsListEmpty;
            CoverPanel.Visibility = noApp ? Visibility.Visible : Visibility.Hidden;
            WebBrowser.Visibility = noApp ? Visibility.Hidden : Visibility.Visible;

            if (string.IsNullOrEmpty(app.Launcher.CurrentApplicationData.AppName))
                return;

            CurrentAppTitle.Content = app.Launcher.CurrentApplicationData.AppName;

            Uri uri = new Uri(app.Launcher.CurrentApplicationData.AppIconPath, UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            CurrentAppIcon.Source = imgSource;

            WebBrowser.Navigate($"https://www.google.com/search?q={app.Launcher.CurrentApplicationData.AppName}");
        }

        private void CurrentAppSettings_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void CurrentAppPlay_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Launcher.StartCurrentApplication();
        }
    }
}
