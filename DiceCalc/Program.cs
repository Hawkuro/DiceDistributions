using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            DiceDist test = new DiceDist(6);
            Console.WriteLine(string.Join("\n",test.Distribution.Select(kvp=>$"{kvp.Key}: {kvp.Value*100:F}%")));

            Console.ReadLine();
        }
    }
}
