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
    class MenuModel : Game1
    {
        private Sprite menuBackground;
        
        private SpriteAnimationDynamic menuStart;
        private SpriteAnimationDynamic menuSettings;
        private SpriteAnimationDynamic menuQuit;

        private SpriteAnimationDynamic[] spriteAnimationList;
        private MouseState mouseState;
        
        
        public void LoadContent(ContentManager theContentManager)
        {
            string path = "Assets/2D/";

            //Loading static elements
            //menuBackground = new Sprite();
            //menuBackground.LoadContent(theContentManager, path + "menuBackground");

            //Loading Buttons
            Texture2D texture = theContentManager.Load<Texture2D>(path + "startAnimation");
            menuStart = new SpriteAnimationDynamic(texture, 5, 1, new Vector2(200, 100));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //menuBackground.Draw(spriteBatch);
            menuStart.Draw(spriteBatch);
        }

        public void Update()
        {
            mouseState = Mouse.GetState();
            if (new Rectangle(mouseState.X, mouseState.Y, 20, 20).Intersects(menuStart.GetRectangle()))
            {
                System.Diagnostics.Debug.WriteLine("Intersected: " + menuStart.GetRectangle() + " " + mouseState.ToString());
                menuStart.Update();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Układ buttonu" + menuStart.GetRectangle().ToString());
                System.Diagnostics.Debug.WriteLine("Układ myszki X:"+mouseState.X+" Y:"+mouseState.Y+" Width: "+(mouseState.X+(int)20)+" Height:"+(mouseState.Y+(int)20));
                menuStart.ResetFrame();
            }
        }
    }
}
