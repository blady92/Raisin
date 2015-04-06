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

        //private MouseState mouseState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        ////test Animation sprite
        //private Sprite mousePointer;
        //private SpriteAnimationDynamic sa;

        //private User user;
        //private MenuState menu;
        //private PauseState pause;
        //private MainGame game;
        //private GameState gameState;

        Model model;
        Vector3 obj1 = new Vector3(0, -20, 0);
        Vector3 obj2 = new Vector3(0, 0, 0);
        Matrix view = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);

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
            //user = new User();
            //gameState = new MenuState();
            //menu = new MenuState();
            //pause = new PauseState();
            //game = new MainGame();
            //gameState = menu;
            //gameState.StateGame = "mainMenu";
            //base.Initialize();
        }

        //Oh hell... ಠ_ಠ
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("box2");

            //menu.LoadContent(this.Content);
            //pause.LoadContent(this.Content);
            //game.LoadContent(this.Content);
            //mousePointer = new Sprite(40, 40);
            //mousePointer.LoadContent(this.Content, "Assets/2D/mousePointer");
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {

            obj1 += new Vector3(0, 0.1f, 0);
            obj2 += new Vector3(0, 0.003f, 0);

            //user.Upadte(this);
            //mouseState = Mouse.GetState();
            //gameState.Update(mouseState);
            //if (gameState.StateGame == "mainMenu")
            //{
            //    gameState = menu;
            //    gameState.StateGame = "";
            //}
            //if (gameState.StateGame == "start")
            //{
            //    gameState = game;
            //    gameState.StateGame = "";
            //}
            //if (gameState.StateGame == "exit")
            //{
            //    Thread.Sleep(500);
            //    Quit();
            //}
            //if (user.StateGame == "pauseMenu")
            //{
            //    gameState = pause;
            //    gameState.StateGame = "";
            //    user.StateGame = "";
            //}
            //if (gameState.StateGame == "resume")
            //{
            //    gameState = game;
            //    gameState.StateGame = "";
            //    user.StateGame = "";
            //}
            //if (gameState.StateGame == "exitToMenu")
            //{
            //    gameState.StateGame = "mainMenu";
            //}
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix ship1WorldMatrix = Matrix.CreateTranslation(obj1);
            Matrix ship2WorldMatrix = Matrix.CreateTranslation(obj2);
            DrawModel(model, ship1WorldMatrix, view, projection);
            DrawModel(model, ship2WorldMatrix, view, projection);

            //gameState.Draw(spriteBatch);
            //mousePointer.DrawByVector(spriteBatch, Mouse.GetState());
            base.Draw(gameTime);
        }

        //Rest of functions

        public void DrawModel(Model mode, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                }
                
                mesh.Draw();
            }
        }
        public void Quit()
        {
            this.Exit();
        }
    }
}
