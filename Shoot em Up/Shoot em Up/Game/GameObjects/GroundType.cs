using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shoot_em_Up
{
    [DataContract]
    class GroundType
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public float Restitution { get; set; }
        [DataMember]
        public float StaticFriction { get; set; }
        [DataMember]
        public float KineticFriction { get; set; }
        [DataMember]
        public string Sprite { get; set; }
        [DataMember]
        public int[] SpriteTileSize { get; set; }
    }
}
