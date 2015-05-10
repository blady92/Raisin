using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.AudioEngine;
using Cyber.CItems;
using Cyber.CItems.CStaticItem;
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

        
        private KeyboardState oldState;
        private KeyboardState newState;
        private AudioController audio;

        public AudioController Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        //Load Models        
        private ModelTest wallModel;
        private ColliderController colliderController;
        private List<StaticItem> wallList;
        private StageParser stageParser;

        private float przesuniecie;
        Walls walls;

        //Barriers for clock manipulation
        Boolean addPushed = false;
        Boolean subPushed = false;

        //Matrix view = Matrix.CreateLookAt(new Vector3(500, 500, 700), new Vector3(5, 5, 5), Vector3.UnitZ);
        Matrix view = Matrix.CreateLookAt(new Vector3(200, 200, 0), new Vector3(0.1f, 0.1f, 0.1f), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 10000f);


        //TESTOWANE
        private StaticItem samantha;

        public void LoadContent(ContentManager theContentManager)
        {
            wallList = new List<StaticItem>();

            samantha = new StaticItem("Assets/3D/Characters/Ally_Bunker", new Vector3(20, -100, 0));
            samantha.LoadItem(theContentManager);
            samantha.Type = StaticItemType.none;

            stageParser = new StageParser();
            Stage stage = stageParser.ParseBitmap("../../../CStageParsing/stage1.bmp");
            walls = new Walls(stage);
            Debug.WriteLine("Ilość górnych ścianek to: " + walls.WallsUp.Count);

            ////Ładowanie ścianek
            for (int i = 0; i < walls.Count; i++)
            {
                wallList.Add(new StaticItem("Assets/3D/Interior/Interior_Wall_Base"));
                wallList[i].LoadItem(theContentManager);
                wallList[i].Type = StaticItemType.wall;
            }

            Debug.WriteLine("End of Loading");
        }

        public void SetUpScene()
        {
            ////Setup them position on the world at the start, then recreate cage. Order is necessary!
            
            #region Walls setups
            int i = 0;
            float mnoznikPrzesuniecaSciany;
            #endregion

            samantha.FixCollider(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));

            #region WallsUp
            mnoznikPrzesuniecaSciany = 19.5f;
            for (int j = 0; j < walls.WallsUp.Count; i++, j++)
            {
            wallList[i].Rotation = -90;
            Vector3 move = new Vector3(walls.WallsUp[j].X * mnoznikPrzesuniecaSciany,
                                        walls.WallsUp[j].Y * mnoznikPrzesuniecaSciany - 4,
                                        0.0f);
            wallList[i].Position = move;
            wallList[i].FixCollider(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5, 15f));
            }
            //Debug.WriteLine("Załadowane");

            #endregion
            #region WallsDown
            for (int j = 0; j < walls.WallsDown.Count; i++, j++)
            {
                wallList[i].Rotation = 90;
                Vector3 move = new Vector3( walls.WallsDown[j].X * mnoznikPrzesuniecaSciany, 
                                            walls.WallsDown[j].Y * mnoznikPrzesuniecaSciany + 4, 
                                            0.0f);
                wallList[i].Position = move;
                wallList[i].FixCollider(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5f, 15f));
            }
            #endregion
            #region WallsLeft
            for (int j = 0; j < walls.WallsLeft.Count; i++, j++)
            {
                wallList[i].Rotation = 180;
                Vector3 move = new Vector3( walls.WallsLeft[j].X * mnoznikPrzesuniecaSciany - 4, 
                                            walls.WallsLeft[j].Y * mnoznikPrzesuniecaSciany, 
                                            0.0f);
                wallList[i].Position = move;
                wallList[i].FixCollider(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-5f, -7f, 15f));
            }
            #endregion
            #region WallsRight
            for (int j = 0; j < walls.WallsRight.Count; i++, j++)
            {
                wallList[i].Rotation = 0;
                Vector3 move = new Vector3(walls.WallsRight[j].X * mnoznikPrzesuniecaSciany + 4, walls.WallsRight[j].Y * mnoznikPrzesuniecaSciany, 2.0f);
                wallList[i].Position = move;
                wallList[i].FixCollider(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion


            colliderController = new ColliderController(wallList);
        }


        public void SetUpClock()
        {
            //Clock clock = Clock.Instance;
            //clock.RemainingSeconds = /*4 * 60*/20;
            //clock.AddEvent(Clock.BEFOREOVER, 0, TimePassed);
            //clock.Pause();
        }

        private void TimePassed(object sender, int time)
        {
            Debug.WriteLine("TIMEOUT");
        }

        public override void Draw(GraphicsDevice device, GameTime gameTime)
        {
            view = Matrix.CreateLookAt(new Vector3(samantha.Position.X + 200,
                samantha.Position.Y + 200,
                samantha.Position.Z + 400), new Vector3(
                samantha.Position.X,
                samantha.Position.Y,
                samantha.Position.Z),
                Vector3.UnitZ);

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            
            Matrix samanthaView = Matrix.Identity * Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) *
                       Matrix.CreateTranslation(samantha.Position);
            Matrix samanthaColliderView = Matrix.CreateTranslation(samantha.ColliderInternal.Position);
            samantha.DrawItem(device, samanthaView, view, projection);
            samantha.ColliderExternal.DrawBouding(device, samanthaColliderView, view, projection);

            for (int i = 0; i < wallList.Count; i++)
            {
                //TUTEJ SIĘ MNOŻY MACIERZE W ZALEŻNOŚCI OD OBROTU
                Matrix wallView = Matrix.Identity *
                                    Matrix.CreateRotationZ(MathHelper.ToRadians(wallList[i].Rotation)) *
                                    Matrix.CreateTranslation(wallList[i].Position);
                Matrix wallColliderView = Matrix.CreateTranslation(wallList[i].ColliderInternal.Position);
                wallList[i].DrawItem(device, wallView, view, projection);
                //wallList[i].ColliderInternal.DrawBouding(device, wallColliderView, view, projection);
            }
            base.Draw(gameTime);
        }

        public override void Update()
        {
            KeyboardState newState = Keyboard.GetState();

            //colliderController.IsCollidedType();

            if (newState.IsKeyDown(Keys.T))
            {
                if (Clock.Instance.CanResume())
                {
                    Clock.Instance.Resume();
                    Debug.WriteLine("Starting clock...");
                }
                if (newState.IsKeyDown(Keys.Add))
                {
                    addPushed = true;
                }
                if (newState.IsKeyUp(Keys.Add) && addPushed)
                {
                    addPushed = false;
                    Clock.Instance.AddSeconds(60);
                    Debug.WriteLine("ADDED +1");
                }
                if (newState.IsKeyDown(Keys.Subtract))
                {
                    subPushed = true;
                }
                if (newState.IsKeyUp(Keys.Subtract) && subPushed)
                {
                    subPushed = false;
                    Clock.Instance.AddSeconds(-60);
                    Debug.WriteLine("ADDED -1");
                }
            }
            if (newState.IsKeyDown(Keys.Y))
            {
                if (Clock.Instance.CanPause())
                {
                    Clock.Instance.Pause();
                    Debug.WriteLine("Stopped clock");
                }
            }

            Vector3 move = new Vector3(0, 0, 0);
            if(newState.IsKeyDown(Keys.W))
	            move = new Vector3(0, -1f, 0);
            if(newState.IsKeyDown(Keys.S))
	            move = new Vector3(0, 1f, 0);
            if(newState.IsKeyDown(Keys.A))
	            move = new Vector3(1f, 0, 0);
            if(newState.IsKeyDown(Keys.D))
	            move = new Vector3(-1f, 0, 0);
            oldState = newState;
            samantha.ColliderExternal.RecreateCage(move);
            
            //Akcja dodatkowa do wywołania jeżeli zajdzie kolizja
            colliderController.PlayAudio = audio.Play0;
            //No i sprawdzenie czy zaszła kolizja i późniejsze 
            colliderController.CheckCollision(samantha, move);
        }

        public StaticItemType IsCollidedType()
        {
            foreach (StaticItem wall in wallList)
                if (samantha.ColliderExternal.AABB.Intersects(wall.ColliderExternal.AABB))
                    return StaticItemType.wall;
            return StaticItemType.none;
        }
    }
}
