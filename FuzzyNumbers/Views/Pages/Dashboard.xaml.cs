using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Helpers;
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
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,
                    Values = new ChartValues<double> { double.NaN, double.NaN, double.NaN, double.NaN, 0, 1, 1, 1, 1, 1, 1, 1, 0 }
                },
                new LineSeries
                {
                    Title = "Fuzzy number #2",
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    PointGeometry = null,
                    DataLabels = false,
                    Values = new ChartValues<double> { double.NaN, double.NaN, double.NaN, double.NaN,  double.NaN, 0, 1, 1, 1, 1, 1 , 0 },
                },
                new LineSeries
                {
                    Title = "Fuzzy number #3",
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    PointGeometry = null,
                    DataLabels = false,
                    Values = new ChartValues<double> { double.NaN, double.NaN, double.NaN, double.NaN,  double.NaN, 0, 1, 1, 1, 1, 0, double.NaN },
                },
                new LineSeries
                {
                    Title = "Fuzzy number #4",
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,
                    Values = new ChartValues<double> { double.NaN, 0, 1, double.NaN,  double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN },
                },
                new LineSeries
                {
                    Title = "Fuzzy number #5",
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    PointGeometry = null,
                    DataLabels = false,
                    Values = new ChartValues<double> { double.NaN, 1, 0, 0,  1, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN },
                }
            };

            cartesianChart.AxisY.Add(new Axis
            {
                MinValue = 0,
                MaxValue = 1,
                Separator = new LiveCharts.Wpf.Separator
                {
                    StrokeThickness = 1,
                    Step = 1
                }

            });

            DataContext = this;
        }

        private void TextBoxInputFuzzy_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string tag = (sender as TextBox).Tag.ToString().ToLower().Trim();
            string value = (sender as TextBox).Text;
#if DEBUG
            System.Diagnostics.Debug.WriteLine("Key up (" + tag + "), value is: " + value);
#endif
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

        private void Button_AddNew(object sender, RoutedEventArgs e)
        {

        }

        private void ComboClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;


            if (textInputOne == null || textInputTwo == null || textInputThree == null)
                return;


            switch (index)
            {
                case 0:
                    textInputOne.IsEnabled = true;
                    textInputTwo.IsEnabled = textInputThree.IsEnabled = false;
                        break;

                case 1:
                    textInputOne.IsEnabled = textInputTwo.IsEnabled = true;
                    textInputThree.IsEnabled = false;
                    break;

                case 2:
                    textInputOne.IsEnabled = textInputTwo.IsEnabled = true;
                    textInputThree.IsEnabled = false;
                    break;

                case 3:
                    textInputOne.IsEnabled = textInputTwo.IsEnabled = textInputThree.IsEnabled = true;
                    break;
            }
        }
    }
}
