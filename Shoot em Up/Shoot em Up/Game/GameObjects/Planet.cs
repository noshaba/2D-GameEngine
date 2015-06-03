using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFML.Graphics;
using SFML.System;

namespace Shoot_em_Up
{
    [DataContract]
    class Planet
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string BackgroundImagePath { get; set; }
        [DataMember]
        public string BackgroundMusicPath { get; set; }
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

        public Sprite backgroundSprite;

        public void Init()
        {
            backgroundSprite = new Sprite(new Texture(BackgroundImagePath), new IntRect(0,0,8000, 700));
        }

        public void AddGround()
        {
            for (int i = 0; i < Ground.Length; ++i)
                Game.Add(GroundTypes[Ground[i]].GetTile(i));
        }
    }
}
