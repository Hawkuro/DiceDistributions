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
        private int _divisor;

        private int _indexPeriod;

        private readonly Dictionary<int, int> _distributionDividends;

        public Dictionary<int, double> Distribution
            => _distributionDividends.Keys.ToDictionary(k => k, k => ((double) _distributionDividends[k])/_divisor);

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
