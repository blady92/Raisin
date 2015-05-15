using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public class Chair : StageObject
    {
        public Chair(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Chair"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Chair(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class Table : StageObject
    {
        public Table(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Table"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Table(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class Terminal : StageObject
    {
        public Terminal(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Terminal"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Terminal(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class Lamp : StageObject
    {
        public Lamp(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Lamp"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Lamp(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class OxygenGenerator : StageObject
    {
        public OxygenGenerator(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Oxygen_Generator"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new OxygenGenerator(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class Column : StageObject
    {
        public Column(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Collumn"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Column(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }
}
