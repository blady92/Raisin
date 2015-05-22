using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CGameStateEngine
{
    class GameState : Game
    {
        internal enum States
        {
            startMenu,
            pauseMenu,
            mainGame,
            loadMenu,
            saveMenu,
            settingsMenu,
            exit,
            endGame,
            loadingGame
        }

        private States state;

        public States State
        {
            get { return state; }
            set { state = value; }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }
        public virtual void Draw(GraphicsDevice device) { }

        public virtual void Draw(GraphicsDevice device, GameTime gameTime) { }
        public virtual void Draw(GraphicsDevice device, Matrix world, Matrix view, Matrix projection) { }

        public virtual void Draw(GraphicsDevice device, SpriteBatch spriteBatch, GameTime gameTime) { }
        public virtual void Draw(GraphicsDevice device, SpriteBatch spriteBatch, GameTime gameTime, ContentManager theContentManager) { }
        public virtual void Draw(   GraphicsDevice device, SpriteBatch spriteBatch,
                                    GameTime gameTime, Matrix world, Matrix view, Matrix projection,
                                    ref float cameraRotation) { }

        public virtual void Draw(GraphicsDevice device, SpriteBatch spriteBatch, GameTime gameTime, Matrix world, Matrix view, Matrix projection) { }

        public virtual void Update() { }

        public virtual void Update(GameTime gameTime, KeyboardState currentKeyboardState) { }

        public virtual void Update(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance) { }
        public virtual void Update(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance, ref Vector3 cameraTarget, ref float cameraZoom) { }
    }
}
