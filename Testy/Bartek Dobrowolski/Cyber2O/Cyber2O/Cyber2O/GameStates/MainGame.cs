using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O.GameStates
{
    class MainGame : GameState
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;

        private Sprite gameBackground;
        private int i = 0;
        private int angle = 0;
        private float value = 0;

        Matrix view = Matrix.CreateLookAt(new Vector3(40, 20, 50), new Vector3(10, 10, 0), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 1000f);

        private ModelTest2 model;
        private Cage modelCage, wallCage;
        private List<ModelTest2> WallList;
        private List<Cage> wallListCage;

        private bool floorCollide, wallCollide;
        private KeyboardState oldState;
        private KeyboardState newState;
        private bool upDir, downDir, leftDir, rightDir;

        public MainGame(GraphicsDevice device)
        {
            this.device = device;
        }
        //No to lecim z tematem, będzie zajebiście (I hope so!)
        //Bo jak nie, to wszystko pójdzie w diabły (╯°□°)╯︵ ┻━┻
        //a tego nie chcemy 
        //┬─┬﻿ ノ( ゜-゜ノ)
        public override void LoadContent(ContentManager theContentManager)
        {
            //Load whole 3D/Music Stuffs   
            WallList = new List<ModelTest2>();
            wallListCage = new List<Cage>();
            //Setup models and its cages here
            model = new ModelTest2("Assets/3D/shipX");
            model.LoadContent(theContentManager);
            modelCage = new Cage();
            modelCage.SetBoudings(model.Model);
            modelCage.CreateCage();
            modelCage.BoudingBoxResizeOnce(0.5f, 0.5f, 1);
            modelCage.MoveBoundingBox(new Vector3(0, 0, 1.0f));

            for (int i = 0; i < 3; i++)
            {
                WallList.Add(new ModelTest2("Assets/3D/wallX"));
            }

            for (int i = 0; i < WallList.Count; i++)
            {
                WallList[i].LoadContent(theContentManager);
                wallListCage.Add(new Cage());
                wallListCage[i].SetBoudings(WallList[i].Model);
                wallListCage[i].CreateCage();
                wallListCage[i].RecreateCage(new Vector3(0, 0, 2.0f));
            }

            //Setup them position on the world at the start, then recreate cage. Order is necessary!
            //Ship setups
            Vector3 shipVec = new Vector3(0, -10, 0);
            model.Position += shipVec;
            modelCage.RecreateCage(shipVec);

            //Walls setups
            for (int i = 0; i < WallList.Count; i++)
            {
                Vector3 move = new Vector3(0.0f, i * 2f, 0.0f);
                wallListCage[i].RecreateCage(move);
                WallList[i].Position = move;
            }

            upDir = downDir = leftDir = rightDir = true;
        }

        public override void Update(MouseState mouseState)
        {
            //all methods for update state of basic game
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.W))
            {
                i++;
                Vector3 move = new Vector3(0, -0.05f, 0);
                modelCage.RecreateCage(move);
                if (!IsCollided())
                {
                    model.Position += move;
                }
                else
                {
                    move = new Vector3(0, 0.05f, 0);
                    modelCage.RecreateCage(move);
                }
            }
            if (newState.IsKeyDown(Keys.S))
            {
                i++;
                Vector3 move = new Vector3(0, 0.05f, 0);
                modelCage.RecreateCage(move);
                if (!IsCollided())
                {
                    model.Position += move;
                }
                else
                {
                    move = new Vector3(0, -0.05f, 0);
                    modelCage.RecreateCage(move);
                }
            }
            if (newState.IsKeyDown(Keys.A))
            {
                i++;
                Vector3 move = new Vector3(0.05f, 0, 0);
                modelCage.RecreateCage(move);
                if (!IsCollided())
                {
                    model.Position += move;
                }
                else
                {
                    move = new Vector3(-0.05f, 0, 0);
                    modelCage.RecreateCage(move);
                }
            }
            if (newState.IsKeyDown(Keys.D))
            {
                i++;
                Vector3 move = new Vector3(-0.05f, 0, 0);
                modelCage.RecreateCage(move);
                if (!IsCollided())
                {
                    model.Position += move;
                }
                else
                {
                    move = new Vector3(0.05f, 0, 0);
                    modelCage.RecreateCage(move);
                }
            }
            oldState = newState;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Matrix modelView = Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(model.Position);
            Matrix cageView = Matrix.CreateTranslation(modelCage.Position);
            model.DrawModel(modelView, view, projection);
            modelCage.DrawBouding(device, cageView, view, projection);

            for (int i = 0; i < WallList.Count; i++)
            {
                Matrix model2View = Matrix.CreateTranslation(WallList[i].Position);
                Matrix cageView2 = Matrix.CreateTranslation(wallListCage[i].Position);
                WallList[i].DrawModel(model2View, view, projection);
                wallListCage[i].DrawBouding(device, cageView2, view, projection);
            }
        }

        public bool IsCollided()
        {
            foreach (Cage wallCage in wallListCage)
                if (modelCage.AABB.Intersects(wallCage.AABB))
                {
                    Debug.WriteLine("Collided!");
                    return true;
                }
            return false;
        }
    }
}
