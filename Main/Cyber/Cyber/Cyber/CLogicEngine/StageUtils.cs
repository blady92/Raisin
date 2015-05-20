using Cyber.CStageParsing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    public class StageUtils
    {
        public static readonly double PRZESUNIECIE = 19.5d; 
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

        public static Vector3 BitmapCoordsToStageVector(Position coords)
        {
            double[] param = new double[2];
            param[0] = coords.X;
            param[1] = coords.Y;
            double[] res = BitmapCoordsToStageCoords(param);
            Vector3 result = new Vector3((float)res[0], (float)res[1], 0f);
            return result;
        }

        public static Position StageVectorToBitmapCoords(Vector3 coords)
        {
            return new Position((int)(coords.X / PRZESUNIECIE), (int)(coords.Y / PRZESUNIECIE));
        }

        public static bool[,] RoomListToFreeSpaceMap(List<Room> rooms)
        {
            bool[,] result =  new bool[rooms[0].Width, rooms[0].Height];
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = 0; j < rooms[i].Width; j++)
                {
                    for (int k = 0; k < rooms[i].Height; k++)
                    {
                        result[j,k] |= rooms[i].Structure[j, k];
                    }
                }
            }
            return result;
        }
    }
}
