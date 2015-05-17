using Cyber.CItems.CStaticItem;
using Cyber.CLogicEngine;
using Cyber.CollisionEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        private List<NPC> robots = new List<NPC>();

        private const int chasingTime = 20;

        private ColliderController colliderController = null;

        private Thread t;

        #region ACCESSORS
        internal ColliderController ColliderController
        {
            get { return colliderController; }
            set { colliderController = value; }
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
            robots.Add(r);
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

            foreach (NPC r in robots)
            {
                r.Chase(target.Position);
            }
            Clock clock = Clock.Instance;
            clock.AddEvent(Clock.FROMNOW, chasingTime, StopChase);
        }

        private void StopChase(object sender, int time)
        {
            foreach(NPC r in robots)
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
            foreach (var npc in robots)
            {
                if (npc.GetNextWaypoint() != Vector3.Zero)
                {
                    Vector3 move = new Vector3(0, 0, 1f);
                    colliderController.CheckCollision(npc, move);
                }
            }
        }
    }
}
