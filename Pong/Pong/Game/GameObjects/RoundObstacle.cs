using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using Physics;

namespace Pong {
    class RoundObstacle : Circle {
        public RoundObstacle(Vector2f position, float rotation, Color color) : base(position, rotation) {
            FillColor = color;
            Restitution = 1.0f;
        }

        public RoundObstacle(Vector2f position, float rotation, Color color, float density)
            : base(position, rotation, density) {
            FillColor = color;
            Restitution = 1.0f;
        }
    }
}
