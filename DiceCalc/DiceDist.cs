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

        private static int gcd(int a, int b)
        {
            return b == 0 ? a : gcd(b, a%b);
        }

        private DiceDist(Dictionary<int, int> distributionDividends, int divisor)
        {
            var GCD = distributionDividends.Values.Aggregate(divisor, gcd);

            if (GCD != 1)
            {
                divisor /= GCD;
                distributionDividends = distributionDividends.ToDictionary(kvp => kvp.Key, kvp => kvp.Value/GCD);
            }

            _distributionDividends = distributionDividends;
            _divisor = divisor;
        }

        public DiceDist(int dSize)
        {
            _distributionDividends = Enumerable.Range(1,dSize).ToDictionary(k=>k,k=>1);

            _divisor = dSize;
        }

        public override string ToString()
        {
            return string.Join("\n", Distribution.Select(kvp => $"{kvp.Key}: {kvp.Value*100:F}%"));
        }

        public static DiceDist operator +(DiceDist a, DiceDist b)
        {
            var dD = new Dictionary<int, int>();
            foreach (var kvpa in a._distributionDividends)
            {
                foreach (var kvpb in b._distributionDividends)
                {
                    var newKey = kvpa.Key + kvpb.Key;
                    if (dD.ContainsKey(newKey))
                    {
                        // a and b are assumed independent, note divisors also multiplied
                        dD[newKey] += kvpa.Value * kvpb.Value;
                    }
                    else
                    {
                        dD.Add(newKey, kvpa.Value * kvpb.Value);
                    }
                }
            }

            return new DiceDist(dD, a._divisor * b._divisor);
        }

        public static DiceDist operator -(DiceDist d)
        {
            return new DiceDist(d._distributionDividends.ToDictionary(kvp=>-kvp.Key,kvp=>kvp.Value),d._divisor);
        }

        public static DiceDist operator +(DiceDist d, int i)
        {
            var dD = d._distributionDividends.ToDictionary(kvp => kvp.Key + i, kvp => kvp.Value);
            return new DiceDist(dD, d._divisor);
        }

        public static DiceDist operator +(int i, DiceDist d)
        {
            return d + i;
        }

        public static DiceDist operator -(DiceDist d, int i)
        {
            return d + (-i);
        }

        public static DiceDist operator -(int i, DiceDist d)
        {
            return i + (-d);
        }

        public static DiceDist operator *(DiceDist a, DiceDist b)
        {
            var dD = new Dictionary<int, int>();
            foreach (var kvpa in a._distributionDividends)
            {
                foreach (var kvpb in b._distributionDividends)
                {
                    var newKey = kvpa.Key * kvpb.Key;
                    if (dD.ContainsKey(newKey))
                    {
                        // a and b are assumed independent, note divisors also multiplied
                        dD[newKey] += kvpa.Value * kvpb.Value;
                    }
                    else
                    {
                        dD.Add(newKey, kvpa.Value * kvpb.Value);
                    }
                }
            }

            return new DiceDist(dD, a._divisor * b._divisor);
        }


        public static DiceDist operator *(DiceDist d, int m)
        {
            if (m == 0)
            {
                return new DiceDist(new Dictionary<int, int> { {0,1} },1);
            }

            var dD = d._distributionDividends.ToDictionary(kvp => kvp.Key * m, kvp => kvp.Value);
            return new DiceDist(dD, d._divisor);
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
            var dD = new Dictionary<int,int>();
            foreach (var kvp in d._distributionDividends)
            {
                var newKey = Round(kvp.Key*m, rounding);
                if (dD.ContainsKey(newKey))
                {
                    dD[newKey] += kvp.Value;
                }
                else
                {
                    dD.Add(newKey,kvp.Value);
                }
            }

            return new DiceDist(dD,d._divisor);
        }


        public static DiceDist NumDivWithRounding(DiceDist d, int q, RoundingType rounding)
        {
            var dD = new Dictionary<int, int>();
            foreach (var kvp in d._distributionDividends)
            {
                var newKey = Round((double) kvp.Key / q, rounding);
                if (dD.ContainsKey(newKey))
                {
                    dD[newKey] += kvp.Value;
                }
                else
                {
                    dD.Add(newKey, kvp.Value);
                }
            }

            return new DiceDist(dD, d._divisor);
        }

        public static DiceDist DiceDiv(DiceDist a, DiceDist b, RoundingType rounding)
        {
            var dD = new Dictionary<int, int>();
            foreach (var kvpa in a._distributionDividends)
            {
                foreach (var kvpb in b._distributionDividends)
                {
                    var newKey = Round((double)kvpa.Key / kvpb.Key, rounding);
                    if (dD.ContainsKey(newKey))
                    {
                        // a and b are assumed independent, note divisors also multiplied
                        dD[newKey] += kvpa.Value * kvpb.Value;
                    }
                    else
                    {
                        dD.Add(newKey, kvpa.Value * kvpb.Value);
                    }
                }
            }

            return new DiceDist(dD, a._divisor * b._divisor);
        }
    }
}
