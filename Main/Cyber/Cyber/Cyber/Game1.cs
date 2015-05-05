using System.Collections.Generic;
using System.Diagnostics;
using Cyber.Audio;
using Cyber.AudioEngine;
using Cyber.CGameStateEngine;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Cyber
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ContentManager contentManager;

        private int maxWidth = 1366;
        private int maxHeight = 768;
        private bool fullscreen = false;
        private bool mouseVisibility = true;

        private Sprite mousePointer;

        //Load Engines
        private AudioModel audioModel;
        private AudioController audioController;



        //Load Menu
        private LogicEngine LogicEngine;
        private GameStateMainMenu mainMenu;
        private GameStateMainGame mainGame;
        private GameStatePauseMenu pauseMenu;
        private List<GameState> menus;

        //Input Readings
        private KeyboardState oldState;

        //Animation & Skinning Data
        private SkinningAnimation skinningAndAnimationLoader;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = fullscreen;
            graphics.PreferredBackBufferWidth = maxWidth;
            graphics.PreferredBackBufferHeight = maxHeight;
            IsMouseVisible = mouseVisibility;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            #region INITIALIZE INPUT READINGS
            oldState = Keyboard.GetState();
            #endregion

            #region INITIALIZE AUDIO ENGINE
            audioModel = new AudioModel("standard");
            audioController = new AudioController(audioModel);
            audioController.setAudio();
            #endregion

            #region INITIALIZE GAMESTATES AND MENUS
            mainMenu = new GameStateMainMenu();
            pauseMenu = new GameStatePauseMenu();
            mainGame  = new GameStateMainGame();
            
            menus = new List<GameState>();
            menus.Add(mainMenu);
            menus.Add(mainGame);
            menus.Add(pauseMenu);
            #endregion INITIALIZE GAMESTATES            

            #region INITIALIZE LOGIC ENGINE
            LogicEngine = new LogicEngine(menus);
            #endregion INITIALIZE LOGIC ENGINE

            #region INITIALIZE ANIMATION & SKINNING DATA
            skinningAndAnimationLoader = new SkinningAnimation();

          
            #endregion

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            mainMenu.LoadContent(this.Content);
            mainGame.LoadContent(this.Content);
            mainGame.SetUpScene();

            mousePointer = new Sprite(40, 40);
            mousePointer.LoadContent(this.Content, "Assets/2D/mousePointer");

            //test ³adowania modelu statycznego
            skinningAndAnimationLoader.LoadContent_StaticModel(contentManager, "Assets//3D//Interior//oxygen_generator");

            //test ³adowania modelu skinned z animacj¹ -> II parametr to: model, III parametr to: nazwa animacji do odpalenia
            skinningAndAnimationLoader.LoadContent_SkinnedModel(contentManager, "Assets//3D//Characters//dude", "Take 001");

            //test ³adowania w³asnego Shadera
            skinningAndAnimationLoader.LoadContent_ShaderEffect(contentManager, "Assets//ShadersFX//TestEffect");

            //test ³adowania tekstury (2048x2048)
            skinningAndAnimationLoader.LoadContent_Texture(contentManager, "Assets//3D//Characters//grungie");
            
        }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            UpdateInputs();

            audioController.runAudio();

            if (LogicEngine.GetState() == GameState.States.mainGame)
            {
                LogicEngine.LogicGame();
            }
            else if (LogicEngine.GetState() == GameState.States.startMenu)
            {
                LogicEngine.LogicMenu();
            }

            base.Update(gameTime);
        }

        private void UpdateInputs()
        {
            KeyboardState newState = Keyboard.GetState();

            if (CheckKeyPressed(ref newState, Keys.Escape))
            {
                Debug.WriteLine(LogicEngine.GetState());
                if (LogicEngine.GetState().Equals(GameState.States.mainGame))
                {
                    //Debug.WriteLine("Changing to mainMenu");
                    LogicEngine.GameState.State = GameState.States.startMenu;
                }
                else
                {
                    //Debug.WriteLine("Changing to mainGame");
                    LogicEngine.GameState.State = GameState.States.mainGame;
                }
            }

            oldState = newState;
        }

        //Checks, if current key has just been pressed
        private bool CheckKeyPressed(ref KeyboardState newState, Keys keyToCheck)
        {
            return newState.IsKeyDown(keyToCheck) && !oldState.IsKeyDown(keyToCheck);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (LogicEngine.GetState() == GameState.States.startMenu)
            { 
                mainMenu.Draw(spriteBatch);
            }
            else if (LogicEngine.GetState() == GameState.States.mainGame)
            {
                mainGame.Draw(this.GraphicsDevice);
            }
            else if (LogicEngine.GetState() == GameState.States.exit)
            {
                Quit();
            }
            mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            base.Draw(gameTime);
        }

        public void Quit()
        {
            this.Exit();
        }
    }
}
