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
using System.Diagnostics;

namespace Platformer
{
    class Bullet  : KillableObject
    {
        private Vector2f bend;
        public KillableObject shooter;
        private Stopwatch clock;
        private int lifeTime;

        public Bullet(KillableObject shooter, int dmg, float radius, float density, Vector2f position, Vector2f speed, Vector2f bend,
            Color color, int outlineThickness, Color outlineColor, int lifeTime)
            : base(shooter.faction, dmg, 1, new []{ new Circle(new Vector2f(), 0, radius, density)}, position + speed + bend, 0)
        {
            this.RigidBodyParent = this;
            this.Restitution = 1.0f;
            this.rigidBody.Velocity = speed * 5;
            this.shooter = shooter;
            this.bend = bend;
            this.drawable[0].FillColor = color;
            this.drawable[0].OutlineThickness = outlineThickness;
            this.drawable[0].OutlineColor = outlineColor;
            this.lifeTime = lifeTime;
            this.clock = new Stopwatch();
            this.clock.Start();
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
            if (this.clock.ElapsedMilliseconds > this.lifeTime)
            {
                this.hp = 0;
                this.alive = false;
                this.clock.Reset();
            }
            rigidBody.Velocity += bend;
        }
    }
}