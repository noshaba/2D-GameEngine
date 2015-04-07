using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Pong {
    abstract class Circle : CircleShape {
        public Collision.Type type = Collision.Type.Circle;

        protected State current;
        protected State previous;

        public Circle(Vector2f position, float radius) : base(radius) {
            Origin = new Vector2f(Position.X + radius * 0.5f, Position.Y + radius * 0.5f);
            Position = position;
        }

        public Vector2f Momentum {
            get { return current.momentum; }
            set { current.momentum = value; }
        }

        public void Update(float dt) {
            previous = current;
            current.Integrate(dt);
        }
    }
}
