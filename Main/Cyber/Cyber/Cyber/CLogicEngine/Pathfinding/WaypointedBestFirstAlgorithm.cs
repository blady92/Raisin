using Cyber.CItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    public class WaypointedBestFirstAlgorithm : IPathfindingAlgorithm
    {
        private List<Waypoint> waypoints;

        private static readonly int safetyMargin = 1000;
        private static readonly float MIN_FIND_DISTANCE = 200f;
        public WaypointedBestFirstAlgorithm()
        {
            this.waypoints = new Level2Waypoints().Waypoints;
        }

        public List<Vector3> FindWayToPlace(Vector3 from, Vector3 to)
        {
            List<Vector3> result = new List<Vector3>();
            Waypoint initializer = FindNearest(from);
            //Position initializer = new Position(-freeSpaceMap.GetLength(0), -freeSpaceMap.GetLength(1));
            Stack<Waypoint> moves = new Stack<Waypoint>();
            moves.Push(initializer);
            int safetySwitch = 0;
            List<Waypoint> closedNodes = new List<Waypoint>();
            if ((from - to).Length() < MIN_FIND_DISTANCE)
            {
                result.Add(to);
                return result;
            }
            while(true)
            {
                Waypoint peak = moves.Peek();
                Waypoint next = null;
                //for every available move
                foreach(var neigh in peak.Neighbours)
                {
                    float currentNextPosToTargetDistance = (StageUtils.BitmapCoordsToStageVector(neigh.Position) - to).Length();
                    float bestNextPosToTargetDistance = -1.0f;
                    if (next != null)
                    {
                        bestNextPosToTargetDistance = (StageUtils.BitmapCoordsToStageVector(next.Position) - to).Length();
                    }
                    //find move that has shortest distance to target
                    //player can move there and algo have never been there
                    if (
                        bestNextPosToTargetDistance == -1.0f ||
                        currentNextPosToTargetDistance < bestNextPosToTargetDistance &&
                        ! closedNodes.Contains(neigh)
                    )
                    {
                        next = neigh;
                    }
                }
                Waypoint actual = peak;
                //if there is at least one node we can go to
                //do it, if it isn't go back
                if (next != initializer)
                {
                    moves.Push(next);
                    closedNodes.Add(next);
                }
                else
                {
                    closedNodes.Add(peak);
                    moves.Pop();
                    if (moves.Count == 0)
                    {
                        //if we had to clear whole stack leave with nothing
                        break;
                    }
                }
                //exit if |peak - actual| > 1.5 * |actual - to|
                float toNext = (StageUtils.BitmapCoordsToStageVector(moves.Peek().Position) - StageUtils.BitmapCoordsToStageVector(actual.Position)).Length();
                float toTarget = (to - StageUtils.BitmapCoordsToStageVector(actual.Position)).Length();
                if (toNext > 1.5 * toTarget)
                {
                    moves.Pop();
                    break;
                }
                safetySwitch++;
                if (safetySwitch > safetyMargin)
                {
                    break;
                }
            }
            
            //moves.Push(StageUtils.StageVectorToBitmapCoords(to));

            //StageUtils.PrintPath(freeSpaceMap, moves);

            while (moves.Count != 0)
            {
                result.Insert(0, StageUtils.BitmapCoordsToStageVector(moves.Pop().Position));
            }
            result.Add(to);
            return result;
        }

        private Waypoint FindNearest(Vector3 point)
        {
            Waypoint nearest = waypoints[0];
            foreach(Waypoint w in waypoints)
            {
                float distance = (StageUtils.BitmapCoordsToStageVector(w.Position) - point).Length();
                float nearestDistance = (StageUtils.BitmapCoordsToStageVector(nearest.Position) - point).Length();
                if (distance < nearestDistance)
                {
                    nearest = w;
                }
            }
            return nearest;
        }

        /// <summary>
        /// Get position coordinates after performing NEXTMOVE
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="nextMove"></param>
        /// <returns></returns>
        public Position GetNextPosition(Position pos, Moves nextMove)
        {
            switch(nextMove)
            {
                case Moves.D:
                    return new Position(pos.X, pos.Y + 1);
                case Moves.DL:
                    return new Position(pos.X - 1, pos.Y + 1);
                case Moves.DR:
                    return new Position(pos.X + 1, pos.Y + 1);
                case Moves.L:
                    return new Position(pos.X - 1, pos.Y);
                case Moves.R:
                    return new Position(pos.X + 1, pos.Y);
                case Moves.U:
                    return new Position(pos.X, pos.Y - 1);
                case Moves.UL:
                    return new Position(pos.X - 1, pos.Y - 1);
                case Moves.UR:
                    return new Position(pos.X + 1, pos.Y - 1);
                default:
                    throw new ArgumentException("Expected one of enum fields");
            }
        }

        /// <summary>
        /// Get next position coordinates only if there is no wall between current and next position
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="nextMove"></param>
        /// <param name="freeSpaceMap"></param>
        /// <returns></returns>
        Position GetNextPositionIfFree(Position pos, Moves nextMove, bool[,] freeSpaceMap)
        {
            Position nextPosition = GetNextPosition(pos, nextMove);
            if (nextPosition.X < 0 || nextPosition.Y < 0 || nextPosition.X >= freeSpaceMap.GetLength(0) || nextPosition.Y >= freeSpaceMap.GetLength(1))
            {
                return null;
            }
            switch(nextMove)
            {
                case Moves.DL:
                    {
                        Position left = GetNextPosition(pos, Moves.L);
                        Position down = GetNextPosition(pos, Moves.D);
                        if (!freeSpaceMap[left.X, left.Y] || !freeSpaceMap[down.X, down.Y])
                        {
                            return null;
                        }
                        break;
                    }
                case Moves.DR:
                    {
                        Position right = GetNextPosition(pos, Moves.R);
                        Position down = GetNextPosition(pos, Moves.D);
                        if (!freeSpaceMap[right.X, right.Y] || !freeSpaceMap[down.X, down.Y])
                        {
                            return null;
                        }
                        break;
                    }
                case Moves.UL:
                    {
                        Position left = GetNextPosition(pos, Moves.L);
                        Position up = GetNextPosition(pos, Moves.U);
                        if (!freeSpaceMap[left.X, left.Y] || !freeSpaceMap[up.X, up.Y])
                        {
                            return null;
                        }
                        break;
                    }
                case Moves.UR:
                    {
                        Position right = GetNextPosition(pos, Moves.R);
                        Position up = GetNextPosition(pos, Moves.U);
                        if (!freeSpaceMap[right.X, right.Y] || !freeSpaceMap[up.X, up.Y])
                        {
                            return null;
                        }
                        break;
                    }
            }
            return freeSpaceMap[nextPosition.X, nextPosition.Y] ? nextPosition : null;
        }
    }
}