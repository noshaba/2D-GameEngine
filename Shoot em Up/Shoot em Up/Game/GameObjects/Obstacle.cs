using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Obstacle : GameObject
    {
        private float hp;
        private float dmg;
        private float points;
        public int type;
        public int[] tileSize;
         public Obstacle(int type, int[]ts, float density, float restitution, float staticFriction, float kineticFriction, Texture texture, Vector2f position, float health, float points, float dmg)
            : base(texture, position, 0, density)
         {
             this.rigidBody.Restitution = restitution;
             this.rigidBody.StaticFriction = staticFriction;
             this.rigidBody.KineticFriction = kineticFriction;
             this.hp = health;
             this.dmg = dmg;
             this.points = points;
             this.type = type;
             this.tileSize = ts;
        }
    }
}
