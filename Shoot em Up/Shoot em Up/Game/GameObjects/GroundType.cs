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
        public string SpritePath { get; set; }
        [DataMember]
        public uint[] SpriteTileSize { get; set; }

        public Sprite groundSprite;
        public uint tileNumber;

        public void Init()
        {
            groundSprite = new Sprite(new Texture(SpritePath));
            tileNumber = (SpriteTileSize[0] / groundSprite.Texture.Size.X) * (SpriteTileSize[1] / groundSprite.Texture.Size.Y);
        }

        public GroundTile getTile(Vector2f position)
        {
            return new GroundTile(Restitution, StaticFriction, KineticFriction, new Texture(SpritePath), position, 0);
        }
    }
}
