using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyber.CItems.CStaticItem;

namespace Cyber.CAdditionalLibs
{
    class VectorComparator : IComparable<StaticItem>
    {
        public float x { get; set; }
        public float y { get; set; }
        public int CompareTo(StaticItem item)
        {
            if (this.x == item.Position.X)
            {
                return 1;
            }
            return item.Position.Y.CompareTo(this.y);
        }
    }
}
