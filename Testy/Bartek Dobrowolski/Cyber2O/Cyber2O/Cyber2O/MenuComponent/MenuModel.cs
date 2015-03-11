using System;
using System.Collections.Generic;
using System.Linq;
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
    class MenuModel : Game1
    {
        private Sprite menuBackground;
        private Sprite menuStart;
        private Sprite menuSettings;
        private Sprite menuQuit;

        private SpriteAnimationDynamic menuTest;
        public void menuInitialize()
        {
            //menuBackground = new Sprite();
            //menuStart = new Sprite();
            //menuSettings = new Sprite();
            //menuQuit = new Sprite();
        }
        public void LoadContent(ContentManager theContentManager)
        {
            //string path = "Assets/2D/";
            //menuBackground.LoadContent(theContentManager, path + "test");
            //menuStart.LoadContent(theContentManager, path + "start", path+"startHover");
            //menuSettings.LoadContent(theContentManager, path + "settings");
            //menuQuit.LoadContent(theContentManager, path + "quit");

            //menuBackground.Position = new Vector2(0, 0);
            //menuStart.Position = new Vector2(136, 36);
            //menuSettings.Position = new Vector2(136, 72);
            //menuQuit.Position = new Vector2(136, 108);

            Texture2D menuTestButton = Content.Load<Texture2D>("Assets/2D/startAnimation");
            menuTest = new SpriteAnimationDynamic(menuTestButton, 2, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menuTest.Draw(spriteBatch, new Vector2(100, 100));
        }

        public void Update()
        {
            MouseState mouseState;
            mouseState = Mouse.GetState();
            System.Diagnostics.Debug.WriteLine("Mouse points are: ("+mouseState.X+", "+mouseState.Y+")");
            if (new Rectangle(mouseState.X, mouseState.Y, 40, 40).Intersects(menuTest.GetRectangle()))
            {
                System.Diagnostics.Debug.WriteLine("Intersected");
            }
        }
    }
}
