using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    class Ball : CircleShape {

        private State current;
        private State previous;

        public Vector2f Momentum {
            get { return current.momentum; }
            set { current.momentum = value; }
        }

        public Ball(Vector2f position, float radius, Color color) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
            FillColor = color;
            current = new State(position);
            previous = current;
        }

        public Ball(Vector2f position, float radius, Color color, float mass) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
            FillColor = color;
            current = new State(position,mass,(float) (Math.PI * 0.25 * Math.Pow(radius,4)) );
            previous = current;
        }

        public void Update(float dt){
            previous = current;
            current.Integrate(dt);
        }
    }
}
