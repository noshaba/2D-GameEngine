using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Physics;

namespace Platformer
{
    class Pendulum
    {
        GameObject[] joints;
        public Pendulum(string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, 
            int animationIndex, Vector2f position, float rotation, float density, uint jointNumber, float jointDistance)
        {
            joints = new GameObject[jointNumber];
            for (int i = 0; i < jointNumber; ++i)
            {
                joints[i] = new GameObject(texturePath, spriteTileSize, spriteSize, tileIndices,
                    animationIndex, position + i * new Vector2f(0, jointDistance), rotation, density);
                Game.Add(joints[i]);
            }
            joints[0].Moveable = false;
            for (int i = 0; i < jointNumber - 1; ++i)
                Game.AddJoint(new DistanceConstraint(joints[i].rigidBody, joints[i+1].rigidBody, jointDistance));
        }
    }
}
