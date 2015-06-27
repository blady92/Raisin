using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Cyber.CItems;
using Cyber.CItems.Notes;
using Cyber.CLogicEngine;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;

namespace Cyber
{
    [DataContract]
    public class PlotTwistClass
    {
        [DataMember]
        public bool loaded { get; set; }
        [DataMember]
        private List<string> dialogsList;
        [DataMember]
        public List<int> BreakPoints { get; set; }
        [DataMember]
        public int dialogNumber { get; set; }
        [DataMember]
        public List<string> BreakPointsText { get; set; }
        private string line;

        [DataMember]
        private bool getTime;
        [DataMember]
        private bool passedThroughGate;
        [DataMember]
        private bool gate1Opened;
        [DataMember]
        private bool allyChecked;
        [DataMember]
        private bool allyHacked;
        [DataMember]
        private bool levelCleared;
        [DataMember]
        private bool generatorFound;
        [DataMember]
        private bool generatorAccess;
        [DataMember]
        private bool generatorOn;
        [DataMember]
        private bool possibleEscaping;

        public CommandNotes Notes { get; set; }
        //Czy wykonujemy zadanie, czy opowiadamy fabułę
        [DataMember]
        public bool action { get; set; }


        #region Aksesory do elementów fabuły
        [DataMember]
        public bool GetTime1
        {
            get { return getTime; }
            set { getTime = value; }
        }

        [DataMember]
        public bool PassedThroughGate
        {
            get { return passedThroughGate; }
            set { passedThroughGate = value; }
        }

        [DataMember]
        public bool AllyChecked
        {
            get { return allyChecked; }
            set { allyChecked = value; }
        }
        [DataMember]
        public bool Gate1Opened
        {
            get { return gate1Opened; }
            set { gate1Opened = value; }
        }
        [DataMember]
        public bool LevelCleared
        {
            get { return levelCleared; }
            set { levelCleared = value; }
        }
        [DataMember]
        public bool GeneratorAccess
        {
            get { return generatorAccess; }
            set { generatorAccess = value; }
        }
        [DataMember]
        public bool GeneratorOn
        {
            get { return generatorOn; }
            set { generatorOn = value; }
        }
        [DataMember]
        public bool AllyHacked
        {
            get { return allyHacked; }
            set { allyHacked = value; }
        }

        [DataMember]
        public bool SamChecked { get; set; }
        [DataMember]
        public bool GeneratorFound
        {
            get { return generatorFound; }
            set { generatorFound = value; }
        }

        [DataMember]
        public bool PossibleEscape
        {
            get { return possibleEscaping; }
            set { possibleEscaping = value; }
        }
        #endregion

        public PlotTwistClass()
        {
            loaded = false;
            passedThroughGate = false;
            gate1Opened = false;
            allyChecked = false;
            allyHacked = false;
            generatorFound = false;
            generatorAccess = false;
            generatorOn = false;
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
            BreakPoints.Add(12);

            BreakPoints.Add(14);
            BreakPoints.Add(20);
            BreakPoints.Add(21);
            BreakPoints.Add(22);
            BreakPoints.Add(23);
            BreakPoints.Add(24);

            BreakPointsText = new List<string>();
            //Dla linijki 7
            BreakPointsText.Add("Theo: Use 'GetTime' to know up how much time you have to finish operation.");
            //Dla linijki 13
            BreakPointsText.Add("Samantha: Well... It's hard to don't know where is teleporter here...");

            //Druga plansza
            //Dla linijki 15
            BreakPointsText.Add("Theo: Use 'OpenGate ID' where ID is id of gate to open them.");

            //Dla linijki 21
            BreakPointsText.Add("Samantha: I should do something quickly. Let's see those robots.");
            //Dla linijki 22
            BreakPointsText.Add("Theo: Use 'AllySleep ID' to screw the corpo-robot up. ID is his unique identifier.");
            //Dla linijki 23
            BreakPointsText.Add("Theo: Find the generator here.");
            //Dla linijki 24
            BreakPointsText.Add("Theo: Use 'AccessGenerator' to get the generator ID.");
            //Dla linijki 25
            BreakPointsText.Add("Theo: Use 'Free ID to release oxygen.");


            file.Close();
            loaded = true;
        }

        #region ACCESSORS
        public void GetTime()
        {
            if (!getTime) { 
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = false;
                getTime = true;
                Notes.LearnNewCommand(new Command("GetTime", "Doesn't need any ID. Use to know up, when Cyber2O will finish oxygen privatization", CommandType.normal));
                //Notes.LearnNewCommand(new Command("OpenGate <ID>", "Opens gates with appriopriate ID", CommandType.normal));
                //Notes.LearnNewCommand(new Command("AccessGenerator", "None of ID is needed. Returns ID to nearest Generator.", CommandType.normal));
                //Notes.LearnNewCommand(new Command("Free <ID>", "Use to release oxygen by running nearest generator. ID is gotten from AccessGenerator command.", CommandType.normal));
            }
        }

        public void ThroughGate()
        {
            if (!passedThroughGate) { 
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                dialogNumber++;
                action = false;
                passedThroughGate = true;
            }
        }

        public void OpenGate1()
        {
            if (!gate1Opened) { 
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = false;
                gate1Opened = true;
                Notes.LearnNewCommand(new Command("OpenGate <ID>", "Opens gates with appriopriate ID", CommandType.normal));
            }
        }

        public void CheckAlly()
        {
            if (!AllyChecked && Gate1Opened) {
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = true;
                AllyChecked = true;
            }
        }

        public void HackAlly()
        {
            if (!allyHacked && allyChecked)
            {
                dialogNumber--;
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = true;
                allyHacked = true;
                Notes.LearnNewCommand(new Command("AllySleep <ID>", "ID is enemy identificator. Makes enemies impossibe to move. Radar of this enemy will be turned down. After passing time this enemy will turn on and work in the same way as before.", CommandType.defense));
            }
        }

        public void FoundGenerator()
        {
            if (!generatorFound && AllyHacked)
            {
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = true;
                generatorFound = true;
            }
        }

        

        public void AccessGenerator()
        {
            if (!generatorAccess && generatorFound) {
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = true;
                generatorAccess = true;
                dialogNumber--;
                Notes.LearnNewCommand(new Command("AccessGenerator", "No ID is needed. Returns ID of nearly Generator.", CommandType.normal));
            }
        }

        public void RunGenerator()
        {
            if (generatorAccess && generatorFound)
            {
                BreakPoints.RemoveAt(0);
                BreakPointsText.RemoveAt(0);
                action = true;
                generatorOn = true;
                Notes.LearnNewCommand(new Command("Free <ID>", "Use to release oxygen by running nearly generator. ID is gotten from AccessGenerator command.", CommandType.normal));
            }
        }
        #endregion

        public string getActualDialog()
        {
            if (BreakPoints.Contains(dialogNumber))
            {
                if (!GetTime1           ||
                    !passedThroughGate  ||
                    !gate1Opened        ||
                    !allyChecked        ||
                    !allyHacked         ||
                    !GeneratorFound     ||
                    !generatorFound     ||
                    !generatorAccess    ||
                    !generatorOn        
                    )
                    return BreakPointsText[0];
            }
            if (dialogNumber < dialogsList.Count)
            {
                dialogNumber++;
                if (allyChecked)
                    dialogNumber++;
                if (dialogNumber > 11)
                    possibleEscaping = true;
                if(dialogNumber < 25)
                    return dialogsList[dialogNumber];
            }
            return "";
        }

        public void Reset()
        {
            loaded = false;
            passedThroughGate = false;
            gate1Opened = false;
            allyChecked = false;
            allyHacked = false;
            generatorFound = false;
            generatorAccess = false;
            generatorOn = false;
            dialogNumber = 1;
        }

        public void TeleportToGenerator()
        {
            allyHacked = allyChecked = gate1Opened = passedThroughGate = loaded = true;
            dialogNumber = 22;
        }
    }
}
