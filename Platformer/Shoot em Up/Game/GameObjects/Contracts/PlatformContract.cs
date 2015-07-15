using ImageProcessing;
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
        public int Rotation { get; set; }
        [DataMember]
        public string SpritePath { get; set; }
        [DataMember]
        public int[] SpriteSize { get; set; }
        [DataMember]
        public int[] SpriteTileSize { get; set; }
        [DataMember]
        public int[] Tiles { get; set; }
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
           /* String path = "../Content/platform.png";
            Vector2f pos = new Vector2f(800,500);
            int tileSize = 100;
            int[] tiles = new int[] { 0,1,1,1,1,2};*/

            /*
            //number of platforms in json
            for (int i = 0; i < 1; i++)
            {
               //add platform
            }*/
            //Game.Add(new Platform(SpritePath, new Vector2f(Position[0], Position[1]), Rotation, SpriteSize, SpriteTileSize, Tiles, 
             //   KineticFriction, StaticFriction, Restitution, Density));
            Spawner s = new Spawner(new Vector2f(Position[0], Position[1]));
            s.Add(new Platform(SpritePath, new Vector2f(Position[0], Position[1]), Rotation, SpriteSize, SpriteTileSize, Tiles,
                KineticFriction, StaticFriction, Restitution, Density));
            Game.spawners.Add(s);

        }
    }
}
