using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Platformer
{
    class JSONManager
    {
        public static T deserializeJson<T>(string result)
        {
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result)))
            {
                ms.Position = 0;
                return (T)jsonSer.ReadObject(ms);
            }
        }
    }
}
