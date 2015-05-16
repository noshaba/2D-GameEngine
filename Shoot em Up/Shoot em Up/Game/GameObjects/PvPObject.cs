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
        public bool alive = true;
        public int hp;

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
    }
}
