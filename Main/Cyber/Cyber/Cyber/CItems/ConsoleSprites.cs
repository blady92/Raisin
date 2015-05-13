using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyber.CItems
{
    class ConsoleSprites
    {
        private bool isUsed;
        public SpriteAnimationDynamic console { get; set; }
        public Sprite consoleAdditional { get; set; }
        public Sprite consoleButton { get; set; }

        #region ACCESSORS

        public bool IsUsed
        {
            get { return isUsed; }
            set { isUsed = value; }
        }

        public SpriteAnimationDynamic Console
        {
            get { return console; }
            set { console = value; }
        }
        #endregion

        public void LoadContent(ContentManager theContentManager)
        {
            console = new SpriteAnimationDynamic("Assets/2D/consoleAnimation", false); //Ustawienie byle jak
            console.LoadAnimationHover(theContentManager);
            console.SpritePosition = new Vector2(0, 768-console.TextureList[0].Height);

            //consoleAdditional = new Sprite(0,0);
            //consoleAdditional.LoadContent(theContentManager, "Assets/2D/consoleAdditional");
            //pokazanie
            //consoleAdditional.Position = new Vector2(console.SpriteAccessor.Width, console.Position.Y);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
           console.DrawAnimation(spriteBatch);
        }

        public void Update()
        {
            if (isUsed)
                console.UpdateAnimation();
            else 
                console.UpdateReverse();
        }

        public void HideConsole()
        {
            console.UpdateReverse();
        }

        public void ShowConsole()
        {
            console.UpdateTillEnd();
        }
    }
}
