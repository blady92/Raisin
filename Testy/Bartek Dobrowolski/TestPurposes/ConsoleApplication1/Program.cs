using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random densityRandom = new Random();
            Random frequencyRandom = new Random();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("\n\nTest Series: " + i + "\n --------------------------------------");
                for (int j = 0; j < densityRandom.Next(0, 150); j++)
                {
                    Console.WriteLine("Particles number: " + j + " densiryRandom: " + densityRandom.Next(0,50) + " frequencyRandom: " + frequencyRandom.Next(0,100));
                }
            }
        }
    }
}
