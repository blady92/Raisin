using Cyber.CStageParsing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return MarkWallBordersAsForbidden(result);
        }

        /// <summary>
        /// Marks all fields with contact with wall as non-free to prevent NPCs beeing stuck at walls
        /// </summary>
        /// <param name="freeSpaceMap"></param>
        /// <returns>Modified free space map</returns>
        public static bool[,] MarkWallBordersAsForbidden(bool[,] freeSpaceMap)
        {
            PrintMap(freeSpaceMap);
            bool[,] newMap = new bool[freeSpaceMap.GetLength(0), freeSpaceMap.GetLength(1)];
            //for each row besides last
            for (int rowi = 0; rowi < freeSpaceMap.GetLength(0); rowi++)
            {
                int coli;
                //for each column besides last
                for (coli = 0; coli < freeSpaceMap.GetLength(1); coli++)
                {
                    newMap[rowi, coli] = true;

                    //previous line

                    /*
                     * +---+---+---+
                     * | ! |   |   |
                     * +---+---+---+
                     * |   | ? |   |
                     * +---+---+---+    ? = !
                     * |   |   |   |
                     * +---+---+---+
                     */
                    if (rowi != 0 && coli != 0 && !freeSpaceMap[rowi - 1, coli - 1])
                    {
                        newMap[rowi, coli] = false;
                    }

                    /*
                     * +---+---+---+
                     * |   | ! |   |
                     * +---+---+---+
                     * |   | ? |   |
                     * +---+---+---+    ? = !
                     * |   |   |   |
                     * +---+---+---+
                     */
                    if (rowi != 0 && !freeSpaceMap[rowi - 1, coli])
                    {
                        newMap[rowi, coli] = false;
                    }

                    /*
                     * +---+---+---+
                     * |   |   | ! |
                     * +---+---+---+
                     * |   | ? |   |
                     * +---+---+---+    ? = !
                     * |   |   |   |
                     * +---+---+---+
                     */
                    if (rowi != 0 && (coli != freeSpaceMap.GetLength(1) - 1) && !freeSpaceMap[rowi - 1, coli + 1])
                    {
                        newMap[rowi, coli] = false;
                    }

                    //current line

                    /*
                     * +---+---+---+
                     * |   |   |   |
                     * +---+---+---+
                     * | ! | ? |   |
                     * +---+---+---+    ? = !
                     * |   |   |   |
                     * +---+---+---+
                     */
                    if (coli != 0 && !freeSpaceMap[rowi, coli - 1])
                    {
                        newMap[rowi, coli] = false;
                    }

                    /*
                     * +---+---+---+
                     * |   |   |   |
                     * +---+---+---+
                     * |   |? !|   |
                     * +---+---+---+    ? = !
                     * |   |   |   |
                     * +---+---+---+
                     */
                    if (!freeSpaceMap[rowi, coli])
                    {
                        newMap[rowi, coli] = false;
                    }

                    /*
                     * +---+---+---+
                     * |   |   |   |
                     * +---+---+---+
                     * |   | ? | ! |
                     * +---+---+---+    ? = !
                     * |   |   |   |
                     * +---+---+---+
                     */
                    if ((coli != freeSpaceMap.GetLength(1) - 1) && !freeSpaceMap[rowi, coli + 1])
                    {
                        newMap[rowi, coli] = false;
                    }

                    //next line

                    /*
                     * +---+---+---+
                     * |   |   |   |
                     * +---+---+---+
                     * |   | ? |   |
                     * +---+---+---+    ? = !
                     * | ! |   |   |
                     * +---+---+---+
                     */
                    if ((rowi != freeSpaceMap.GetLength(0) - 1) && coli != 0 && !freeSpaceMap[rowi + 1, coli - 1])
                    {
                        newMap[rowi, coli] = false;
                    }

                    /*
                     * +---+---+---+
                     * |   |   |   |
                     * +---+---+---+
                     * |   | ? |   |
                     * +---+---+---+    ? = !
                     * |   | ! |   |
                     * +---+---+---+
                     */
                    if ((rowi != freeSpaceMap.GetLength(0) - 1) && !freeSpaceMap[rowi + 1, coli])
                    {
                        newMap[rowi, coli] = false;
                    }

                    /*
                     * +---+---+---+
                     * |   |   |   |
                     * +---+---+---+
                     * |   | ? |   |
                     * +---+---+---+    ? = !
                     * |   |   | ! |
                     * +---+---+---+
                     */
                    if ((rowi != freeSpaceMap.GetLength(0) - 1) && (coli != freeSpaceMap.GetLength(1) - 1) && !freeSpaceMap[rowi + 1, coli + 1])
                    {
                        newMap[rowi, coli] = false;
                    }
                }
            }
            PrintMap(newMap);
            return newMap;
        }

        public static void PrintMap(bool[,] map)
        {
#if DEBUG
            Debug.WriteLine("Started printing the map");
            for (int row = 0; row < map.GetLength(1); row++)
            {
                for (int col = 0; col < map.GetLength(0); col++)
                {
                    Debug.Write(map[col,row] ? " _ " : " # ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("Finished printing the map");
#endif
        }

        public static void PrintPath(bool[,] map, IEnumerable<Position> path)
        {
#if DEBUG
            Debug.WriteLine("Started printing path on map");
            for (int row = 0; row < map.GetLength(1); row++)
            {
                for (int col = 0; col < map.GetLength(0); col++)
                {
                    if (path.Any(o => o.X == col && o.Y == row))
                    {
                        Debug.Write(" $ ");
                    }
                    else if (!map[col,row])
                    {
                        Debug.Write(" # ");
                    }
                    else
                    {
                        Debug.Write(" _ ");
                    }
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("Finished printing path on map");
#endif
        }

        public class PositionComparer : IEqualityComparer<Position>
        {
            public bool Equals(Position x, Position y)
            {
                if (x != null)
                {
                    return x.Equals(y);
                }
                return false;
            }

            public int GetHashCode(Position obj)
            {
                if (obj != null)
                {
                    return obj.GetHashCode();
                }
                throw new ArgumentNullException();
            }
        }
    }
}
