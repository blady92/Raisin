using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyber2O
{
    class MenuModel
    {
        private Sprite menuBackground;
        private SpriteAnimationDynamic[] spriteAnimationList;
        private SpriteAnimationDynamic sprite;

        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            //Loading static elements
            menuBackground = new Sprite(0,0);
            menuBackground.LoadContent(theContentManager, path + "menuBackground");

            //Loading Buttons
            string[] textureList = new[] { "startAnimationButton", "loadAnimationButton", "settingsAnimationButton", "exitAnimationButton" };
            spriteAnimationList = new SpriteAnimationDynamic[textureList.Length];
            for (int i = 0; i < textureList.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(textureList.Length);
                spriteAnimationList[i] = new SpriteAnimationDynamic(path+textureList[i], true);
                spriteAnimationList[i].LoadAnimationHover(theContentManager);
                spriteAnimationList[i].LoadClickAnimation(theContentManager);
            }
            //spriteAnimationList[0].LoadClickAnimation(theContentManager, 1);
            spriteAnimationList[0].SpritePosition = new Vector2(450, 235 + 36 * 1);
            spriteAnimationList[1].SpritePosition = new Vector2(475, 235 + 40 + 36 * 2);
            spriteAnimationList[2].SpritePosition = new Vector2(475, 235 + 80 + 36 * 3 );
            spriteAnimationList[3].SpritePosition = new Vector2(450, 235 + 120 + 36 * 4);

            //sprite = new SpriteAnimationDynamic(path+textureList[0], true);
            //sprite.LoadAnimationHover(theContentManager, 7);
            //sprite.LoadClickAnimation(theContentManager, 7);
            //sprite.SpritePosition = new Vector2(50, 50);
        }

        public void Update(MouseState mouse)
        {
            mouse = Mouse.GetState();
            MouseState oldMouseState = new MouseState();

            foreach (SpriteAnimationDynamic s in spriteAnimationList)
            {
                if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(s.GetFrameRectangle()))
                {
                    if (s.Clicked)
                    {
                        s.UpdateClickFrame();
                    }
                    else
                    {
                        s.UpdateAnimation();
                    }
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {
                        s.UpdateClickAnimation(true);
                    }
                }
                else
                {
                    s.UpdateReverse();
                    s.ResetClickFrame();
                    s.UpdateClickAnimation(false);
                }
            }
            //if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(sprite.GetFrameRectangle()))
            //{
            //    if (sprite.Clicked)
            //    {
            //        System.Diagnostics.Debug.WriteLine("Obiekt kliknięty");
            //        sprite.UpdateClickFrame();
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("Nie kliknięty");
            //        sprite.UpdateAnimation();
            //    }
            //    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
            //    {
            //        sprite.UpdateClickAnimation(true);
            //    }
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Reversuję");
            //    sprite.UpdateReverse();
            //    System.Diagnostics.Debug.WriteLine("Resetuję klikanie");
            //    sprite.ResetClickFrame();
            //    sprite.UpdateClickAnimation(false);
            //}
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            menuBackground.DrawByRectangle(spriteBatch);
            foreach (SpriteAnimationDynamic s in spriteAnimationList)
            {
                s.DrawAnimation(spriteBatch);
            }
            //sprite.DrawAnimation(spriteBatch);
        }
    }
}
