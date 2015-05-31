using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Shoot_em_Up
{
    class GroundTile : GameObject
    {
        public GroundTile(float restitution, float staticFriction, float kineticFriction, string texturePath, int[] spriteTileSize, int[] spriteSize, int tiles, int index)
            : base(texturePath, spriteTileSize, spriteSize, EMath.random.Next(0, tiles), new Vector2f(0,0), 0)
        {
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.rigidBody.COM = new Vector2f(spriteTileSize[0] * 0.95f * (index + .5f), Game.HEIGHT - spriteTileSize[1] + this.rigidBody.Centroid.Y);
        }
    }
}
