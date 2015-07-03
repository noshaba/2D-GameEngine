using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFML.Graphics;
using SFML.System;

namespace Platformer
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
        public int Length { get; set; }
        [DataMember]
        public Achievement Achievement { get; set; }
        [DataMember]
        public int AchievementMinRequirement { get; set; }
        [DataMember]
        public GroundType[] GroundTypes { get; set; }
        [DataMember]
        public int[] Ground { get; set; }
        [DataMember]
        public int[] GroundTiles { get; set; }
        [DataMember]
        public int[] GroundTileHeight { get; set; }

        public Sprite backgroundSprite;

        public void Init()
        {
            backgroundSprite = new Sprite(new Texture(BackgroundImagePath), new IntRect(0, 0, Length, 700));
            backgroundSprite.Texture.Repeated = true;
        }

        public void AddGround()
        {
           for (int i = 0; i < Ground.Length; ++i)
            {
                Game.Add(GroundTypes[Ground[i]].GetTile(i, GroundTiles[i], GroundTileHeight[i]));
            }
        }
    }
}
