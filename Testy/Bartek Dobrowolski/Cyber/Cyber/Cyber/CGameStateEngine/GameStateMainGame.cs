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

namespace Cyber.CGameStateEngine
{
    class GameStateMainGame : GameState
    {
        private int i = 0;
        private int angle = 0;
        private float value = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 60), new Vector3(10, 10, 0), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), 800f / 600f, 0.1f, 1000f);
        
        //Load Models        
        private ModelTest samanthaModel;
        private ModelTest wallModel;
        private Collider samanthaCollider;
        private Collider wallCollider;
        private ColliderController colliderController;
        private List<ModelTest> wallList;
        private List<Collider> wallListColliders;

        private KeyboardState oldState, newState;

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
            for (int i = 0; i < 1; i++)
            {
                wallList.Add(new ModelTest("Assets/3D/wall"));
                wallListColliders.Add(new Collider());
                wallList[i].LoadContent(theContentManager);
                wallListColliders[i].SetBoudings(wallList[i].Model);
                wallListColliders[i].CreateColliderBoudingBox();
            }

            Debug.WriteLine("End of Loading");
        }

        //No to ustawiamy ten szajs, ale równo w rządku, by nikt dwa razy wpierdolunie dostał :v
        public void SetUpScene()
        {
            ////Setup them position on the world at the start, then recreate cage. Order is necessary!
            //Samantha setups

            //Co te kolidery manianę odpierdalajo...
            Vector3 vector = new Vector3(0, -20, 2.0f);
            samanthaModel.Position += vector;
            samanthaCollider.RecreateCage(vector);
             
            /*Kochany dzienniczku.
             * Kolider zadziałał.
             * Mam nadzieję, że nikt nie zobaczy, że Samantha na razie jest statkiem kosmicznym, lawl :v
             */

            //Walls setups
            for (int i = 0; i < wallListColliders.Count; i++)
            {
                Vector3 move = new Vector3(0.0f, i * 10.0f, 2.0f);
                wallListColliders[i].RecreateCage(move);
                wallList[i].Position += move;
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
                Matrix wallView = Matrix.CreateTranslation(wallListColliders[i].Position);
                Matrix wallColliderView = Matrix.CreateTranslation(wallListColliders[i].Position);
                wallList[i].DrawModel(wallView, view, projection);
                wallListColliders[i].DrawBouding(device, wallColliderView, view, projection);
            }
        }

        //CZAS NA UPA, MOTHERF...
        public override void Update()
        {
            //Poruszanie Samanthą widoku z góry.
            newState = Keyboard.GetState();
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
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
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
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
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
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
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
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
