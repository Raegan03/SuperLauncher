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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            Button button = sender as Button;
            Image image = VisualTreeHelper.GetChild(button, 0) as Image;
            if(WindowState == WindowState.Normal)
            {
                image.Source = new BitmapImage(new Uri(@"/Resources/Images/Fullscreen.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri(@"/Resources/Images/Normal.png", UriKind.Relative));
            }
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
    }
}
