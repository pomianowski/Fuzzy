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

    [Serializable]
    public struct FuzzyValue
    {
        public double? x { get; set; }
        public double? value { get; set; }
    }

    [Serializable]
    public class FuzzySet
    {
        private static int _extender = 6;

        public CalcType Type = CalcType.Unknown;

        public List<FuzzyValue> absolutePoints = new List<FuzzyValue> { };

        public FuzzyValue a = new FuzzyValue { value = null, x = null };
        public FuzzyValue b = new FuzzyValue { value = null, x = null };
        public FuzzyValue c = new FuzzyValue { value = null, x = null };

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
                        points.Add(new ObservablePoint((double)this.absolutePoints[i].value - _extender, (double)this.absolutePoints[i].x));
                    
                    points.Add(new ObservablePoint((double)this.absolutePoints[i].value, (double)this.absolutePoints[i].x));

                    if (i == this.absolutePoints.Count - 1)
                        points.Add(new ObservablePoint((double)this.absolutePoints[i].value + _extender, (double)this.absolutePoints[i].x));
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
                    points.Add(new ObservablePoint((double)a.value - _extender, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    points.Add(new ObservablePoint((double)b.value + _extender, (double)b.x));
                    break;
                case CalcType.T:
                    points.Add(new ObservablePoint((double)a.value - _extender, (double)a.x));
                    points.Add(new ObservablePoint((double)a.value, (double)a.x));
                    points.Add(new ObservablePoint((double)b.value, (double)b.x));
                    points.Add(new ObservablePoint((double)c.value, (double)c.x));
                    points.Add(new ObservablePoint((double)c.value + _extender, (double)c.x));
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
            if(this.Type == CalcType.Gamma && set.Type == CalcType.L)
                return SumLGamma(set, this);
            else if(this.Type == CalcType.L && set.Type == CalcType.Gamma)
                return SumLGamma(this, set);

            return new FuzzySet{ };
        }

        private FuzzySet SumLGamma(FuzzySet setOne, FuzzySet setTwo)
        {
            FuzzySet returnedSet = new FuzzySet { };

            if (setOne.b.value < setTwo.a.value)
            {
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setOne.a.x,
                    value = setOne.a.value
                });
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setOne.b.x,
                    value = setOne.b.value
                });
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setTwo.a.x,
                    value = setTwo.a.value
                });
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setTwo.b.x,
                    value = setTwo.b.value
                });
            }
            else if (setOne.b.value == setTwo.a.value)
            {
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setOne.a.x,
                    value = setOne.a.value
                });
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setOne.b.x,
                    value = setOne.b.value
                });
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setTwo.b.x,
                    value = setTwo.b.value
                });
            }
            else if(setOne.a.value >= setTwo.b.value)
            {
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setOne.a.x,
                    value = setOne.a.value
                });
            }

            return returnedSet;
        }

        public FuzzySet Complement()
        {
            FuzzySet freturn = this;

            if(freturn.a.x != null)
                freturn.a.x = 1 - freturn.a.x;

            if (freturn.b.x != null)
                freturn.b.x = 1 - freturn.b.x;

            if (freturn.c.x != null)
                freturn.c.x = 1 - freturn.c.x;

            return freturn;
        }
    }
}
