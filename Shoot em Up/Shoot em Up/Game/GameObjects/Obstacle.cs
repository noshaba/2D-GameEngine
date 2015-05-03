using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Physics;

namespace Shoot_em_Up {
    class Obstacle : Polygon {
        public Obstacle(Vector2f position, float rotation, Color color) : base(position, rotation) {
            FillColor = color;
        }

        public Obstacle(Vector2f position, float rotation, Color color, float density) : base(position, rotation, density) {
            FillColor = color;
        }
    }
}
