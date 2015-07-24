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
    class CoinContract
    {
        /*
         * {
        "Points": 10,
        "SpawnPatternID": 3,
        "NumberOfObjects": 10,
        "SpawnStartPosition": [
            800,
            1200
        ],
        "SpritePath": "../Content/CoinSprite.png",
        "SpriteSize": [
            50,
            300
        ],
        "SpriteTileSize": [
            50,
            50
        ]
    }
         */
        [DataMember]
        public int Points { get; set; }
        [DataMember]
        public int Faction { get; set; }
        [DataMember]
        public int NumberOfObjects { get; set; }
        [DataMember]
        public int[] SpawnStartPosition { get; set; }
        [DataMember]
        public string SpritePath { get; set; }
        [DataMember]
        public int[] SpriteSize { get; set; }
        [DataMember]
        public int[] SpriteTileSize { get; set; }

        public void Init()
        {
            Spawner s = new Spawner(new Vector2f(SpawnStartPosition[0], SpawnStartPosition[1]));
            int x, y;
            for (int i = 0; i < NumberOfObjects; i++)
            {
                x = SpawnStartPosition[0] + i * SpriteTileSize[0];
                y = SpawnStartPosition[1];
                s.Add(new Coin(SpriteTileSize, new int[]{0},0,0,0,0,0,SpritePath, SpriteSize, new Vector2f(x,y), 0,1,Points,0, Game.factions[Faction]));
            }
            Game.spawners.Add(s);
        }
    }
}
