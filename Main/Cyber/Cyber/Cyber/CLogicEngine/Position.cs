using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Cyber.CLogicEngine
{
    [DataContract]
    public class Position : IEquatable<Position>
    {
        [DataMember]
        private int x, y;

        public int Y
        {
            get { return y; }
        }

        public int X
        {
            get { return x; }
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Position))
            {
                return base.Equals(obj);
            }
            else
            {
                return Equals((Position)obj);
            }
        }

        public bool Equals(Position other)
        {
            if (other == null)
            {
                return false;
            }
            return other.X == this.X && other.Y == this.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
