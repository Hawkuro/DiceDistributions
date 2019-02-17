using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalc
{
    public enum RoundingType
    {
        ToEven = MidpointRounding.ToEven,
        AwayFromZero = MidpointRounding.AwayFromZero,
        Floor = 2,
        Ceil = 3
    }

    public class DiceDist
    {
        // The divisor to be divided into the dividends to get the probabilities
        private readonly int _divisor;

        private readonly int _indexPeriod;

        // _distributionDividends' keys are possible results, the values the dividend of their probability
        private readonly Dictionary<int, int> _distributionDividends;

        private Dictionary<int, double> _Distribution;
        // Distribution keys are possible results, the values their probability
        public Dictionary<int, double> Distribution
            =>
                _Distribution ??
                (_Distribution =
                    _distributionDividends.ToDictionary(kvp => kvp.Key, kvp => (double) kvp.Value/_divisor));

        private double? _ExpectedValue;
        public double ExpectedValue => _ExpectedValue ?? (_ExpectedValue = Distribution.Select(kvp => kvp.Key*kvp.Value).Sum()).Value;

        private double? _Variance;
        // Calculated via the law that Var[X] = E[x^2] - E[x]^2
        public double Variance
            => _Variance ?? (_Variance = Distribution.Select(kvp => kvp.Key*kvp.Key*kvp.Value).Sum() - ExpectedValue*ExpectedValue).Value;

        public double StdDeviance => Math.Sqrt(Variance);

        public DiceDist(Dictionary<int, int> distributionDividends, int divisor, int indexPeriod)
        {
            _distributionDividends = distributionDividends;
            _divisor = divisor;
            _indexPeriod = indexPeriod;
        }

        public DiceDist(int dSize)
        {
            _distributionDividends = new Dictionary<int, int>();

            for (int i = 1; i <= dSize; i++)
            {
                _distributionDividends.Add(i,1);
            }

            _divisor = dSize;
            _indexPeriod = 1;
        }

        public override string ToString()
        {
            return string.Join("\n", Distribution.Select(kvp => $"{kvp.Key}: {kvp.Value*100:F}%"));
        }

        public static DiceDist operator +(DiceDist a, DiceDist b)
        {
            throw new NotImplementedException();
        }


        public static DiceDist operator +(DiceDist d, int i)
        {
            var dD = d._distributionDividends.ToDictionary(kvp => kvp.Key + i, kvp => kvp.Value);
            return new DiceDist(dD, d._divisor, d._indexPeriod);
        }

        public static DiceDist operator +(int i, DiceDist d)
        {
            return d + i;
        }


        public static DiceDist operator *(DiceDist a, DiceDist b)
        {
            throw new NotImplementedException();
        }


        public static DiceDist operator *(DiceDist d, int m)
        {
            if (m == 0)
            {
                return new DiceDist(new Dictionary<int, int> { {0,1} },1,1);
            }

            var dD = d._distributionDividends.ToDictionary(kvp => kvp.Key * m, kvp => kvp.Value);
            return new DiceDist(dD, d._divisor, d._indexPeriod * m);
        }

        public static DiceDist operator *(int m, DiceDist d)
        {
            return d*m;
        }

        private static int Round(double d, RoundingType rType)
        {
            switch (rType)
            {
                case RoundingType.ToEven:
                    return (int) Math.Round(d, MidpointRounding.ToEven);
                case RoundingType.AwayFromZero:
                    return (int) Math.Round(d, MidpointRounding.AwayFromZero);
                case RoundingType.Ceil:
                    return (int) Math.Ceiling(d);
                case RoundingType.Floor:
                    return (int) Math.Floor(d);
                default:
                    throw new Exception("RoundingType not valid");
            }
        }

        public static DiceDist NumMultWithRounding(DiceDist d, double m, RoundingType rounding)
        {
            throw new NotImplementedException();
        }


        public static DiceDist NumDivWithRounding(DiceDist d, int q, RoundingType rounding)
        {
            throw new NotImplementedException();
        }
    }
}
