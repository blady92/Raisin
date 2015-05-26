using Cyber.CGameStateEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Cyber.CLogicEngine
{
    class JSONSerializer : GameSerializer
    {
        public void Serialize(DataContainer state, string path)
        {
            FileStream output = new FileStream(path, FileMode.Create);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DataContainer));
            ser.WriteObject(output, state);
            output.Close();
        }

        public DataContainer Deserialize(string path)
        {
            FileStream input = new FileStream(path, FileMode.Open);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DataContainer));
            object o = ser.ReadObject(input);
            input.Close();
            return (DataContainer)o;
        }
    }
}
