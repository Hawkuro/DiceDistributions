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
            Console.WriteLine(test);
            Console.WriteLine($"Expected Value = {test.ExpectedValue}");
            Console.WriteLine($"Variance = {test.Variance}");

            Console.ReadLine();
        }
    }
}
