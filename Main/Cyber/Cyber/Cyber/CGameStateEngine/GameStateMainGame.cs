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

        //2D elements
        private ConsoleSprites console;
        private Icon iconOverHead;

        //3D elements
        private StaticItem samantha;
        //private StaticItem terminal;
        private ColliderController colliderController;
        private List<StaticItem> stageElements;
        private List<StaticItem> npcList;
        private StageParser stageParser;
        private Stage stage;

        private float przesuniecie;
        StageStructure stageStructure;

        //Barriers for clock manipulation
        Boolean addPushed = false;
        Boolean subPushed = false;

        //Matrix view = Matrix.CreateLookAt(new Vector3(500, 500, 700), new Vector3(5, 5, 5), Vector3.UnitZ);
        Matrix view = Matrix.CreateLookAt(new Vector3(200, 200, 0), new Vector3(0.1f, 0.1f, 0.1f), Vector3.UnitZ);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 10000f);


        //TESTOWANE
        private bool loaded = false;

        public void LoadContent(ContentManager theContentManager, GraphicsDevice device)
        {
            #region Load 2D elements
            console = new ConsoleSprites();
            console.LoadContent(theContentManager);

            //UWAZAC NA WYMIARY OKNA
            //(1366 - 32) / 2, 768 / 2 - 120, StaticIcon.none
            iconOverHead = new Icon((device.Viewport.Width - 32) / 2, device.Viewport.Height / 2 - 120, StaticIcon.none);
            iconOverHead.LoadContent(theContentManager);
            #endregion

            #region Load 3D elements
            samantha = new StaticItem("Assets/3D/Characters/Ally_Bunker");
            samantha.LoadItem(theContentManager);
            samantha.Type = StaticItemType.none;

            stageElements = new List<StaticItem>();
            npcList = new List<StaticItem>();

            stageParser = new StageParser();
            stage = stageParser.ParseBitmap("../../../CStageParsing/stage3.bmp");
            stageStructure = new StageStructure(stage, StageStructureGenerationStrategy.GENEROUS);

            foreach (StageObject stageObj in stage.Objects)
            {
                StaticItem item = new StaticItem(stageObj.StaticObjectAsset);
                item.LoadItem(theContentManager);
                if (stageObj is Terminal)
                {
                    item.Type = StaticItemType.terminal;
                }
                else
                {
                    item.Type = StaticItemType.decoration;
                }
                stageElements.Add(item);
            }

            #endregion

            #region NPCs
            foreach (StageNPC stageNPC in stage.NPCs)
            {
                NPC npc = new NPC(stageNPC.StaticObjectAsset);
                npc.LoadItem(theContentManager);
                npc.Type = StaticItemType.tank;
                npcList.Add(npc);
                AI.Instance.AddRobot(npc);
            }

            Debug.WriteLine("Ilość narożników to: " + stageStructure.ConcaveCorners.Count + " lub " + stageStructure.ConvexCorners.Count);
            #endregion
            #region Ładowanie ścian
            for (int i = 0; i < stageStructure.Walls.Count; i++)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Wall_Base");
                item.LoadItem(theContentManager);
                item.Type = StaticItemType.wall;
                stageElements.Add(item);
            }
            for (int i = 0; i < stageStructure.ConcaveCorners.Count; i++)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Wall_Concave");
                item.LoadItem(theContentManager);
                item.Type = StaticItemType.wall;
                stageElements.Add(item);
            }
            for (int i = 0; i < stageStructure.ConvexCorners.Count; i++)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Wall_Convex");
                item.LoadItem(theContentManager);
                item.Type = StaticItemType.wall;
                stageElements.Add(item);
            }
            #endregion
            #region Ładowanie podłóg
            foreach (Pair<int, int> point in stageStructure.Floor.Floors)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Floor");
                item.LoadItem(theContentManager);
                item.Type = StaticItemType.none; // TODO: dodać typ floor Dobrotek: Dodane
                stageElements.Add(item);
            }
            #endregion
            Debug.WriteLine("End of Loading");
        }

        public void LookAtSam(ref Vector3 cameraTarget)
        {
            cameraTarget.X = stage.PlayerPosition.X;
            cameraTarget.Y = stage.PlayerPosition.Y;
        }

        public void SetUpScene()
        {
            ////Setup them position on the world at the start, then recreate cage. Order is necessary!
            #region setups
            int i = 0;
            float mnoznikPrzesuniecaSciany = 19.5f;
            float mnoznikPrzesunieciaOther = mnoznikPrzesuniecaSciany;
            float terminalZ = 50.0f;
            float objectZ = 0.0f;
            float wallOffset = 9.75f;
            float cornerOffset = 5.5f;
            #endregion

            
            samantha.Position = new Vector3(stage.PlayerPosition.X * mnoznikPrzesunieciaOther, 
                                            stage.PlayerPosition.Y * mnoznikPrzesunieciaOther,
                                            0.0f);
            samantha.FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));

            //terminal.Position = new Vector3(20, -100, 39);
            //terminal.FixColliderExternal(new Vector3(1.5f, 1.5f, 1.5f), new Vector3(15f, 20f, 20f));
            //terminal.FixColliderInternal(new Vector3(0.75f, 0.75f, 0.75f), new Vector3(10, 10, 0));
            //terminal.FixColliderInternal(new Vector3(0.75f, 0.75f, 0.75f), new Vector3(20f, 20f, 0f));

            
            #region Objects
            for (int j = 0; j < stage.Objects.Count; i++, j++)
            {
                float z;
                Vector3 move = new Vector3();
                if (stage.Objects[j] is Terminal)
                {
                    z = terminalZ;
                    move = new Vector3(stage.Objects[j].GetBlock().X * mnoznikPrzesunieciaOther,
                                        stage.Objects[j].GetBlock().Y * mnoznikPrzesunieciaOther,
                                        z);
                    stageElements[i].Position = move;
                    stageElements[i].FixColliderExternal(new Vector3(1.5f, 1.5f, 1.5f), new Vector3(15f, 20f, 20f));
                    stageElements[i].FixColliderInternal(new Vector3(0.75f, 0.75f, 0.75f), new Vector3(10, 10, 0));
                }
                else
                {
                    z = objectZ;
                    move = new Vector3(stage.Objects[j].GetBlock().X * mnoznikPrzesunieciaOther,
                                            stage.Objects[j].GetBlock().Y * mnoznikPrzesunieciaOther,
                                            z);
                    stageElements[i].Position = move;
                    stageElements[i].Rotation = stage.Objects[j].Rotation;
                }
            }
            #endregion
            #region NPCs
            for (int j = 0; j < stage.NPCs.Count; j++)
            {
                Vector3 move = new Vector3(stage.NPCs[j].GetBlock().X * mnoznikPrzesunieciaOther,
                                        stage.NPCs[j].GetBlock().Y * mnoznikPrzesunieciaOther,
                                        0.0f);
                npcList[j].Position = move;
                npcList[j].Rotation = stage.NPCs[j].Rotation;
                npcList[j].FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
                npcList[j].FixColliderExternal(new Vector3(2,2,2), new Vector3(-15f, -15f, 10f));
            }

            #endregion
            #region WallsUp
            for (int j = 0; j < stageStructure.Walls.WallsUp.Count; i++, j++)
            {
            stageElements[i].Rotation = -90;
            Vector3 move = new Vector3(stageStructure.Walls.WallsUp[j].X * mnoznikPrzesuniecaSciany,
                                        stageStructure.Walls.WallsUp[j].Y * mnoznikPrzesuniecaSciany - wallOffset,
                                        0.0f);
            stageElements[i].Position = move;
            stageElements[i].FixColliderInternal(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5, 15f));
            }

            #endregion
            #region WallsDown
            for (int j = 0; j < stageStructure.Walls.WallsDown.Count; i++, j++)
            {
                stageElements[i].Rotation = 90;
                Vector3 move = new Vector3(stageStructure.Walls.WallsDown[j].X * mnoznikPrzesuniecaSciany,
                                            stageStructure.Walls.WallsDown[j].Y * mnoznikPrzesuniecaSciany + wallOffset, 
                                            0.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5f, 15f));
            }
            #endregion
            #region WallsLeft
            for (int j = 0; j < stageStructure.Walls.WallsLeft.Count; i++, j++)
            {
                stageElements[i].Rotation = 180;
                Vector3 move = new Vector3(stageStructure.Walls.WallsLeft[j].X * mnoznikPrzesuniecaSciany - wallOffset,
                                            stageStructure.Walls.WallsLeft[j].Y * mnoznikPrzesuniecaSciany, 
                                            0.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-5f, -7f, 15f));
            }
            #endregion
            #region WallsRight
            for (int j = 0; j < stageStructure.Walls.WallsRight.Count; i++, j++)
            {
                stageElements[i].Rotation = 0;
                Vector3 move = new Vector3(stageStructure.Walls.WallsRight[j].X * mnoznikPrzesuniecaSciany + wallOffset,
                                            stageStructure.Walls.WallsRight[j].Y * mnoznikPrzesuniecaSciany,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConcaveCornersLowerLeft
            for (int j = 0; j < stageStructure.ConcaveCorners.ConcaveCornersLowerLeft.Count; i++, j++ )
            {
                stageElements[i].Rotation = 180;
                Vector3 move = new Vector3(stageStructure.ConcaveCorners.ConcaveCornersLowerLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            stageStructure.ConcaveCorners.ConcaveCornersLowerLeft[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConcaveCornersLowerRight
            for (int j = 0; j < stageStructure.ConcaveCorners.ConcaveCornersLowerRight.Count; i++, j++)
            {
                stageElements[i].Rotation = 90;
                Vector3 move = new Vector3(stageStructure.ConcaveCorners.ConcaveCornersLowerRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            stageStructure.ConcaveCorners.ConcaveCornersLowerRight[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConcaveCornersUpperLeft
            for (int j = 0; j < stageStructure.ConcaveCorners.ConcaveCornersUpperLeft.Count; i++, j++)
            {
                stageElements[i].Rotation = 270;
                Vector3 move = new Vector3(stageStructure.ConcaveCorners.ConcaveCornersUpperLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            stageStructure.ConcaveCorners.ConcaveCornersUpperLeft[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConcaveCornersUpperRight
            for (int j = 0; j < stageStructure.ConcaveCorners.ConcaveCornersUpperRight.Count; i++, j++)
            {
                stageElements[i].Rotation = 0;
                Vector3 move = new Vector3(stageStructure.ConcaveCorners.ConcaveCornersUpperRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            stageStructure.ConcaveCorners.ConcaveCornersUpperRight[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConvexCornersLowerLeft
            for (int j = 0; j < stageStructure.ConvexCorners.ConvexCornersLowerLeft.Count; i++, j++)
            {
                stageElements[i].Rotation = 180;
                Vector3 move = new Vector3(stageStructure.ConvexCorners.ConvexCornersLowerLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            stageStructure.ConvexCorners.ConvexCornersLowerLeft[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConvexCornersLowerRight
            for (int j = 0; j < stageStructure.ConvexCorners.ConvexCornersLowerRight.Count; i++, j++)
            {
                stageElements[i].Rotation = 90;
                Vector3 move = new Vector3(stageStructure.ConvexCorners.ConvexCornersLowerRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            stageStructure.ConvexCorners.ConvexCornersLowerRight[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConvexCornersUpperLeft
            for (int j = 0; j < stageStructure.ConvexCorners.ConvexCornersUpperLeft.Count; i++, j++)
            {
                stageElements[i].Rotation = 270;
                Vector3 move = new Vector3(stageStructure.ConvexCorners.ConvexCornersUpperLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            stageStructure.ConvexCorners.ConvexCornersUpperLeft[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion
            #region ConvexCornersUpperRight
            for (int j = 0; j < stageStructure.ConvexCorners.ConvexCornersUpperRight.Count; i++, j++)
            {
                stageElements[i].Rotation = 0;
                Vector3 move = new Vector3(stageStructure.ConvexCorners.ConvexCornersUpperRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            stageStructure.ConvexCorners.ConvexCornersUpperRight[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                stageElements[i].Position = move;
                stageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
            }
            #endregion

            #region Floor setups
            float mnoznikPrzesunieciaPodlogi = mnoznikPrzesuniecaSciany;
            for (int j = 0; j < stageStructure.Floor.Count; i++, j++)
            {
                Vector3 move = new Vector3(stageStructure.Floor.Floors[j].X * mnoznikPrzesunieciaPodlogi,
                                            stageStructure.Floor.Floors[j].Y * mnoznikPrzesunieciaPodlogi,
                                            0.0f);
                stageElements[i].Position = move;
                //stageSurroundingsList[i].FixColliderInternal(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5, 15f));
            }
            #endregion

            //stageSurroundingsList.Add(terminal);
            colliderController = new ColliderController(console, iconOverHead);
            colliderController.staticItemList = stageElements;
            colliderController.npcItem = npcList;
            colliderController.samantha = samantha;

            #region Inicjalizacja AI
            AI ai = AI.Instance;
            ai.ColliderController = colliderController;
            #endregion
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

        public override void Draw(GraphicsDevice device, SpriteBatch spriteBatch, GameTime gameTime, Matrix world, Matrix view, Matrix projection)
        {
            //view = Matrix.CreateLookAt(new Vector3(samantha.Position.X + 200,
            //    samantha.Position.Y + 200,
            //    samantha.Position.Z + 400), new Vector3(
            //    samantha.Position.X,
            //    samantha.Position.Y,
            //    samantha.Position.Z),
            //    Vector3.UnitZ);

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            

            Matrix samanthaView = Matrix.Identity * Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) *
                       Matrix.CreateTranslation(samantha.Position);
            
            Matrix samanthaColliderView = Matrix.CreateTranslation(samantha.ColliderInternal.Position);
            samantha.DrawItem(device, samanthaView, view, projection);
            samantha.ColliderInternal.DrawBouding(device, samanthaColliderView, view, projection);

            //Matrix terminalView = Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) *
            //Matrix.CreateTranslation(terminal.Position);
            //Matrix terminalColliderView = Matrix.CreateTranslation(terminal.ColliderExternal.Position);
            //Matrix terminalColliderInternallView = Matrix.CreateTranslation(terminal.ColliderInternal.Position);

            //terminal.DrawItem(device, terminalView, view, projection);
            //terminal.ColliderExternal.DrawBouding(device, terminalColliderView, view, projection);
            //terminal.ColliderInternal.DrawBouding(device, terminalColliderInternallView, view, projection);

            //Przyda się do testowania pojedynczych elementów, ale foreach coś wydaje się być wydajniejszy, dunno why O.o

            //for (int i = 0; i < 4; i++)
            //{
            //    //TUTEJ SIĘ MNOŻY MACIERZE W ZALEŻNOŚCI OD OBROTU
            //    Matrix stageElementView = Matrix.Identity *
            //                        Matrix.CreateRotationZ(MathHelper.ToRadians(stageElements[i].Rotation)) *
            //                        Matrix.CreateTranslation(stageElements[i].Position);
            //    Matrix stageElementColliderView = Matrix.CreateTranslation(stageElements[i].ColliderInternal.Position);
            //    stageElements[i].DrawItem(device, stageElementView, view, projection);
            //    stageElements[i].ColliderExternal.DrawBouding(device, stageElementColliderView, view, projection);
            //    stageElements[i].ColliderInternal.DrawBouding(device, stageElementColliderView, view, projection);
            //}

            foreach (StaticItem stageElement in stageElements)
            {
                Matrix stageElementView = Matrix.Identity *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(stageElement.Rotation)) *
                    Matrix.CreateTranslation(stageElement.Position);
                stageElement.DrawItem(device, stageElementView, view, projection);

                //Matrix stageElementColliderView = Matrix.CreateTranslation(stageElement.ColliderInternal.Position);
                //stageElements[i].ColliderExternal.DrawBouding(device, stageElementColliderView, view, projection);
                //stageElements[i].ColliderInternal.DrawBouding(device, stageElementColliderView, view, projection);
            }
            foreach (StaticItem item in npcList)
            {
                Matrix stageElementView = Matrix.Identity *
                                          Matrix.CreateRotationZ(MathHelper.ToRadians(item.Rotation)) *
                                          Matrix.CreateTranslation(item.Position);
                item.DrawItem(device, stageElementView, view, projection);

                Matrix stageElementColliderView = Matrix.CreateTranslation(item.ColliderInternal.Position);
                item.ColliderExternal.DrawBouding(device, stageElementColliderView, view, projection);
                item.ColliderInternal.DrawBouding(device, stageElementColliderView, view, projection);
            }

            iconOverHead.Draw(spriteBatch);
            console.Draw(spriteBatch);
            base.Draw(gameTime);
        }


        public override void Update(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance, ref Vector3 cameraTarget)
        {
            console.Update();
            KeyboardState newState = currentKeyboardState;

            //Kuba edit:
            samantha.SkinnedModel.UpdateCamera(device, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
      
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
            colliderController.PlayAudio = audio.Play0;
            if (!console.IsUsed)
            {
                if (newState.IsKeyDown(Keys.W)) { 
                    move = new Vector3(0, 1f, 0);
                    colliderController.CheckCollision(samantha, move);
                    //Debug.WriteLine("Sam X: " + samantha.Position.X);
                    //Debug.WriteLine("Sam Y: " + samantha.Position.Y);
                    //Debug.WriteLine("Sam Z: " + samantha.Position.Z);
                    cameraTarget.Y = samantha.Position.Y;
                }
                if (newState.IsKeyDown(Keys.S)) { 
	                move = new Vector3(0, -1f, 0);
                    colliderController.CheckCollision(samantha, move);
                    //Debug.WriteLine("Sam X: " + samantha.Position.X);
                    //Debug.WriteLine("Sam Y: " + samantha.Position.Y);
                    //Debug.WriteLine("Sam Z: " + samantha.Position.Z);
                    cameraTarget.Y = samantha.Position.Y;
                }
                if (newState.IsKeyDown(Keys.A)) { 
                    move = new Vector3(-1f, 0, 0);
                    colliderController.CheckCollision(samantha, move);
                    //Debug.WriteLine("Sam X: " + samantha.Position.X);
                    //Debug.WriteLine("Sam Y: " + samantha.Position.Y);
                    //Debug.WriteLine("Sam Z: " + samantha.Position.Z);
                    cameraTarget.X = -samantha.Position.X;
                }
                if (newState.IsKeyDown(Keys.D))
                {

                    move = new Vector3(1f, 0, 0);
                    colliderController.CheckCollision(samantha, move);
                    //Debug.WriteLine("Sam X: " + samantha.Position.X);
                    //Debug.WriteLine("Sam Y: " + samantha.Position.Y);
                    //Debug.WriteLine("Sam Z: " + samantha.Position.Z);
                    cameraTarget.X = -samantha.Position.X;
                }
            }
            else
            {
                console.Action();
            }

            colliderController.CallTerminalAfterCollision(samantha);
            if (colliderController.EnemyCollision(samantha))
            {
                Debug.WriteLine("Sam zlokalizowana w "+samantha.Position.ToString());
                AI.Instance.AlertOthers(samantha);
            }
            /*else
            {
                Debug.WriteLine("Uff jestem bezpieczna");
            }*/
            console.Update();
            oldState = newState;

            AI.Instance.MoveNPCs(null);
        }
    }
}
