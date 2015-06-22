using Cyber.AudioEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyber.CGameStateEngine;
using Cyber.CItems.CStaticItem;
using Cyber.CLogicEngine;
using Cyber.CStageParsing;
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

    public class AudioCommand : IConsoleCommand
    {
        private Game game;
        private AudioController audioController;
        public AudioCommand(Game game, AudioController audioController)
        {
            this.game = game;
            this.audioController = audioController;
        }
        public string Description
        {
            get { return "Controls audio flow"; }
        }

        public string Execute(string[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                if (arguments.Length == 1 && arguments[0].Equals("reset"))
                {
                    audioController.resetAudio();
                    return "audio reset";
                }
                if (arguments.Length == 2 && arguments[0].Equals("play"))
                {
                    try
                    {
                        int i = int.Parse(arguments[1]);
                        if (i >= 0 && i < audioController.Audio.CueList.Count)
                        {
                            audioController.Audio.CueList[i].Play();
                            return "audio : playing track " + i + " : " + audioController.Audio.CueList[i].Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        return "audio : error playing " + arguments[1];
                    }
                }
            }
            return "audio : processing error";
        }

        public string Name
        {
            get { return "audio"; }
        }
    }

    public class OpenGateCommand : IConsoleCommand
    {
        private GameStateMainGame gameStateMainGame;

        public OpenGateCommand(GameStateMainGame gameStateMainGame)
        {
            this.gameStateMainGame = gameStateMainGame;
        }
        public string Execute(string[] arguments)
        {
            if (arguments == null || arguments.Length != 1)
            {
                return "Command open takes only 1 argument";
            }

            foreach (var gate in gameStateMainGame.stageElements.Where(x => arguments[0].Equals(x.ID)))
            {
                gate.ColliderInternal = null;
                gameStateMainGame.plot.OpenGate1();
                return "Gate opened";
            }
            
            return "Gate not found";
        }

        public string Name
        {
            get { return "OpenGate"; }
        }

        public string Description
        {
            get { return "Opens a gate"; }
        }
    }

    public class GetTimeCommand : IConsoleCommand
    {
        private GameStateMainGame gameStateMainGame;

        public GetTimeCommand(GameStateMainGame gameStateMainGame)
        {
            this.gameStateMainGame = gameStateMainGame;
        }

        public string Execute(string[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                return "Command open takes onsly 1 argument";
            }
            gameStateMainGame.plot.GetTime();
            return Clock.Instance.RemainingSeconds + " seconds left";
        }

        public string Name
        {
            get { return "GetTime"; }
        }

        public string Description
        {
            get { return "Returns amount of time until Earth is airless."; }
        }
    }

    public class AllySleepCommand : IConsoleCommand
    {
        private GameStateMainGame gameStateMainGame;

        public AllySleepCommand(GameStateMainGame gameStateMainGame)
        {
            this.gameStateMainGame = gameStateMainGame;
        }

        public string Execute(string[] arguments)
        {
            if (arguments == null || arguments.Length != 1)
            {
                return "Command takes only 1 argument";
            }
            string robotId = arguments[0];
            NPC robot = AI.Robots.Find(x => x.ID == robotId);
            if (robot != null)
            {
                AI.Robots.Remove(robot);
                gameStateMainGame.plot.HackAlly();
                return "Robot " + robotId + " screwed";
            }
            return "Robot " + robotId + " not found or already disabled";
        }

        public string Name
        {
            get { return "AllySleep"; }
        }

        public string Description
        {
            get { return "Puts given robot to a temporal comma."; }
        }
    }

    public class AccessGeneratorCommand : IConsoleCommand
    {
        private GameStateMainGame gameStateMainGame;

        public AccessGeneratorCommand(GameStateMainGame gameStateMainGame)
        {
            this.gameStateMainGame = gameStateMainGame;
        }

        public string Execute(string[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                return "Command takes no arguments";
            }
            foreach (var oxygenGenerator in gameStateMainGame.stageElements.Where(x => x.Type == StaticItemType.oxygenGenerator))
            {
                if (gameStateMainGame.plot.GeneratorFound) { 
                    gameStateMainGame.plot.AccessGenerator();
                    return oxygenGenerator.ID;
                }
            }
            return "Oxygen generator not found";
        }

        public string Name
        {
            get { return "AccessGenerator"; }
        }

        public string Description
        {
            get { return "Receives generator ID"; }
        }
    }
    public class FreeCommand : IConsoleCommand
    {
        private GameStateMainGame gameStateMainGame;

        public FreeCommand(GameStateMainGame gameStateMainGame)
        {
            this.gameStateMainGame = gameStateMainGame;
        }

        public string Execute(string[] arguments)
        {
            if (arguments == null || arguments.Length != 1)
            {
                return "Command takes only 1 argument";
            }
            foreach (
                var oxygenGenerator in
                    gameStateMainGame.stageElements.Where(
                    x => 
                        x.Type == StaticItemType.oxygenGenerator &&
                        arguments[0].Equals(x.ID)
                    ))
            {
                gameStateMainGame.plot.RunGenerator();
                return "Game should end here!";
            }
            return "No such generator";
        }

        public string Name
        {
            get { return "Free"; }
        }

        public string Description
        {
            get { return "Frees oxygen from a generator"; }
        }
    }
}
