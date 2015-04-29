using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber2O
{
    class Robot
    {
        //TODO: unit tests ???
        Queue<Vector3> patrolWaypoints;
        Vector3 chasingPosition = Vector3.Zero;

        public Robot() { }

        public Robot(List<Vector3> waypoints)
        {
            foreach (Vector3 v in waypoints)
            {
                patrolWaypoints.Enqueue(v);
            }
        }

        /// <summary>
        /// Tell robot where he has to go
        /// </summary>
        /// <param name="pos"></param>
        public void Chase(Vector3 pos)
        {
            chasingPosition = pos;
        }

        /// <summary>
        /// Interrupt chase
        /// </summary>
        public void StopChasing()
        {
            chasingPosition = Vector3.Zero;
        }

        /// <summary>
        /// Check where the robot is currently going
        /// </summary>
        /// <returns></returns>
        public Vector3 GetNextWaypoint()
        {
            if (chasingPosition == Vector3.Zero)
                return patrolWaypoints.Peek();
            else
                return chasingPosition;
        }

        private void AlertEveryone()
        {
            /*
             * TODO: wywołać gdy gracz wejdzie w pole widzenia robota
             */
            AI.Instance.AlertOthers();
        }
    }
}
