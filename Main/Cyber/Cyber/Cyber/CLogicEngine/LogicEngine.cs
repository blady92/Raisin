using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Cyber.CLogicEngine;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CGameStateEngine
{
    class LogicEngine : Game
    {
        private GameState gameState;
        private GameStateMainMenu gameStateMainMenu;
        private GameStateMainGame gameStateMainGame;
        private GameStateLoadMenu gameStateLoadMenu;
        private GameStatePauseMenu gameStatePauseMenu;
        private GameStateLoadingGame gameStateLoadingMenu;
        private GameStateEndGame gameStateEndGame;

        private ContentManager theContentManager;
        private GraphicsDevice device;
        private List<GameState> menus;
        private KeyboardState oldState;
        private KeyboardState currentKeyboardState;
        public bool endGame { get; set; }

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public LogicEngine(List<GameState> menus, ContentManager theContentManager, GraphicsDevice device)
        {
            this.theContentManager = theContentManager;
            this.device = device;
            this.menus = menus;
            gameState = new GameState();
            gameStateMainMenu = (GameStateMainMenu)menus[0];
            gameStateMainGame = (GameStateMainGame)menus[1];
            gameStatePauseMenu = (GameStatePauseMenu)menus[2];
            gameStateLoadMenu = (GameStateLoadMenu)menus[3];
            gameStateLoadingMenu = (GameStateLoadingGame)menus[4];
            gameStateEndGame = (GameStateEndGame)menus[5];
        }

        #region MAIN MENU LOGIC
        public void LogicMenu()
        {
            endGame = false;
            MouseState mouse;
            mouse = Mouse.GetState();
            MouseState oldMouseState = new MouseState();
            for (int i = 0; i < gameStateMainMenu.SpriteAnimationList.Length; i++)
            {
                if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(gameStateMainMenu.SpriteAnimationList[i].GetFrameRectangle()))
                {
                    if (gameStateMainMenu.SpriteAnimationList[i].Clicked)
                    {
                        gameStateMainMenu.SpriteAnimationList[i].UpdateClickFrame();
                        if (gameStateMainMenu.SpriteAnimationList[i].ClickCurrentFrameAccessor == gameStateMainMenu.SpriteAnimationList[i].ClickTextureList.Length - 1)
                        {
                            switch (i)
                            {
                                case 0:
                                    gameStateMainGame.SetUpClock();
                                    gameStateMainGame.SetUpScene(device);
                                    gameState.State = GameState.States.mainGame;
                                    break;
                                case 1:
                                    gameStateMainGame.SetUpClock();
                                    gameStateMainGame.SetUpScene(device);
                                    gameState.State = GameState.States.mainGame;

                                    //GameSerializer ser = new JSONSerializer();
                                    //DataContainer data = ser.Deserialize("state.json");
                                    //data.Apply(gameStateMainGame);
                                    //gameState.State = GameState.States.loadMenu;
                                    break;
                                case 2:
                                //    base.StateGame = "settings";
                                //    break;
                                //gameState.State = GameState.States.startMenu;
                                //break;                                
                                case 3:
                                    gameState.State = GameState.States.exit;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        gameStateMainMenu.SpriteAnimationList[i].UpdateAnimation();
                    }
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {
                        gameStateMainMenu.SpriteAnimationList[i].UpdateClickAnimation(true);
                    }
                }
                else
                {
                    gameStateMainMenu.SpriteAnimationList[i].UpdateReverse();
                    gameStateMainMenu.SpriteAnimationList[i].ResetClickFrame();
                    gameStateMainMenu.SpriteAnimationList[i].UpdateClickAnimation(false);
                }
            }
        }
        #endregion
        #region PAUSE MENU LOGIC

        public void LogicPauseMenu()
        {
            MouseState mouse;
            mouse = Mouse.GetState();
            //Debug.WriteLine(mouse.ToString());
            MouseState oldMouseState = new MouseState();
            for (int i = 0; i < gameStatePauseMenu.SpriteAnimationList.Length; i++)
            {
                if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(gameStatePauseMenu.SpriteAnimationList[i].GetFrameRectangle()))
                {
                    if (gameStatePauseMenu.SpriteAnimationList[i].Clicked)
                    {
                        gameStatePauseMenu.SpriteAnimationList[i].UpdateClickFrame();
                        if (gameStatePauseMenu.SpriteAnimationList[i].ClickCurrentFrameAccessor == gameStatePauseMenu.SpriteAnimationList[i].ClickTextureList.Length - 1)
                        {
                            switch (i)
                            {
                                case 0:
                                    gameState.State = GameState.States.mainGame;
                                    break;
                                case 1:
                                    //save game
                                    //DataContainer state = new DataContainer(gameStateMainGame);
                                    //GameSerializer serializer = new JSONSerializer();
                                    //serializer.Serialize(state, "state.json");
                                    break;
                                case 2:
                                //    base.StateGame = "settings";
                                //    break;
                                //gameState.State = GameState.States.startMenu;
                                //break;                                
                                case 3:
                                    Clock.Instance.Destroy();
                                    AI.Destroy();
                                    Thread.Sleep(300);
                                    gameState.State = GameState.States.startMenu;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        gameStatePauseMenu.SpriteAnimationList[i].UpdateAnimation();
                    }
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {
                        gameStatePauseMenu.SpriteAnimationList[i].UpdateClickAnimation(true);
                    }
                }
                else
                {
                    gameStatePauseMenu.SpriteAnimationList[i].UpdateReverse();
                    gameStatePauseMenu.SpriteAnimationList[i].ResetClickFrame();
                    gameStatePauseMenu.SpriteAnimationList[i].UpdateClickAnimation(false);
                }
            }
        }
        #endregion
        #region LOAD MENU LOGIC

        public void LogicLoadMenu(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance)
        {
            gameStateLoadMenu.Update(device, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
        }
        #endregion
        #region GAME LOGIC
        public void LogicGame(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance, ref Vector3 cameraTarget, ref float cameraZoom)
        {
            gameStateMainGame.Update(device, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance, ref cameraTarget, ref cameraZoom);
            currentKeyboardState = Keyboard.GetState();
            if (endGame)
            {
                GameState.State = GameState.States.endGame;
            }
            if (gameStateMainGame.escaped)
            {
                if (gameStateMainGame.level == Level.level1)
                {
                    gameStateMainGame.level = Level.level2;
                }
                else
                {
                    gameStateMainGame.level = Level.level1;
                }
            }
            oldState = currentKeyboardState;
        }

        public void LogicChangeLevel(ContentManager theContentManager, GraphicsDevice device)
        {
            //if (gameStateMainGame.level == Level.level1)
            //{
            //    gameStateMainGame.level = Level.level2;
            //}
            //else
            //{
            //    gameStateMainGame.level = Level.level1;
            //}
            GameState.State = GameState.States.loadingGame;
            Thread.Sleep(200);
            gameStateMainGame.LoadContent(theContentManager, device);
            gameStateMainGame.SetUpScene(device);
            GameState.State = GameState.States.mainGame;
        }

        #endregion
        #region END GAME
        //Przygotować klikanie czy user chce zacząć poziom od nowa, czy wrócić do menu i zakończyć zabawę
        public void LogicEndGame(GraphicsDevice device, ContentManager theContentManager)
        {
            MouseState mouse;
            mouse = Mouse.GetState();
            //Debug.WriteLine(mouse.ToString());
            MouseState oldMouseState = new MouseState();
            for (int i = 0; i < gameStateEndGame.SpriteAnimationList.Length; i++)
            {
                if (new Rectangle(mouse.X, mouse.Y, 1, 1).Intersects(gameStateEndGame.SpriteAnimationList[i].GetFrameRectangle()))
                {
                    gameStateEndGame.SpriteAnimationList[i].UpdateAnimation();
                    if (mouse.LeftButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
                    {
                        switch (i)
                        {
                            case 0:
                                endGame = false;
                                gameState.State = GameState.States.loadingGame;
                                break;
                            case 1:
                                gameState.State = GameState.States.startMenu;
                                break;
                        }
                    }
                }
                else
                {
                    gameStateEndGame.SpriteAnimationList[i].UpdateReverse();
                }
            }
        }
        #endregion
        public GameState.States GetState()
        {
            return gameState.State;
        }
    }
}