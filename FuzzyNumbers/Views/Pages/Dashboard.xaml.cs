using System.Windows;
using System.Windows.Controls;

namespace FuzzyNumbers.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Button_Calc(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString().ToLower().Trim();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Calc tag is: " + tag);
#endif
            switch (tag)
            {
                default:
                    break;
            }
        }
    }
}
