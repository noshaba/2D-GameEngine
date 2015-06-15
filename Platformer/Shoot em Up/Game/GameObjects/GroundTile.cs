﻿using System;
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
        public GroundTile(float restitution, float staticFriction, float kineticFriction, string texturePath, int[] spriteTileSize, int[] spriteSize, int tile, int index)
            : base(texturePath, spriteTileSize, spriteSize, tile, new Vector2f(0,0), 0, 0)
        {
            this.rigidBody.Restitution = restitution;
            this.rigidBody.StaticFriction = staticFriction;
            this.rigidBody.KineticFriction = kineticFriction;
            this.rigidBody.COM = new Vector2f(spriteTileSize[0] * 0.95f * (index + .5f), Game.HEIGHT - spriteTileSize[1] + this.rigidBody.Centroid.Y);
        }
    }
}
