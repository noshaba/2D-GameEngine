using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Diagnostics;

namespace Platformer
{
    class Player : KillableObject
    {

        //Faction faction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Player(Faction faction, Vector2f position, String texture, int[]tileSize, int[]spriteSize, int[]tileIndices)
            : base(faction, 0, 1000, texture, tileSize, spriteSize,tileIndices, 0, position, 0, 0.01f)
        {
            foreach (Body body in rigidBodies)
            {
                body.Restitution = 0;
                body.rotateable = false;
                body.Parent = this;
            }
            this.speed = 80;
            this.score = 0;
            this.currentState = new PlayerIdle( this);
            this.animated = true;
        }

        public void Move(Keyboard.Key k)
        {
            this.currentState.HandleInput(k, true);
        }

        public void Release(Keyboard.Key k)
        {
            this.currentState.HandleInput(k, false);
        }

        public void Stop()
        {
            this.rigidBody.Velocity = new Vector2f(0, 0);
            this.rigidBody.AngularVelocity = 0;
        }
    }
}

