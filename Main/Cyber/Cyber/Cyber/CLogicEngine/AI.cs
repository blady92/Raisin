using Cyber.CItems.CStaticItem;
using Cyber.CLogicEngine;
using Cyber.CollisionEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cyber
{
    class AI
    {
        //TODO: unit tests ???
        private static volatile AI instance;
        private static object syncRoot = new Object();

        public static List<NPC> Robots = new List<NPC>();

        private const int chasingTime = 20;

        private ColliderController colliderController = null;
        private bool[,] freeSpaceMap = null;

        private Thread t;

        private IPathfindingAlgorithm pathfindingAlgorithm = null;

        private Position lastSamPosition = null;

        #region ACCESSORS
        internal ColliderController ColliderController
        {
            get { return colliderController; }
            set { colliderController = value; }
        }

        public bool[,] FreeSpaceMap
        {
            get { return freeSpaceMap; }
            set 
            {
                freeSpaceMap = value; 
                pathfindingAlgorithm = new BestFirstAlgorithm(freeSpaceMap);
            }
        }

        /// <summary>
        /// Safely get an instance of AI
        /// </summary>
        public static AI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AI();
                    }
                }

                return instance;
            }
        }
        #endregion

        public AI()
        {
            /*t = new Thread(new ParameterizedThreadStart(SteeringLoop));
            t.Start();
            while (!t.IsAlive) ;*/
        }

        /// <summary>
        /// Populate the robot list
        /// </summary>
        /// <param name="r"></param>
        public void AddRobot(NPC r)
        {
            Robots.Add(r);
        }

        /// <summary>
        /// Tell every robot on map location of the player
        /// </summary>
        /// <param name="target">Target that will be chased</param>
        public void AlertOthers(StaticItem target)
        {
            if (colliderController == null)
            {
                throw new Exception("You should set collider controller before beeing able to steer NPC's");
            }
            if (!StageUtils.StageVectorToBitmapCoords(target.Position).Equals(lastSamPosition))
            {
                lastSamPosition = StageUtils.StageVectorToBitmapCoords(target.Position);
                foreach (NPC npc in Robots)
                {
                    npc.Chase(pathfindingAlgorithm.FindWayToPlace(npc.Position, target.Position));
                }
            }
            Clock clock = Clock.Instance;
            clock.AddEvent(Clock.FROMNOW, chasingTime, StopChase);
        }

        private void StopChase(object sender, int time)
        {
            foreach(NPC r in Robots)
            {
                r.StopChasing();
            }
            /* TODO: znaleźć metodę na usuwanie zdarzeń z zegara, tak żeby można było wywoływać AlertOthers()
             * cały czas gdy gracz jest w polu widzenia któregokolwiek z robotów
             */
        }

        /// <summary>
        /// Function that is telling all of NPC's to go
        /// </summary>
        public void MoveNPCs(object obj)
        {
            foreach (var npc in Robots)
            {
                if (npc.GetNextWaypoint() != Vector3.Zero)
                {
                    Vector3 move = GetDirectionTo(npc.Position, npc.GetNextWaypoint());
                    if (true/*TODO: if npc is almost on the next waypoint*/)
                    {
                        /*rotate waypoint*/
                    }
                    colliderController.CheckCollision(npc, move);
                    //Debug.WriteLine("NPC: przeniosłem się do "+npc.Position);
                }
            }
        }

        private Vector3 GetDirectionTo(Vector3 from ,Vector3 to)
        {
            Vector3 result = (to - from);
            result.Normalize();
            return result;
        }

        public static void Destroy()
        {
            foreach (var robot in Robots)
            {
                robot.StopChasing();
            }
            instance = null;
            Debug.WriteLine("Destroying AI...");
        }
    }
}
