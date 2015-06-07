using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    class KillableObject : GameObject
    {
        public int score;
        public int hp;
        public int maxHP = 1;
        public int damage;
        public int points;
        protected KillableObject opponent;
        public bool alive = true;
        public bool shield;
        public int shieldHp;
        public int maxShieldHp;
        public Faction faction;
        protected IRigidBody[] bodies;

        public KillableObject(Faction faction, Vector2f position, float rotation, float radius)
            : base(position, rotation, radius)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, radius) };
            this.faction = faction;
        }

        public KillableObject(Faction faction, Vector2f position, float rotation, float radius, float density)
            : base(position, rotation, radius, density)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, radius) };
            this.faction = faction;
        }

        public KillableObject(Faction faction, Vector2f normal, Vector2f position, Vector2f size, float rotation)
            : base(normal, position, size, rotation)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, size.Y / 2) };
            this.faction = faction;
        }

        public KillableObject(Faction faction, Vector2f[] vertices, Vector2f position, float rotation)
            : base(vertices, position, rotation)
        {
            this.bodies = new[] { this.rigidBody };
            this.faction = faction;
        }
       /* public KillableObject( Texture texture, Vector2f position, float rotation)
            : base(texture, position, rotation)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, texture.Size.Y / 2) };
        }*/

        public KillableObject(Faction faction, Vector2f[] vertices, Vector2f position, float rotation, float density)
            : base(vertices, position, rotation, density)
        {
            this.faction = faction;
        }

        public KillableObject(Faction faction, String texture, int[]ts, int[]spriteSize, int a, Vector2f position, float rotation, float density)
            : base(texture, ts, spriteSize, a, position, 0, density)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, spriteSize[0] / 2) };
            this.faction = faction;
        }

        public override void Update()
        {
            if (rigidBody.Collision.collision)
            {
                opponent = this.rigidBody.Collision.obj.Parent as KillableObject;
                if (opponent != null && !shield)
                {
                    // decrease HP
                    this.hp -= opponent.damage * (100 - opponent.faction.Reputation[(int)this.faction.ID]) / 100;
                }
                if (opponent != null && shield)
                {
                    this.shieldHp -= opponent.damage * (100 - opponent.faction.Reputation[(int)this.faction.ID]) / 100;
                }
            }
            base.Update();
        }

        public override void LateUpdate()
        {

            opponent = null;
            // change color with hp / MAXHP
            this.drawable.FillColor = this.hp <= this.maxHP * 0.25 ? Color.Red : drawable.FillColor;
            this.display = this.hp > 0;
            base.LateUpdate();
        }

        protected void UpdateBodies()
        {
            for (int i = 0; i < this.bodies.Length; i++)
            {
                this.bodies[i].COM = this.rigidBody.COM;
                this.bodies[i].Velocity = this.rigidBody.Velocity;
                this.bodies[i].AngularVelocity = this.rigidBody.AngularVelocity;
                this.bodies[i].Orientation = this.rigidBody.Orientation;
                this.drawables[i].Position = this.drawable.Position;
                this.drawables[i].Origin = this.drawable.Origin;
            }
        }

        protected void ShieldOn(Vector2f velocity, Vector2f pos)
        {
            this.shield = true;
            this.rigidBody = this.bodies[1];
        }

        protected void ShieldOff(Vector2f velocity, Vector2f pos)
        {
            this.shield = false;
            this.rigidBody = this.bodies[0];
        }

    }
}