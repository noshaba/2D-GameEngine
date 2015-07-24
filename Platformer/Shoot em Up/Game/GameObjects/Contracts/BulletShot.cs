using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Platformer
{
    [DataContract]
    class BulletShot
    {
        [DataMember]
        public float[] BulletSpeed { get; set; }
        [DataMember]
        public float[] Bend { get; set; }
        [DataMember]
        public float[] Offset { get; set; }
    }
}
