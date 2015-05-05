using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace Cyber.CConsoleEngine
{
    public class SayHelloCommand : IConsoleCommand
    {
        public string Description
        {
            get { return "Forces current CyberOS supervisor to identify itself"; }
        }

        public string Execute(string[] arguments)
        {
            return "Hello, I'm TheOS, nice to meet you!";
        }

        public string Name
        {
            get { return "hello"; }
        }
    }

    public class SudoCommand : IConsoleCommand
    {
        public string Description
        {
            get { return "Executes command as superuser"; }
        }

        public string Execute(string[] arguments)
        {
            return @"We trust you have received the usual lecture from the local System
Administrator. It usually boils down to these three things:

    #1) Respect the privacy of others.
    #2) Think before you type.
    #3) With great power comes great responsibility.

Operation not permitted!";
        }

        public string Name
        {
            get { return "sudo"; }
        }
    }


}
