﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Platformer
{
    [DataContract]
    class Faction
    {
        public enum Type { None, Player, AI }

        [DataMember]
        public Type ID { get; set; }
        [DataMember]
        public bool GainableRep { get; set; }
        [DataMember]
        public int[] Reputation { get; set; }
    }
}