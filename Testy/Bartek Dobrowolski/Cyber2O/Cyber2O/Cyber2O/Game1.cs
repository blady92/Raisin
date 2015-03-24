using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyber2O
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //For game size and screens
        private int maxWidth = 1366;
        private int maxHeight = 768;
        private bool fullscreen = false;
        private bool mouseVisibility = true;
        private int cx, cy, x, y;

        private MouseState mouseState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //test Animation sprite
        private Sprite mousePointer;
        private SpriteAnimationDynamic sa;

        private User user;
        private MenuModel menu;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = fullscreen;
            graphics.PreferredBackBufferWidth = maxWidth;
            graphics.PreferredBackBufferHeight = maxHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Inicjalizacja klawiszy
            user = new User();
            menu = new MenuModel();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu.LoadContent(this.Content);
            mousePointer = new Sprite(40, 40);
            mousePointer.LoadContent(this.Content, "Assets/2D/mousePointer");
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            user.Upadte(this);
            mouseState = Mouse.GetState();
            menu.Update(mouseState);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            menu.Draw(spriteBatch);
            mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            base.Draw(gameTime);
        }

        //Rest of functions
        public void Quit()
        {
            this.Exit();
        }
    }
}
