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
        private int maxWidth = 1336;
        private int maxHeight = 668;
        private SpriteAnimation sa;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        private MenuModel menu;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = maxWidth;
            graphics.PreferredBackBufferHeight = maxHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            menu = new MenuModel();
            menu.menuInitialize();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //menu.LoadContent(this.Content);
            Texture2D saTexture = Content.Load<Texture2D>("Assets/2D/startAnimation");
            sa = new SpriteAnimation(saTexture, 2, 1);
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            sa.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //spriteBatch.Begin();
            //menu.Draw(this.spriteBatch);
            //spriteBatch.End();
            sa.Draw(spriteBatch, new Vector2(200, 200));
            
            base.Draw(gameTime);
        }
    }
}
