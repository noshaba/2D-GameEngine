using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Ball : Circle {

        public Collision.Type type = Collision.Type.Circle;

        public Ball(Vector2f position, float radius, Color color) : base(radius) {
            current = new State(position);
            previous = current;
            FillColor = color;
        }

        public Ball(Vector2f position, float radius, Color color, float mass) : base(radius) {
            current = new State(position, mass, (float) (Math.PI * 0.25 * Math.Pow(radius,4)) );
            previous = current;
            FillColor = color;
        }
    }
}
