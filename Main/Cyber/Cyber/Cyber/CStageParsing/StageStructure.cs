﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public enum StageStructureGenerationStrategy
    {
        GENEROUS, GREEDY
    }

    public class StageStructure
    {
        private int lampFrequency = 3;
        public WallsStructure Walls { get; set; }
        public FloorStructure Floor { get; set; }
        public ConcaveCornersStructure ConcaveCorners { get; set; }
        public ConvexCornersStructure ConvexCorners { get; set; }

        public int Count
        {
            get
            {
                return Walls.Count + Floor.Count + ConvexCorners.Count + ConcaveCorners.Count;
            }
        }

        public StageStructure(Stage stage, StageStructureGenerationStrategy strategy)
        {
            Walls = new WallsStructure();
            Floor = new FloorStructure();
            ConcaveCorners = new ConcaveCornersStructure();
            ConvexCorners = new ConvexCornersStructure();

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

            switch (strategy)
            {
                case StageStructureGenerationStrategy.GREEDY:
                    GreedyGenerate(stage, structure);
                    break;
                case StageStructureGenerationStrategy.GENEROUS:
                    GenerousGenerate(stage, structure);
                    break;
            }
            
        }

        private void GenerousGenerate(Stage stage, bool[,] structure)
        {
            for (int h = 0; h < stage.Height; h++)
            {
                for (int w = 0; w < stage.Width; w++)
                {
                    Pair<int, int> currentPoint = new Pair<int, int>(w, h);

                    if (isTrue(structure, w, h))
                    {
                        Floor.Floors.Add(currentPoint);
                    }
                    else
                    {
                        bool up = false, left = false, right = false, down = false;

                        left = isTrue(structure, w - 1, h);
                        right = isTrue(structure, w + 1, h);
                        up = isTrue(structure, w, h - 1);
                        down = isTrue(structure, w, h + 1);

                        if (up && down && left && right)
                        {
                            Column column = new Column(stage.Height, stage.Width);
                            column.Structure[w, h] = true;
                            stage.Objects.Add(column);
                            Floor.Floors.Add(currentPoint);
                            continue;
                        }

                        bool upperLeft = false, upperRight = false, lowerLeft = false, lowerRight = false;

                        upperLeft = isTrue(structure, w - 1, h - 1);
                        upperRight = isTrue(structure, w + 1, h - 1);
                        lowerLeft = isTrue(structure, w - 1, h + 1);
                        lowerRight = isTrue(structure, w + 1, h + 1);

                        int stageCount = Count; // zapamiętuje, ile póki co jest struktur na planszy

                        if (up)
                        {
                            if (left)
                            {
                                ConvexCorners.ConvexCornersLowerRight.Add(currentPoint);
                            }
                            if (right)
                            {
                                ConvexCorners.ConvexCornersLowerLeft.Add(currentPoint);
                            }
                            if (!left && !right)
                            {
                                Walls.WallsDown.Add(currentPoint);
                                if ((w + h) % lampFrequency == 0)
                                {
                                    Lamp lamp = new Lamp(stage.Height, stage.Width);
                                    lamp.Structure[w, h] = true;
                                    lamp.Rotation = 90;
                                    stage.Objects.Add(lamp);
                                }
                            }
                        }
                        else
                        {
                            if (!left && upperLeft)
                            {
                                ConcaveCorners.ConcaveCornersLowerRight.Add(currentPoint);
                            }
                            if (!right && upperRight)
                            {
                                ConcaveCorners.ConcaveCornersLowerLeft.Add(currentPoint);
                            }
                        }
                        if (down)
                        {
                            if (left)
                            {
                                ConvexCorners.ConvexCornersUpperRight.Add(currentPoint);
                            }
                            if (right)
                            {
                                ConvexCorners.ConvexCornersUpperLeft.Add(currentPoint);
                            }
                            if (!left && !right)
                            {
                                Walls.WallsUp.Add(currentPoint);
                                if ((w + h) % lampFrequency == 0)
                                {
                                    Lamp lamp = new Lamp(stage.Height, stage.Width);
                                    lamp.Structure[w, h] = true;
                                    lamp.Rotation = 270;
                                    stage.Objects.Add(lamp);
                                }
                            }
                        }
                        else
                        {
                            if (!left && lowerLeft)
                            {
                                ConcaveCorners.ConcaveCornersUpperRight.Add(currentPoint);
                            }
                            if (!right && lowerRight)
                            {
                                ConcaveCorners.ConcaveCornersUpperLeft.Add(currentPoint);
                            }
                        }
                        if (!up && !down)
                        {
                            if (left)
                            {
                                Walls.WallsRight.Add(currentPoint);
                                if ((w + h) % lampFrequency == 0)
                                {
                                    Lamp lamp = new Lamp(stage.Height, stage.Width);
                                    lamp.Structure[w, h] = true;
                                    lamp.Rotation = 0;
                                    stage.Objects.Add(lamp);
                                }
                            }
                            if (right)
                            {
                                Walls.WallsLeft.Add(currentPoint);
                                if ((w + h) % lampFrequency == 0)
                                {
                                    Lamp lamp = new Lamp(stage.Height, stage.Width);
                                    lamp.Structure[w, h] = true;
                                    lamp.Rotation = 180;
                                    stage.Objects.Add(lamp);
                                }
                            }
                        }
                        if (stageCount < Count) // jeśli coś dodano, to trzeba pod to podłożyć podłogę
                        {
                            Floor.Floors.Add(currentPoint);
                        }
                    }
                }
            }
        }

        private void GreedyGenerate(Stage stage, bool[,] structure)
        {
            for (int h = 0; h < stage.Height; h++)
            {
                for (int w = 0; w < stage.Width; w++)
                {
                    if (isTrue(structure, w, h))
                    {
                        Pair<int, int> currentPoint = new Pair<int, int>(w, h);

                        Floor.Floors.Add(currentPoint);

                        bool up = false, left = false, right = false, down = false;
                        bool upperLeft = false, upperRight = false, lowerLeft = false, lowerRight = false;

                        left = !isTrue(structure, w - 1, h);
                        right = !isTrue(structure, w + 1, h);
                        up = !isTrue(structure, w, h - 1);
                        down = !isTrue(structure, w, h + 1);

                        upperLeft = !isTrue(structure, w - 1, h - 1);
                        upperRight = !isTrue(structure, w + 1, h - 1);
                        lowerLeft = !isTrue(structure, w - 1, h + 1);
                        lowerRight = !isTrue(structure, w + 1, h + 1);

                        if (up)
                        {
                            if (left)
                            {
                                ConcaveCorners.ConcaveCornersUpperLeft.Add(currentPoint);
                            }
                            if (right)
                            {
                                ConcaveCorners.ConcaveCornersUpperRight.Add(currentPoint);
                            }
                            if (!left && !right)
                            {
                                Walls.WallsUp.Add(currentPoint);
                            }
                        }
                        else
                        {
                            if (!left && upperLeft)
                            {
                                ConvexCorners.ConvexCornersUpperLeft.Add(currentPoint);
                            }
                            if (!right && upperRight)
                            {
                                ConvexCorners.ConvexCornersUpperRight.Add(currentPoint);
                            }
                        }

                        if (down)
                        {
                            if (left)
                            {
                                ConcaveCorners.ConcaveCornersLowerLeft.Add(currentPoint);
                            }
                            if (right)
                            {
                                ConcaveCorners.ConcaveCornersLowerRight.Add(currentPoint);
                            }
                            if (!left && !right)
                            {
                                Walls.WallsDown.Add(currentPoint);
                            }
                        }
                        else
                        {
                            if (!left && lowerLeft)
                            {
                                ConvexCorners.ConvexCornersLowerLeft.Add(currentPoint);
                            }
                            if (!right && lowerRight)
                            {
                                ConvexCorners.ConvexCornersLowerRight.Add(currentPoint);
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
                /*
                CornersLowerLeft = new List<Pair<int, int>>();
                CornersLowerRight = new List<Pair<int, int>>();
                CornersUpperLeft = new List<Pair<int, int>>();
                CornersUpperRight = new List<Pair<int, int>>();
                */
            }
            public int Count
            {
                get
                {
                    return WallsDown.Count + WallsLeft.Count + WallsRight.Count + WallsUp.Count;
                    //    + CornersLowerLeft.Count + CornersLowerRight.Count + CornersUpperLeft.Count + CornersUpperRight.Count;
                }
            }

            public List<Pair<int, int>> WallsUp { get; set; }
            public List<Pair<int, int>> WallsDown { get; set; }
            public List<Pair<int, int>> WallsRight { get; set; }
            public List<Pair<int, int>> WallsLeft { get; set; }
            /*
            public List<Pair<int, int>> CornersUpperLeft { get; set; }
            public List<Pair<int, int>> CornersUpperRight { get; set; }
            public List<Pair<int, int>> CornersLowerLeft { get; set; }
            public List<Pair<int, int>> CornersLowerRight { get; set; }
            */
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

        public class ConcaveCornersStructure
        {
            public List<Pair<int, int>> ConcaveCornersUpperLeft { get; set; }
            public List<Pair<int, int>> ConcaveCornersUpperRight { get; set; }
            public List<Pair<int, int>> ConcaveCornersLowerLeft { get; set; }
            public List<Pair<int, int>> ConcaveCornersLowerRight { get; set; }

            public ConcaveCornersStructure()
            {
                ConcaveCornersLowerLeft = new List<Pair<int, int>>();
                ConcaveCornersLowerRight = new List<Pair<int, int>>();
                ConcaveCornersUpperLeft = new List<Pair<int, int>>();
                ConcaveCornersUpperRight = new List<Pair<int, int>>();
            }

            public int Count
            {
                get
                {
                    return ConcaveCornersLowerLeft.Count + ConcaveCornersLowerRight.Count + ConcaveCornersUpperLeft.Count + ConcaveCornersUpperRight.Count;
                }
            }
        }

        public class ConvexCornersStructure
        {
            public List<Pair<int, int>> ConvexCornersUpperLeft { get; set; }
            public List<Pair<int, int>> ConvexCornersUpperRight { get; set; }
            public List<Pair<int, int>> ConvexCornersLowerLeft { get; set; }
            public List<Pair<int, int>> ConvexCornersLowerRight { get; set; }

            public ConvexCornersStructure()
            {
                ConvexCornersLowerLeft = new List<Pair<int, int>>();
                ConvexCornersLowerRight = new List<Pair<int, int>>();
                ConvexCornersUpperLeft = new List<Pair<int, int>>();
                ConvexCornersUpperRight = new List<Pair<int, int>>();
            }

            public int Count
            {
                get
                {
                    return ConvexCornersLowerLeft.Count + ConvexCornersLowerRight.Count + ConvexCornersUpperLeft.Count + ConvexCornersUpperRight.Count;
                }
            }
        }
    }
}
