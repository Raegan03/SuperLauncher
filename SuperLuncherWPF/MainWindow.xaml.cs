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

            app.Launcher.SessionsData.CollectionChanged += SessionsData_CollectionChanged;
            sessionsList.ItemsSource = app.Launcher.SessionsData;
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
            //TODO
        }

        private void CurrentAppPlay_Click(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Launcher.StartCurrentApplication();
        }

        private void UpdateAchievements()
        {
            bool ach1 = true;
            var Ach_1_Uri = new Uri(ach1 
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_1.png" 
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);

            var src = new BitmapImage(Ach_1_Uri);
            Ach_1_Img.Source = new BitmapImage(Ach_1_Uri);
            Ach_1_Label.Content = ach1 ? "First Start" : "???";

            bool ach2 = false;
            var Ach_2_Uri = new Uri(ach2
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_2.png"
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_2_Img.Source = new BitmapImage(Ach_2_Uri);
            Ach_2_Label.Content = ach2 ? "Getting familiar" : "???";

            bool ach3 = true;
            var Ach_3_Uri = new Uri(ach3
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_3.png"
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_3_Img.Source = new BitmapImage(Ach_3_Uri);
            Ach_3_Label.Content = ach3 ? "Here we go again" : "???";

            bool ach4 = false;
            var Ach_4_Uri = new Uri(ach4
                ? "/SuperLauncherWPF;component/Media/Images/Achievement_4.png"
                : "/SuperLauncherWPF;component/Media/Images/Achievement_Lock.png", UriKind.Relative);
            Ach_4_Img.Source = new BitmapImage(Ach_4_Uri);
            Ach_4_Label.Content = ach4 ? "Old pal" : "???";
        }
    }
}
