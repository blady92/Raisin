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
        public void LoadContent(ContentManager theContentManager)
        {

        }

        //Rysowanie samego tła, że się ładuje, bo i tak chuja zawiesi, więc musi być statyczne chociaż
        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
