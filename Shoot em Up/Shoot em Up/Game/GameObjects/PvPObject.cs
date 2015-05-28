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
        protected bool shield;
        protected Polygon shape;

        public Faction Faction { get; private set; }

        public PvPObject(Faction faction, IRigidBody state) : base(state) {
            this.Faction = faction;
            this.shape = (Polygon)this.rigidBody;
        }

        public PvPObject(Faction faction, Collision.Type type, Vector2f position, float var, float density) : base(type, position, var, density)
        {
            this.Faction = faction;
        }

        public PvPObject(Faction faction, Vector2f normal, Vector2f position, Vector2f size, float rotation) : base(normal, position, size, rotation)
        {
            this.Faction = faction;
            this.shape = (Polygon)this.rigidBody;
        }
        
        public PvPObject(Faction faction,  Vector2f[] vertices, Vector2f position, float rotation) : base(vertices, position, rotation)
        {
            this.Faction = faction;
            this.shape = (Polygon)this.rigidBody;
        }

        public PvPObject(Faction faction, Texture texture, Vector2f position, float rotation) : base(texture, position, rotation)
        {
            this.Faction = faction;
            this.shape = (Polygon)this.rigidBody;
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

        protected void shieldOn(Vector2f velocity)
        {
            this.shield = true;
            this.rigidBody = new Circle(new Vector2f(this.rigidBody.COM.X, this.rigidBody.COM.Y), this.drawable.Texture.Size.Y / 2);
            this.rigidBody.Velocity = velocity;
        }

        protected void shieldOff(Vector2f velocity)
        {
            this.shield = false;
            this.rigidBody = this.shape;
            this.rigidBody.Velocity = velocity;
        }

        protected void shieldOn()
        {
            this.shield = true;
            this.rigidBody = new Circle(new Vector2f(this.rigidBody.COM.X, this.rigidBody.COM.Y), this.drawable.Texture.Size.Y / 2);
            
        }

        protected void shieldOff()
        {
            this.shield = false;
            this.rigidBody = this.shape;
        }
    }
}
