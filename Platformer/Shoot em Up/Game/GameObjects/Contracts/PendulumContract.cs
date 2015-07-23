using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFML.System;

namespace Platformer
{
    [DataContract]
    class PendulumContract
    {
        [DataMember]
        public string BallSpritePath { get; set; }
        [DataMember]
        public int[] BallSpriteSize { get; set; }
        [DataMember]
        public int[] BallTileSize { get; set; }
        [DataMember]
        public int[] TileIndices { get; set; }
        [DataMember]
        public int AnimationIndex { get; set; }
        [DataMember]
        public int[] Position { get; set; }
        [DataMember]
        public int Rotation { get; set; }
        [DataMember]
        public float Density { get; set; }
        [DataMember]
        public uint NumberOfBalls { get; set; }
        [DataMember]
        public int DistanceBetweenBalls { get; set; }

        public void Init()
        {
            new Pendulum(BallSpritePath, BallTileSize, BallSpriteSize, TileIndices, AnimationIndex,
                new Vector2f(Position[0], Position[1]), Rotation, Density, NumberOfBalls, DistanceBetweenBalls);
        }
    }
}
