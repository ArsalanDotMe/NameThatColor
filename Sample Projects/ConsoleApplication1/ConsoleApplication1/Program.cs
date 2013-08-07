using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NameThatColor;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            NTC ntc = new NTC();
            Console.WriteLine("The close main color is :" + ntc.getMainColorName(0xFF, 0xFD, 0xE7));
            Console.WriteLine("The color shade is :" + ntc.getColorName(0xFF, 0xFD, 0xE7));
        }
    }
}
