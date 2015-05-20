using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    public class Position
    {
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
    }
}
