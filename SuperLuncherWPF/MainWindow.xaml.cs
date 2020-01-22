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
            WebBrowser.Navigate("http://google.com");

            var app = Application.Current as App;
            applicationsList.ItemsSource = app.Launcher.SuperLauncherAppDatas;

            bool noApp = app.Launcher.SuperLauncherAppDatas.Count == 0;
            CoverPanel.Visibility = noApp ? Visibility.Visible : Visibility.Hidden;
            WebBrowser.Visibility = noApp ? Visibility.Hidden : Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

            var image = FindLogicalChildren<Image>(sender as DependencyObject)?.FirstOrDefault();
            if (image == null)
                return;

            image.Source = WindowState == WindowState.Normal
                ? new BitmapImage(new Uri("/SuperLuncherWPF;component/Media/Images/Fullscreen.png", UriKind.Relative))
                : new BitmapImage(new Uri("/SuperLuncherWPF;component/Media/Images/Normal.png", UriKind.Relative));
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Top_Title(object sender, RoutedEventArgs e)
        {
            var label = sender as Label;
            label.Content = Application.Current.MainWindow.Title;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public IEnumerable<T> FindLogicalChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var queue = new Queue<DependencyObject>(new[] { parent });

            while (queue.Any())
            {
                var reference = queue.Dequeue();
                var children = LogicalTreeHelper.GetChildren(reference);
                var objects = children.OfType<DependencyObject>();

                foreach (var o in objects)
                {
                    if (o is T child)
                        yield return child;

                    queue.Enqueue(o);
                }
            }
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select new application for launcher",
                Filter = "Application (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(openFileDialog.FileName);
                var bitmap = icon.ToBitmap();

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "Icon"
                };

                if(saveFileDialog.ShowDialog() == true)
                {
                    bitmap.Save(saveFileDialog.FileName);
                }

                Process process = new Process();
                process.StartInfo.FileName = openFileDialog.FileName;
                process.Start();
                RuntimeMonitor rtm = new RuntimeMonitor(5000);
                process.WaitForExit();
                rtm.Close();
            }
        }

        private void AddNewAplication(object sender, RoutedEventArgs e)
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
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;

            var app = Application.Current as App;
            app.Launcher.SelectApplication((Guid)fe.Tag);
        }
    }

    public class RuntimeMonitor
    {
        System.Timers.Timer timer;

        // Define other methods and classes here
        public RuntimeMonitor(double intervalMs) // in milliseconds
        {
            timer = new System.Timers.Timer();
            timer.Interval = intervalMs;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_func);
            timer.AutoReset = true;
            timer.Start();
        }

        void timer_func(object source, object e)
        {
            Console.WriteLine("Yes");
        }

        public void Close()
        {
            timer.Stop();
        }
    }
}
