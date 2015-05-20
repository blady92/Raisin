using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace Cyber.CConsoleEngine
{
    public class GameConsoleCommand
    {
        private IConsoleCommand command;

        public GameConsoleCommand (IConsoleCommand command)
        {
            this.command = command;
        }

        public string Execute(string[] args)
        {
            return command.Execute(args);
        }

        public string Name { 
            get
            {
                return command.Name;
            }
        }
    }
}
