using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    [DataContract]
    class Spawn
    {

        [DataMember]
        public String type { get; set; }

        [DataMember]
        public int spawnAt { get; set; }
        [DataMember]
        public int x { get; set; }
        [DataMember]
        public int y { get; set; }
    }
}
