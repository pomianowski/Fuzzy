using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyNumbers
{
    /*
     * Uszczegółowienie zadania:
     * Zaimplementować realizację działań na zbiorach rozmytych (iloczyn, suma i dopełnienie). 
     * Umożliwić wprowadzanie ciągów operacji. Wybieranie dyskretyzacji dziedziny funkcji. Zapisywanie, odczytywanie obiektów rozmytych. Obliczanie wartości przynależności dla podanego argumentu.
     *
     * Dodatkowe cechy programu:
     * reprezentacja graficzna poszczególnych zbiorów rozmytych oraz wyników,
     * wprowadzanie zbiorów jako krzywych,
     * zapisywanie i wczytywanie z pliku.
    */

    public enum CalcType
    {
        Unknown = 0,
        Singleton,
        Gamma,
        L,
        T
    }

    public struct FuzzyValue
    {
        public double? x { get; set; }
        public double? value { get; set; }
    }

    public class FuzzySet
    {
        public CalcType Type = CalcType.Unknown;


        public FuzzyValue a = new FuzzyValue { value = null, x = null };
        public FuzzyValue b = new FuzzyValue { value = null, x = null };
        public FuzzyValue c = new FuzzyValue { value = null, x = null };

        public ChartValues<ObservablePoint> GetPlot()
        {
            ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint> { };

            switch (this.Type)
            {
                case CalcType.Singleton:
                    points.Add(new ObservablePoint((double)a.x, 0));
                    points.Add(new ObservablePoint((double)a.x, 1));
                    break;
                case CalcType.Gamma:
                    points.Add(new ObservablePoint((double)a.value - 5, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    points.Add(new ObservablePoint((double)b.value + 5, (double)b.x));
                    break;
                case CalcType.L:
                    points.Add(new ObservablePoint((double)a.value - 5, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    points.Add(new ObservablePoint((double)b.value + 5, (double)b.x));
                    break;
                case CalcType.T:
                    points.Add(new ObservablePoint((double)a.value - 5, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    points.Add(new ObservablePoint((double)c.value, (double)c.x));
                    points.Add(new ObservablePoint((double)c.value + 5, (double)c.x));
                    break;
                default:
                    break;
            }
            //

            return points;
        }

        public FuzzySet Product(FuzzySet set)
        {
            return set;
        }

        public FuzzySet Sum(FuzzySet set)
        {
            return set;
        }

        public FuzzySet Complement(FuzzySet set)
        {
            return set;
        }
    }
}
