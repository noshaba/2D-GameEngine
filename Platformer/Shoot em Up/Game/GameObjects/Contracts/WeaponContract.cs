using Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Platformer
{
    [DataContract]
    class WeaponContract
    {
        [DataMember]
        public int FireRate { get; set; }
        [DataMember]
        public int MaxBulletLifetime { get; set; }
        [DataMember]
        public BulletShot[] Shoot { get; set; }
    }
}
