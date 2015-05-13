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
        public string text { get; set; }
        public SpriteFont font { get; set; }
        private Keys[] allKeys;
        private List<Keys> possibleKeys;
        private KeyboardState newPressKey;
        private KeyboardState oldPressKey;
        private int lenght;
        
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
            font = theContentManager.Load<SpriteFont>("Assets/Fonts/ConsoleFont");
            text = "Pressed: ";
            lenght = text.Length;
            SetupKeys();

            //consoleAdditional = new Sprite(0,0);
            //consoleAdditional.LoadContent(theContentManager, "Assets/2D/consoleAdditional");
            //pokazanie
            //consoleAdditional.Position = new Vector2(console.SpriteAccessor.Width, console.Position.Y);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
           console.DrawAnimation(spriteBatch);
            if (console.LoadingFinished())
            {
                //od tego momentu można zacząć pisać tekst
                spriteBatch.Begin();
                spriteBatch.DrawString(font, text, new Vector2(20, Game1.maxHeight-80), Color.Red);
                spriteBatch.End();
            }
        }

        public void Update()
        {
            if (isUsed){
                console.UpdateAnimation();
                allKeys = Keyboard.GetState().GetPressedKeys();
                newPressKey = Keyboard.GetState();
                for (int i = 0; i < allKeys.Length; i++)
                {
                    if (newPressKey.IsKeyDown(allKeys[i]) && oldPressKey.IsKeyUp(allKeys[i]))
                    {
                        if (possibleKeys.Contains(allKeys[i]))
                        {
                            text += ParseKey(allKeys[i]);
                        }
                    }
                }
                if (newPressKey.IsKeyDown(Keys.Back) && oldPressKey.IsKeyUp(Keys.Back))
                {
                    if(text.Length > lenght)
                        text = text.Remove(text.Length - 1);
                }
                else if (newPressKey.IsKeyDown(Keys.Enter) && oldPressKey.IsKeyUp(Keys.Enter))
                {
                    Debug.WriteLine("Dodane do rozmowy chuje!");
                }
                oldPressKey = newPressKey;
            }
            else 
                console.UpdateReverse();
        }

        public void SetupKeys()
        {
            possibleKeys = new List<Keys>();
            possibleKeys.Add(Keys.Q);
            possibleKeys.Add(Keys.W);
            possibleKeys.Add(Keys.E);
            possibleKeys.Add(Keys.R);
            possibleKeys.Add(Keys.T);
            possibleKeys.Add(Keys.Y);
            possibleKeys.Add(Keys.U);
            possibleKeys.Add(Keys.I);
            possibleKeys.Add(Keys.O);
            possibleKeys.Add(Keys.P);
            possibleKeys.Add(Keys.A);
            possibleKeys.Add(Keys.S);
            possibleKeys.Add(Keys.D);
            possibleKeys.Add(Keys.F);
            possibleKeys.Add(Keys.G);
            possibleKeys.Add(Keys.H);
            possibleKeys.Add(Keys.J);
            possibleKeys.Add(Keys.K);
            possibleKeys.Add(Keys.L);
            possibleKeys.Add(Keys.Z);
            possibleKeys.Add(Keys.X);
            possibleKeys.Add(Keys.C);
            possibleKeys.Add(Keys.V);
            possibleKeys.Add(Keys.B);
            possibleKeys.Add(Keys.N);
            possibleKeys.Add(Keys.M);
            possibleKeys.Add(Keys.I);
            
            possibleKeys.Add(Keys.OemSemicolon);
            possibleKeys.Add(Keys.OemOpenBrackets);
            possibleKeys.Add(Keys.OemCloseBrackets);
            possibleKeys.Add(Keys.OemPeriod);
            possibleKeys.Add(Keys.LeftShift);
            
            possibleKeys.Add(Keys.D0);
            possibleKeys.Add(Keys.D1);
            possibleKeys.Add(Keys.D2);
            possibleKeys.Add(Keys.D3);
            possibleKeys.Add(Keys.D4);
            possibleKeys.Add(Keys.D5);
            possibleKeys.Add(Keys.D6);
            possibleKeys.Add(Keys.D7);
            possibleKeys.Add(Keys.D8);
            possibleKeys.Add(Keys.D9);

            possibleKeys.Add(Keys.NumPad0);
            possibleKeys.Add(Keys.NumPad1);
            possibleKeys.Add(Keys.NumPad2);
            possibleKeys.Add(Keys.NumPad3);
            possibleKeys.Add(Keys.NumPad4);
            possibleKeys.Add(Keys.NumPad5);
            possibleKeys.Add(Keys.NumPad6);
            possibleKeys.Add(Keys.NumPad7);
            possibleKeys.Add(Keys.NumPad8);
            possibleKeys.Add(Keys.NumPad9);
        }

        public string ParseKey(Keys k)
        {
            switch (k)
            {
                case Keys.NumPad0: case Keys.D0: return "0"; 
                case Keys.NumPad1: case Keys.D1: return "1"; 
                case Keys.NumPad2: case Keys.D2: return "2"; 
                case Keys.NumPad3: case Keys.D3: return "3"; 
                case Keys.NumPad4: case Keys.D4: return "4"; 
                case Keys.NumPad5: case Keys.D5: return "5"; 
                case Keys.NumPad6: case Keys.D6: return "6"; 
                case Keys.NumPad7: case Keys.D7: return "7"; 
                case Keys.NumPad8: case Keys.D8: return "8"; 
                case Keys.NumPad9: case Keys.D9: return "9"; 

                case Keys.OemPeriod: return ".";
                case Keys.LeftShift: return "";
            }
            return k.ToString();
        }

        public void HideConsole()
        {
            console.UpdateReverse();
        }

        public void ShowConsole()
        {
            console.UpdateTillEnd();
        }

        public void Action()
        {
            
        }
    }
}
