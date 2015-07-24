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
        public float[] Speed { get; set; }
        [DataMember]
        public float[] Bend { get; set; }
        [DataMember]
        public float[] Offset { get; set; }
        [DataMember]
        public float Density { get; set; }
        [DataMember]
        public int Damage { get; set; }
        [DataMember]
        public byte[] FillColor { get; set; }
        [DataMember]
        public int OutlineThickness { get; set; }
        [DataMember]
        public byte[] OutlineColor { get; set; }
        [DataMember]
        public float Radius { get; set; }
    }
}
