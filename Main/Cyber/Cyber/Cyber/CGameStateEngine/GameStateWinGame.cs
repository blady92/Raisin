using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CGameStateEngine
{
    internal class GameStateWinGame : GameState
    {
        private Sprite background;
        private Sprite winText;

        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            background = new Sprite(0, 0);
            background.LoadContent(theContentManager, path + "endGameScreen");

            //winText = new Sprite(460, 400);
            //winText.LoadContent(theContentManager, path+"win");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.DrawByRectangle(spriteBatch);
        }
    }
}
