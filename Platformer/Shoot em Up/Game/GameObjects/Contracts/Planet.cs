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
        public int[] PortalPosition { get; set; }
        [DataMember]
        public string PortalSprite {get;set;}
        [DataMember]
        public int[] PortalSpriteSize { get; set; }
        [DataMember]
        public int[] PortalTileSize { get; set; }
        [DataMember]
        public int[] PortalOpen { get; set; }
        [DataMember]
        public int[] PortalClosed { get; set; }
        [DataMember]
        public int RequiredPoints { get; set; }
        [DataMember]
        public int KillPercentage { get; set; }
        [DataMember]
        public string BackgroundTile { get; set; }
        [DataMember]
        public float[] Gravity { get; set; }
        [DataMember]
        public float Damping { get; set; }
        [DataMember]
        public int[] Size { get; set; }
        [DataMember]
        public int[] WindowSize { get; set; }
        [DataMember]
        public GroundType[] GroundTypes { get; set; }
        [DataMember]
        public int[] Ground { get; set; }
        [DataMember]
        public int[] GroundTiles { get; set; }
        [DataMember]
        public int[] GroundTileHeight { get; set; }

        public Sprite sky;

        public void Init()
        {
            sky = new Sprite(new Texture(BackgroundTile), new IntRect(0, 0, WindowSize[0], WindowSize[1]));
            sky.Texture.Repeated = true;
            sky.Origin = new Vector2f(WindowSize[0], WindowSize[1]) * 0.5f;
        }

        public void AddGround()
        {
           for (int i = 0; i < Ground.Length; ++i)
            {
               Game.Add(GroundTypes[Ground[i]].GetTile(i, GroundTiles[i], GroundTileHeight[i]));
               for (int j = 0; j < GroundTileHeight[i] - 1; ++j)
                   Game.Add(GroundTypes[Ground[i]].GetTile(i, 0, j+1));
            }
        }
    }
}
