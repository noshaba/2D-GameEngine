using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Obstacle : KillableObject
    {
        private float hp;
        private float dmg;
        private float points;
        public int type;

       // string texturePath, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f position, float rotation, float density
         public Obstacle(int type, int[]ts, float density, float restitution, float staticFriction, float kineticFriction, String texture, int[]spriteSize, Vector2f position, float health, float points, float dmg)
            : base(texture, ts, spriteSize,0, position, 0, density)
         {
             this.rigidBody.Restitution = restitution;
             this.rigidBody.StaticFriction = staticFriction;
             this.rigidBody.KineticFriction = kineticFriction;
             this.hp = health;
             this.dmg = dmg;
             this.points = points;
             this.type = type;
        }
    }
}
