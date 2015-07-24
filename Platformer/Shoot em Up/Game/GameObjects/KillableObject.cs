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
        public int maxHP;
        public int damage;
        public int points;
        protected List<KillableObject> opponents = new List<KillableObject>();
        public bool alive = true;
        public bool shield;
        public int shieldHp;
        public int maxShieldHp;
        public Faction faction;

        public KillableObject(KillableObject copy)
            : base(copy as GameObject)
        {
            this.score = copy.score;
            this.hp = copy.hp;
            this.maxHP = copy.maxHP;
            this.damage = copy.damage;
            this.points = copy.points;
            this.shield = copy.shield;
            this.shieldHp = copy.shieldHp;
            this.maxShieldHp = copy.maxShieldHp;
            this.faction = copy.faction;
        }

        public KillableObject(Faction faction, int dmg, int hp, IRigidBody[]bodies, Shape[] drawables, 
            Vector2f position, float rotation) 
            : base(bodies,drawables,position,rotation)
        {
            this.faction = faction;
            this.damage = dmg;
            this.hp = hp;
            this.maxHP = hp;
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

        /*

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
       */

        public override void EarlyUpdate()
        {
            opponents.Clear();
            foreach (Collision collision in rigidBody.Collision)
            {
                if (collision.collision)
                {
                    KillableObject opponent = collision.obj as KillableObject;
                    if (opponent != null && !shield)
                    {
                        opponents.Add(opponent);
                        // decrease HP
                        this.hp -= opponent.damage * 
                            (100 - opponent.faction.Reputation[(int)this.faction.ID]) / 100;
                    }
                    if (opponent != null && shield)
                    {
                        opponents.Add(opponent);
                        this.shieldHp -= opponent.damage * 
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
        }
    }
}