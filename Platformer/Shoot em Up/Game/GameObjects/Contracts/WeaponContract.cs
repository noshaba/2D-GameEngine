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
    /*    [DataMember]
        public int BulletPattern { get; set; }
        [DataMember]
        public String BulletImage { get; set; }
        [DataMember]
        public int FireRate { get; set; }
        [DataMember]
        public Collision.Type BulletCollisionType { get; set; }*/

                /*
         * "Weapon": 
         * {
         *      "BulletDamage" : 5,
         *      "BulletPath" : "...",
         *      "SpriteTileSize" : [10, 10],
         *      "SpriteSize" : [10, 10],
         *      "TileIndices" : [0],
         *      "AnimationIndex" : 0,
         *      "BulletDensity" : 0.5,
         *      "FireRate" : 100,
         *      "Shoot" : 
         *      [
         *          {
         *              "BulletSpeed" : [5, 0],
         *              "Bend" : [0, 0],
         *              "Offset" : [0, 0]
         *          }
         *      ]
         * }
         */

        [DataMember]
        public int BulletDamage { get; set; }
        [DataMember]
        public string BulletPath { get; set; }
        [DataMember]
        public int[] SpriteTileSize { get; set; }
        [DataMember]
        public int[] SpriteSize { get; set; }
        [DataMember]
        public int[] TileIndices { get; set; }
        [DataMember]
        public int AnimationIndex { get; set; }
        [DataMember]
        public float BulletDensity { get; set; }
        [DataMember]
        public int FireRate { get; set; }
        [DataMember]
        public BulletShot[] Shoot { get; set; }
    }
}
