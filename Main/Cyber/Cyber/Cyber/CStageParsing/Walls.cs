using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public class Walls
    {
        public List<Pair<int, int>> WallsUp { get; set; }
        public List<Pair<int, int>> WallsDown { get; set; }
        public List<Pair<int, int>> WallsRight { get; set; }
        public List<Pair<int, int>> WallsLeft { get; set; }

        public int Count
        {
            get
            {
                return WallsDown.Count + WallsLeft.Count + WallsRight.Count + WallsUp.Count;
            }
        }

        public Walls(Stage stage)
        {
            WallsUp = new List<Pair<int, int>>();
            WallsDown = new List<Pair<int, int>>();
            WallsLeft = new List<Pair<int, int>>();
            WallsRight = new List<Pair<int, int>>();

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
            /*
            foreach (Room room in stage.Rooms)
            {
                bool [,] structure = room.Structure;
                for (int h = 0; h < room.Height; h++)
                {
                    for (int w = 0; w < room.Width; w++)
                    {
                        if (isTrue(structure,w,h))
                        {
                            if(!isTrue(structure, w - 1, h))
                            {
                                WallsLeft.Add(new Pair<int, int>(w, h));
                            } 
                            if (!isTrue(structure, w + 1, h))
                            {
                                WallsRight.Add(new Pair<int, int>(w, h));
                            } 
                            if (!isTrue(structure, w, h - 1))
                            {
                                WallsUp.Add(new Pair<int, int>(w, h));
                            } 
                            if (!isTrue(structure, w, h + 1))
                            {
                                WallsDown.Add(new Pair<int, int>(w, h));
                            }
                        }
                    }
                }
            }
            */
            for (int h = 0; h < stage.Height; h++)
            {
                for (int w = 0; w < stage.Width; w++)
                {
                    if (isTrue(structure, w, h))
                    {
                        if (!isTrue(structure, w - 1, h))
                        {
                            WallsLeft.Add(new Pair<int, int>(w, h));
                        }
                        if (!isTrue(structure, w + 1, h))
                        {
                            WallsRight.Add(new Pair<int, int>(w, h));
                        }
                        if (!isTrue(structure, w, h - 1))
                        {
                            WallsUp.Add(new Pair<int, int>(w, h));
                        }
                        if (!isTrue(structure, w, h + 1))
                        {
                            WallsDown.Add(new Pair<int, int>(w, h));
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
    }
}
