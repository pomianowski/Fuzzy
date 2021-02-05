using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;

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

    //checkbox ciagi operacji, usuwaj wejscia, zostawiaj wyjscia
    //przykryc nowym gridem

    public enum CalcType
    {
        Unknown = 0,
        Singleton,
        Gamma,
        L,
        T,
        CSingleton,
        CT
    }

    [Serializable]
    public struct FuzzyValue
    {
        public double? x { get; set; }
        public double? value { get; set; }
    }

    [Serializable]
    public class FuzzySet
    {
        private int _extenderMax = 6;
        private int _extenderMin = 6;

        public CalcType Type = CalcType.Unknown;
        public List<FuzzyValue> absolutePoints = new List<FuzzyValue> { };

        public FuzzyValue a = new FuzzyValue { value = null, x = null };
        public FuzzyValue b = new FuzzyValue { value = null, x = null };
        public FuzzyValue c = new FuzzyValue { value = null, x = null };

        public void SetExtender(int min = 6, int max = 6)
        {
            //Extender min / max are the current symbolic display boundary points that exist to facilitate visualization
            this._extenderMin = min;
            this._extenderMax = max;
        }

        public ChartValues<ObservablePoint> GetPlot()
        {
            ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint> { };

            if(this.absolutePoints != null && this.absolutePoints.Count > 0)
            {
                for (int i = 0; i < this.absolutePoints.Count; i++)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("absolutePoint");
                    System.Diagnostics.Debug.WriteLine(this.absolutePoints[i].x);
                    System.Diagnostics.Debug.WriteLine(this.absolutePoints[i].value);
#endif
                    if(i == 0)
                        points.Add(new ObservablePoint((double)this.absolutePoints[i].value - this._extenderMin, (double)this.absolutePoints[i].x));
                    
                    points.Add(new ObservablePoint((double)this.absolutePoints[i].value, (double)this.absolutePoints[i].x));

                    if (i == this.absolutePoints.Count - 1)
                        points.Add(new ObservablePoint((double)this.absolutePoints[i].value + this._extenderMax, (double)this.absolutePoints[i].x));
                }
                return points;
            }

            switch (this.Type)
            {
                case CalcType.Singleton:
                    points.Add(new ObservablePoint((double)a.x, 0));
                    points.Add(new ObservablePoint((double)a.x, 1));
                    break;
                case CalcType.Gamma:
                case CalcType.L:
                    if (this.Type == CalcType.L) //symbolic line for L
                        points.Add(new ObservablePoint((double)a.value - this._extenderMin, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    if(this.Type == CalcType.Gamma) //symbolic line for Gamma
                        points.Add(new ObservablePoint((double)b.value + this._extenderMax, (double)b.x));
                    break;
                case CalcType.T:
                    points.Add(new ObservablePoint((double)a.value - this._extenderMin, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    points.Add(new ObservablePoint((double)c.value, (double)c.x));
                    points.Add(new ObservablePoint((double)c.value + this._extenderMax, (double)c.x));
                    break;
                default:
                    break;
            }
            //

            return points;
        }

        public FuzzySet Product(FuzzySet sSet)
        {
            return FuzzySolver.Product(this, sSet);
        }

        public FuzzySet Sum(FuzzySet sSet)
        {
            return FuzzySolver.Summary(this, sSet);
        }

        public FuzzySet Complement()
        {
            return FuzzySolver.Complement(this);
        }
    }
}
