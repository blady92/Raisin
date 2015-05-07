using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CStageParsing
{
    public class Stage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Corridor> Corridors { get; set; }
        public List<StageObject> Objects { get; set; }
        public List<StageNPC> NPCs { get; set; }

        public Stage()
        {
            Rooms = new List<Room>();
            Corridors = new List<Corridor>();
            Objects = new List<StageObject>();
            NPCs = new List<StageNPC>();
        }
    }
}
