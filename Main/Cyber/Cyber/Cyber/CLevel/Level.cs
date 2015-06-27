using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.CAdditionalLibs;
using Cyber.CGameStateEngine;
using Cyber.CItems.CStaticItem;
using Cyber.CLogicEngine;
using Cyber.CStageParsing;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CLevel
{
    public abstract class LevelStage
    {
        public Level level { get; set; }
        public List<StaticItem> StageElements { get; set; }
        public StageParser StageParser { get; set; }
        public Stage Stage { get; set; }
        public StageStructure StageStructure { get; set; }
        public ContentManager TheContentManager { get; set; }
        public StaticItem SamanthaGhostController { get; set; }
        public List<StaticItem> ConnectedColliders { get; set; }
        public List<StaticItem> npcList { get; set; }
        public StaticItem escapeCollider { get; set; }
        public ParticleEmitter escapeemitter { get; set; }
        public ParticleEmitter generatorParticles { get; set; }
        public StaticItem podjazd { get; set; }
        protected IDGenerator generatedID;
        protected GameStateMainGame GameStateMainGame;

        public LevelStage(GameStateMainGame game)
        {
            GameStateMainGame = game;
            generatedID = new IDGenerator();
            generatedID.GenerateID();
            StageElements = new List<StaticItem>();
            StageParser = new StageParser();
            ConnectedColliders = new List<StaticItem>();
            npcList = new List<StaticItem>();
            SamanthaGhostController = game.samanthaGhostController;
        }

        #region Initials
        protected int i = 0;
        protected float mnoznikPrzesuniecaSciany = 19.5f;
        protected float terminalZ = 50.0f;
        protected float objectZ = 0.0f;
        protected float wallOffset = 9.75f;
        protected float cornerOffset = 5.5f;
        #endregion

        /// <summary>
        /// stage = stageParser.ParseBitmap(...);
        /// </summary>
        public abstract void ParseStage();

        public virtual void LoadObjects()
        {
            foreach (StageObject stageObj in Stage.Objects)
            {
                StaticItem item = new StaticItem(stageObj.StaticObjectAsset);
                item.LoadItem(TheContentManager);
                if (stageObj is Terminal)
                {
                    item.Type = StaticItemType.terminal;
                }
                else if (stageObj is Column)
                {
                    item.Type = StaticItemType.column;
                }
                else
                {
                    item.Type = StaticItemType.decoration;
                }
                StageElements.Add(item);
            }
        }

        public void LoadContent(GraphicsDevice device)
        {
            TheContentManager = GameStateMainGame.theContentManager;
            ParseStage();
            StageStructure = new StageStructure(Stage, StageStructureGenerationStrategy.GENEROUS);
            LoadObjects();
            LoadNPCs();
            LoadWalls();
            LoadFloors();
            LoadSceneEscape(device);
            LoadGates();
        }

        protected abstract void LoadGates();

        protected abstract void LoadSceneEscape(GraphicsDevice device);

        protected void LoadFloors()
        {
            foreach (Pair<int, int> point in StageStructure.Floor.Floors)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Floor");
                item.LoadItem(TheContentManager);
                item.Type = StaticItemType.floor;
                StageElements.Add(item);
            }
        }

        protected void LoadWalls()
        {
            for (int i = 0; i < StageStructure.Walls.Count; i++)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Wall_Base");
                item.LoadItem(TheContentManager);
                item.Type = StaticItemType.wall;
                StageElements.Add(item);
            }
            for (int i = 0; i < StageStructure.ConcaveCorners.Count; i++)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Wall_Concave");
                item.LoadItem(TheContentManager);
                item.Type = StaticItemType.concave;
                StageElements.Add(item);
            }
            for (int i = 0; i < StageStructure.ConvexCorners.Count; i++)
            {
                StaticItem item = new StaticItem("Assets/3D/Interior/Interior_Wall_Convex");
                item.LoadItem(TheContentManager);
                item.Type = StaticItemType.convex;
                StageElements.Add(item);
            }
        }

        protected void LoadNPCs()
        {
            foreach (StageNPC stageNPC in Stage.NPCs)
            {
                NPC npc = new NPC(stageNPC.StaticObjectAsset);
                npc.LoadItem(TheContentManager);
                if (stageNPC is Tank)
                    npc.Type = StaticItemType.tank;
                if (stageNPC is Spy)
                    npc.Type = StaticItemType.spy;
                if (stageNPC is Flyer)
                    npc.Type = StaticItemType.flyer;
                npc.EnemySawSam = false;
                npcList.Add(npc);
                AI.Instance.AddRobot(npc);
            }
        }

        public void SetUpScene(GraphicsDevice device)
        {
            i = 0;
            StageElements.Add(escapeCollider);
            ConnectedColliders.Add(escapeCollider);
            SetUpGates(device);
            SetUpSamantha(device);
            SetUpObjects(device);
            SetUpNPCs(device);
            SetUpWallsAndFloor(device);
        }

        protected void SetUpWallsAndFloor(GraphicsDevice device)
        {
            #region WallsUp
            List<StaticItem> colliderConnectedAlllWallsUp = new List<StaticItem>();
            //Zapisane współrzędne, by się nie powielały
            List<float> coordY = new List<float>();
            //Tymczasowa ścianka
            StaticItem wall = null;

            float positionY = 0;

            for (int j = 0; j < StageStructure.Walls.WallsUp.Count; i++, j++)
            {
                StageElements[i].Rotation = -90;
                Vector3 move = new Vector3(StageStructure.Walls.WallsUp[j].X * mnoznikPrzesuniecaSciany,
                                            StageStructure.Walls.WallsUp[j].Y * mnoznikPrzesuniecaSciany - wallOffset,
                                            0.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5, 15f));

                if (coordY.Contains(move.Y))
                {
                    //Wyciągnij ten element, co ma te współrzędne
                    for (int w = 0; w < colliderConnectedAlllWallsUp.Count; w++)
                    {
                        if (colliderConnectedAlllWallsUp[w].Position.Y == move.Y)
                            wall = colliderConnectedAlllWallsUp[w];
                    }
                    if (positionY == move.Y && (move.X - StageElements[i - 1].Position.X) < 30)
                    {
                        wall.ColliderInternal.AABB = JoinToFirstCollider(wall.ColliderInternal.AABB, StageElements[i].ColliderInternal.AABB);
                    }
                    else
                    {
                        positionY = move.Y;
                        wall = new StaticItem(StageElements[i].PathToModel);
                        wall.Position = StageElements[i].Position;
                        wall.ColliderInternal = StageElements[i].ColliderInternal;
                        colliderConnectedAlllWallsUp.Add(wall);
                    }
                }
                else
                {
                    coordY.Add(move.Y);
                    positionY = move.Y;
                    wall = new StaticItem(StageElements[i].PathToModel);
                    wall.Position = StageElements[i].Position;
                    wall.ColliderInternal = StageElements[i].ColliderInternal;
                    colliderConnectedAlllWallsUp.Add(wall);
                }
            }

            foreach (StaticItem connectedWalls in colliderConnectedAlllWallsUp)
            {
                ConnectedColliders.Add(connectedWalls);
            }

            #endregion
            #region WallsDown
            //Odpowiednie elementy do łączenia
            List<StaticItem> colliderConnectedAlllWallDown = new List<StaticItem>();
            //Zapisane współrzędne, by się nie powielały
            coordY.Clear();
            //Tymczasowa ścianka
            wall = null;

            for (int j = 0; j < StageStructure.Walls.WallsDown.Count; i++, j++)
            {
                StageElements[i].Rotation = 90;
                Vector3 move = new Vector3(StageStructure.Walls.WallsDown[j].X * mnoznikPrzesuniecaSciany,
                                            StageStructure.Walls.WallsDown[j].Y * mnoznikPrzesuniecaSciany + wallOffset,
                                            0.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5f, 15f));
                if (coordY.Contains(move.Y))
                {
                    //Wyciągnij ten element, co ma te współrzędne
                    for (int w = 0; w < colliderConnectedAlllWallDown.Count; w++)
                    {
                        if (colliderConnectedAlllWallDown[w].Position.Y == move.Y)
                            wall = colliderConnectedAlllWallDown[w];
                    }
                    if (positionY == move.Y && (move.X - StageElements[i - 1].Position.X) < 30)
                    {
                        wall.ColliderInternal.AABB = JoinToFirstCollider(wall.ColliderInternal.AABB, StageElements[i].ColliderInternal.AABB);
                    }
                    else
                    {
                        positionY = move.Y;
                        wall = new StaticItem(StageElements[i].PathToModel);
                        wall.Position = StageElements[i].Position;
                        wall.ColliderInternal = StageElements[i].ColliderInternal;
                        colliderConnectedAlllWallDown.Add(wall);
                    }
                }
                else
                {
                    coordY.Add(move.Y);
                    positionY = move.Y;
                    wall = new StaticItem(StageElements[i].PathToModel);
                    wall.Position = StageElements[i].Position;
                    wall.ColliderInternal = StageElements[i].ColliderInternal;
                    colliderConnectedAlllWallDown.Add(wall);
                }
            }

            foreach (StaticItem connectedWalls in colliderConnectedAlllWallDown)
            {
                ConnectedColliders.Add(connectedWalls);
            }
            #endregion
            #region WallsLeft
            List<StaticItem> colliderConnectedAlllWallLeft = new List<StaticItem>();
            //Zapisane współrzędne, by się nie powielały
            List<float> coordX = new List<float>();
            //Tymczasowa ścianka
            wall = null;
            float positionX = 0;

            StageStructure.Walls.WallsLeft = StageStructure.Walls.WallsLeft.OrderBy(p => p.X).ToList();

            for (int j = 0; j < StageStructure.Walls.WallsLeft.Count; i++, j++)
            {
                StageElements[i].Rotation = 180;
                Vector3 move = new Vector3(StageStructure.Walls.WallsLeft[j].X * mnoznikPrzesuniecaSciany - wallOffset,
                                            StageStructure.Walls.WallsLeft[j].Y * mnoznikPrzesuniecaSciany,
                                            0.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-5f, -7f, 15f));

                if (coordX.Contains(move.X))
                {
                    //Wyciągnij ten element, co ma te współrzędne
                    for (int w = 0; w < colliderConnectedAlllWallLeft.Count; w++)
                    {
                        if (colliderConnectedAlllWallLeft[w].Position.X == move.X)
                            wall = colliderConnectedAlllWallLeft[w];
                    }
                    if (positionX == move.X && (move.Y - StageElements[i - 1].Position.Y) < 30)
                    {
                        wall.ColliderInternal.AABB = JoinToFirstCollider(wall.ColliderInternal.AABB, StageElements[i].ColliderInternal.AABB);
                    }
                    else
                    {
                        positionX = move.X;
                        wall = new StaticItem(StageElements[i].PathToModel);
                        wall.Position = StageElements[i].Position;
                        wall.ColliderInternal = StageElements[i].ColliderInternal;
                        colliderConnectedAlllWallLeft.Add(wall);
                    }
                }
                else
                {
                    coordX.Add(move.X);
                    positionX = move.X;
                    wall = new StaticItem(StageElements[i].PathToModel);
                    wall.Position = StageElements[i].Position;
                    wall.ColliderInternal = StageElements[i].ColliderInternal;
                    colliderConnectedAlllWallLeft.Add(wall);
                }
            }
            foreach (StaticItem connectedWalls in colliderConnectedAlllWallLeft)
            {
                ConnectedColliders.Add(connectedWalls);
            }

            #endregion
            #region WallsRight

            //Odpowiednie elementy do łączenia
            List<StaticItem> colliderConnectedAlllWallRight = new List<StaticItem>();
            //Zapisane współrzędne, by się nie powielały
            coordX.Clear();
            //Tymczasowa ścianka
            wall = null;

            StageStructure.Walls.WallsRight = StageStructure.Walls.WallsRight.OrderBy(p => p.X).ToList();
            for (int j = 0; j < StageStructure.Walls.WallsRight.Count; i++, j++)
            {
                StageElements[i].Rotation = 0;
                Vector3 move = new Vector3(StageStructure.Walls.WallsRight[j].X * mnoznikPrzesuniecaSciany + wallOffset,
                                            StageStructure.Walls.WallsRight[j].Y * mnoznikPrzesuniecaSciany,
                                            2.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.1f, 0.2f, 1.4f), new Vector3(-7f, -5f, 15f));
                if (coordX.Contains(move.X))
                {
                    for (int w = 0; w < colliderConnectedAlllWallRight.Count; w++)
                    {
                        if (colliderConnectedAlllWallRight[w].Position.X == move.X)
                            wall = colliderConnectedAlllWallRight[w];
                    }
                    if (positionX == move.X && (move.Y - StageElements[i - 1].Position.Y) < 30)
                    {
                        wall.ColliderInternal.AABB = JoinToFirstCollider(wall.ColliderInternal.AABB, StageElements[i].ColliderInternal.AABB);
                    }
                    else
                    {
                        positionX = move.X;
                        wall = new StaticItem(StageElements[i].PathToModel);
                        wall.Position = StageElements[i].Position;
                        wall.ColliderInternal = StageElements[i].ColliderInternal;
                        colliderConnectedAlllWallRight.Add(wall);
                    }
                }
                else
                {
                    coordX.Add(move.X);
                    positionX = move.X;
                    wall = new StaticItem(StageElements[i].PathToModel);
                    wall.Position = StageElements[i].Position;
                    wall.ColliderInternal = StageElements[i].ColliderInternal;
                    colliderConnectedAlllWallRight.Add(wall);
                }
            }
            foreach (StaticItem connectedWalls in colliderConnectedAlllWallRight)
            {
                ConnectedColliders.Add(connectedWalls);
            }
            #endregion
            #region ConcaveCornersLowerLeft
            for (int j = 0; j < StageStructure.ConcaveCorners.ConcaveCornersLowerLeft.Count; i++, j++)
            {
                StageElements[i].Rotation = 180;
                Vector3 move = new Vector3(StageStructure.ConcaveCorners.ConcaveCornersLowerLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            StageStructure.ConcaveCorners.ConcaveCornersLowerLeft[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConcaveCornersLowerRight
            for (int j = 0; j < StageStructure.ConcaveCorners.ConcaveCornersLowerRight.Count; i++, j++)
            {
                StageElements[i].Rotation = 90;
                Vector3 move = new Vector3(StageStructure.ConcaveCorners.ConcaveCornersLowerRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            StageStructure.ConcaveCorners.ConcaveCornersLowerRight[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConcaveCornersUpperLeft
            for (int j = 0; j < StageStructure.ConcaveCorners.ConcaveCornersUpperLeft.Count; i++, j++)
            {
                StageElements[i].Rotation = 270;
                Vector3 move = new Vector3(StageStructure.ConcaveCorners.ConcaveCornersUpperLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            StageStructure.ConcaveCorners.ConcaveCornersUpperLeft[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConcaveCornersUpperRight
            for (int j = 0; j < StageStructure.ConcaveCorners.ConcaveCornersUpperRight.Count; i++, j++)
            {
                StageElements[i].Rotation = 0;
                Vector3 move = new Vector3(StageStructure.ConcaveCorners.ConcaveCornersUpperRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            StageStructure.ConcaveCorners.ConcaveCornersUpperRight[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConvexCornersLowerLeft
            for (int j = 0; j < StageStructure.ConvexCorners.ConvexCornersLowerLeft.Count; i++, j++)
            {
                StageElements[i].Rotation = 180;
                Vector3 move = new Vector3(StageStructure.ConvexCorners.ConvexCornersLowerLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            StageStructure.ConvexCorners.ConvexCornersLowerLeft[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.2f, 0.2f, 1.4f), new Vector3(-25f, 5f, 15f));
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConvexCornersLowerRight
            for (int j = 0; j < StageStructure.ConvexCorners.ConvexCornersLowerRight.Count; i++, j++)
            {
                StageElements[i].Rotation = 90;
                Vector3 move = new Vector3(StageStructure.ConvexCorners.ConvexCornersLowerRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            StageStructure.ConvexCorners.ConvexCornersLowerRight[j].Y * mnoznikPrzesuniecaSciany + cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.2f, 0.2f, 1.4f), new Vector3(0f, 5, 15f));
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConvexCornersUpperLeft
            for (int j = 0; j < StageStructure.ConvexCorners.ConvexCornersUpperLeft.Count; i++, j++)
            {
                StageElements[i].Rotation = 270;
                Vector3 move = new Vector3(StageStructure.ConvexCorners.ConvexCornersUpperLeft[j].X * mnoznikPrzesuniecaSciany - cornerOffset,
                                            StageStructure.ConvexCorners.ConvexCornersUpperLeft[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.2f, 0.2f, 1.4f), new Vector3(-25f, -25f, 15f));
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region ConvexCornersUpperRight
            for (int j = 0; j < StageStructure.ConvexCorners.ConvexCornersUpperRight.Count; i++, j++)
            {
                StageElements[i].Rotation = 0;
                Vector3 move = new Vector3(StageStructure.ConvexCorners.ConvexCornersUpperRight[j].X * mnoznikPrzesuniecaSciany + cornerOffset,
                                            StageStructure.ConvexCorners.ConvexCornersUpperRight[j].Y * mnoznikPrzesuniecaSciany - cornerOffset,
                                            2.0f);
                StageElements[i].Position = move;
                StageElements[i].FixColliderInternal(new Vector3(0.2f, 0.2f, 1.4f), new Vector3(0f, -25, 15f));
                //ConnectedColliders.Add(StageElements[i]);
            }
            #endregion
            #region Floor setups
            float mnoznikPrzesunieciaPodlogi = mnoznikPrzesuniecaSciany;
            for (int j = 0; j < StageStructure.Floor.Count; i++, j++)
            {
                Vector3 move = new Vector3(StageStructure.Floor.Floors[j].X * mnoznikPrzesunieciaPodlogi,
                                            StageStructure.Floor.Floors[j].Y * mnoznikPrzesunieciaPodlogi,
                                            -4.8f);
                StageElements[i].Position = move;
                //    stageSurroundingsList[i].FixColliderInternal(new Vector3(0.2f, 0.1f, 1.4f), new Vector3(-7, -5, 15f));
            }
            #endregion
        }

        protected void SetUpNPCs(GraphicsDevice device)
        {
            StaticItem radar = new StaticItem("Assets/3D/radar");
            radar.LoadItem(TheContentManager);
            SamanthaGhostController.Radar = radar;
            radar.Position = SamanthaGhostController.ColliderInternal.Position + new Vector3(-25f, -25f, 10);
            radar.FixColliderInternal(new Vector3(3, 3, 3), new Vector3(0, 0, 0));
            SamanthaGhostController.Radar = radar;

            for (int j = 0; j < Stage.NPCs.Count; j++)
            {

                Vector3 move = new Vector3(Stage.NPCs[j].GetBlock().X * mnoznikPrzesuniecaSciany,
                                        Stage.NPCs[j].GetBlock().Y * mnoznikPrzesuniecaSciany,
                                        0.0f);
                npcList[j].Position = move;
                npcList[j].Rotation = Stage.NPCs[j].Rotation;
                if (npcList[j].Type == StaticItemType.tank)
                {
                    npcList[j].FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
                    npcList[j].FixColliderExternal(new Vector3(2, 2, 2), new Vector3(-15f, -15f, 10f));
                }
                else if (npcList[j].Type == StaticItemType.spy)
                {
                    npcList[j].FixColliderInternal(new Vector3(0.4f, 0.4f, 1f), new Vector3(-25f, -5f, 10f));
                    npcList[j].FixColliderExternal(new Vector3(1, 1, 1), new Vector3(-25f, -5f, 10f));
                }
                else if (npcList[j].Type == StaticItemType.flyer)
                {
                    Debug.WriteLine("Ustawiam Flyera");
                    npcList[j].FixColliderInternal(new Vector3(0.5f, 0.5f, 1f), new Vector3(-10f, -10f, 10f));
                    npcList[j].FixColliderExternal(new Vector3(1, 1, 1), new Vector3(-5f, -30f, 10f));
                }
                npcList[j].Radar = radar;
                npcList[j].ID = generatedID.IDs[0];
                npcList[j].DrawID = false;
                npcList[j].MachineIDHeight = new Vector3(0, 0, 60);
                generatedID.IDs.RemoveAt(0);
                npcList[j].ApplyIDBilboard(device, TheContentManager, move);
                npcList[j].bilboards = new BillboardSystem(device, TheContentManager,
                    TheContentManager.Load<Texture2D>("Assets/2D/warning"), new Vector2(80),
                    move + new Vector3(0, 0, 100));
                npcList[j].BilboardHeight = new Vector3(0, 0, 100);

                GameStateMainGame.npcBillboardsList.Add(npcList[j]);
            }
        }

        protected void SetUpObjects(GraphicsDevice device)
        {
            for (int j = 0; j < Stage.Objects.Count; i++, j++)
            {
                float z;
                Vector3 move = new Vector3();
                #region Terminals
                if (Stage.Objects[j] is Terminal)
                {
                    z = terminalZ;
                    move = new Vector3(Stage.Objects[j].GetBlock().X * mnoznikPrzesuniecaSciany,
                                        Stage.Objects[j].GetBlock().Y * mnoznikPrzesuniecaSciany,
                                        z);
                    StageElements[i].Position = move;
                    StageElements[i].FixColliderExternal(new Vector3(1.5f, 1.5f, 1.5f), new Vector3(15f, 20f, 20f));
                    StageElements[i].FixColliderInternal(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(8, 8, 0));
                    StageElements[i].bilboards = new BillboardSystem(device, TheContentManager,
                        TheContentManager.Load<Texture2D>("Assets/2D/buttonTab"),
                        new Vector2(60),
                        move + new Vector3(0, 0, 20)
                        );
                    StageElements[i].BilboardHeight = new Vector3(0, 0, 20);
                }
                #endregion
                #region Columns
                else if (Stage.Objects[j] is Column)
                {
                    z = terminalZ;
                    move = new Vector3(Stage.Objects[j].GetBlock().X * mnoznikPrzesuniecaSciany,
                        Stage.Objects[j].GetBlock().Y * mnoznikPrzesuniecaSciany,
                        0);
                    StageElements[j].Position = move;
                    StageElements[j].FixColliderInternal(new Vector3(0.2f, 0.2f, 1f), new Vector3(-8, -8, -50));
                }
                #endregion
                #region Generator
                else if (Stage.Objects[j] is OxygenGenerator)
                {
                    z = terminalZ;
                    move = new Vector3(Stage.Objects[j].GetBlock().X * mnoznikPrzesuniecaSciany,
                        Stage.Objects[j].GetBlock().Y * mnoznikPrzesuniecaSciany,
                        -50);
                    Debug.WriteLine("Generator position " + move);
                    StageElements[j].Type = StaticItemType.oxygenGenerator;
                    StageElements[j].Position = move;
                    StageElements[j].Rotation = 270;
                    StageElements[j].FixColliderInternal(new Vector3(0.52f, 0.2f, 0.5f), new Vector3(-42, 6, 60));
                    StageElements[j].ID = generatedID.IDs[0];
                    StageElements[j].DrawID = false;
                    StageElements[j].MachineIDHeight = new Vector3(-30, 0, 150);
                    generatedID.IDs.RemoveAt(0);
                    StageElements[j].ApplyIDBilboard(device, TheContentManager, move);
                }
                #endregion
                #region Rest Things
                else
                {
                    z = objectZ;
                    move = new Vector3(Stage.Objects[j].GetBlock().X * mnoznikPrzesuniecaSciany,
                                            Stage.Objects[j].GetBlock().Y * mnoznikPrzesuniecaSciany,
                                            z);
                    StageElements[i].Position = move;
                    StageElements[i].Rotation = Stage.Objects[j].Rotation;
                }
                #endregion
                ConnectedColliders.Add(StageElements[i]);
            }
        }

        protected void SetUpSamantha(GraphicsDevice device)
        {
            SamanthaGhostController.Position = new Vector3(Stage.PlayerPosition.X * mnoznikPrzesuniecaSciany,
                                            Stage.PlayerPosition.Y * mnoznikPrzesuniecaSciany,
                                            0.0f);
            SamanthaGhostController.FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
            SamanthaGhostController.FixColliderExternal(new Vector3(1.25f, 1.25f, 1.25f), new Vector3(-25f, -25f, 20f));
        }

        protected abstract void SetUpGates(GraphicsDevice device);

        protected BoundingBox JoinToFirstCollider(BoundingBox box1, BoundingBox box2)
        {
            return BoundingBox.CreateMerged(box1, box2);
        }
    }
}
