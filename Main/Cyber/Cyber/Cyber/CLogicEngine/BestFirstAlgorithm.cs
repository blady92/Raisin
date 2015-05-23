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

        private static readonly int safetyMargin = 10000;
        public BestFirstAlgorithm(bool[,] freeSpaceMap)
        {
            this.freeSpaceMap = freeSpaceMap;
        }

        public List<Vector3> FindWayToPlace(Vector3 from, Vector3 to)
        {
            List<Vector3> result = new List<Vector3>();
            Stack<Position> moves = new Stack<Position>();
            moves.Push(StageUtils.StageVectorToBitmapCoords(from));
            int safetySwitch = 0;
            List<Position> closedNodes = new List<Position>();
            while((StageUtils.BitmapCoordsToStageVector(moves.Peek()) - to).Length() > StageUtils.PRZESUNIECIE)
            {
                Position peak = moves.Peek();
                Position next = peak;
                //for every available move
                foreach(Moves m in Enum.GetValues(typeof(Moves)))
                {
                    float currentNextPosToTargetDistance = (StageUtils.BitmapCoordsToStageVector(GetNextPosition(peak, m)) - to).Length();
                    float bestNextPosToTargetDistance = (StageUtils.BitmapCoordsToStageVector(next) - to).Length();
                    //find move that has shortest distance to target
                    //player can move there and algo have never been there
                    if (
                        currentNextPosToTargetDistance < bestNextPosToTargetDistance &&
                        freeSpaceMap[GetNextPosition(peak, m).X, GetNextPosition(peak, m).Y] &&
                        ! closedNodes.Contains(GetNextPosition(peak, m))
                    )
                    {
                        next = GetNextPosition(peak, m);
                    }
                }
                //if there is at least one node we can go to
                //do it, if it isn't go back
                if (next != peak)
                {
                    moves.Push(next);
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
            while (moves.Count != 0)
            {
                result.Insert(0, StageUtils.BitmapCoordsToStageVector(moves.Pop()));
            }
            return result;
        }

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
    }
}
