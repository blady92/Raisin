using System;
using System.Collections.Generic;
using System.Threading;
using Cyber.Audio;
using Cyber.AudioEngine;
using Cyber.CAdditionalLibs;
using Cyber.CGameStateEngine;
using Cyber.CLogicEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNAGameConsole;
using Cyber.CConsoleEngine;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace Cyber
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static float maxWidth = 1366;
        public static float maxHeight = 768;
        private bool fullscreen = false;
        private bool mouseVisibility = false;

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
        private GameStateEndGame endGame;
        private GameStateLoadingGame loadingGame;
        private GameStateWinGame winGame;
        private List<GameState> menus;

        //Input Readings
        private KeyboardState oldState;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;

        //Game Console
        private GameConsole console;

        //Camera Parameters
        float cameraArc = 47.0f;
        float cameraRotation = -300.0f;
        float cameraDistance = 6000;
        float cameraFarBuffer = 30000;
        Vector3 cameraTarget = new Vector3(0, 0, 0);
        float cameraZoom = 2.5f;

        //Video Stuff
        Video video;
        VideoPlayer videoPlayer;
        Texture2D videoTexture;
        Rectangle videoRectangle;
        public int state = 0;

        bool BGDramaticisPlaying = false;
        bool BGisPlaying = false;
        bool exitPlayed = false;
        bool backFromMenu = false;
    

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = fullscreen;
            graphics.PreferredBackBufferWidth = (int)maxWidth;
            graphics.PreferredBackBufferHeight = (int)maxHeight;
            IsMouseVisible = mouseVisibility;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            #region INITIALIZE INPUT READINGS
            oldState = Keyboard.GetState();
            #endregion

            #region INITIALIZE AUDIO ENGINE
            audioModel = new AudioModel("CyberBank");
            audioController = new AudioController(audioModel);
            audioController.setAudio();
            #endregion

            #region INITIALIZE GAMESTATES AND MENUS
            mainMenu = new GameStateMainMenu();
            pauseMenu = new GameStatePauseMenu();
            mainGame  = new GameStateMainGame();
            mainGame.level = Level.level1;
            loadMenu = new GameStateLoadMenu();
            loadingGame = new GameStateLoadingGame();
            endGame = new GameStateEndGame();
            winGame = new GameStateWinGame();
            mainGame.Audio = audioController;

            menus = new List<GameState>();
            menus.Add(mainMenu);
            menus.Add(mainGame);
            menus.Add(pauseMenu);
            menus.Add(loadMenu);
            menus.Add(loadingGame);
            menus.Add(endGame);
            menus.Add(winGame);
            #endregion INITIALIZE GAMESTATES            

            #region INITIALIZE LOGIC ENGINE
            LogicEngine = new LogicEngine(menus, this.Content, this.GraphicsDevice, this);
            #endregion INITIALIZE LOGIC ENGINE

            #region INITIALIZE VIDEO CUTSCENE
            videoPlayer = new VideoPlayer();
            #endregion

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            mainMenu.LoadContent(this.Content);
            mainGame.LoadContent(this.Content, this.GraphicsDevice);
            pauseMenu.LoadContent(this.Content);
            loadMenu.LoadContent(this.Content);
            loadingGame.LoadContent(this.Content);
            endGame.LoadContent(this.Content);
            winGame.LoadContent(this.Content);
            mousePointer = new Sprite(40, 40);
            mousePointer.LoadContent(this.Content, "Assets/2D/mousePointer");

            #region VIDEO
            video = Content.Load<Video>("Assets/Video/intro_mm");
            videoRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            if (state == 0)
            videoPlayer.Play(video);
            videoPlayer.Pause();
         
            #endregion

            #region CONSOLE
            console = new GameConsole(this, spriteBatch, DeveloperConsoleEngine.GetDefaultGameConsoleOptions(this));
            console.AddCommand(new SayHelloCommand());
            console.AddCommand(new SudoCommand());
            console.AddCommand(new AudioCommand(this, audioController));
            console.AddCommand(new PositonCommand(mainGame));
            #endregion

        }

        protected override void UnloadContent()
        {
            audioController.BGMusicDramaticController("Stop");
            audioController.cinematicExitController("Stop");
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            UpdateInputs();

            switch(state)
            {
                case 0:
                    if (videoPlayer.State == MediaState.Stopped)
                        state = 1;    
                    break;
                case 1:
                    break;
            }

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            if (LogicEngine.GetState() == GameState.States.mainGame)
            {
                videoPlayer.Resume();
                if (currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    state = 1;
                }

                if(state == 1)
                {
                    videoPlayer.Pause();
                    LogicEngine.LogicGame(this.GraphicsDevice, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance, ref cameraTarget, ref cameraZoom);
                }

            }
            else if (LogicEngine.GetState() == GameState.States.winGame)
            {
                audioController.BGMusicDramaticController("Pause");
                if (!exitPlayed)
                {
                    audioController.cinematicExitController("Play");
                    exitPlayed = true;
                }
               
                LogicEngine.LogicWinGame();
            }
            else if (LogicEngine.GetState() == GameState.States.startMenu)
            {
                LogicEngine.LogicMenu();
            }
            else if (LogicEngine.GetState() == GameState.States.loadMenu)
            {
                LogicEngine.LogicLoadMenu(this.GraphicsDevice, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
            }
            else if (LogicEngine.GetState() == GameState.States.loadingGame)
            {
                loadingGame.Draw(spriteBatch);
                mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
                LogicEngine.LogicChangeLevel(this.Content, this.GraphicsDevice);
                Thread.Sleep(1500);
            }
            else if (LogicEngine.GetState() == GameState.States.pauseMenu)
            {
                audioController.BGMusicDramaticController("Pause");
                LogicEngine.LogicPauseMenu();
            }
            else if (LogicEngine.GetState() == GameState.States.endGame)
            {
                audioController.walkingController("Pause");
                LogicEngine.LogicEndGame(this.GraphicsDevice, this.Content);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            // TODO: Add your drawing code here
            #region rysowanie menu g��wnego
            if (LogicEngine.GetState() == GameState.States.startMenu)
            {
              
                mainMenu.Draw(spriteBatch);
                mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
                if(!BGisPlaying)
                {
                    audioController.BGMusicController("Play");
                    BGisPlaying = true;
                }
              
            }
            #endregion
            #region rysowanie menu pauzy
            else if (LogicEngine.GetState() == GameState.States.pauseMenu)
            {
               
                    audioController.BGMusicController("Resume");
                
                
                pauseMenu.Draw(spriteBatch); 
                mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            }
            #endregion
            #region rysowanie g��wnej gry
            else if (LogicEngine.GetState() == GameState.States.mainGame)
            {
                backFromMenu = true;
                audioController.BGMusicController("Pause");
                
                videoTexture = videoPlayer.GetTexture();
                spriteBatch.Begin();
                switch (state)
                {
                    case 0:
                        spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
                        break;
                    case 1:
                        GraphicsDevice.Clear(Color.Black);
                        videoPlayer.IsMuted = true;
                        if (!BGDramaticisPlaying)
                        {
                            audioController.BGMusicDramaticController("Play");
                            BGDramaticisPlaying = true;
                        }
                        if(backFromMenu)
                        {
                            audioController.BGMusicDramaticController("Resume");
                            audioController.BGMusicController("Pause");
                        }
                        
                        break;
                }

                spriteBatch.End();

                if(state == 1)
                {
                    mainGame.LookAtSam(ref cameraTarget);
                    mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
                    Vector3 cameraPosition = new Vector3(0, -14.1759f, -cameraDistance);
                    Vector3 cameraUpVector = Vector3.Up;

                    Matrix world = Matrix.Identity;

                    Matrix view = 
                                  Matrix.CreateTranslation(-mainGame.returnSamanthaPosition().X, -mainGame.returnSamanthaPosition().Y, 0) *
                                 // Matrix.CreateTranslation(0, 0, 0) *
                                  Matrix.CreateRotationZ(MathHelper.ToRadians(cameraRotation)) *
                                  Matrix.CreateRotationY(MathHelper.ToRadians(-180.0f)) *
                                  Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                                  Matrix.CreateLookAt(cameraPosition, new Vector3(0,0,0), cameraUpVector) *
                                  Matrix.CreateScale(cameraZoom, cameraZoom, 1.0f);
                
                    Matrix projection = Matrix.CreateOrthographic(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 1, cameraFarBuffer);
                
                    mainGame.Draw(this.GraphicsDevice, this.spriteBatch, gameTime, world, view, projection, ref cameraRotation);
                }
            }
            #endregion
            #region rysowanie okna �adowania poziomu
            else if (LogicEngine.GetState() == GameState.States.loadingGame)
            {
                Debug.WriteLine("By�em tutaj!");
                loadingGame.Draw(spriteBatch);
                //mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            }
            #endregion
            #region rysowanie planszy powt�rki poziomu w przypadku przegranej
            else if (LogicEngine.GetState() == GameState.States.endGame)
            {
                endGame.Draw(spriteBatch);
                mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            }
            #endregion
            else if (LogicEngine.GetState() == GameState.States.winGame)
            {
                winGame.Draw(spriteBatch);
            }
            #region rysowanie menu �adowania poziom�w
            else if (LogicEngine.GetState() == GameState.States.loadMenu)
            {
                mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            }
            #endregion
            #region koniec wszystkiego
            else if (LogicEngine.GetState() == GameState.States.exit)
            {
                Quit();
            }
            #endregion
            base.Draw(gameTime);    
        }

        private void UpdateInputs()
        {
            KeyboardState newState = Keyboard.GetState();

            //if (newState.IsKeyDown(Keys.O))
            //{
            //    cameraArc += 0.1f;
            //    Debug.WriteLine("Camera Arc is: " + cameraArc);
            //}
            //if (newState.IsKeyDown(Keys.I))
            //{
            //    cameraArc -= 0.1f;
            //    Debug.WriteLine("Camera Arc is: " + cameraArc);
            //}

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
            audioController.BGMusicDramaticController("Stop");
            audioController.resetAudio();
            this.Exit();
        }
    }
}