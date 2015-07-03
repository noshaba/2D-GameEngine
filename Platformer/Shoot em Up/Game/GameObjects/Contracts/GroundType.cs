﻿using System;
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
        public int[] SpriteTileSize { get; set; }
        [DataMember]
        public int[] SpriteSize { get; set; }

        public int Tiles
        {
            get
            {
                return (SpriteSize[0] / SpriteTileSize[0]) * (SpriteSize[1] / SpriteTileSize[1]);
            }
        }
        public GroundTile GetTile(int index, int tileIndex, int tileHeight)
        {
            // float restitution, float staticFriction, float kineticFriction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, int index, int height
            return new GroundTile(Restitution, StaticFriction, KineticFriction, SpritePath, SpriteTileSize, SpriteSize, new int[] {tileIndex}, 0,index, tileHeight);
        }
    }
}
