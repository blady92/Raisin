using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyber.CItems.CStaticItem
{
    class Decoration : StaticItem
    {
        private itemType type;

        internal enum itemType
        {
            terminal,
            wall,
            decoration //coś jak krzesła, stoły etc
        }

        public itemType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Decoration(string path, Vector3 position, itemType type) : base(path, position)
        {
            type = type;
        }

    }
}
