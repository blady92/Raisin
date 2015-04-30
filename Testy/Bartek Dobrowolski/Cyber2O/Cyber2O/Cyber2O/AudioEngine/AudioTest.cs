using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber2O.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyber2O
{
    class AudioTest : Game
    {
        SpriteBatch spriteBatch;

        private AudioModel audio;
        private KeyboardState newState;
        private KeyboardState oldState;

        public AudioTest()
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            audio = new AudioModel();
            audio.AudioEngineName = "standard";
            base.Initialize();
        }
        protected override void LoadContent()
        { }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.P) && oldState.IsKeyUp(Keys.P))
            {
                Debug.WriteLine("Playing audio");
                audio.SoundBank.GetCue("vrag").Play();
            }
            if (newState.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                Debug.WriteLine("Playing audio 2");
                audio.SoundBank.GetCue("pierwaja").Play();
            }
            oldState = newState;
        }
    }
}
