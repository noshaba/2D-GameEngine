using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Maths;

namespace Platformer
{
    class GroundTile : GameObject
    {
        public GroundTile(float restitution, float staticFriction, float kineticFriction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, int index, int height)
            : base(texturePath, spriteTileSize, spriteSize, tileIndices, animationIndex, new Vector2f(0,0), 0, 0)
        {
            RigidBodyParent = this;
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.rigidBody.COM = new Vector2f(spriteTileSize[0] * (index + .5f), Game.levelSize.Y - height*spriteTileSize[1] + spriteTileSize[1] * .5f);
        }
    }
}
