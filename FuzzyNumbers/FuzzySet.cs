using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyNumbers
{

    enum CalcType
    {
        Singleton = 0,
        Gamma,
        L,
        T
    }

    struct FuzzyValue
    {
        public double x { get; set; }
        public double value { get; set; }
    }

    class FuzzySet
    {
        public CalcType Type;

        public FuzzyValue a;
        public FuzzyValue b;
        public FuzzyValue c;

        public FuzzySet Product(FuzzySet set)
        {
            return set;
        }

        public FuzzySet Sum(FuzzySet set)
        {
            return set;
        }

        public FuzzySet Adjunct(FuzzySet set)
        {
            return set;
        }
    }
}
