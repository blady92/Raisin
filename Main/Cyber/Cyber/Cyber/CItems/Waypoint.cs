using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cyber.CItems
{
    public class Waypoint
    {
        public Vector3 Position { get; private set; }
        public List<Waypoint> Neighbours { get; private set; }

        public Waypoint(float x, float y)
        {
            Neighbours = new List<Waypoint>();
            Position = new Vector3(x, y, 0f);
        }
    }

    public class Level2Waypoints
    {
        public List<Waypoint> Waypoints { get; private set; }

        public Level2Waypoints()
        {
            Waypoints = new List<Waypoint>();

            Waypoints.Add(new Waypoint(8, 2));
            Waypoints.Add(new Waypoint(8, 27));
            Waypoints.Add(new Waypoint(18, 28));
            Waypoints.Add(new Waypoint(18, 15));
            Waypoints.Add(new Waypoint(18, 2));
            Waypoints.Add(new Waypoint(50, 17));
            Waypoints.Add(new Waypoint(68, 15));
            Waypoints.Add(new Waypoint(50, 36));
            Waypoints.Add(new Waypoint(60, 36));
            Waypoints.Add(new Waypoint(26, 36));
            Waypoints.Add(new Waypoint(26, 61));
            Waypoints.Add(new Waypoint(50, 62));
            Waypoints.Add(new Waypoint(76, 62));
            Waypoints.Add(new Waypoint(78, 45));
            Waypoints.Add(new Waypoint(85, 45));
            Waypoints.Add(new Waypoint(78, 30));

            add(0, 1, 4);
            add(1, 0, 2);
            add(2, 1, 3);
            add(3, 2, 4, 5);
            add(4, 0, 3);
            add(5, 3, 6, 7);
            add(6, 5);
            add(7, 5, 8, 9, 11);
            add(8, 7);
            add(9, 7, 10);
            add(10, 9, 11);
            add(11, 10, 7, 12);
            add(12, 11, 13);
            add(13, 12, 14, 15);
            add(14, 13);
            add(15, 13);
        }

        private void add(int waypointNumber, params int[] neighbours)
        {
            foreach (var neighbour in neighbours)
            {
                Waypoints[waypointNumber].Neighbours.Add(Waypoints[neighbour]);
            }
        }
    }
}
