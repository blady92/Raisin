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
        Vector3 chasingPosition = Vector3.Zero;

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
        public void Chase(Vector3 pos)
        {
            chasingPosition = pos;
            Debug.WriteLine("NPC: idę do "+pos);
        }

        /// <summary>
        /// Interrupt chase
        /// </summary>
        public void StopChasing()
        {
            Debug.WriteLine("NPC: poddaję się");
            chasingPosition = Vector3.Zero;
        }

        /// <summary>
        /// Check where the robot is currently going
        /// </summary>
        /// <returns></returns>
        public Vector3 GetNextWaypoint()
        {
            if (chasingPosition == Vector3.Zero)
            {
                if (patrolWaypoints.Count == 0)
                    return Vector3.Zero;
                return patrolWaypoints.Peek();
            }
            else
                return chasingPosition;
        }
    }
}
