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
            KeyboardState newState = Keyboard.GetState();  
            //if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            //{
            //    System.Diagnostics.Debug.WriteLine("wciśnięte");
            //}
            //if (newState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            //{
            //    System.Diagnostics.Debug.WriteLine("puszczone");
            //}
            if (newState.IsKeyDown(Keys.Space))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i);
            }
            if (newState.IsKeyUp(Keys.Space))
            {
                System.Diagnostics.Debug.WriteLine("Puszczone");
            }
            oldState = newState; 
        }

        public bool PressW()
        {
            return oldState.IsKeyDown(Keys.W);
        }

        public bool PressS()
        {
            return oldState.IsKeyDown(Keys.S);
        }

        public bool PressA()
        {
            return oldState.IsKeyDown(Keys.A);
        }

        public bool PressD()
        {
            return oldState.IsKeyDown(Keys.D);
        }

    }
}
