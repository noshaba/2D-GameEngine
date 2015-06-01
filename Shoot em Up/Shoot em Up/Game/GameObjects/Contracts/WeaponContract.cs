using Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Shoot_em_Up
{
    [DataContract]
    class WeaponContract
    {
        [DataMember]
        public int BulletPattern { get; set; }
        [DataMember]
        public String WeaponImage { get; set; }
        [DataMember]
        public String BulletImage { get; set; }
        [DataMember]
        public int FireRate { get; set; }
        [DataMember]
        public Collision.Type BulletCollisionType { get; set; }
    }
}
