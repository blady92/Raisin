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
        //For game size
        private int maxWidth = 1366;
        private int maxHeight = 768;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //test Animation sprite
        private SpriteAnimationDynamic sa;
        private User user;
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
            user = new User();
            menu = new MenuModel();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            user.Upadte();
            menu.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            menu.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
