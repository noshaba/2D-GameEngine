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
        public Astroid()
            : base(new Vector2f(20.0f, 20.0f), 200)
        {
            this.Position = new Vector2f(100, 200);
            this.Texture = new Texture("../Content/dirt.png");
            this.life = 100;

        }
    }
}
