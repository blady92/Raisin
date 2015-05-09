using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public class StageStructure
    {
        public WallsStructure Walls { get; set; }
        public FloorStructure Floor { get; set; }

        public int Count
        {
            get
            {
                return Walls.Count + Floor.Count;
            }
        }

        public StageStructure(Stage stage)
        {
            Walls = new WallsStructure();
            Floor = new FloorStructure();

            bool [,] structure = new bool [stage.Width, stage.Height];

            foreach (Room room in stage.Rooms)
            {
                for (int h = 0; h < room.Height; h++)
                {
                    for (int w = 0; w < room.Width; w++)
                    {
                        structure[w, h] |= room.Structure[w, h];
                    }
                }
            }

            foreach (Corridor corridor in stage.Corridors)
            {
                for (int h = 0; h < corridor.Height; h++)
                {
                    for (int w = 0; w < corridor.Width; w++)
                    {
                        structure[w, h] |= corridor.Structure[w, h];
                    }
                }
            }

            for (int h = 0; h < stage.Height; h++)
            {
                for (int w = 0; w < stage.Width; w++)
                {
                    if (isTrue(structure, w, h))
                    {
                        Pair<int, int> currentPoint = new Pair<int, int>(w, h);

                        Floor.Floors.Add(currentPoint);

                        bool up = false, left = false, right = false, down = false;

                        left = !isTrue(structure, w - 1, h);
                        right = !isTrue(structure, w + 1, h);
                        up = !isTrue(structure, w, h - 1);
                        down = !isTrue(structure, w, h + 1);

                        if (up)
                        {
                            if (left)
                            {
                                Walls.CornersUpperLeft.Add(currentPoint);
                            }
                            if (right)
                            {
                                Walls.CornersUpperRight.Add(currentPoint);
                            }
                            if (!left && !right)
                            {
                                Walls.WallsUp.Add(currentPoint);
                            }
                        }
                        if (down)
                        {
                            if (left)
                            {
                                Walls.CornersLowerLeft.Add(currentPoint);
                            }
                            if (right)
                            {
                                Walls.CornersLowerRight.Add(currentPoint);
                            }
                            if (!left && !right)
                            {
                                Walls.WallsDown.Add(currentPoint);
                            }
                        }
                        if (!up && !down)
                        {
                            if (left)
                            {
                                Walls.WallsLeft.Add(currentPoint);
                            }
                            if (right)
                            {
                                Walls.WallsRight.Add(currentPoint);
                            }
                        }
                    }
                }
            }
        }
        private bool isTrue(bool[,] structure, int x, int y)
        {
            try
            {
                return structure[x, y];
            } catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public class WallsStructure
        {
            public WallsStructure()
            {
                WallsUp = new List<Pair<int, int>>();
                WallsDown = new List<Pair<int, int>>();
                WallsLeft = new List<Pair<int, int>>();
                WallsRight = new List<Pair<int, int>>();
                CornersLowerLeft = new List<Pair<int, int>>();
                CornersLowerRight = new List<Pair<int, int>>();
                CornersUpperLeft = new List<Pair<int, int>>();
                CornersUpperRight = new List<Pair<int, int>>();
            }
            public int Count
            {
                get
                {
                    return WallsDown.Count + WallsLeft.Count + WallsRight.Count + WallsUp.Count
                        + CornersLowerLeft.Count + CornersLowerRight.Count + CornersUpperLeft.Count + CornersUpperRight.Count;
                }
            }

            public List<Pair<int, int>> WallsUp { get; set; }
            public List<Pair<int, int>> WallsDown { get; set; }
            public List<Pair<int, int>> WallsRight { get; set; }
            public List<Pair<int, int>> WallsLeft { get; set; }
            public List<Pair<int, int>> CornersUpperLeft { get; set; }
            public List<Pair<int, int>> CornersUpperRight { get; set; }
            public List<Pair<int, int>> CornersLowerLeft { get; set; }
            public List<Pair<int, int>> CornersLowerRight { get; set; }
        }

        public class FloorStructure
        {
            public FloorStructure()
            {
                Floors = new List<Pair<int, int>>();
            }
            public List<Pair<int, int>> Floors { get; set; }

            public int Count
            {
                get
                {
                    return Floors.Count;
                }
            }
        }
    }
}
