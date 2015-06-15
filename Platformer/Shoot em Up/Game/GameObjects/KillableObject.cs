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

        public KillableObject(Faction faction, Vector2f position, float rotation, float radius)
            : base(position, rotation, radius)
        {
            this.faction = faction;
        }

        public KillableObject(Faction faction, Vector2f position, float rotation, float radius, float density)
            : base(position, rotation, radius, density)
        {
            this.faction = faction;
        }

        public KillableObject(Faction faction, Vector2f normal, Vector2f position, Vector2f size, float rotation)
            : base(normal, position, size, rotation)
        {
            this.faction = faction;
        }

        public KillableObject(Faction faction, String texture, int[]spriteTileSize, int[]spriteSize, int animationIndex, Vector2f position, float rotation, float density)
            : base(texture, spriteTileSize, spriteSize, animationIndex, position, rotation, density)
        {
            this.faction = faction;
        }

        public KillableObject(Faction faction, String texture, int[] spriteTileSize, int[] spriteSize, int animationIndex, Vector2f centroid, Vector2f position, float rotation, float density)
            : base(texture, spriteTileSize, spriteSize, animationIndex, centroid, position, rotation, density)
        {
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
            for (int i = 0; i < this.rigidBodies.Length; i++)
            {
                this.rigidBodies[i].Current = this.rigidBody.Current;
                this.rigidBodies[i].Previous = this.rigidBody.Previous;
            }
        }
    }
}