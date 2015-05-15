using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public abstract class GenerableStructureImplementation : IGenerable
    {
        public int Height { get; set; }
        public int Width { get; set; }
        protected bool[,] structure;

        public GenerableStructureImplementation(int height, int width)
        {
            structure = new bool[width, height];
            Width = width;
            Height = height;
        }

        public bool[,] Structure
        {
            get
            {
                return structure;
            }
            set
            {
                structure = value;
            }
        }

        abstract public IGenerable clone();
        abstract public bool IsSingleBlock();
    }
    public class Room : GenerableStructureImplementation
    {
        public Room(int height, int width) : base(height, width) { }
        override public IGenerable clone()
        {
            IGenerable instance = new Room(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }

        override public bool IsSingleBlock()
        {
            return false;
        }
    }

    public class Corridor : GenerableStructureImplementation
    {

        public Corridor(int height, int width) : base(height, width) { }
        override public IGenerable clone()
        {
            IGenerable instance = new Corridor(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }

        override public bool IsSingleBlock()
        {
            return false;
        }
    }

    public abstract class StageObject : GenerableStructureImplementation
    {
        public StageObject(int height, int width) : base(height, width) 
        {
        }
        public abstract string StaticObjectAsset { get; }

        override public bool IsSingleBlock()
        {
            return true;
        }
        public Pair<int, int> GetBlock()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Structure[j, i])
                    {
                        return new Pair<int, int>(j, i);
                    }
                }
            }
            return null;
        }
    }

    public abstract class StageNPC : GenerableStructureImplementation
    {
        public StageNPC(int height, int width) : base(height, width)
        {
            Rotation = 0;
        }
        public int Rotation { get; set; }
        public abstract string StaticObjectAsset { get; }
        override public bool IsSingleBlock()
        {
            return true;
        }
        public Pair<int, int> GetBlock()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Structure[j, i])
                    {
                        return new Pair<int, int>(j, i);
                    }
                }
            }
            return null;
        }
    }

    public class PlayerPosition : GenerableStructureImplementation
    {
        public PlayerPosition(int height, int width) : base(height, width)
        {
        }

        public override IGenerable clone()
        {
            IGenerable instance = new PlayerPosition(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }

        public override bool IsSingleBlock()
        {
            return true;
        }
        public Pair<int, int> GetBlock()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Structure[j, i])
                    {
                        return new Pair<int, int>(j, i);
                    }
                }
            }
            return null;
        }
    }

    public class NPCDirection : GenerableStructureImplementation
    {
        public NPCDirection(int height, int width) : base(height, width) { }

        public override IGenerable clone()
        {
            IGenerable instance = new NPCDirection(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }

        public override bool IsSingleBlock()
        {
            return true;
        }
        public Pair<int, int> GetBlock()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Structure[j, i])
                    {
                        return new Pair<int, int>(j, i);
                    }
                }
            }
            return null;
        }
    }
}
