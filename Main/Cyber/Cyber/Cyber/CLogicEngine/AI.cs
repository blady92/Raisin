using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber2O
{
    class AI
    {
        //TODO: unit tests ???
        private static volatile AI instance;
        private static object syncRoot = new Object();

        private List<Robot> robots = new List<Robot>();

        private static const int chasingTime = 20;

        /// <summary>
        /// Populate the robot list
        /// </summary>
        /// <param name="r"></param>
        public void AddRobot(Robot r)
        {
            robots.Add(r);
        }

        /// <summary>
        /// Tell every robot on map location of the player
        /// </summary>
        public void AlertOthers()
        {
            foreach (Robot r in robots)
            {
                throw new NotImplementedException("TODO: pobrać pozycję gracza i przekazać robotowi");
                //r.Chase(Vector3.Zero);
                /*
                 * TODO: najfajniej byłoby jeśli ten Vector3 aktualizowałby się w miarę poruszania się gracza
                 * wtedy to co jest powinno w zupełności wystarczyć żeby roboty wiedziały gdzie iść
                 */
            }
            Clock clock = Clock.Instance;
            clock.AddEvent(Clock.FROMNOW, chasingTime, StopChase);
        }

        private void StopChase(object sender, int time)
        {
            foreach(Robot r in robots)
            {
                r.StopChasing();
            }
            /* TODO: znaleźć metodę na usuwanie zdarzeń z zegara, tak żeby można było wywoływać AlertOthers()
             * cały czas gdy gracz jest w polu widzenia któregokolwiek z robotów
             */
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
    }
}
