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
        public int[] SpriteTileSize { get; set; } //not included yet
        [DataMember]
        public int[] TileIndices { get; set; } //not included yet

        public void Init()
        {
            /*for (int i = 0; i<NumberOfObjects; i++) {
                // bool hover, Collision.Type type, int[] spriteTileSize, int[] tileIndices, int animationIndex, float density, float restitution, float staticFriction, float kineticFriction, String texturePath, int[] spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction
                Game.Add(new Obstacle(Hover,CollisionType, SpriteTileSize, TileIndices, 0, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]), 0,Health, Points, Damage, Game.factions[(int) Faction]));
                //here adjust SpawnStartPosition according to spawnPattern 
                SpawnStartPosition[0] += EMath.random.Next(500, 1000);
                SpawnStartPosition[1] += EMath.random.Next(0, 100);
            }*/

            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            for (int i = 0; i < NumberOfObjects; i++)
            {
                //s.Add(new Obstacle(Hover, CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]), Health, Points, Damage, Game.factions[(int)Faction]));
                //here adjust SpawnStartPosition according to spawnPattern 
                SpawnStartPosition[0] += EMath.random.Next(500, 1000);
                SpawnStartPosition[1] += EMath.random.Next(0, 100);
            }
           // Game.spawners.Add(s);
        }
    }
}
