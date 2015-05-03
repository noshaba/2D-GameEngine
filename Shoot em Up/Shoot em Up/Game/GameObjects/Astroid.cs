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
            this.Texture = new Texture("../Content/dirt.png");
            this.life = 100;
            this.Velocity = new Vector2f(-50,50);
        }
    }
}
