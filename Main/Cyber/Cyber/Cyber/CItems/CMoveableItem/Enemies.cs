using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyber.CItems
{
    class Enemies : DynamicItem
    {
        private EnemyType enemy;

        internal enum EnemyType
        {
            spy,
            flyer,
            tank
        }

        public EnemyType Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }

        public Enemies(string path, Vector3 position, EnemyType type)
            : base(path, position)
        {
            enemy = type;
        }

    }
}
