using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FuzzyNumbers.Assets.Controls
{
    /// <summary>
    /// Interaction logic for WindowNavigation.xaml
    /// </summary>
    public partial class WindowNavigation : UserControl
    {
        public WindowNavigation()
        {
            InitializeComponent();
        }

        private void AppBarButton(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag.ToString())
            {
                case "minimize":
                    App.Current.MainWindow.WindowState = WindowState.Minimized;
                    break;
                case "maximize":
                    this.Maximize();
                    break;
                case "close":
                    App.Current.Shutdown();
                    break;
            }
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                App.Current.MainWindow.DragMove();
        }

        private void DragMaximize(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                this.Maximize();
        }

        private void Maximize()
        {
            if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
                MaximizeButton.Style = (Style)App.Current.Resources["RestoreButton"];
                App.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                App.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
                MaximizeButton.Style = (Style)App.Current.Resources["MaximizeButton"];
                App.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }
    }
}
