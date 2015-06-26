using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CItems.Notes
{
    class Command
    {
        private CommandType commandType;
        private string command;
        private string commandDescription;

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
        

        public void Draw()
        {
            
        }

        private String parseText(String text, int textBox)
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
