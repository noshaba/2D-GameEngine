using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;

namespace Platformer
{
    class Obstacle : KillableObject
    {
        private Vector2f initPos;
        private bool hover;

        // Faction faction, string texturePath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, float rotation, float density
        public Obstacle(bool hover, int[] animation, int[] spriteTileSize, int[] tileIndices, int animationIndex, float density, float restitution, float staticFriction, float kineticFriction, String texturePath, int[] spriteSize, Vector2f position, float rotation, int health, int points, int dmg, Faction faction)
            : base(faction, dmg, health, texturePath, spriteTileSize, spriteSize, tileIndices, animationIndex, position, rotation, density)
         {
             RigidBodyParent = this;
             this.hover = hover;
             this.initPos = position;
             this.rigidBody.Restitution = restitution;
             this.rigidBody.StaticFriction = staticFriction;
             this.rigidBody.KineticFriction = kineticFriction;
             this.points = points;
             this.animated = true;
             this.currentState = new AnimState(animation, this);
        }

         public override void EarlyUpdate()
         {
             base.EarlyUpdate();
             this.UpdateBodies();
             this.rigidBody = this.rigidBodies[this.animationFrame];
             this.drawable = this.drawables[this.animationFrame];
             if (this.hover)
             {
                 this.rigidBody.COM = new Vector2f(this.rigidBody.COM.X, this.initPos.Y);
                 this.rigidBody.Velocity = new Vector2f(0, 0);
             }
         }
    }
}
