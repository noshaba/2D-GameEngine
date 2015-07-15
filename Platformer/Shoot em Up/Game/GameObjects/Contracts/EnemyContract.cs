using Physics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Platformer
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
        public WeaponContract Weapon { get; set; }

        public void Init()
        {
            //here adjust SpawnStartPosition according to spawnPattern 
            switch (SpawnPatternID) {
                case 0:
                    Block();
                    break;
                case 1:
                    ZigZag();
                    break;
                case 2:
                    Diagonal();
                    break;
                default:
                    Standard();
                    break;
            } 
        }

        private void Standard()
        {
            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            int x, y;
            for (int i = 0; i < NumberOfObjects; i++)
            {
                x = SpawnStartPosition[0] + i * SpriteTileSize[0];
                y = SpawnStartPosition[1];
                //Collision.Type type, int[] tileSize, int[] tileIndices,float density, int animationIndex, float restitution, float staticFriction, float kineticFriction, String texturePath, int[]spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction, int pattern, WeaponContract w
                //s.Add(new Enemy(CollisionType, SpriteTileSize, Density, 0, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), 0, Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
           // Game.spawners.Add(s);
        }
        private void Block()
        {
            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            int rows = (Game.HEIGHT - 100) / (SpriteTileSize[1] + 20);
            int x, y;
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                x = SpawnStartPosition[0] + (i / rows) * SpriteTileSize[0];
                y = SpawnStartPosition[1] + (i % rows) * SpriteTileSize[1];
               // s.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
            //Game.spawners.Add(s);
        }
        private void Diagonal()
        {
            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            int rows = (Game.HEIGHT - 100) / (SpriteTileSize[1] + 20);
            int x, y;
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                x = SpawnStartPosition[0] + i * SpriteTileSize[0];
                y = SpawnStartPosition[1] + (i % rows) * SpriteTileSize[1];
                //s.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
           // Game.spawners.Add(s);
        }

        private void ZigZag()
        {
            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            int rows = (Game.HEIGHT - 100) / (SpriteTileSize[1] + 50);
            int j = -1;
            int x = SpawnStartPosition[0];
            int y = SpawnStartPosition[1];
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                if (i % rows == 0) j = j * (-1);
                x += SpriteTileSize[0];
                y += j * SpriteTileSize[1];
                //s.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon)));
            }
            //Game.spawners.Add(s);
        }

      /* private void Standard()
        {
            int x, y;
            for (int i = 0; i < NumberOfObjects; i++)
            {
                x = SpawnStartPosition[0] + i * SpriteTileSize[0];
                y = SpawnStartPosition[1];
            //    Game.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
        }
        private void Block()
        {
            int rows = (Game.HEIGHT - 100) / (SpriteTileSize[1] + 20);
            int x, y;
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                x = SpawnStartPosition[0] + (i / rows) * SpriteTileSize[0];
                y = SpawnStartPosition[1] + (i % rows) * SpriteTileSize[1];
             //   Game.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
        }
        private void Diagonal()
        {
            int rows = (Game.HEIGHT - 100) / (SpriteTileSize[1] + 20);
            int x, y;
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                x = SpawnStartPosition[0] + i * SpriteTileSize[0];
                y = SpawnStartPosition[1] + (i % rows) * SpriteTileSize[1];
            //    Game.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
        }

        private void ZigZag()
        {
            int rows = (Game.HEIGHT - 100) / (SpriteTileSize[1] + 50);
            int j = -1;
            int x = SpawnStartPosition[0];
            int y = SpawnStartPosition[1];
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                if (i % rows == 0) j = j * (-1);
                x += SpriteTileSize[0];
                y += j * SpriteTileSize[1];
            //    Game.Add(new Enemy(CollisionType, SpriteTileSize, Density, Restitution, StaticFriction, KineticFriction, SpritePath, SpriteSize, new Vector2f(x, y), Health, Points, Damage, Game.factions[(int)Faction], MovementPattern, Weapon));
            }
        }*/
    }
}
