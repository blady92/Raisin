using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CGameStateEngine
{
    class GameStateLoadingGame : GameState
    {
        private Sprite menuBackground;
        private Sprite loadingText;

        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            menuBackground = new Sprite(0, 0);
            menuBackground.LoadContent(theContentManager, path + "menuBackground");

            loadingText = new Sprite(475, 367);
            loadingText.LoadContent(theContentManager, path + "loadingText");
        }

        //Rysowanie samego tła, że się ładuje, bo i tak chuja zawiesi, więc musi być statyczne chociaż
        public override void Draw(SpriteBatch spriteBatch)
        {
            menuBackground.DrawByRectangle(spriteBatch);
            loadingText.DrawByRectangle(spriteBatch);
        }
    }
}
