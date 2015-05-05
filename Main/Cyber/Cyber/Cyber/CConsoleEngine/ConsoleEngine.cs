using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGameConsole;

namespace Cyber.CConsoleEngine
{
    public class ConsoleEngine
    {
        public static GameConsoleOptions GetDefaultGameConsoleOptions(Game game)
        {
            return new GameConsoleOptions
            {
                Font = game.Content.Load<SpriteFont>("Assets/Fonts/GameFont"),
                FontColor = Color.LawnGreen,
                Prompt = "[sam@Cyber2O]$ ",
                PromptColor = Color.Crimson,
                CursorColor = Color.OrangeRed,
                BackgroundColor = new Color(0, 0, 0, 150), //Color.BLACK with transparency
                PastCommandOutputColor = Color.Aqua,
                BufferColor = Color.Gold
            };
        }
    }
}
