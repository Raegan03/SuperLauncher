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

            RunningApp.Visibility = Visibility.Hidden;

            bool noApp = app.Launcher.IsApplicationsListEmpty;
            CoverPanel.Visibility = noApp ? Visibility.Visible : Visibility.Hidden;
            WebBrowser.Visibility = noApp ? Visibility.Hidden : Visibility.Visible;

            app.Launcher.SessionsData.CollectionChanged += SessionsData_CollectionChanged;
            sessionsList.ItemsSource = app.Launcher.SessionsData;
            UpdateInfoView();

            app.Launcher.ViewUpdateReqested += Launcher_ViewUpdateReqested;
            app.Launcher.ApplicationStarted += Launcher_ApplicationStarted;
            app.Launcher.ApplicationClosed += Launcher_ApplicationClosed;
        }

        private void Launcher_ApplicationClosed()
        {
            RunningApp.Visibility = Visibility.Hidden;
            WebBrowser.Visibility = Visibility.Visible;
        }

        private void Launcher_ApplicationStarted()
        {
            var app = Application.Current as App;
            RunningApp.Visibility = Visibility.Visible;
            WebBrowser.Visibility = Visibility.Hidden;

            RuningAppLabel.Content = $"{app.Launcher.CurrentApplicationData.AppName} is running...";
            RuningAppButton.Content = $"Stop {app.Launcher.CurrentApplicationData.AppName}";
        }

        private void Launcher_ViewUpdateReqested()
        {
            UpdateInfoView();
        }

        private void SessionsData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
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

        private void UpdateInfoView()
        {
            var app = Application.Current as App;
            bool noApp = app.Launcher.IsApplicationsListEmpty;
            bool noSession = app.Launcher.IsSessionsListEmpty;

            CoverPanel.Visibility = noApp ? Visibility.Visible : Visibility.Hidden;
            WebBrowser.Visibility = noApp ? Visibility.Hidden : Visibility.Visible;
            CoverSessionPanel.Visibility = noSession ? Visibility.Visible : Visibility.Hidden;

            if (string.IsNullOrEmpty(app.Launcher.CurrentApplicationData.AppName))
                return;

            CurrentAppTitle.Content = app.Launcher.CurrentApplicationData.AppName;

            Uri uri = new Uri(app.Launcher.CurrentApplicationData.AppIconPath, UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            CurrentAppIcon.Source = imgSource;

            WebBrowser.Navigate($"https://www.google.com/search?q={app.Launcher.CurrentApplicationData.AppName}");
            UpdateAchievements();
        }

        private void CurrentAppSettings_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            var settingsWindow = new SettingsWindow();
            if(settingsWindow.ShowDialog() == true)
            {
                app.Launcher.UpdateCurrentApplication(settingsWindow.TemporaryName,
                    settingsWindow.TemporaryAppPath, settingsWindow.TemporaryAppIconPath);

                UpdateInfoView();
            }
        }

        private void CurrentAppPlay_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Launcher.StartCurrentApplication();
        }

        private void UpdateAchievements()
        {
            var app = Application.Current as App;

            bool ach1 = app.Launcher.CurrentApplicationAchievements[1];
            var Ach_1_Uri = new Uri(ach1 
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_1.png" 
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_1_Img.Source = new BitmapImage(Ach_1_Uri);
            Ach_1_Label.Content = ach1 ? "First Start" : "???";

            bool ach2 = app.Launcher.CurrentApplicationAchievements[2];
            var Ach_2_Uri = new Uri(ach2
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_2.png"
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_2_Img.Source = new BitmapImage(Ach_2_Uri);
            Ach_2_Label.Content = ach2 ? "Getting familiar" : "???";

            bool ach3 = app.Launcher.CurrentApplicationAchievements[3];
            var Ach_3_Uri = new Uri(ach3
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_3.png"
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_3_Img.Source = new BitmapImage(Ach_3_Uri);
            Ach_3_Label.Content = ach3 ? "Here we go again" : "???";

            bool ach4 = app.Launcher.CurrentApplicationAchievements[4];
            var Ach_4_Uri = new Uri(ach4
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_4.png"
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_4_Img.Source = new BitmapImage(Ach_4_Uri);
            Ach_4_Label.Content = ach4 ? "Old pal" : "???";
        }

        private void RuningAppButton_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Launcher.StopCurrentApplication();
        }
    }
}
