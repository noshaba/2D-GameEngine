using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Obstacle : Polygon {
        public Obstacle(Vector2f position, float rotation, Color color) : base(position, rotation) {
            FillColor = color;
            Restitution = 1.0f;
        }

        public Obstacle(Vector2f position, float rotation, Color color, float density) : base(position, rotation, density) {
            FillColor = color;
            Restitution = 1.0f;
        }
    }
}
