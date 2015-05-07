using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.AudioEngine;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyber.CLogicEngine;
using Cyber.CStageParsing;

namespace Cyber.CGameStateEngine
{
    class GameStateMainGame : GameState
    {
        private int i = 0;
        private int angle = 0;
        private float value = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Matrix view = Matrix.CreateLookAt(new Vector3(500, 500, 700), new Vector3(5, 5, 5), Vector3.UnitZ);
        Matrix view = Matrix.CreateLookAt(new Vector3(200, 200, 400), new Vector3(0.1f, 0.1f, 0.1f), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 10000f);

        private KeyboardState oldState;
        private KeyboardState newState;
        private AudioController audio;

        public AudioController Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        //Load Models        
        private ModelTest samanthaModel;
        private ModelTest wallModel;
        private Collider samanthaCollider;
        private Collider wallCollider;
        private ColliderController colliderController;
        private List<ModelTest> wallList;
        private List<Collider> wallListColliders;
        private StageParser stageParser;
        private float przesuniecie;
        Walls walls;

        //Barriers for clock manipulation
        Boolean addPushed = false;
        Boolean subPushed = false;

        public void LoadContent(ContentManager theContentManager)
        {
            wallList = new List<ModelTest>();
            wallListColliders = new List<Collider>();

            samanthaModel = new ModelTest("Assets/3D/Characters/Ally_Bunker");
            samanthaModel.LoadContent(theContentManager);

            samanthaCollider = new Collider();
            samanthaCollider.SetBoudings(samanthaModel.Model);
            samanthaCollider.CreateColliderBoudingBox();
            samanthaCollider.MoveBoundingBox(new Vector3(-15f, -15f, 0f));

            stageParser = new StageParser();
            Stage stage = stageParser.ParseBitmap("../../../CStageParsing/stage2.bmp");
            walls = new Walls(stage);
            Debug.WriteLine("Ilość górnych ścianek to: "+walls.WallsUp.Count);

            //Ładowanie przykładowych ścianek
            for (int i = 0; i < walls.Count; i++)
            {
                wallList.Add(new ModelTest("Assets/3D/Interior/Interior_Wall_Base"));
                wallList[i].LoadContent(theContentManager);
                wallListColliders.Add(new Collider());
            }

            Debug.WriteLine("End of Loading");
        }

        public void SetUpScene()
        {
            ////Setup them position on the world at the start, then recreate cage. Order is necessary!
            //Samantha setups
            przesuniecie = 0;
            Vector3 vector = new Vector3(0, -100, 0.0f);
            samanthaModel.Position += vector;
            samanthaCollider.BoudingBoxResizeOnce(0.5f, 0.5f, 1f);
            samanthaCollider.RecreateCage(vector);
            
            #region Walls setups
            int i = 0;
            float mnoznikPrzesuniecaSciany;

            #region WallsUP
            mnoznikPrzesuniecaSciany = 19.5f;
            for (int j = 0; j < walls.WallsUp.Count; i++, j++)
            {
                Debug.WriteLine(walls.WallsUp.Count);
                wallList[i].RotationAngle = -90;
                Vector3 move = new Vector3(walls.WallsUp[j].X * mnoznikPrzesuniecaSciany, walls.WallsUp[j].Y * mnoznikPrzesuniecaSciany - 4, 0.0f);
                //Vector3 move = new Vector3(walls.WallsUp[j].X * 4, walls.WallsUp[j].Y * 4 - 4, 2.0f);
                wallList[i].Position = move;
                wallListColliders[i].SetBoudings(wallList[i].Model);
                wallListColliders[i].CreateColliderBoudingBox();
                wallListColliders[i].BoudingBoxResizeOnce(0.2f, 0.2f, 1.4f);
                wallListColliders[i].MoveBoundingBox(new Vector3(-7, -2, 15f));
                wallListColliders[i].RecreateCage(move);
            }

            #endregion
            #region WallsDown
            for (int j = 0; j < walls.WallsDown.Count; i++, j++)
            {
                wallList[i].RotationAngle = 90;
                Vector3 move = new Vector3(walls.WallsDown[j].X * mnoznikPrzesuniecaSciany, walls.WallsDown[j].Y * mnoznikPrzesuniecaSciany + 4, 2.0f);
                wallList[i].Position = move;
                wallListColliders[i].SetBoudings(wallList[i].Model);
                wallListColliders[i].CreateColliderBoudingBox();
                wallListColliders[i].BoudingBoxResizeOnce(0.2f, 0.2f, 1.4f);
                wallListColliders[i].MoveBoundingBox(new Vector3(-7, -2, 15f));
                wallListColliders[i].RecreateCage(move);
            }

            #endregion
            #region WallsLeft
            for (int j = 0; j < walls.WallsLeft.Count; i++, j++)
            {
                wallList[i].RotationAngle = 180;
                Vector3 move = new Vector3(walls.WallsLeft[j].X * mnoznikPrzesuniecaSciany - 4, walls.WallsLeft[j].Y * mnoznikPrzesuniecaSciany, 2.0f);
                wallList[i].Position = move;
                wallListColliders[i].SetBoudings(wallList[i].Model);
                wallListColliders[i].CreateColliderBoudingBox();
                wallListColliders[i].BoudingBoxResizeOnce(0.2f, 0.2f, 1.4f);
                wallListColliders[i].MoveBoundingBox(new Vector3(-7, -2, 15f));
                wallListColliders[i].RecreateCage(move);
            }
            #endregion
            #region WallsRight
            for (int j = 0; j < walls.WallsRight.Count; i++, j++)
            {
                wallList[i].RotationAngle = 0;
                Vector3 move = new Vector3(walls.WallsRight[j].X * mnoznikPrzesuniecaSciany + 4, walls.WallsRight[j].Y * mnoznikPrzesuniecaSciany, 2.0f);
                wallList[i].Position = move;
                wallListColliders[i].SetBoudings(wallList[i].Model);
                wallListColliders[i].CreateColliderBoudingBox();
                wallListColliders[i].BoudingBoxResizeOnce(0.2f, 0.2f, 1.4f);
                wallListColliders[i].MoveBoundingBox(new Vector3(-7, -2, 15f));
                wallListColliders[i].RecreateCage(move);
            }
            #endregion
            #endregion
        }


        public void SetUpClock()
        {
            Clock clock = Clock.Instance;
            clock.RemainingSeconds = /*4 * 60*/20;
            clock.AddEvent(Clock.BEFOREOVER, 0, TimePassed);
            clock.Pause();
        }

        private void TimePassed(object sender, int time)
        {
            Debug.WriteLine("TIMEOUT");
        }

        public override void Draw(GraphicsDevice device, GameTime gameTime)
        {
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            Matrix modelView = Matrix.Identity * Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(samanthaModel.Position);
            Matrix colliderView = Matrix.CreateTranslation(samanthaCollider.Position);
            
            samanthaModel.DrawModel(modelView, view, projection);
            //samanthaCollider.DrawBouding(device, colliderView, view, projection);

            for (int i = 0; i < wallListColliders.Count; i++)
            {
                //TUTEJ SIĘ MNOŻY MACIERZE W ZALEŻNOŚCI OD OBROTU
                Matrix wallView =   Matrix.Identity * 
                                    Matrix.CreateRotationZ(MathHelper.ToRadians(wallList[i].RotationAngle)) *
                                    Matrix.CreateTranslation(wallList[i].Position);
                Matrix wallColliderView = Matrix.CreateTranslation(wallListColliders[i].Position);
                wallList[i].DrawModel(wallView, view, projection);
                //wallListColliders[i].DrawBouding(device, wallColliderView, view, projection);
            }
            base.Draw(gameTime);
        }

        public override void Update()
        {
            Debug.WriteLine("Mnożnik to: " + przesuniecie);
            KeyboardState newState = Keyboard.GetState();

            #region Do testowania przesunięcia
            //if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            //{
            //    przesuniecie += 0.1f;
            //    //mnoznikPrzesuniecaSciany += 0.1f;
            //    for (int j = 0; j < walls.WallsUp.Count; i++, j++)
            //    {
            //        Vector3 move = new Vector3(walls.WallsUp[j].X * mnoznikPrzesuniecaSciany, walls.WallsUp[j].Y * mnoznikPrzesuniecaSciany - 4, 0.0f);
            //        //Vector3 move = new Vector3(walls.WallsUp[j].X * 4, walls.WallsUp[j].Y * 4 - 4, 2.0f);
            //        wallList[j].Position = move;
            //        wallListColliders[j].SetBoudings(wallList[j].Model);
            //        wallListColliders[j].CreateColliderBoudingBox();
            //        wallListColliders[j].BoudingBoxResizeOnce(0.2f, 0.2f, przesuniecie);
            //        //wallListColliders[i].MoveBoundingBox(move - new Vector3(80, -18, 0));
            //        //wallListColliders[j].MoveBoundingBox(new Vector3(0, 0, przesuniecie));
            //        wallListColliders[j].RecreateCage(move);
            //    }
            //}
            //if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            //{
            //    przesuniecie -= 0.1f;
            //    for (int j = 0; j < walls.WallsUp.Count; i++, j++)
            //    {
            //        Vector3 move = new Vector3(walls.WallsUp[j].X * mnoznikPrzesuniecaSciany, walls.WallsUp[j].Y * mnoznikPrzesuniecaSciany - 4, 0.0f);
            //        //Vector3 move = new Vector3(walls.WallsUp[j].X * 4, walls.WallsUp[j].Y * 4 - 4, 2.0f);
            //        wallList[j].Position = move;
            //        wallListColliders[j].SetBoudings(wallList[j].Model);
            //        wallListColliders[j].CreateColliderBoudingBox();
            //        wallListColliders[j].BoudingBoxResizeOnce(0.2f, 0.2f, przesuniecie);
            //        //wallListColliders[i].MoveBoundingBox(move - new Vector3(80, -18, 0));
            //        //wallListColliders[j].MoveBoundingBox(new Vector3(0, 0, przesuniecie));
            //        wallListColliders[j].RecreateCage(move);
            //    }
            //}
            #endregion

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
                Vector3 move = new Vector3(0, -1f, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(0, 1f, 0);
                    samanthaCollider.RecreateCage(move);
                    audio.playAudio(1);
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                i++;
                Vector3 move = new Vector3(0, 1f, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(0, -1f, 0);
                    samanthaCollider.RecreateCage(move);
                    audio.playAudio(1);
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                i++;
                Vector3 move = new Vector3(1f, 0, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(-1f, 0, 0);
                    samanthaCollider.RecreateCage(move);
                    audio.playAudio(1);
                }
            }
            if (newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                i++;
                Vector3 move = new Vector3(-1f, 0, 0);
                samanthaCollider.RecreateCage(move);
                if (!IsCollided())
                {
                    samanthaModel.Position += move;
                }
                else
                {
                    move = new Vector3(1f, 0, 0);
                    samanthaCollider.RecreateCage(move);
                    audio.playAudio(1);
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
