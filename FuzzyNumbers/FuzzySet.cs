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
                    points.Add(new ObservablePoint((int)a.x, 0));
                    points.Add(new ObservablePoint((int)a.x, 1));
                    break;
                case CalcType.Gamma:
                    break;
                case CalcType.L:
                    break;
                case CalcType.T:
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
