using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    class StraightWayAlgorithm : IPathfindingAlgorithm
    {
        public List<Microsoft.Xna.Framework.Vector3> FindWayToPlace(Microsoft.Xna.Framework.Vector3 from, Microsoft.Xna.Framework.Vector3 to)
        {
            List<Vector3> result = new List<Vector3>();
            result.Add(to);
            return result;
        }

        public Position GetNextPosition(Position pos, Moves nextMove)
        {
            throw new NotImplementedException();
        }
    }
}
