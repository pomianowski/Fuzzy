using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace FuzzyNumbers.Views.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public List<FuzzySet> _fuzzySets;
        public SeriesCollection SeriesCollection { get; set; }
        public Dashboard()
        {
            InitializeComponent();

            this._fuzzySets = new List<FuzzySet> { };
            this.InitializeChart();
        }

        private void InitializeChart()
        {
            SeriesCollection = new SeriesCollection
            {
                /*new LineSeries
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
                    Title = "Fuzzy number test",
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    PointGeometry = null,
                    DataLabels = false,
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(-2, 0),
                        new ObservablePoint(-1, 1),
                        new ObservablePoint(12, 1),
                        new ObservablePoint(13, 0),
                    }
                },
                new LineSeries
                {
                    Title = "Singleton",
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    PointGeometry = null,
                    DataLabels = false,
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(1, 0),
                        new ObservablePoint(1, 1)
                    }
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
                }*/
            };

            DataContext = this;
        }

        private void ShowPopup(string header, string message = null)
        {
            popupMain.Header = header;
            popupMain.Message = message;
            popupMain.Show();
        }

        private FuzzyValue ParseFuzzyValue(string text)
        {
            text = text.Trim();

            if(string.IsNullOrEmpty(text))
                return new FuzzyValue { value = null, x = null };

            text = Regex.Replace(text, "[^0-9,.-]", "");
            string[] points = text.Split(',');

#if DEBUG
            foreach (string point in points)
                System.Diagnostics.Debug.WriteLine("Single detected point for string (" + text + "), is: " + point);
#endif

            double point_x = 0;
            double point_value = 0;

            if (points.Length > 2 || !double.TryParse(points[0], out point_x))
                return new FuzzyValue { value = null, x = null };

            if (points.Length > 1)
                double.TryParse(points[1], out point_value);

            return new FuzzyValue { x = point_x, value = point_value };
        }

        private void ParseCurrentSet()
        {
            int index = _fuzzySets.Count;
            string title = "Unknown";

            CalcType selectedType = CalcType.Unknown;

            switch (comboClass.SelectedIndex)
            {
                case 0:
                    selectedType = CalcType.Singleton;
                    title = "Singleton";
                    break;
                case 1:
                    selectedType = CalcType.Gamma;
                    title = "Gamma";
                    break;
                case 2:
                    selectedType = CalcType.L;
                    title = "L";
                    break;
                case 3:
                    selectedType = CalcType.T;
                    title = "T";
                    break;
            }

            if (selectedType == CalcType.Singleton && string.IsNullOrEmpty(textInputOne.Text))
            {
                this.ShowPopup("It won't work!", "You must enter an (x) value for Singleton.");
                return;
            }

            FuzzyValue value_a = this.ParseFuzzyValue(textInputOne.Text);
            FuzzyValue value_b = this.ParseFuzzyValue(textInputTwo.Text);
            FuzzyValue value_c = this.ParseFuzzyValue(textInputThree.Text);


            this._fuzzySets.Add(new FuzzySet {
                Type = selectedType,
                a = value_a,
                b = value_b,
                c = value_c
            });

            this.SeriesCollection.Add(new LineSeries {
                Title = title + " #" + _fuzzySets.Count,
                StrokeThickness = 1,
                LineSmoothness = 0,
                Fill = System.Windows.Media.Brushes.Transparent,
                PointGeometry = null,
                DataLabels = false,
                Values = this._fuzzySets[this._fuzzySets.Count - 1].GetPlot()
            });
            textInputOne.Text = textInputTwo.Text = textInputThree.Text = string.Empty;

#if DEBUG
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + ", type is = " + this._fuzzySets[index].Type.ToString());
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + " value a is x = " + this._fuzzySets[index].a.x.ToString());
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + " value a is value = " + this._fuzzySets[index].a.value.ToString());
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + " value b is x = " + this._fuzzySets[index].b.x.ToString());
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + " value b is value = " + this._fuzzySets[index].b.value.ToString());
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + " value c is x = " + this._fuzzySets[index].c.x.ToString());
            System.Diagnostics.Debug.WriteLine("FuzzySet #" + index + " value c is value = " + this._fuzzySets[index].c.value.ToString());
#endif
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
            this.ParseCurrentSet();
        }

        private void ComboClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;


            if (textInputOne == null || textInputTwo == null || textInputThree == null)
                return;


            switch (index)
            {
                case 0:
                    textInputOne.Tag = "(x)";
                    textInputOne.IsEnabled = true;
                    textInputTwo.IsEnabled = textInputThree.IsEnabled = false;
                    break;

                case 1:
                    textInputOne.Tag = "(x, a)";
                    textInputOne.IsEnabled = textInputTwo.IsEnabled = true;
                    textInputThree.IsEnabled = false;
                    break;

                case 2:
                    textInputOne.Tag = "(x, a)";
                    textInputOne.IsEnabled = textInputTwo.IsEnabled = true;
                    textInputThree.IsEnabled = false;
                    break;

                case 3:
                    textInputOne.Tag = "(x, a)";
                    textInputOne.IsEnabled = textInputTwo.IsEnabled = textInputThree.IsEnabled = true;
                    break;
            }
        }
    }
}
