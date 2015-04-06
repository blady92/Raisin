using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O.GameStates
{
    class PauseState : GameState
    {
        private SpriteAnimationDynamic[] spriteAnimationList;
        private Sprite menuBackground;

        public override void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
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

        public override void Update(MouseState mouse)
        {
            mouse = Mouse.GetState();
            MouseState oldMouseState = new MouseState();
            for (int i = 0; i<spriteAnimationList.Length; i++)
            {
                if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(spriteAnimationList[i].GetFrameRectangle()))
                {
                    if (spriteAnimationList[i].Clicked)
                    {
                        spriteAnimationList[i].UpdateClickFrame();
                        if (spriteAnimationList[i].ClickCurrentFrameAccessor ==
                            spriteAnimationList[i].ClickTextureList.Length - 1)
                        {
                            switch (i)
                            {
                                case 0:
                                    base.StateGame = "resume";
                                    break;
                                case 1:
                                    base.StateGame = "saveload";
                                    break;
                                case 2:
                                    base.StateGame = "settings";
                                    break;
                                case 3:
                                    base.StateGame = "exitToMenu";
                                    Thread.Sleep(500);
                                    spriteAnimationList[i].Clicked = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        spriteAnimationList[i].UpdateAnimation();
                    }
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {
                        spriteAnimationList[i].UpdateClickAnimation(true);
                    }
                }
                else
                {
                    spriteAnimationList[i].UpdateReverse();
                    spriteAnimationList[i].ResetClickFrame();
                    spriteAnimationList[i].UpdateClickAnimation(false);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            menuBackground.DrawByRectangle(spriteBatch);
            foreach (SpriteAnimationDynamic s in spriteAnimationList)
            {
                s.DrawAnimation(spriteBatch);
            }
        }
    }
}
