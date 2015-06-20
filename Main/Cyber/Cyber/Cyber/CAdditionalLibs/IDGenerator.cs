using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Cyber.CAdditionalLibs
{
    class IDGenerator
    {
        private Random random = new Random();
        private Random characterOrNumber = new Random();
        public List<string> IDs { get; set; }
        
        public void GenerateID()
        {
            IDs = new List<string>();
            string[] filePaths = Directory.GetFiles(@"..//..//..//..//CyberContent//Assets//2D//IDs//");
            foreach (string filePath in filePaths)
            {
                Debug.WriteLine(filePath.Substring(filePath.Length - 10, 6).ToUpper());
                IDs.Add(filePath.Substring(filePath.Length - 10, 6).ToUpper());
            }
            IDs = ShuffleList(IDs);
        }

        public List<string> ShuffleList(List<string> inputList)
        {
            List<string> randomList = new List<string>();
            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); 
                randomList.Add(inputList[randomIndex]);
                inputList.RemoveAt(randomIndex);
            }

            return randomList;
        }

    }
}
