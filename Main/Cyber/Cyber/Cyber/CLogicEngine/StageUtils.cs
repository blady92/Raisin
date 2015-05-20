using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    public class StageUtils
    {
        public static const double PRZESUNIECIE = 19.5d; 
        public static double BitmapCoordToStageCoord(double coord)
        {
            return coord * PRZESUNIECIE;
        }

        public static double[] BitmapCoordsToStageCoords(params double[] coords)
        {
            double[] result = new double[coords.Length];
            for (int i = 0; i < coords.Length; i++)
            {
                result[i] = BitmapCoordToStageCoord(coords[i]);
            }
            return result;
        }
    }
}
