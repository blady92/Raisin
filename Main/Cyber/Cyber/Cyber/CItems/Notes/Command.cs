using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Cyber.Audio;
using Cyber.AudioEngine;

namespace Cyber.CItems.Notes
{
    public class Command
    {
        private CommandType commandType;
        private string command;
        private string commandDescription;
        private Color color;
        #region Accessors
        public CommandType CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

        public string Command1
        {
            get { return command; }
            set { command = value; }
        }

        public string CommandDescription
        {
            get { return commandDescription; }
            set { commandDescription = value; }
        }
        #endregion

        private SpriteFont commandSpriteFont;
        private SpriteFont commandDescriptionSpriteFont;

        private AudioModel audioModel = new AudioModel("CyberBank");
        private AudioController audioController;
        private bool textWritingPlayed = false;

        public List<DisplayMessage> messages = new List<DisplayMessage>();  

        public Command(string command, string commandDescription, CommandType commandType)
        {
            this.command = command;
            this.commandDescription = commandDescription;
            this.commandType = commandType;
        }

        public void LoadFontTypes(ContentManager theContentManager)
        {
            commandSpriteFont = theContentManager.Load<SpriteFont>("Assets/Fonts/ConsoleCommand");
            commandDescriptionSpriteFont = theContentManager.Load<SpriteFont>("Assets/Fonts/ConsoleDescription");

            audioController = new AudioController(audioModel);
            audioController.setAudio();
        }

        public void Draw(SpriteBatch spriteBatch, int dashboardWidth, int enters, int commands)
        {
            if(commandType == CommandType.normal) color = new Color(25, 212, 216);
            if(commandType == CommandType.defense)  color = new Color(91, 51, 210);
            if(commandType == CommandType.attack) color = new Color(255, 0, 98);
            spriteBatch.Begin();
            int marginTop = 110;
            messages.Add(new DisplayMessage(command, TimeSpan.FromSeconds(5.0), new Vector2(Game1.maxWidth - dashboardWidth + 20, marginTop + commands * 20 + enters * 15), color));
            messages.Add(new DisplayMessage(commandDescription, TimeSpan.FromSeconds(5.0), new Vector2(Game1.maxWidth - dashboardWidth + 20, marginTop + 20 + commands * 20 + enters * 15), new Color(52, 56, 56)));
            DrawMessages(spriteBatch);
            spriteBatch.End();
        }


        public void DrawMessages(SpriteBatch spriteBatch)
        {
            if (messages.Count > 0 && messages[0].DrawnMessage.Length < command.Length)
            {
                if(!textWritingPlayed)
                {
                    //audioController.textWritingControllerForNotepad("Play");
                    textWritingPlayed = true;
                }
                else
                {
                    //audioController.textWritingControllerForNotepad("Resume");
                }
             
                DisplayMessage dm = messages[0];
                spriteBatch.DrawString(commandSpriteFont, dm.DrawnMessage, dm.Position, dm.DrawColor);
                dm.DrawnMessage += dm.Message[dm.CurrentIndex].ToString();
                if (dm.CurrentIndex != dm.Message.Length - 1) { dm.CurrentIndex++; messages[0] = dm; }
            }
            else
            {
               
                spriteBatch.DrawString(commandDescriptionSpriteFont, messages[0].DrawnMessage, messages[0].Position, messages[0].DrawColor);
                spriteBatch.DrawString(commandDescriptionSpriteFont, messages[1].DrawnMessage, messages[1].Position, messages[1].DrawColor);
            }
            if (messages.Count > 0 && messages[1].DrawnMessage.Length < commandDescription.Length)
            {
                DisplayMessage dm2 = messages[1];
                spriteBatch.DrawString(commandDescriptionSpriteFont, dm2.DrawnMessage, dm2.Position, dm2.DrawColor);
                dm2.DrawnMessage += dm2.Message[dm2.CurrentIndex].ToString();
                if (dm2.CurrentIndex != dm2.Message.Length - 1) { dm2.CurrentIndex++; messages[1] = dm2; }
            }
            else
            {
                audioController.textWritingControllerForNotepad("Pause");
                spriteBatch.DrawString(commandDescriptionSpriteFont, messages[0].DrawnMessage, messages[0].Position, messages[0].DrawColor);
                spriteBatch.DrawString(commandDescriptionSpriteFont, messages[1].DrawnMessage, messages[1].Position, messages[1].DrawColor);
            }
        }

        public string ParseText(String text, int textBox)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (commandDescriptionSpriteFont.MeasureString(line + word).Length() > textBox)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }
    }
}
