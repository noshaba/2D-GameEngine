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

namespace Platformer
{
    class Bullet  : KillableObject
    {
        private Vector2f bend;
        public KillableObject shooter;

        public Bullet(KillableObject shooter, int dmg, 
            string bulletPath, int[] spriteTileSize, int[] spriteSize, int[] tileIndices,
            int animationIndex, float density, Vector2f position, Vector2f speed, Vector2f bend)
            : base(shooter.faction, dmg, 1, bulletPath, spriteTileSize, spriteSize, tileIndices,
                animationIndex, position + speed + bend, 0, density)
        {
            this.RigidBodyParent = this;
            this.Restitution = 1.0f;
            this.rigidBody.Velocity = speed * 5;
            this.shooter = shooter;
            this.bend = bend;
        }

        public void Charge(Vector2f position, Vector2f speed, Vector2f bend)
        {
            this.rigidBody.COM = position + speed + bend;
            this.rigidBody.Velocity = speed * 5;
            this.bend = bend;
            this.hp = this.maxHP;
            this.alive = true;
            this.display = true;
        }

        public override void EarlyUpdate()
        {
            base.EarlyUpdate();
            if (opponents.Count > 0)
            {
                this.hp = 0;
                this.alive = false;
            }
            foreach (KillableObject opponent in opponents)
            {
                if (opponent.hp <= 0 && opponent.alive)
                {
                    shooter.score += (100 - opponent.faction.Reputation[(int)this.faction.ID]) * opponent.points / 100;
                    opponent.alive = false;
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