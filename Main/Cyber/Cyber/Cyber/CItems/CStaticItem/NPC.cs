using Cyber.CLogicEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Cyber.CItems.CStaticItem
{
    class NPC : StaticItem
    {
        //TODO: unit tests ???
        Queue<Vector3> patrolWaypoints = new Queue<Vector3>();
        Queue<Vector3> chasingWaypoints = new Queue<Vector3>();

        public NPC(string path) : base(path) { }

        public NPC(string path, Vector3 position) : base(path, position) { }

        public NPC(string path, List<Vector3> waypoints) : base(path, waypoints[0])
        {
            foreach (Vector3 v in waypoints)
            {
                patrolWaypoints.Enqueue(v);
            }
            throw new NotImplementedException("Warning: Not tested");
        }

        /// <summary>
        /// Tell robot where he has to go
        /// </summary>
        /// <param name="pos"></param>
        public void Chase(List<Vector3> waypoints)
        {
            chasingWaypoints.Clear();
            foreach (var position in waypoints)
            {
                chasingWaypoints.Enqueue(position);
            }
#if DEBUG
            if (waypoints.Count != 0)
            {
                Debug.WriteLine("NPC: idę do "+waypoints.Last());
            }
#endif
        }

        /// <summary>
        /// Interrupt chase
        /// </summary>
        public void StopChasing()
        {
            Debug.WriteLine("NPC: poddaję się");
            chasingWaypoints.Clear();
        }

        /// <summary>
        /// Check where the robot is currently going
        /// </summary>
        /// <returns></returns>
        public Vector3 GetNextWaypoint()
        {
            if (chasingWaypoints.Count == 0)
            {
                if (patrolWaypoints.Count == 0)
                    return Vector3.Zero;
                return patrolWaypoints.Peek();
            }
            else
            {
                if ((chasingWaypoints.Peek() - Position).Length() < StageUtils.PRZESUNIECIE)
                {
                    chasingWaypoints.Dequeue();
                }
                if (chasingWaypoints.Count == 0)
                {
                    return Vector3.Zero;
                }
                return chasingWaypoints.Peek();
            }
        }
    }
}
