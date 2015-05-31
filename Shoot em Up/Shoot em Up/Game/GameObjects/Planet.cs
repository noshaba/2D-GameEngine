using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFML.Graphics;

namespace Shoot_em_Up
{
    [DataContract]
    class Planet
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string BackgroundImage { get; set; }
        [DataMember]
        public string BackgroundMusic { get; set; }
        [DataMember]
        public float[] Gravity { get; set; }
        [DataMember]
        public bool Friction { get; set; }
        [DataMember]
        public float Damping { get; set; }
        [DataMember]
        public Achievement Achievement { get; set; }
        [DataMember]
        public int AchievementMinRequirement { get; set; }
        [DataMember]
        public GroundType[] GroundTypes { get; set; }
        [DataMember]
        public int[] Ground { get; set; }

        public Sprite backgroundImageSprite;

    }
}
