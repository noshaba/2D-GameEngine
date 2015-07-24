﻿using ImageProcessing;
using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    [DataContract]
    class PlatformContract
    {
        [DataMember]
        public float Density { get; set; }
        [DataMember]
        public float Restitution { get; set; }
        [DataMember]
        public float StaticFriction { get; set; }
        [DataMember]
        public float KineticFriction { get; set; }
        [DataMember]
        public int[] Position { get; set; }
        [DataMember]
        public bool Rotateable { get; set; }
        [DataMember]
        public int Rotation { get; set; }
        [DataMember]
        public bool Breakable { get; set; }
        [DataMember]
        public string SpritePath { get; set; }
        [DataMember]
        public int[] SpriteSize { get; set; }
        [DataMember]
        public int[] SpriteTileSize { get; set; }
        [DataMember]
        public int[] Tiles { get; set; }
        [DataMember]
        public int[] Animation { get; set; }
        [DataMember]
        public int[] Path { get; set; }
        /*
            "Density": 0.01,
            "Restitution": 0,
            "StaticFriction": 2,
            "KineticFriction": 2,
            "Position": [800,500],
            "SpritePath": "../Content/platform.png",
            "SpriteSize": [300,100],
            "SpriteTileSize": [100, 100],
            "Tiles": [0,1,2]
         * 
         * type statioary || tilt || move
         * movePoints[]
         */
        public void Init()
        {
            Game.Add(new Platform(Animation, Breakable, Rotateable, Path, SpritePath, new Vector2f(Position[0], Position[1]), Rotation, SpriteSize, SpriteTileSize, Tiles,
                KineticFriction, StaticFriction, Restitution, Density));
        }
    }
}
