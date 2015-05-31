using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Shoot_em_Up
{
    class GroundTile : GameObject
    {
        public GroundTile(float restitution, float staticFriction, float kineticFriction, Texture texture, float rotation, uint[] spriteTileSize, int index)
            : base(texture, new Vector2f(0,0), rotation)
        {
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.rigidBody.COM = new Vector2f(spriteTileSize[0] * (index + .5f), Game.HEIGHT - spriteTileSize[1] + this.rigidBody.Centroid.Y);
        }
    }
}
