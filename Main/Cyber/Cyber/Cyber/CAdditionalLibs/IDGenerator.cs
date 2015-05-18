using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cyber.CAdditionalLibs
{
    class IDGenerator
    {
        private static Random random = new Random();
        private static Random characterOrNumber = new Random();
        public static string GenerateID()
        {
            string id = "0x";
             //dla określenia zakresu
            for(int i=0; i<6; i++){
                if (characterOrNumber.Next(0, 50)%2 == 0) //Tu losujemy liczbę jakąś
                {
                    id += random.Next(0, 10).ToString();
                }
                else
                {
                    id += Convert.ToChar(random.Next(65,71)).ToString();
                }
            }
            return id;
        }
    }
}
