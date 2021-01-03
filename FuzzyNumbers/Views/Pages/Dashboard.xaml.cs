using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace FuzzyNumbers.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Dashboard()
        {
            InitializeComponent();
            this.InitializeChart();
        }

        private void InitializeChart()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Fuzzy number #1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Fuzzy number #2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                },
                new LineSeries
                {
                    Title = "Result",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                }
            };

            DataContext = this;
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
