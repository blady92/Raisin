using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cyber
{
    public class PlotTwistClass
    {
        public bool loaded { get; set; }
        private List<string> dialogsList;
        public List<int> BreakPoints { get; set; }
        public int dialogNumber { get; set; }
        public List<string> BreakPointsText { get; set; }
        private string line;

        private bool getTime;
        private bool gate1Opened;
        private bool allyHacked;
        private bool levelCleared;
        private bool generatorAccess;
        private bool generatorOn;

        //Czy wykonujemy zadanie, czy opowiadamy fabułę
        public bool action { get; set; }


        #region Aksesory do elementów fabuły
        public bool GetTime1
        {
            get { return getTime; }
            set { getTime = value; }
        }

        public bool Gate1Opened
        {
            get { return gate1Opened; }
            set { gate1Opened = value; }
        }

        public bool LevelCleared
        {
            get { return levelCleared; }
            set { levelCleared = value; }
        }

        public bool GeneratorAccess
        {
            get { return generatorAccess; }
            set { generatorAccess = value; }
        }

        public bool GeneratorOn
        {
            get { return generatorOn; }
            set { generatorOn = value; }
        }

        public bool AllyHacked
        {
            get { return allyHacked; }
            set { allyHacked = value; }
        }

        #endregion

        public PlotTwistClass()
        {
            loaded = false;
            gate1Opened = false;
            generatorAccess = false;
            generatorOn = false;
            allyHacked = false;
            levelCleared = false;
            dialogNumber = 1;
        }

        public void PrintStatus()
        {
            Debug.WriteLine("Status loaded: " + loaded + "\n"+
                "Status Gate: " + gate1Opened + "\n"+
                "Status Ally: " + allyHacked + "\n" + 
                "Status generatorAccess");    
        }

        public void Initialize()
        {
            dialogsList = new List<string>();
            StreamReader file = new StreamReader("...//..//..//..//Cyber//CPlot//mainDialogsTranslated.txt");
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(System.Environment.NewLine, "");
                dialogsList.Add(line);
            }
            BreakPoints = new List<int>();
            BreakPoints.Add(7);
            BreakPoints.Add(14);
            BreakPoints.Add(22);
            BreakPoints.Add(23);
            BreakPoints.Add(24);

            BreakPointsText = new List<string>();
            BreakPointsText.Add("Use GetTime() to know up how much time you have to finish operation.");
            BreakPointsText.Add("Use OpenGate(ID) where ID is id of gate to open them.");
            BreakPointsText.Add("Use AllySleep(ID) to screw the corpo-robot up. ID is his unique identifier.");
            BreakPointsText.Add("Use AccessGenerator() to get the generator ID.");
            BreakPointsText.Add("Use Free(ID) to release oxygen.");
            file.Close();
            loaded = true;
        }

        public void GetTime()
        {
            getTime = true;
        }

        public void OpenGate1()
        {
            gate1Opened = true;
        }

        public void AccessGenerator()
        {
            generatorAccess = true;
        }

        public void RunGenerator()
        {
            generatorOn = true;
        }

        public void HackAlly()
        {
            allyHacked = true;
        }

        public void ClearedStage()
        {
            levelCleared = true;
        }

        public string getActualDialog()
        {
            if (BreakPoints.Contains(dialogNumber))
            {
                if (!getTime)
                    return BreakPointsText[0];
                if (!gate1Opened)
                    return BreakPointsText[1];
                if (!allyHacked)
                    return BreakPointsText[2];
                if (!generatorAccess)
                    return BreakPointsText[3];
                if (!generatorOn)
                    return BreakPointsText[4];
            }
            if (dialogNumber < dialogsList.Count)
            {
                dialogNumber++;
                return dialogsList[dialogNumber];
            }
            return "";
        }
    }
}
