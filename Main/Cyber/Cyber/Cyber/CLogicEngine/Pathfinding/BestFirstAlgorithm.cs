using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    public class BestFirstAlgorithm : IPathfindingAlgorithm
    {
        private bool[,] freeSpaceMap;

        private static readonly int safetyMargin = 1000;
        public BestFirstAlgorithm(bool[,] freeSpaceMap)
        {
            this.freeSpaceMap = freeSpaceMap;
        }

        public List<Vector3> FindWayToPlace(Vector3 from, Vector3 to)
        {
            List<Vector3> result = new List<Vector3>();
            Position initializer = new Position(-freeSpaceMap.GetLength(0), -freeSpaceMap.GetLength(1));
            Stack<Position> moves = new Stack<Position>();
            moves.Push(StageUtils.StageVectorToBitmapCoords(from));
            int safetySwitch = 0;
            List<Position> closedNodes = new List<Position>();
            while((StageUtils.BitmapCoordsToStageVector(moves.Peek()) - to).Length() > StageUtils.PRZESUNIECIE)
            {
                Position peak = moves.Peek();
                Position next = initializer;
                //for every available move
                foreach(Moves m in Enum.GetValues(typeof(Moves)))
                {
                    float currentNextPosToTargetDistance = (StageUtils.BitmapCoordsToStageVector(GetNextPosition(peak, m)) - to).Length();
                    float bestNextPosToTargetDistance = (StageUtils.BitmapCoordsToStageVector(next) - to).Length();
                    //find move that has shortest distance to target
                    //player can move there and algo have never been there
                    if (
                        currentNextPosToTargetDistance < bestNextPosToTargetDistance &&
                        GetNextPositionIfFree(peak, m, freeSpaceMap) != null &&
                        ! closedNodes.Contains(GetNextPosition(peak, m))
                    )
                    {
                        next = GetNextPosition(peak, m);
                    }
                }
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
                result.Insert(0, StageUtils.BitmapCoordsToStageVector(moves.Pop()));
            }
            return result;
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