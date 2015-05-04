using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cyber.CGameStateEngine
{
    class GameStatePauseMenu : GameState
    {
        private SpriteAnimationDynamic[] spriteAnimationList;
        private Sprite menuBackground;

        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            menuBackground = new Sprite(0, 0);
            menuBackground.LoadContent(theContentManager, path + "menuBackground");

            //Load buttons
            string[] textureList = new[] { "resumeAnimationButton", "saveAnimationButton", "settingsAnimationButton", "exitAnimationButton" };
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
    }
}
