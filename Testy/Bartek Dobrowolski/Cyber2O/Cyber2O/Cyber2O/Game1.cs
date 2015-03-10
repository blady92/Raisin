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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private int maxWidth = 1336;
        private int maxHeight = 668;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //Elementy menu
        //private Sprite menuBackground;
        //private Sprite menuStart;
        //private Sprite menuSettings;
        //private Sprite menuQuit;

        private MenuModel menu;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = maxWidth;
            graphics.PreferredBackBufferHeight = maxHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //menu = new MenuModel();

            menu = new MenuModel();
            menu.menuInitialize();

            //menuBackground = new Sprite();
            //menuStart = new Sprite();
            //menuSettings = new Sprite();
            //menuQuit = new Sprite();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //za³adowanie tekstury do zmiennej, która potem bêdzie renderowana
            //String path = "Assets/2D/";
            //menuBackground.LoadContent(this.Content, path+"test");
            //menuStart.LoadContent(this.Content, path+"start");
            //menuSettings.LoadContent(this.Content, path+"settings");
            //menuQuit.LoadContent(this.Content, path+"quit");

            //menuBackground.Position = new Vector2(0,0);
            //menuStart.Position = new Vector2(136,36);
            //menuSettings.Position = new Vector2(136,72);
            //menuQuit.Position = new Vector2(136, 108);

            menu.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            //menuBackground.Draw(this.spriteBatch);
            //menuStart.Draw(this.spriteBatch);
            //menuSettings.Draw(this.spriteBatch);
            //menuQuit.Draw(this.spriteBatch);
            menu.Draw(this.spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
