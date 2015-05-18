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

namespace Shoot_em_Up {
    class Bullet : PvPObject {

        private Vector2f initPosition;
        private const float MAXVELOCITY2 = 40000;
        private Vector2f bend;

        public Bullet(Faction faction, Vector2f position, float radius, Color color, float mass, Vector2f bend, int dmg) : base(faction, Collision.Type.Circle, position, radius, mass) {
            initPosition = position;
            (state as Circle).FillColor = color;
            state.Restitution = 1.0f;
            state.Velocity = new Vector2f(0,-50);
            this.hp = 1;
            this.maxDamage = dmg;
            this.maxPoints = 0;
            this.bend = bend;
        }

        public override void Update()
        {
            state.Velocity += bend;
            base.Update();
        }
    }
}
