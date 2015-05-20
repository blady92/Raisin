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
            result.Add(to);
            Stack<Position> moves = new Stack<Position>();
            moves.Push(StageUtils.StageVectorToBitmapCoords(from));
            int safetySwitch = 0;
            while((StageUtils.BitmapCoordsToStageVector(moves.Peek()) - to).Length() < StageUtils.PRZESUNIECIE)
            {
                ///////////////
                safetySwitch++;
                if (safetySwitch > safetyMargin)
                {
                    break;
                }
            }
            return result;
        }

        public Vector3 GetNextPosition(Moves nextMove)
        {
            throw new NotImplementedException();
        }
    }
}
