using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber2O
{
    class MenuModel
    {
        private Sprite menuBackground;
        private Sprite menuStart;
        private Sprite menuSettings;
        private Sprite menuQuit;

        public void menuInitialize()
        {
            menuBackground = new Sprite();
            menuStart = new Sprite();
            menuSettings = new Sprite();
            menuQuit = new Sprite();
        }
        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";
            menuBackground.LoadContent(theContentManager, path + "test");
            menuStart.LoadContent(theContentManager, path + "start");
            menuSettings.LoadContent(theContentManager, path + "settings");
            menuQuit.LoadContent(theContentManager, path + "quit");

            menuBackground.Position = new Vector2(0, 0);
            menuStart.Position = new Vector2(136, 36);
            menuSettings.Position = new Vector2(136, 72);
            menuQuit.Position = new Vector2(136, 108);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menuBackground.Draw(spriteBatch);
            menuStart.Draw(spriteBatch);
            menuSettings.Draw(spriteBatch);
            menuQuit.Draw(spriteBatch);
        }
    }
}
