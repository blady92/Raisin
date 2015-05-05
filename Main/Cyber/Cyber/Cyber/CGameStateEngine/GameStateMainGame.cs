using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyber.CLogicEngine;

namespace Cyber.CGameStateEngine
{
    class GameStateMainGame : GameState
    {
        private int i = 0;
        private int angle = 0;
        private float value = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix view = Matrix.CreateLookAt(new Vector3(10, 10, 100), new Vector3(5, 5, 0), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 1000f);

        private KeyboardState oldState;
        private KeyboardState newState;

        //Load Models        
        private ModelTest samanthaModel;
        private ModelTest wallModel;
        private Collider samanthaCollider;
        private Collider wallCollider;
        private ColliderController colliderController;
        private List<ModelTest> wallList;
        private List<Collider> wallListColliders;

        //Barriers for clock manipulation
        Boolean addPushed = false;
        Boolean subPushed = false;

        public void LoadContent(ContentManager theContentManager)
        {
            wallList = new List<ModelTest>();
            wallListColliders = new List<Collider>();

            samanthaModel = new ModelTest("Assets/3D/ship");
            samanthaModel.LoadContent(theContentManager);

            samanthaCollider = new Collider();
            samanthaCollider.SetBoudings(samanthaModel.Model);
            samanthaCollider.CreateColliderBoudingBox();
            

            //Ładowanie przykładowych ścianek
            for (int i = 0; i < 4; i++)
            {
                wallList.Add(new ModelTest("Assets/3D/wall"));
                wallList[i].LoadContent(theContentManager);
                wallListColliders.Add(new Collider());
                wallListColliders[i].SetBoudings(wallList[i].Model);
                wallListColliders[i].CreateColliderBoudingBox();
            }

            //Set clock to 4 minutes
            Clock clock = Clock.Instance;
            clock.RemainingSeconds = 4 * 60;
            clock.AddEvent(Clock.AFTERSTART, 20, TimePassed);
            clock.Pause();

            Debug.WriteLine("End of Loading");
        }

        private void TimePassed(object sender, int time)
        {
            Debug.WriteLine("TIMEOUT");
        }

        public void SetUpScene()
        {
            ////Setup them position on the world at the start, then recreate cage. Order is necessary!
            //Samantha setups
            Vector3 vector = new Vector3(0, -10, 2.0f);
            samanthaModel.Position += vector;
            samanthaCollider.RecreateCage(vector);

            //Walls setups
            for (int i = 0; i < wallListColliders.Count; i++)
            {
                Vector3 move = new Vector3(0.0f, i * 7.0f, 2.0f);
                wallList[i].Position = move;
                wallListColliders[i].RecreateCage(move);
            }
        }

        public override void Draw(GraphicsDevice device)
        {
            Matrix modelView = Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(samanthaModel.Position);
            Matrix colliderView = Matrix.CreateTranslation(samanthaCollider.Position);
            
            samanthaModel.DrawModel(modelView, view, projection);
            samanthaCollider.DrawBouding(device, colliderView, view, projection);

            for (int i = 0; i < wallListColliders.Count; i++)
            {
                Matrix wallView = Matrix.CreateTranslation(wallList[i].Position);
                Matrix wallColliderView = Matrix.CreateTranslation(wallListColliders[i].Position);
                wallList[i].DrawModel(wallView, view, projection);
                wallListColliders[i].DrawBouding(device, wallColliderView, view, projection);
            }

        }

        public override void Update()
        {
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T))
            {
                if (Clock.Instance.CanResume())
                {
                    Clock.Instance.Resume();
                    Debug.WriteLine("Starting clock...");
                }
                if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Add))
                {
                    addPushed = true;
                }
                if (newState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Add) && addPushed)
                {
                    addPushed = false;
                    Clock.Instance.AddSeconds(60);
                    Debug.WriteLine("ADDED +1");
                }
                if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Subtract))
                {
                    subPushed = true;
                }
                if (newState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Subtract) && subPushed)
                {
                    subPushed = false;
                    Clock.Instance.AddSeconds(-60);
                    Debug.WriteLine("ADDED -1");
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
            {
                if (Clock.Instance.CanPause())
                {
                    Clock.Instance.Pause();
                    Debug.WriteLine("Stopped clock");
                }
            }
            //Zmiana pozycji modela do narysowania
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                i++;
                Vector3 move = new Vector3(0, -0.05f, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(0, 0.05f, 0);
                    samanthaCollider.RecreateCage(move);
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                i++;
                Vector3 move = new Vector3(0, 0.05f, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(0, -0.05f, 0);
                    samanthaCollider.RecreateCage(move);
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                i++;
                Vector3 move = new Vector3(0.05f, 0, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(-0.05f, 0, 0);
                    samanthaCollider.RecreateCage(move);
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                i++;
                Vector3 move = new Vector3(-0.05f, 0, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(0.05f, 0, 0);
                    samanthaCollider.RecreateCage(move);
                }
            }
            oldState = newState;
        }

        public void Keys()
        {

        }


        public bool IsCollided()
        {
            foreach (Collider wallCollider in wallListColliders)
                if (samanthaCollider.AABB.Intersects(wallCollider.AABB))
                    return true;
            return false;
        }
    }

}
