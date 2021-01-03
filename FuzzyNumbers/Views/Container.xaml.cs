using System.Windows;
using System.Windows.Navigation;

namespace FuzzyNumbers.Views
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : Window
    {
        public enum Pages
        {
            Dashboard = 0
        }
        public Container()
        {
            InitializeComponent();
            this.Navigate(Pages.Dashboard);
        }

        public void Navigate(Pages page)
        {
            switch (page)
            {
                default:
                    rootFrame.Navigate(new Views.Pages.Dashboard());
                    break;
            }
        }

        private void FrameOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Content == null)
                return;
            rootFrame.NavigationService.RemoveBackEntry();
        }
    }
}
