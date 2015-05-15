using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public class Tank : StageNPC
    {
        public Tank(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Characters/Ally_Bunker"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Tank(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class Spy : StageNPC
    {
        public Spy(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Characters/Ally_Bunker"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Spy(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }

    public class Flyer : StageNPC
    {
        public Flyer(int height, int width) : base(height, width) { }
        public override string StaticObjectAsset
        {
            get { return "Assets/3D/Characters/Ally_Bunker"; }
        }

        public override IGenerable clone()
        {
            IGenerable instance = new Flyer(Height, Width);
            instance.Structure = (bool[,])Structure.Clone();
            return instance;
        }
    }
}
