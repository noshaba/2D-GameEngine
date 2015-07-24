using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Physics;
using Maths;

namespace Platformer {

    /*
     * Example Obstacle as found in json
        "Density": 0,
        "Restitution": 0.5,
        "StaticFriction": 0.5,
        "KineticFriction": 0.1,
        "CollisionType": 1,
        "Health": 25,
        "Points": 10,
        "Damage": 5,
        "SpawnPatternID": 0,
        "NumberOfObjects": 15,
        "SpawnStartPosition": [240, 200],
        "SpritePath": "sprite_path",
        "SpriteSize": [240, 200],
        "SpriteTileSize": [100, 100]
     */
    [DataContract]
    class ObstacleContract 
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
        public bool Hover { get; set; }
        [DataMember]
        public int  Health { get; set; }
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
        public int[] TileIndices { get; set; } //not included yet

        public void Init()
        {
            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            int x, y;
            for (int i = 0; i < NumberOfObjects; i++)
            {
                x = SpawnStartPosition[0] + i * SpriteTileSize[0];
                y = SpawnStartPosition[1];
                s.Add(new Obstacle(Hover, CollisionType, SpriteTileSize, new int[] {0}, 0, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]), 0, Health, Points, Damage, Game.factions[(int)Faction]));
                Game.spawners.Add(s);
            }
           // Game.spawners.Add(s);
        }
    }
}
