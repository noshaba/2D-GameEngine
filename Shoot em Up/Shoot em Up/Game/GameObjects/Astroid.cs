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
    class Astroid : Polygon
    {
        public int life;
        public Astroid(int x, int y)
            : base(new Vector2f(x, y), 200, 0.01f)
        {

            Texture = new Texture("../Content/astroid.png");
            Velocity = new Vector2f(EMath.random.Next(-50,50),EMath.random.Next(10,30));
            Restitution = 1.0f;
            this.life = 100;
        }

        public override void ReactToCollision(Collision colliInfo)
        {
            Console.WriteLine("Astroid hit something");

        }
    }
}
