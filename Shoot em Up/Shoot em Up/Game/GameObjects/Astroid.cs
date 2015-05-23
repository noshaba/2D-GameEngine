using Maths;
using Physics;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Astroid : PvPObject
    {
        public Astroid(Faction faction, int x, int y)
            : base(faction, Collision.Type.Polygon, new Vector2f(x, y), 200, 0.01f)
        {

            (rigidBody as Polygon).Texture = new Texture("../Content/astroid.png");
            (rigidBody as Polygon).Texture.Repeated = true;
            rigidBody.Velocity = new Vector2f(EMath.random.Next(-50,50),EMath.random.Next(10,30));
            rigidBody.Restitution = 1.0f;
            this.hp = 50;
            this.maxDamage = 500;
            this.maxPoints = 20;
        }
    }
}
