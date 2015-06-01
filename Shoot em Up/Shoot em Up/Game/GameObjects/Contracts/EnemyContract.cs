using Physics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Shoot_em_Up
{

    /*   {
         "Density": 0,
         "Restitution": 0.5,
         "StaticFriction": 0.5,
         "KineticFriction": 0.1,
         "CollisionType": 1,
         "Faction": 0,
         "Health": 25,
         "Points": 10,
         "Damage": 5,
         "SpawnPatternID": 0,
         "NumberOfObjects": 15,
         "SpawnStartPosition": [240, 200],
         "SpritePath": "sprite_path",
         "SpriteSize": [100, 100],
         "SpriteTileSize": [100, 100],
         "Weapon": {
            "BulletPattern": "name",
            "WeaponImage": "image_path",
            "BulletImage": "image_path",
            "BulletCollisionType": 0
         }
      },
     */

    [DataContract]
    class EnemyContract
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
        public Collision.Type CollisionType { get; set; }
        [DataMember]
        public Faction.Type Faction { get; set; }
        [DataMember]
        public int Health { get; set; }
        [DataMember]
        public int Points { get; set; }
        [DataMember]
        public int Damage { get; set; }
        [DataMember]
        public int SpawnPatternID { get; set; }
        [DataMember]
        public int NumberOfObjects { get; set; }
        [DataMember]
        public int[] SpawnStartPosition { get; set; }
        [DataMember]
        public String SpritePath { get; set; }
        [DataMember]
        public int[] SpriteSize { get; set; }
        [DataMember]
        public int[] SpriteTileSize { get; set; }
        [DataMember]
        public int MovementPattern { get; set; }
        [DataMember]
        public WeaponContract[] Weapons {get;set;}

        public void Init()
        {
            for (int i = 0; i < NumberOfObjects; i++)
            {
                Game.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern));
                //here adjust SpawnStartPosition according to spawnPattern 
            }
        }
    }
}
