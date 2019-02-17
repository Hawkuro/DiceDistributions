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
        private readonly int _divisor;

        private readonly int _indexPeriod;

        private readonly Dictionary<int, int> _distributionDividends;

        private Dictionary<int, double> _Distribution;
        public Dictionary<int, double> Distribution
            =>
                _Distribution ??
                (_Distribution =
                    _distributionDividends.ToDictionary(kvp => kvp.Key, kvp => (double) kvp.Value/_divisor));

        private double? _ExpectedValue;
        public double ExpectedValue => _ExpectedValue ?? (_ExpectedValue = Distribution.Select(kvp => kvp.Key*kvp.Value).Sum()).Value;

        private double? _Variance;
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


        public static DiceDist operator +(DiceDist a, int b)
        {
            throw new NotImplementedException();
        }


        public static DiceDist operator *(DiceDist a, DiceDist b)
        {
            throw new NotImplementedException();
        }


        public static DiceDist operator *(DiceDist a, int b)
        {
            throw new NotImplementedException();
        }


        public static DiceDist NumMultWithRounding(DiceDist a, double b, RoundingType rounding)
        {
            throw new NotImplementedException();
        }
    }
}
