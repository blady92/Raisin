using System.Collections.Generic;
using Cyber.Audio;
using Cyber.AudioEngine;
using Cyber.CGameStateEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int maxWidth = 1366;
        private int maxHeight = 768;
        private bool fullscreen = false;
        private bool mouseVisibility = true;

        private Sprite mousePointer;

        //Load Engines and Models
        private AudioModel audioModel;
        private AudioController audioController;

        //Load Menu
        private MenuLogicEngine menuLogic;
        private GameStateMainMenu mainMenu;
        private GameStatePauseMenu pauseMenu;
        private List<GameState> menus;

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
            
            //INITIALIZE AUDIO ENGINE
            audioModel = new AudioModel("standard");
            audioController = new AudioController(audioModel);
            audioController.setAudio();

            //INITIALIZE GAME STATE ENGINE
            mainMenu = new GameStateMainMenu();
            pauseMenu = new GameStatePauseMenu();
            menus = new List<GameState>();
            menus.Add(mainMenu);
            menus.Add(pauseMenu);
            menuLogic = new MenuLogicEngine(menus);
            menuLogic.SetUp();
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainMenu.LoadContent(this.Content);
            mousePointer = new Sprite(40, 40);
            mousePointer.LoadContent(this.Content, "Assets/2D/mousePointer");
        }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            audioController.runAudio();
            
            menuLogic.LogicMenu();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            mainMenu.Draw(spriteBatch);
            mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            base.Draw(gameTime);
        }
    }
}
