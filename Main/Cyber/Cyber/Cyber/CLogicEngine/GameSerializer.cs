using Cyber.CGameStateEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.CLogicEngine
{
    interface GameSerializer
    {
        void Serialize(DataContainer state, String path);
        DataContainer Deserialize(String path);
    }
}
