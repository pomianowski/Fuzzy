using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyNumbers
{
    class FuzzySolver
    {
        public static FuzzySet Product(FuzzySet setOne, FuzzySet setTwo)
        {
            return setOne;
        }

        public static FuzzySet Summary(FuzzySet setOne, FuzzySet setTwo)
        {
            //s-norma
            if (setOne.Type == CalcType.Gamma && setTwo.Type == CalcType.L)
                return SumLGamma(setTwo, setOne);
            else if (setOne.Type == CalcType.L && setTwo.Type == CalcType.Gamma)
                return SumLGamma(setOne, setTwo);

            return new FuzzySet { };
        }

        protected static FuzzySet SumLGamma(FuzzySet setOne, FuzzySet setTwo)
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
            else if (setOne.a.value >= setTwo.b.value)
            {
                returnedSet.absolutePoints.Add(new FuzzyValue
                {
                    x = setOne.a.x,
                    value = setOne.a.value
                });
            }

            return returnedSet;
        }

        public static FuzzySet Complement(FuzzySet setOne)
        {
            FuzzySet freturn = setOne;

            if (freturn.a.x != null)
                freturn.a.x = 1 - freturn.a.x;

            if (freturn.b.x != null)
                freturn.b.x = 1 - freturn.b.x;

            if (freturn.c.x != null)
                freturn.c.x = 1 - freturn.c.x;

            return freturn;
        }
    }
}
