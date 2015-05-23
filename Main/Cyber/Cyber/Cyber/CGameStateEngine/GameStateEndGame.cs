using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CGameStateEngine
{
    class GameStateEndGame : GameState
    {
        private Sprite background;
        private Sprite youHaveLost;
        public SpriteAnimationDynamic[] SpriteAnimationList { get; set; }

        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            background = new Sprite(0,0);
            background.LoadContent(theContentManager, path + "menuBackground");
            youHaveLost = new Sprite(400, 200);
            youHaveLost.LoadContent(theContentManager, path + "youHaveLost");

            string[] textureList = { "yes", "no" };
            SpriteAnimationList = new SpriteAnimationDynamic[textureList.Length];

            for (int i = 0; i < textureList.Length; i++)
            {
                SpriteAnimationList[i] = new SpriteAnimationDynamic(path + textureList[i], false);
                SpriteAnimationList[i].LoadAnimationHover(theContentManager);
            }
            SpriteAnimationList[0].SpritePosition = new Vector2(450, 300);
            SpriteAnimationList[1].SpritePosition = new Vector2(550, 300);
        }

        //A tu ofc rysowanko "You've lost..."
        public override void Draw(SpriteBatch spriteBatch)
        {
            background.DrawByRectangle(spriteBatch);
            youHaveLost.DrawByRectangle(spriteBatch);
            foreach (SpriteAnimationDynamic s in SpriteAnimationList)
            {
                s.DrawAnimation(spriteBatch);
            }
        }
    }
}
