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
        public int hp;
        public int maxDamage;
        public int maxPoints;
        protected PvPObject attacked;

        public Faction Faction { get; private set; }

        public PvPObject(Faction faction, IState state) : base(state) {
            this.Faction = faction;
        }

        public PvPObject(Faction faction, Collision.Type type, Vector2f position, float var, float density) : base(type, position, var, density)
        {
            this.Faction = faction;
        }

        public PvPObject(Faction faction, Vector2f normal, Vector2f position, Vector2f size, float rotation) : base(normal, position, size, rotation)
        {
            this.Faction = faction;
        }

        public override void Update()
        {
            if (state.Collision.collision)
            {
                attacked = this.state.Collision.obj.Parent as PvPObject;
                if (attacked != null)
                {
                    attacked.hp -= (int)((1 - this.Faction.Reputation[(int)attacked.Faction.ID]) * this.maxDamage);
                    this.hp -= (int)((1 - attacked.Faction.Reputation[(int)this.Faction.ID]) * attacked.maxDamage);
                }
            }
            base.Update();
        }

        public override void LateUpdate()
        {
            if (attacked != null)
            {
                this.Faction.Reputation[(int)attacked.Faction.ID] -= attacked.Faction.GainableRep && attacked.hp <= 0
                    && 1 <= this.Faction.Reputation[(int)attacked.Faction.ID] ? 1 : 0;
                attacked = null;
            }
            this.display = this.hp > 0;
            base.LateUpdate();
        }
    }
}
