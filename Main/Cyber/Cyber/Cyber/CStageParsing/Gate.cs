using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public class Gate : StageObject
    {
        public Gate(int height, int width) : base(height, width) { }

        public override IGenerable clone()
        {
            IGenerable instance = new Gate(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }

        public override bool IsSingleBlock()
        {
            return false;
        }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Wall_Lasers"; }
        }
    }

    public class GateBounding : StageObject
    {
        public GateBounding(int height, int width) : base(height, width)
        {
        }

        public override IGenerable clone()
        {
            IGenerable instance = new GateBounding(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }

        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Interior/Interior_Wall_Lasers"; }
        }
    }
}
