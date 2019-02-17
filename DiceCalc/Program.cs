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
            DiceDist test = 10 * new DiceDist(10) + new DiceDist(10) - 10;
            Console.WriteLine(test);
            Console.WriteLine($"Expected Value = {test.ExpectedValue}");
            Console.WriteLine($"Variance = {test.Variance}");
            Console.WriteLine($"Standard Deviation = {test.StdDeviance}");

            Console.ReadLine();
        }
    }
}
