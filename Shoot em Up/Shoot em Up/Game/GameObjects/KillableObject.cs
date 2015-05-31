using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using SFML.Graphics;
using SFML.System;

namespace Shoot_em_Up
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
        protected IRigidBody[] bodies;

        public KillableObject( Vector2f position, float rotation, float radius)
            : base(position, rotation, radius)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, radius) };
        }

        public KillableObject( Vector2f position, float rotation, float radius, float density)
            : base(position, rotation, radius, density)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, radius) };
        }

        public KillableObject( Vector2f normal, Vector2f position, Vector2f size, float rotation)
            : base(normal, position, size, rotation)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, size.Y / 2) };
        }

        public KillableObject( Vector2f[] vertices, Vector2f position, float rotation)
            : base(vertices, position, rotation)
        {
            this.bodies = new[] { this.rigidBody };
        }
       /* public KillableObject( Texture texture, Vector2f position, float rotation)
            : base(texture, position, rotation)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, texture.Size.Y / 2) };
        }*/

        public KillableObject( Vector2f[] vertices, Vector2f position, float rotation, float density)
            : base(vertices, position, rotation, density)
        {
        }

        public KillableObject( String texture, int[]ts, int[]spriteSize, int a, Vector2f position, float rotation, float density)
            : base(texture, ts, spriteSize, a, position, 0, density)
        {
            this.bodies = new[] { this.rigidBody, new Circle(this, position, rotation, spriteSize[0] / 2) };
        }

        public override void Update()
        {
            if (rigidBody.Collision.collision)
            {
                opponent = this.rigidBody.Collision.obj.Parent as KillableObject;
                if (opponent != null && !shield)
                {
                    // decrease HP
                    this.hp -= opponent.damage;
                }
                if (opponent != null && shield)
                {
                    this.shieldHp -= opponent.damage;
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

        protected void updateBodies()
        {
            this.bodies[0].COM = this.rigidBody.COM;
            this.bodies[1].COM = this.rigidBody.COM;
            this.bodies[0].Velocity = this.rigidBody.Velocity;
            this.bodies[1].Velocity = this.rigidBody.Velocity;
            this.bodies[0].AngularVelocity = this.rigidBody.AngularVelocity;
            this.bodies[1].AngularVelocity = this.rigidBody.AngularVelocity;
            this.bodies[0].Orientation = this.rigidBody.Orientation;
            this.bodies[1].Orientation = this.rigidBody.Orientation;
        }

        protected void shieldOn(Vector2f velocity, Vector2f pos)
        {
            this.shield = true;
            this.rigidBody = this.bodies[1];
        }

        protected void shieldOff(Vector2f velocity, Vector2f pos)
        {
            this.shield = false;
            this.rigidBody = this.bodies[0];
        }


    }
}