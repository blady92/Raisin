using System.Collections.Generic;
using Cyber.Audio;
using Cyber.AudioEngine;
using Cyber.CGameStateEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNAGameConsole;
using Cyber.CConsoleEngine;

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

        //Load Engines
        private AudioModel audioModel;
        private AudioController audioController;



        //Load Menu
        private LogicEngine LogicEngine;
        private GameStateMainMenu mainMenu;
        private GameStateMainGame mainGame;
        private GameStatePauseMenu pauseMenu;
        private GameStateLoadMenu loadMenu;
        private List<GameState> menus;

        //Input Readings
        private KeyboardState oldState;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
     //   private MouseState prevMouseState;

        //Game Console
        private GameConsole console;

        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 500;

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
            loadMenu = new GameStateLoadMenu();

            mainGame.Audio = audioController;

            menus = new List<GameState>();
            menus.Add(mainMenu);
            menus.Add(mainGame);
            menus.Add(pauseMenu);
            menus.Add(loadMenu);
            #endregion INITIALIZE GAMESTATES            

            #region INITIALIZE LOGIC ENGINE
            LogicEngine = new LogicEngine(menus);
            #endregion INITIALIZE LOGIC ENGINE

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            mainMenu.LoadContent(this.Content);
            mainGame.LoadContent(this.Content);
            pauseMenu.LoadContent(this.Content);
            loadMenu.LoadContent(this.Content);
            mainGame.SetUpScene();

            mousePointer = new Sprite(40, 40);
            mousePointer.LoadContent(this.Content, "Assets/2D/mousePointer");

            #region CONSOLE
            console = new GameConsole(this, spriteBatch, ConsoleEngine.GetDefaultGameConsoleOptions(this));
            console.AddCommand(new SayHelloCommand());
            console.AddCommand(new SudoCommand());
            console.AddCommand(new AudioCommand(this, audioController));
            #endregion

        }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            UpdateInputs();

            audioController.runAudio();
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            if (LogicEngine.GetState() == GameState.States.mainGame)
            {
                LogicEngine.LogicGame();
            }
            else if (LogicEngine.GetState() == GameState.States.startMenu)
            {
                LogicEngine.LogicMenu();
            }
            else if (LogicEngine.GetState() == GameState.States.loadMenu)
            {
                LogicEngine.LogicLoadMenu(this.GraphicsDevice, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
            }
            else if (LogicEngine.GetState() == GameState.States.pauseMenu)
            {
                LogicEngine.LogicPauseMenu();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (LogicEngine.GetState() == GameState.States.startMenu)
            { 
                mainMenu.Draw(spriteBatch);
            }
            else if (LogicEngine.GetState() == GameState.States.pauseMenu)
            {
                pauseMenu.Draw(spriteBatch);
            }
            else if (LogicEngine.GetState() == GameState.States.mainGame)
            {
                mainGame.Draw(this.GraphicsDevice, gameTime);
            }
            else if (LogicEngine.GetState() == GameState.States.loadMenu)
            {
                Matrix[] transforms = loadMenu.returnModelTransforms();
                Matrix world = transforms[loadMenu.returnModelParentBoneIndex()];

                Matrix view = Matrix.CreateTranslation(0, -40, 0) *
                              Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                              Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                              Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                                  new Vector3(0, 30, 100), Vector3.Up);

                Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.GraphicsDevice.Viewport.AspectRatio, 1, 100000);


                loadMenu.Draw(this.GraphicsDevice, world, view, projection);
            }
            else if (LogicEngine.GetState() == GameState.States.exit)
            {
                Quit();
            }
            mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            base.Draw(gameTime);
        }

        private void UpdateInputs()
        {
            KeyboardState newState = Keyboard.GetState();

            if (CheckKeyPressed(ref newState, Keys.Escape))
            {
                //Debug.WriteLine(LogicEngine.GetState());
                if (LogicEngine.GetState().Equals(GameState.States.mainGame))
                {
                    //Debug.WriteLine("Changing to mainMenu");
                    LogicEngine.GameState.State = GameState.States.pauseMenu;
                }
                /*
            else if (LogicEngine.GetState().Equals(GameState.States.pauseMenu))
            {
                //Debug.WriteLine("Changing to mainGame");
                LogicEngine.GameState.State = GameState.States.mainGame;
            }
                 * */
            }

            oldState = newState;
        }

        //Checks, if current key has just been pressed
        private bool CheckKeyPressed(ref KeyboardState newState, Keys keyToCheck)
        {
            return newState.IsKeyDown(keyToCheck) && !oldState.IsKeyDown(keyToCheck);
        }

        public void Quit()
        {
            audioController.resetAudio();
            this.Exit();
        }
    }
}