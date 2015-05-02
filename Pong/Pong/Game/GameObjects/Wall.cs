using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Wall : Polygon {
        public Wall(Vector2f position, float hw, float hh, Color color) : base() {
            SetBox(position, hw, hh, 0);
            FillColor = color;
        }
    }
}
