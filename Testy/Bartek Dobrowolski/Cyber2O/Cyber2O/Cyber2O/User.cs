using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Cyber2O.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O
{
    class User
    {
        private KeyboardState oldState;
        private KeyboardState newState;
        private int i = 0;
        public string StateGame { get; set; }


        public void Upadte(Game1 game)
        {
            JustPress(game);
            LongPress(Keys.W, "W");
            LongPress(Keys.S, "S");
            LongPress(Keys.A, "A");
            LongPress(Keys.D, "D");
        }

        public bool JustPress(Keys key, string letter)
        {
            if(newState.IsKeyDown(key) && oldState.IsKeyUp(key))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i + ". wciśnięte E");
                return true;
            }
            oldState = newState;
            return false;
        }

        public void JustPress(Game1 game)
        {
            KeyboardState newState = Keyboard.GetState();
            Keys returnKeys = new Keys();
            //Klawisz E do np uruchamiania konsoli
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                i++;
                System.Diagnostics.Debug.WriteLine(i + ". wciśnięte E");
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
                this.StateGame = "pauseMenu";
                Debug.WriteLine("pauseMenu set");
            }
            if (newState.IsKeyUp(Keys.Escape) && oldState.IsKeyDown(Keys.Escape))
            {
                System.Diagnostics.Debug.WriteLine(i + ". puszczone Esc");
                this.StateGame = "pauseMenu";
                Debug.WriteLine("pauseMenu set");
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
                StateGame = "enter";
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
