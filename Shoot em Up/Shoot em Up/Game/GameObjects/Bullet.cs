using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Physics;
using Maths;
using SFML.System;

namespace Shoot_em_Up {
    class Bullet : PvPObject {

        private Vector2f initPosition;
        private const float MAXVELOCITY2 = 40000;
        private Vector2f bend;
        private Player shooter;

        public Bullet(Player shooter, Faction faction, Vector2f position, float radius, Color color, float mass, int dmg, Vector2f speed, Vector2f bend) : base(faction, Collision.Type.Circle, position + speed + bend, radius, mass) {
            initPosition = position + speed + bend;
            (rigidBody as Circle).FillColor = color;
            rigidBody.Restitution = 1.0f;
            rigidBody.Velocity = speed;
            this.shooter = shooter;
            this.hp = 100;
            this.maxDamage = dmg;
            this.maxPoints = 0;
            this.bend = bend;
        }

        public override void Update()
        {
            base.Update();
            if (rigidBody.Collision.collision)
            {
                this.hp = 0;
                this.alive = false;
                if (opponent != null)
                {
                    if(opponent.hp <= 0 && opponent.alive) {
                        shooter.score += (100 - opponent.Faction.Reputation[(int)this.Faction.ID]) * opponent.maxPoints / 100;
                        opponent.alive = false;
                    }
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            rigidBody.Velocity += bend;
        }
    }
}
