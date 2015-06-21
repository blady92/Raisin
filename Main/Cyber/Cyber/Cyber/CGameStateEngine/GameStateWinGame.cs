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
            winText = new Sprite(450, 350);
            background.LoadContent(theContentManager, path + "menuBackground");
            winText.LoadContent(theContentManager, path + "endGameText");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.DrawByRectangle(spriteBatch);
            winText.DrawByRectangle(spriteBatch);
        }
    }
}
