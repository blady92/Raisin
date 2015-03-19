using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber2O
{
    public interface Responsable
    {
        void response(string message);
    }
    public class Terminal : Responsable
    {
        private Dictionary<string, ResponseEventHandler> dict;
        public Responsable Responser { get; set; }
        
        public delegate void ResponseEventHandler(object sender, List<string> parameters, Responsable responser);
        public void process(string message)
        {
            string[] args = message.Split(' ');
            if (dict.ContainsKey(args[0]))
            {
                List<string> parameters = args.ToList();
                parameters.Remove(args[0]);
                ResponseEventHandler handler = dict[args[0]];
                handler(this, parameters, Responser);
            }
        }

        public void response(string message)
        {
            Console.WriteLine(message);
        }

        public Terminal()
        {
            Responser = this;
            dict = new Dictionary<string, ResponseEventHandler>();
            dict.Add("open", handleOpen);
            dict.Add("close", handleClose);
            dict.Add("sudo", handleSudo);
        }

        private void handleOpen(object sender, List<string> parameters, Responsable responser)
        {
            if (parameters != null && parameters.Count > 0)
            {
                responser.response("Opened " + parameters[0]);
            }
            else
            {
                responser.response("Command \"open\" needs an argument");
            }
        }
        private void handleClose(object sender, List<string> parameters, Responsable responser)
        {
            if (parameters != null && parameters.Count > 0)
            {
                responser.response("Closed " + parameters[0]);
            }
            else
            {
                responser.response("Command \"close\" needs an argument");
            }
        }
        private void handleSudo(object sender, List<string> parameters, Responsable responser)
        {
            responser.response(@"We trust you have received the usual lecture from the local System
Administrator. It usually boils down to these three things:

    #1) Respect the privacy of others.
    #2) Think before you type.
    #3) With great power comes great responsibility.

Operation not permitted!");
        }


    }
}
