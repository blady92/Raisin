using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyber2O
{
    class AudioTest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Audio audio;
        private KeyboardState newState;
        private KeyboardState oldState;

        public AudioTest()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            audio = new Audio();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            audio.LoadContent(this.Content, "Assets/Music/kawalek");
            audio.IsRepeat(true);
        }
        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.P) && oldState.IsKeyUp(Keys.P))
            {
                Debug.WriteLine("Playing audio");
                audio.AudioPlay();
            }
            if (newState.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                Debug.WriteLine("Stopping audio");
                audio.AudioStop();
            }
            if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                Debug.WriteLine("Pause audio");
                audio.AudioPause();;
            }
            if (newState.IsKeyDown(Keys.R) && oldState.IsKeyUp(Keys.R))
            {
                Debug.WriteLine("Resume audio");
                audio.AudioResume();
            }
            if (newState.IsKeyDown(Keys.Q) && oldState.IsKeyUp(Keys.Q))
            {
                audio.VolumeDown();
            }
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                audio.VolumeUp();
            }
            oldState = newState;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }


    }
}
