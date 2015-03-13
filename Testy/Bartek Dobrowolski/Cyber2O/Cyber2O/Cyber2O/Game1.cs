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
        private int maxWidth = 1336;
        private int maxHeight = 468;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //test Animation sprite
        private SpriteAnimationDynamic sa;
        private User user;
        
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D saTexture = Content.Load<Texture2D>("Assets/2D/startAnimation");
            sa = new SpriteAnimationDynamic(saTexture, 2, 1, new Vector2(200,200));
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            user.Upadte();
            //MouseState mouseState;
            //mouseState = Mouse.GetState();
            //System.Diagnostics.Debug.WriteLine("Mouse points are: (" + mouseState.X + ", " + mouseState.Y + ")");
            //if (new Rectangle(mouseState.X, mouseState.Y, 40, 40).Intersects(sa.GetRectangle()))
            //{
            //    System.Diagnostics.Debug.WriteLine("Intersected");
            //    sa.Update();
            //}
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sa.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
