using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Cyber.CAdditionalLibs
{
    public class IDGenerator
    {
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
            while (inputList.Count > 0)
            {
                var randomIndex = r.Next(0, inputList.Count); 
                randomList.Add(inputList[randomIndex]);
                inputList.RemoveAt(randomIndex);
            }

            return randomList;
        }

    }
}
