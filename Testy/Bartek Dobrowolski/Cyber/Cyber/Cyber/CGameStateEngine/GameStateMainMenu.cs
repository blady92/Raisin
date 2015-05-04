using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber.CGameStateEngine
{
    class GameStateMainMenu : GameState
    {
        #region MODELVIEW

        private Sprite menuBackground;
        private SpriteAnimationDynamic[] spriteAnimationList;
        private SpriteAnimationDynamic sprite;

        public Sprite MenuBackground
        {
            get { return menuBackground; }
            set { menuBackground = value; }
        }

        public SpriteAnimationDynamic[] SpriteAnimationList
        {
            get { return spriteAnimationList; }
            set { spriteAnimationList = value; }
        }

        public SpriteAnimationDynamic Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        #endregion MODELVIEW

        #region LOADCONTENT

        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            //Loading static elements
            menuBackground = new Sprite(0, 0);
            menuBackground.LoadContent(theContentManager, path + "menuBackground");

            string[] textureList = new[] { "startAnimationButton", "loadAnimationButton", "settingsAnimationButton", "exitAnimationButton" };
            spriteAnimationList = new SpriteAnimationDynamic[textureList.Length];
            for (int i = 0; i < textureList.Length; i++)
            {
                spriteAnimationList[i] = new SpriteAnimationDynamic(path + textureList[i], true);
                spriteAnimationList[i].LoadAnimationHover(theContentManager);
                spriteAnimationList[i].LoadClickAnimation(theContentManager);
            }

            spriteAnimationList[0].SpritePosition = new Vector2(450, 235 + 36 * 1);
            spriteAnimationList[1].SpritePosition = new Vector2(475, 235 + 40 + 36 * 2);
            spriteAnimationList[2].SpritePosition = new Vector2(475, 235 + 80 + 36 * 3);
            spriteAnimationList[3].SpritePosition = new Vector2(450, 235 + 120 + 36 * 4);

        }

        #endregion LOADCONTENT
        
        #region DRAW

        public override void Draw(SpriteBatch spriteBatch)
        {
            menuBackground.DrawByRectangle(spriteBatch);
            foreach (SpriteAnimationDynamic s in spriteAnimationList)
            {
                s.DrawAnimation(spriteBatch);
            }
        }
        #endregion DRAW

    }
}
