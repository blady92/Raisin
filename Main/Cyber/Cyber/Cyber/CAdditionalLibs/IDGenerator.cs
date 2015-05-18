using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cyber.CAdditionalLibs
{
    class IDGenerator
    {
        public static string GenerateID()
        {
            string id = "0x";
            Random random = new Random(); //dla określenia zakresu
            for (int i = 0; i < 6; i++)
            {
                int liczba = random.Next(0, 10);
                if (liczba%2 == 0) // parzysta -> liczba
                {
                    liczba = random.Next(0, 10);
                    id += Convert.ToString(liczba);
                }
                else
                {
                    liczba = random.Next(65, 71);
                    Debug.WriteLine("Wylosowana litera to: " + liczba);
                    id += Convert.ToChar(liczba).ToString();
                }
            }
            return id;
        }
    }
}
