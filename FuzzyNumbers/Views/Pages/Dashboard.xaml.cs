using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace FuzzyNumbers.Views.Pages
{
    public struct FuzzySymbolic
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public double? a { get; set; }
        public double? ax { get; set; }
        public double? b { get; set; }
        public double? bx { get; set; }
        public double? c { get; set; }
        public double? cx { get; set; }

        public List<FuzzyValue> absolutePoints { get; set; }
    }
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

            this.InitializeChart();
        }

        private void InitializeChart()
        {
            this._fuzzySets = new List<FuzzySet> { };
            SeriesCollection = new SeriesCollection{};
            DataContext = this;
        }

        private void DrawSet(string title, IChartValues values)
        {
            this.SeriesCollection.Add(new LineSeries
            {
                Title = title,
                StrokeThickness = 1,
                LineSmoothness = 0,
                Fill = System.Windows.Media.Brushes.Transparent,
                PointGeometry = null,
                DataLabels = false,
                Values = values
            });
        }

        private void ShowPopup(string header, string message = null, int time = 2000)
        {
            popupMain.Header = header;
            popupMain.Message = message;
            popupMain.Timeout = time;
            popupMain.Show();
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
            
            if(!this.VerifyForm(selectedType))
                return;

            FuzzyValue value_a = this.ParseFuzzyValue(textInputOne.Text);
            FuzzyValue value_b = this.ParseFuzzyValue(textInputTwo.Text);
            FuzzyValue value_c = this.ParseFuzzyValue(textInputThree.Text);

            if(!this.VerifyCorrectness(selectedType, value_a, value_b, value_c))
                return;

            this.ShowPopup("Addition", "A new fuzzy set is just being added.");
            this._fuzzySets.Add(new FuzzySet {
                Type = selectedType,
                a = value_a,
                b = value_b,
                c = value_c
            });
            this.DrawSet(title + " #" + _fuzzySets.Count, this._fuzzySets[this._fuzzySets.Count - 1].GetPlot());

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

        private FuzzyValue ParseFuzzyValue(string text)
        {
            text = text.Trim();

            if (string.IsNullOrEmpty(text))
                return new FuzzyValue { value = null, x = null };

            text = Regex.Replace(text, "[^0-9,.-]", "");
            string[] points = text.Split(',');

#if DEBUG
            foreach (string point in points)
                System.Diagnostics.Debug.WriteLine("Single detected point for string (" + text + "), is: " + point);
#endif

            double point_x = 0;
            double point_value = 0;

            if (points.Length > 2 || !double.TryParse(points[0].Replace('.', ','), out point_x))
                return new FuzzyValue { value = null, x = null };

            if (points.Length > 1)
                double.TryParse(points[1].Replace('.', ','), out point_value);

            return new FuzzyValue { x = point_x, value = point_value };
        }
        private bool VerifyForm(CalcType selectedType)
        {
            if (selectedType == CalcType.Singleton && string.IsNullOrEmpty(textInputOne.Text))
            {
                this.ShowPopup("It won't work!", "You must enter an (x) value for Singleton.");
                return false;
            }
            else if (selectedType == CalcType.Gamma && string.IsNullOrEmpty(textInputOne.Text) && string.IsNullOrEmpty(textInputTwo.Text))
            {
                this.ShowPopup("It won't work!", "You must enter an (x,a) and (x,b) value for Gamma.");
                return false;
            }
            else if (selectedType == CalcType.L && string.IsNullOrEmpty(textInputOne.Text) && string.IsNullOrEmpty(textInputTwo.Text))
            {
                this.ShowPopup("It won't work!", "You must enter an (x,a) and (x,b) value for L.");
                return false;
            }
            else if (selectedType == CalcType.T && string.IsNullOrEmpty(textInputOne.Text) && string.IsNullOrEmpty(textInputTwo.Text) && string.IsNullOrEmpty(textInputThree.Text))
            {
                this.ShowPopup("It won't work!", "You must enter an (x,a), (x,b) and (x,c) value for t.");
                return false;
            }

            return true;
        }

        private bool VerifyCorrectness(CalcType selectedType, FuzzyValue a, FuzzyValue b, FuzzyValue c)
        {
            //Value must exist
            if (selectedType == CalcType.Singleton && a.x == null)
            {
                this.ShowPopup("It won't work!", "The specified (x) value is not valid.");
                return false;
            }
            else if ((selectedType == CalcType.Gamma || selectedType == CalcType.T || selectedType == CalcType.L) && (a.x == null || a.value == null))
            {
                this.ShowPopup("It won't work!", "The specified (x,a) value is not valid.");
                return false;
            }
            else if ((selectedType == CalcType.Gamma || selectedType == CalcType.T || selectedType == CalcType.L) && (b.x == null || b.value == null))
            {
                this.ShowPopup("It won't work!", "The specified (x,b) value is not valid.");
                return false;
            }
            else if (selectedType == CalcType.T && (c.x == null || c.value == null))
            {
                this.ShowPopup("It won't work!", "The specified (x,c) value is not valid.");
                return false;
            }

            switch (selectedType)
            {
                case CalcType.Gamma:
                    //a < b
                    //x1 < x2
                    if (a.value > b.value)
                    {
                        this.ShowPopup("It won't work!", "The value of 'a' cannot be greater than the value of 'b'");
                        return false;
                    }
                    else if (a.x > b.x)
                    {
                        this.ShowPopup("It won't work!", "The value of x in (x,a) cannot be greater than the value of x in (x,b)");
                        return false;
                    }
                    break;
                case CalcType.L:
                    // a > b
                    // x1 > x2
                    if (a.value > b.value)
                    {
                        this.ShowPopup("It won't work!", "The value of 'a' cannot be greater than the value of 'b'");
                        return false;
                    }
                    else if (a.x < b.x)
                    {
                        this.ShowPopup("It won't work!", "The value of x in (x,a) must be greater than the value of x in (x,b)");
                        return false;
                    }
                    break;
                case CalcType.T:
                    //a < b < c
                    //x1 < x2 > x2
                    if (a.value > b.value)
                    {
                        this.ShowPopup("It won't work!", "The value of 'a' cannot be greater than the value of 'b'");
                        return false;
                    }
                    else if(b.value > c.value)
                    {
                        this.ShowPopup("It won't work!", "The value of 'b' cannot be greater than the value of 'c'");
                    }
                    else if (a.x > b.x)
                    {
                        this.ShowPopup("It won't work!", "The value of x in (x,a) cannot be greater than the value of x in (x,b)");
                        return false;
                    }
                    else if (b.x < c.x)
                    {
                        this.ShowPopup("It won't work!", "The value of x in (x,b) cannot be greater than the value of x in (x,c)");
                        return false;
                    }
                    break;
            }
            
            //X must be between
            if ((selectedType == CalcType.Gamma || selectedType == CalcType.L || selectedType == CalcType.T) && (a.x < 0 || a.x > 1) )
            {
                this.ShowPopup("It won't work!", "The value of x in (x,a) must be between 0-1");
                return false;
            }
            else if ((selectedType == CalcType.Gamma || selectedType == CalcType.L || selectedType == CalcType.T) && (b.x < 0 || b.x > 1))
            {
                this.ShowPopup("It won't work!", "The value of x in (x,b) must be between 0-1");
                return false;
            }
            else if (selectedType == CalcType.T && (c.x < 0 || c.x > 1))
            {
                this.ShowPopup("It won't work!", "The value of x in (x,c) must be between 0-1");
                return false;
            }

            return true;
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
            bool isCalcMode = (bool)checkboxCalculator.IsChecked;

            if (this._fuzzySets.Count < 1)
            {
                this.ShowPopup("It won't work!", "To perform the operation, it is required to add at least one fuzzy set.");
                return;
            }
            else if (this._fuzzySets.Count > 128)
            {
                this.ShowPopup("Tranquilo amigo!", "The program cannot operate with such a large number of sets.");
                return;
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Calc tag is: " + tag);
#endif
            switch (tag)
            {
                case "complement":
                    this.ShowPopup("Calculating", "Computing complements of fuzzy sets.");

                    FuzzySet complement;
                    List<FuzzySet> tempSets = new List<FuzzySet> { };

                    for (int i = 0; i < this._fuzzySets.Count; i++)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("Complemented FuzzySet #" + i + ", Type: " + this._fuzzySets[i].Type.ToString());
#endif

                        complement = this._fuzzySets[i].Complement();
                        this.DrawSet("Complement #" + i, complement.GetPlot());

                        if (isCalcMode)
                        {
                            //Delete the currently complemented fuzzy set in favor of the newly created one
                            this._fuzzySets.RemoveAt(i);
                            this.SeriesCollection.RemoveAt(i);
                        }

                        tempSets.Add(complement);
                    }

                    foreach (FuzzySet set in tempSets)
                        this._fuzzySets.Add(set);

                    break;
                    case "sum":
                        FuzzySet summary = this._fuzzySets[0];

                        for (int i = 1; i < this._fuzzySets.Count; i++)
                        {
                            summary = summary.Sum(this._fuzzySets[i]);
                        }

                        this.ShowPopup("Processing...", "The sum of the fuzzy sets is being calculated.", 1000);

                        this.DrawSet("Summary #" + (this._fuzzySets.Count + 1), summary.GetPlot());
                        this._fuzzySets.Add(summary);
                    break;
                        default:
                            break;
            }
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            if (this._fuzzySets.Count < 1)
                return;

            this.ShowPopup("Processing...", "All sets of fuzzy numbers are being removed...", 1000);
            this._fuzzySets = new List<FuzzySet> { };
            this.SeriesCollection.Clear();
        }

        private void Button_Import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Title = "Fuzzy Sets",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                CheckPathExists = true,
                Multiselect = false,
                Filter = "Fuzzy set|*.fset|All files (*.*)|*.*"
            };

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    List<FuzzySet> importedSets = Encoder.Read<List<FuzzySet>>(fileDialog.FileName);
                    this.ShowPopup("Processing...", "All sets of fuzzy numbers are being imported...", 1000);

                    foreach (FuzzySet single in importedSets)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("Imported set");
                        System.Diagnostics.Debug.WriteLine(single.a.x);
                        System.Diagnostics.Debug.WriteLine(single.a.value);
                        System.Diagnostics.Debug.WriteLine(single.b.x);
                        System.Diagnostics.Debug.WriteLine(single.b.value);
                        System.Diagnostics.Debug.WriteLine(single.c.x);
                        System.Diagnostics.Debug.WriteLine(single.c.value);
#endif
                        this._fuzzySets.Add(single);
                        this.DrawSet("[I] " + single.Type.ToString() + " #" + (this._fuzzySets.Count + 1), single.GetPlot());
                    }
                }
                catch
                {
                    this.ShowPopup("It won't work!", "Failed to correctly import fuzzy sets from the file.", 5000);
                }    
            }
        }
        private void Button_Export(object sender, RoutedEventArgs e)
        {
            if (this._fuzzySets.Count < 1)
            {
                this.ShowPopup("It won't work!", "You must add at least one fuzzy set to perform the export operation", 5000);
                return;
            }
            else
            {
                SaveFileDialog fileDialog = new SaveFileDialog()
                {
                    Title = "Fuzzy Sets",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    Filter = "Fuzzy set|*.fset|All files (*.*)|*.*"
                };

                if (fileDialog.ShowDialog() == true)
                {
                    try
                    {
                        this.ShowPopup("Processing...", "All sets of fuzzy numbers are being exported to file:\n" + fileDialog.FileName, 5000);
                        Encoder.Write<List<FuzzySet>>(fileDialog.FileName, this._fuzzySets);
                    }
                    catch
                    {
                        this.ShowPopup("It won't work!", "Failed to correctly export fuzzy sets from the file.", 5000);
                    }
                }
            }
        }

        private void Button_Results(object sender, RoutedEventArgs e)
        {
            if (this._fuzzySets.Count < 1)
            {
                this.ShowPopup("It won't work!", "You must add at least one fuzzy set to perform the export operation", 5000);
                return;
            }
            else
            {
                SaveFileDialog fileDialog = new SaveFileDialog()
                {
                    Title = "Fuzzy Sets",
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    Filter = "Fuzzy set results |*.txt|All files (*.*)|*.*"
                };

                if (fileDialog.ShowDialog() == true)
                {
                    try
                    {
                        StringBuilder r = new StringBuilder();
                        r.AppendLine("-----------------------");
                        r.AppendLine("Fuzzy Sets Results File");
                        r.AppendLine("v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                        r.AppendLine("-----------------------");
                        r.AppendLine("");

                        for (int i = 0; i < this._fuzzySets.Count; i++)
                        {
                            r.AppendLine("");
                            r.AppendLine("Fuzzy Number #" + ( i + 1 ));
                            r.AppendLine("---------------");
                            r.AppendLine("Class: " + this._fuzzySets[i].Type.ToString());
                            r.Append("a = ");
                            r.AppendLine(this._fuzzySets[i].a.value != null ? this._fuzzySets[i].a.value.ToString() : "not set");
                            r.Append("(x;a) = ");
                            r.AppendLine(this._fuzzySets[i].a.x != null ? this._fuzzySets[i].a.x.ToString() : "not set");
                            r.Append("b = ");
                            r.AppendLine(this._fuzzySets[i].b.value != null ? this._fuzzySets[i].b.value.ToString() : "not set");
                            r.Append("(x;b) = ");
                            r.AppendLine(this._fuzzySets[i].b.x != null ? this._fuzzySets[i].b.x.ToString() : "not set");
                            r.Append("c = ");
                            r.AppendLine(this._fuzzySets[i].c.value != null ? this._fuzzySets[i].c.value.ToString() : "not set");
                            r.Append("(x;c) = ");
                            r.AppendLine(this._fuzzySets[i].c.x != null ? this._fuzzySets[i].c.x.ToString() : "not set");

                            if(this._fuzzySets[i].absolutePoints.Count > 0)
                            {
                                r.AppendLine("\tPoints:");
                                for (int j = 0; j < this._fuzzySets[i].absolutePoints.Count; j++)
                                {
                                    r.AppendLine("\tPoint (#" + ( j + 1 ) + ")");
                                    r.AppendLine("\ta' = " + this._fuzzySets[i].absolutePoints[j].value);
                                    r.AppendLine("\t(x;a') = " + this._fuzzySets[i].absolutePoints[j].x);
                                }
                            }
                        }

                        this.ShowPopup("Processing...", "All sets of fuzzy numbers are being exported to file:\n" + fileDialog.FileName, 5000);
                        System.IO.File.WriteAllText(fileDialog.FileName, r.ToString());
                    }
                    catch
                    {
                        this.ShowPopup("It won't work!", "Failed to correctly write results to file.", 5000);
                    }
                }
            }
        }

        private void Button_AddNew(object sender, RoutedEventArgs e)
        {
            this.ParseCurrentSet();
        }

        private void Button_View(object sender, RoutedEventArgs e)
        {
            if (gridResults.Visibility == Visibility.Visible)
                gridResults.Visibility = Visibility.Hidden;
            else
            {
                List<FuzzySymbolic> symList = new List<FuzzySymbolic> { };

                for (int i = 0; i < this._fuzzySets.Count; i++)
                    symList.Add(new FuzzySymbolic
                    {
                        Name = "#" + ( i + 1 ) + " " + this._fuzzySets[i].Type.ToString(),
                        Type = (int)this._fuzzySets[i].Type,
                        a = this._fuzzySets[i].a.value,
                        ax = this._fuzzySets[i].a.x,
                        b = this._fuzzySets[i].b.value,
                        bx = this._fuzzySets[i].b.x,
                        c = this._fuzzySets[i].c.value,
                        cx = this._fuzzySets[i].c.x,
                        absolutePoints = this._fuzzySets[i].absolutePoints
                    });

                icontrolSymbolic.ItemsSource = symList;
                gridResults.Visibility = Visibility.Visible;
            }
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
