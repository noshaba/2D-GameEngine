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
    class PvPObject : GameObject
    {
        public int score;
        public int hp;
        public int maxHP = 1;
        public int maxDamage;
        public int maxPoints;
        protected PvPObject opponent;
        public bool alive = true;
        public bool shield;
        protected IRigidBody[] bodies;

        public Faction Faction { get; private set; }

        public PvPObject(Faction faction, IRigidBody state) : base(state) {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody };
        }

        public PvPObject(Faction faction, Vector2f position, float rotation, float radius)
            : base(position, rotation, radius)
        {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody, new Circle(position, rotation, radius) };
        }

        public PvPObject(Faction faction, Vector2f position, float rotation, float radius, float density)
            : base(position, rotation, radius, density)
        {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody, new Circle(position, rotation, radius) };
        }

        public PvPObject(Faction faction, Vector2f normal, Vector2f position, Vector2f size, float rotation) : base(normal, position, size, rotation)
        {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody, new Circle(position, rotation, size.Y/2) };
        }
        
        public PvPObject(Faction faction,  Vector2f[] vertices, Vector2f position, float rotation) : base(vertices, position, rotation)
        {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody };
        }

        public PvPObject(Faction faction, Texture texture, Vector2f position, float rotation) : base(texture, position, rotation)
        {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody, new Circle(position, rotation, texture.Size.Y/2) };
        }

        public PvPObject(Faction faction, Vector2f[] vertices, Vector2f position, float rotation, float density)
            : base(vertices, position, rotation, density)
        {
            this.Faction = faction;
        }

        public PvPObject(Faction faction, Texture texture, Vector2f position, float rotation, float density)
            : base(texture, position, rotation, density)
        {
            this.Faction = faction;
            this.bodies = new[] { this.rigidBody, new Circle(position, rotation, texture.Size.Y / 2) };
        }

        public override void Update()
        {
            if (rigidBody.Collision.collision)
            {
                opponent = this.rigidBody.Collision.obj.Parent as PvPObject;
                if (opponent != null && !shield)
                {
                    // decrease HP
                    this.hp -= opponent.maxDamage * (100 - opponent.Faction.Reputation[(int)this.Faction.ID]) / 100;
                    // decrease reputation with the opponent's faction if the opponent is dead
                    this.Faction.Reputation[(int)opponent.Faction.ID] +=
                        opponent.Faction.GainableRep && !opponent.alive &&
                        1 <= this.Faction.Reputation[(int)opponent.Faction.ID] ?
                        -1 : 0;
                }
            }
            base.Update();
        }

        public override void LateUpdate()
        {
            
            opponent = null;
            // change color with hp / MAXHP
            this.drawable.FillColor = this.hp <= this.maxHP*0.25 ? Color.Red : drawable.FillColor;
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
            //this.rigidBody.COM = pos;
        }

        protected void shieldOff(Vector2f velocity, Vector2f pos)
        {
            this.shield = false;
            this.rigidBody = this.bodies[0];
            //this.rigidBody.COM = pos;
        }
    }
}
