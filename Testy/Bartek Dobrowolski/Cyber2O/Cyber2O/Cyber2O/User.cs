using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O
{
    class User
    {
        private KeyboardState oldState;
        private KeyboardState newState;
        private int i = 0;
        
        public void Upadte()
        {
            //newState = Keyboard.GetState();  
            //if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            //{
            //    System.Diagnostics.Debug.WriteLine("wciśnięte");
            //}
            //if (newState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            //{
            //    System.Diagnostics.Debug.WriteLine("puszczone");
            //}
            //if (newState.IsKeyDown(Keys.Space))
            //{
            //    i++;
            //    System.Diagnostics.Debug.WriteLine(i);
            //}
            //if (newState.IsKeyUp(Keys.Space))
            //{
            //    System.Diagnostics.Debug.WriteLine("Puszczone");
            //}
            //oldState = newState; 
            //LongPress(Keys.W, "W");
            //LongPress(Keys.S, "S");
            //LongPress(Keys.A, "A");
            //LongPress(Keys.D, "D");
            JustPress();
            LongPress(Keys.W, "W");
            LongPress(Keys.S, "S");
            LongPress(Keys.A, "A");
            LongPress(Keys.D, "D");
        }

        public void JustPress()
        {
            KeyboardState newState = Keyboard.GetState();

            //Klawisz E do np uruchamiania konsoli
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i+". wciśnięte E");
            }
            if (newState.IsKeyUp(Keys.E) && oldState.IsKeyDown(Keys.E))
            {
                System.Diagnostics.Debug.WriteLine(i+". puszczone E");
            }

            //Klawisz Escape
            if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i + ". wciśnięte Esc");
            }
            if (newState.IsKeyUp(Keys.Escape) && oldState.IsKeyDown(Keys.Escape))
            {
                System.Diagnostics.Debug.WriteLine(i + ". puszczone Esc");
            }

            //Klawisz Enter
            if (newState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i + ". wciśnięte Enter");
            }
            if (newState.IsKeyUp(Keys.Enter) && oldState.IsKeyDown(Keys.Enter))
            {
                System.Diagnostics.Debug.WriteLine(i + ". puszczone Enter");
            }
            oldState = newState;            
        }
        public void LongPress(Keys key, string letter)
        {
            KeyboardState newState = Keyboard.GetState();  
            if (newState.IsKeyDown(key))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i+". wciśnięte " + letter);
            }
            if (newState.IsKeyUp(key))
            {
                //System.Diagnostics.Debug.WriteLine("puszczone " + letter);
            }
            oldState = newState;
        }
    }
}
