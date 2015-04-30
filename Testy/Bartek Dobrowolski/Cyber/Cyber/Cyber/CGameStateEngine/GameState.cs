using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CGameStateEngine
{
    class GameState
    {
        internal enum States
        {
            startMenu,
            pauseMenu,
            mainGame,
            exit
        }

        private States state;

        public States State
        {
            get { return state; }
            set { state = value; }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }
    }
}
