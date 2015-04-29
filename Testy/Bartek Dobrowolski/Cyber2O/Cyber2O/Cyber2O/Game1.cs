using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cyber2O.GameStates;
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

        private MouseState mouseState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        ////test Animation sprite
        private Sprite mousePointer;
        private SpriteAnimationDynamic sa;

        private User user;
        private MenuState menu;
        private PauseState pause;
        private MainGame game;
        private GameState gameState;

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
            gameState = new MenuState();
            menu = new MenuState();
            pause = new PauseState();
            game = new MainGame(this.GraphicsDevice);
            gameState = menu;
            gameState.StateGame = "mainMenu";
            base.Initialize();
        }

        //Oh hell... ಠ_ಠ
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menu.LoadContent(this.Content);
            pause.LoadContent(this.Content);
            game.LoadContent(this.Content);
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
            gameState.Update(mouseState);
            if (gameState.StateGame != "game")
            {
                if (gameState.StateGame == "mainMenu")
                {
                    gameState = menu;
                    gameState.StateGame = "";
                }
                if (gameState.StateGame == "start")
                {
                    gameState = game;
                    gameState.StateGame = "";
                }
                if (gameState.StateGame == "exit")
                {
                    Thread.Sleep(500);
                    Quit();
                }
                if (user.StateGame == "pauseMenu")
                {
                    gameState = pause;
                    gameState.StateGame = "";
                    user.StateGame = "";
                }
                if (gameState.StateGame == "resume")
                {
                    gameState = game;
                    gameState.StateGame = "";
                    user.StateGame = "";
                }
                if (gameState.StateGame == "exitToMenu")
                {
                    gameState.StateGame = "mainMenu";
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            gameState.Draw(spriteBatch);
            mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            base.Draw(gameTime);
        }

        public void Quit()
        {
            this.Exit();
        }
    }
}
