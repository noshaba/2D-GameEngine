using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using SFML.Graphics;
using SFML.System;
using System.Diagnostics;

namespace Platformer
{
    class KillableObject : GameObject
    {
        public int score;
        public int hp;
        public int maxHP;
        public int damage;
        public int points;
        protected List<KillableObject> opponents = new List<KillableObject>();
        public bool alive = true;
        public Faction faction;
        public bool timed = false;
        private Stopwatch lifetime;

        public KillableObject(Faction faction, int dmg, int hp, IRigidBody[]bodies, Shape[] drawables, 
            Vector2f position, float rotation) 
            : base(bodies,drawables,position,rotation)
        {
            this.faction = faction;
            this.damage = dmg;
            this.hp = hp;
            this.maxHP = hp;
        }

        public KillableObject(bool timed, Faction faction, int dmg, int hp, IRigidBody[] bodies, Shape[] drawables,
            Vector2f position, float rotation)
            : base(bodies, drawables, position, rotation)
        {
            this.faction = faction;
            this.damage = dmg;
            this.hp = hp;
            this.maxHP = hp;
            this.timed = timed;
            if (timed)
            {
                lifetime = new Stopwatch();
                lifetime.Start();
            }
        }

        public KillableObject(Faction faction, int dmg, int hp, string texturePath, int[] spriteTileSize, 
            int[] spriteSize, int[] tileIndices, int animationIndex, Vector2f position, 
            float rotation, float density)
            : base(texturePath, spriteTileSize, spriteSize, tileIndices, animationIndex, 
            position, rotation, density)
        {
            this.faction = faction;
            this.damage = dmg;
            this.hp = hp;
            this.maxHP = hp;
        }

        public KillableObject(Faction faction, int dmg, int hp, IRigidBody[] bodies, Vector2f position, 
            float rotation)
            : base(bodies, position, rotation)
        {
            this.faction = faction;
            this.damage = dmg;
            this.hp = hp;
            this.maxHP = hp;
        }

        public override void EarlyUpdate()
        {
            opponents.Clear();
            foreach (Collision collision in rigidBody.Collision)
            {
                if (collision.collision)
                {
                    KillableObject opponent = collision.obj as KillableObject;
                    if (opponent != null)
                    {
                        opponents.Add(opponent);
                        // decrease HP
                        this.hp -= opponent.damage * 
                            (100 - opponent.faction.Reputation[(int)this.faction.ID]) / 100;
                    }
                }
            }
            base.EarlyUpdate();
        }

        public override void LateUpdate()
        {
            // change color with hp / MAXHP
            foreach (Shape shape in drawable)
                shape.FillColor = this.hp <= this.maxHP * 0.25 ? Color.Red : shape.FillColor;
            this.display = this.hp > 0;
            base.LateUpdate();
            if (timed)
                if (lifetime.ElapsedMilliseconds >= 10000)
                    this.hp = 0;
        }
    }
}