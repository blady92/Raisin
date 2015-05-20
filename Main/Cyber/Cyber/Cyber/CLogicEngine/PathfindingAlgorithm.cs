using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    public interface IPathfindingAlgorithm
    {
        /// <summary>
        /// Search map graph to find shortest path from one point to another
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        List<Vector3> FindWayToPlace(Vector3 from, Vector3 to);
        Vector3 GetNextPosition(Moves nextMove);
    }
}
