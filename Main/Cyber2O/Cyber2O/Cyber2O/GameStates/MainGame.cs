using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O.GameStates
{
    class MainGame : GameState
    {
        private Sprite gameBackground;

        //No to lecim z tematem, będzie zajebiście (I hope so!)
        //Bo jak nie, to wszystko pójdzie w diabły (╯°□°)╯︵ ┻━┻
        //a tego nie chcemy 
        //┬─┬﻿ ノ( ゜-゜ノ)
        public override void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            gameBackground = new Sprite(0, 0);
            gameBackground.LoadContent(theContentManager, path + "gameBackground");
            //Load whole 3D/Music Stuffs   

        }

        public override void Update(MouseState mouseState)
        {
            //all methods for update state of basic game
            base.Update(mouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gameBackground.DrawByRectangle(spriteBatch);
            //All drawable things
            base.Draw(spriteBatch);
        }
    }
}
