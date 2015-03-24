using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "..//..//..//Cyber2O//Cyber2O//Cyber2OContent//Assets//2D//";
            string[] menu = Directory.GetFiles(path + "startAnimationButton/");
            foreach (string file in menu)
            {
                string input = file;
                string output = input.Substring(input.IndexOf("Assets"));
                Console.Out.WriteLine(output);
            }
        }
    }
}
