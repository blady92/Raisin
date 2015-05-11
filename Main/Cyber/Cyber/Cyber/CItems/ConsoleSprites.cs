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
        private Sprite console;
        private Sprite consoleAdditional;
        private Sprite consoleButton;
        private float speedMovement;
        private bool yAnimationDone;
        private bool xAnimationDone;

        #region ACCESSORS

        public bool IsUsed
        {
            get { return isUsed; }
            set { isUsed = value; }
        }

        public Sprite ConsoleBackground
        {
            get { return console; }
            set { console = value; }
        }

        public Sprite ConsoleButton
        {
            get { return consoleButton; }
            set { consoleButton = value; }
        }

        public float SpeedMovement
        {
            get { return speedMovement; }
            set { speedMovement = value; }
        }
        #endregion

        public void LoadContent(ContentManager theContentManager)
        {   console = new Sprite(0, 0); //Ustawienie byle jak
            console.LoadContent(theContentManager, "Assets/2D/console");
            
            //UWAGA NA WYMIARY OKNA GRY
            //console.Position = new Vector2(-console.SpriteAccessor.Width + 30, 768-100);
            //Schowanie głównej konsoli
            console.Position = new Vector2(-console.SpriteAccessor.Width + 30, 768 - 30);
            //Maksymalne pokazanie
            //console.Position = new Vector2(-console.SpriteAccessor.Width + console.SpriteAccessor.Width, 768 - console.SpriteAccessor.Height);


            consoleAdditional = new Sprite(0,0);
            consoleAdditional.LoadContent(theContentManager, "Assets/2D/consoleAdditional");
            //schowanie
            consoleAdditional.Position = new Vector2(console.Position.X + consoleAdditional.SpriteAccessor.Width + 3, console.Position.Y);
            //pokazanie
            consoleAdditional.Position = new Vector2(console.SpriteAccessor.Width, console.Position.Y);

            xAnimationDone = false;
            yAnimationDone = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            consoleAdditional.DrawByRectangle(spriteBatch);
            console.DrawByRectangle(spriteBatch);
        }

        public void Update()
        {
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown((Keys.Y)))
            {
                isUsed = !isUsed;
            }
            if (isUsed)
            {
                ShowConsole(new Vector2(20, 10));
            }
            else
            {
                //Debug.WriteLine("Not used");
            }

            Debug.WriteLine("Pozycja minimalna : " + console.Position.Y + " oraz wychylenie " + (768 - console.SpriteAccessor.Height));
        }
        public void HideConsole()
        {
            
        }

        public void ShowConsole(Vector2 speed)
        {
            if(!yAnimationDone){
                if (console.Position.Y > 768 - console.SpriteAccessor.Height)
                {
                    console.Position.Y -= speed.Y;
                    consoleAdditional.Position.Y += speed.Y;
                }
                else
                {
                    yAnimationDone = true;
                }
            }
            if(yAnimationDone)
            {
                if (console.Position.X < -console.SpriteAccessor.Width + console.SpriteAccessor.Width)
                {
                    console.Position.X += speed.X;
                    consoleAdditional.Position.X += speed.X;
                }
            }
        }
    }
}
