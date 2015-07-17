﻿using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Platform : GameObject
    {
        Vector2f globalPosition;
        private bool breakable;
        // SpritePath, Position, Rotation, SpriteSize, SpriteTileSize, Tiles, KineticFriction, StaticFriction, Restitution, Density
       public Platform(bool breakable, bool rotateable, String path, Vector2f position, float rotation, int[] spriteSize, int[] tileSize, int[] tiles,
           float kineticFriction, float staticFriction, float restitution, float density)
            : base(path, tileSize, spriteSize, tiles, 0, position, rotation,density)
        {
            this.breakable = breakable;
            this.Rotateable = rotateable;
            globalPosition = position;
            foreach (Body body in rigidBodies)
            {
                body.moveable = false;
                body.Restitution = restitution;
                body.StaticFriction = staticFriction;
                body.KineticFriction = kineticFriction;
                body.Parent = this;
                for (int i = 0; i < body.bodies.Length; ++i)
                {
                    body.bodies[i].COM += new Vector2f(i * tileSize[0], 0);
                }
                body.UpdateCentroid();
                body.COM = position;
                body.UpdateBoundingCircle();
            }
        }

       public void Shatter()
       {

           if (this.breakable)
           {
               Game.Remove(this);

               Vector2f pos;
               for (int i = 0; i < rigidBody.bodies.Length; ++i)
               {
                   pos = rigidBody.bodies[i].Center;
                   rigidBody.bodies[i].Centroid = -rigidBody.bodies[i].Centroid;
                   Game.Add(new KillableObject(Game.factions[0], 500, 100, new[] { rigidBody.bodies[i] }, new[] { drawables[0][i] }, pos, 0));
               }
           }
       }
    }
}
